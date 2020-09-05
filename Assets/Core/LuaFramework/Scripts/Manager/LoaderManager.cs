using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif
namespace LuaFramework
{
    public class LoaderManager : Manager
    {
        string random = DateTime.Now.ToString("yyyymmddhhmmss");
        string webUrl;// 远程资源目录
        string fileUrl;//= url + f + "?v=" + random; //接取服务器资源
        string dataPath;//本地数据目录
        string localfile;//= (dataPath + f).Trim();
        Dictionary<string, bool> loadingMap;
        List<ResLoader> loaderQueue;
        Dictionary<string, LoaderInfo> assetBundleLoaderMap;
        Dictionary<string, ResLoader> preLoaderMap;
        void Awake()
        {
            loadingMap = new Dictionary<string, bool>();
            loaderQueue = new List<ResLoader>();
            assetBundleLoaderMap = new Dictionary<string, LoaderInfo>();
            preLoaderMap = new Dictionary<string, ResLoader>();
            webUrl = AppConst.WebUrl; // 远程资源目录
            dataPath = Util.DataPath; // 本地数据目录
        }

        #region 处理场景资源

        const string sceneRoot = "scene/";//unity中 assets目录下场景资源的根目录
        ///<summary>
        /// 加载除了GameManager 中已经加载剩下的场景资源，这里可以理解为动态地加载外部场景资源
        /// sceneId 资源名字，注意这里的不是assetbundle资源名
        /// finishCallback 加载结束回调，失败时，参数为null，成功时为场景ID => 如果为null 即约定为后台预加载资源
        /// progressCallback 加载过程中回调，表作进度显示
        /// useLocal 是否使用本地app包资源 
        /// 注 一般后台预加载资源 主要是遍历场景传送节点，将传送场景进行加载
        ///</summary>
        public void LoadScene(string sceneId, Action<string> finishCallback, Action<float> progressCallback, bool useLocal = false)
        {

            ResLoader loader = null;
            string abName = sceneRoot + sceneId + AppConst.ExtName;
            if (finishCallback == null)
            {
                loader = new ResLoader(sceneId, sceneRoot, finishCallback, progressCallback);
                if (!PreLoadScene(loader)) return;
            }
            else
            {
                if (preLoaderMap.ContainsKey(abName))
                {
                    loader = preLoaderMap[abName];
                    preLoaderMap.Remove(abName);
                    loader.finishCallback = finishCallback;
                    loader.progressCallback = progressCallback;
                }
                else
                {
                    loader = new ResLoader(sceneId, sceneRoot, finishCallback, progressCallback);
                }
            }
            LoadSceneResHandler(loader, useLocal);


            //LoadSceneResHandler(new ResLoader(sceneId, sceneRoot, finishCallback, progressCallback), useLocal);
        }
        public void LoadScene(string sceneId, LuaInterface.LuaFunction finishCallback, LuaInterface.LuaFunction progressCallback, bool useLocal = false)
        {
            ResLoader loader = null;
            string abName = sceneRoot + sceneId + AppConst.ExtName;
            if (finishCallback == null)
            {
                loader = new ResLoader(sceneId, sceneRoot, finishCallback, progressCallback);
                if (!PreLoadScene(loader))
                    return;
            }
            else
            {
                if (preLoaderMap.ContainsKey(abName))
                {
                    loader = preLoaderMap[abName];
                    preLoaderMap.Remove(abName);
                    loader.finishLuaCallback = finishCallback;
                    loader.progressLuaCallback = progressCallback;
                }
                else
                {
                    loader = new ResLoader(sceneId, sceneRoot, finishCallback, progressCallback);
                }
            }
            LoadSceneResHandler(loader, useLocal);

            //LoadSceneResHandler(new ResLoader(sceneId, sceneRoot, finishCallback, progressCallback), useLocal);
        }

        /// 与 LoadScene联用 记录预加载对象
        bool PreLoadScene(ResLoader loader)
        {
            LoaderInfo loaderInfo = assetBundleLoaderMap[loader.abName];//远程资源信息
            string remoteKey = loaderInfo.key;//远程资源key值
            string abName = loader.abName;
            if (IsLoaded(abName, loaderInfo.key) || preLoaderMap.ContainsKey(abName))
                return false;
            preLoaderMap.Add(abName, loader);
            return true;
        }

        private void LoadSceneResHandler(ResLoader resLoader, bool useLocal)
        {
            GC.Collect();
            if (AppConst.DebugMode || useLocal)
            {// 本地调试
                StartCoroutine(DoRenderScene(resLoader));
            }
            else
            {
#if SyncLocal //不进行更新
	        ResManager.GetSceneAB(resLoader.resId);
	        StartCoroutine(DoRenderScene(resLoader));
#else
                LoadRes(resLoader, (string v) =>
                {
                    if (!string.IsNullOrEmpty(v))
                    {
                        ResManager.GetSceneAB(resLoader.resId);

                        print("LoaderManager 完成加载场景资源 --->" + resLoader.resId);
                        if(preLoaderMap.ContainsKey(resLoader.abName)) return;// 预加载的，不再执行

                        StartCoroutine(DoRenderScene(resLoader));
                    }
                    else
                    {
                        print("场景资源 " + resLoader.resId + " 不存在！");
                        resLoader.CallFinish("");
                    }
                });
#endif
            }
        }

        private IEnumerator DoRenderScene(ResLoader resLoader)
        {
#if UNITY_5_3_OR_NEWER
        AsyncOperation op = SceneManager.LoadSceneAsync(resLoader.resId);
#else
	    AsyncOperation op = Application.LoadLevelAsync(resLoader.resId);
#endif

            while (!op.isDone)
            {
                resLoader.CallProgress(op.progress * 0.1f + 0.8f);
                yield return null;
            }
            if (op.isDone)
            {
                resLoader.CallProgress(1.0f);
                yield return new WaitForEndOfFrame();
                resLoader.CallFinish(resLoader.resId);
                // Util.ClearMemory();
            }
        }

        #endregion


        /// <summary>处理队列加载资源</summary>
        private void LoadRes(ResLoader resLoader, Action<string> quequeHandler)
        {
            if (assetBundleLoaderMap.ContainsKey(resLoader.abName))
            {
                resLoader.quequeHandler = quequeHandler;//记录加载完成时的队列回调
                LoaderInfo loaderInfo = assetBundleLoaderMap[resLoader.abName];
                if (IsLoaded(resLoader.abName, loaderInfo.key))//已经加载
                {
                    resLoader.quequeHandler(resLoader.resId);
                    return;
                }
                loaderQueue.Add(resLoader);
                if (loadingMap.ContainsKey(resLoader.abName))//已经加载中
                    return;
                loadingMap.Add(resLoader.abName, true);
                StartCoroutine(_LoadRes());
            }
            else
            {
                quequeHandler(resLoader.resId);
            }
        }

        IEnumerator _LoadRes()
        {
            if (loaderQueue.Count == 0) yield break;
            ResLoader item = loaderQueue[0];
            loaderQueue.RemoveAt(0);

            string f = item.abName;//assetbundle资源名
            string path;//本地资源位置
            string localKey;//当前本地资源key值
            LoaderInfo loaderInfo = assetBundleLoaderMap[f];//远程资源信息
            string remoteKey = loaderInfo.key;//远程资源key值

            #region 再检查是否已经加载
            if (IsLoaded(f, loaderInfo.key))
            {
                item.quequeHandler(item.resId);
                loadingMap.Remove(f);
                item.quequeHandler(item.resId);
                yield return StartCoroutine(_LoadRes());
                yield break;
            }
            #endregion

            if (!assetBundleLoaderMap.ContainsKey(f))
            {
                Debug.LogError("不存在资源:" + f);
                loadingMap.Remove(f);
                item.quequeHandler("");
                yield return StartCoroutine(_LoadRes());
                yield break;
            }
            fileUrl = webUrl + f + "?v=" + random; //接取服务器资源
            localfile = (dataPath + f).Trim();

            bool canUpdate = !File.Exists(localfile);// 是否需要更新
            path = Path.GetDirectoryName(localfile);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!canUpdate) //检查是否更新
            {
                localKey = PlayerPrefs.GetString(f);
                canUpdate = !remoteKey.Equals(localKey);
                if (canUpdate)
                    File.Delete(localfile);
            }

            if (canUpdate)
            { //更新或新增文件
                WWW www = new WWW(fileUrl);
                while (!www.isDone)
                {
                    item.CallProgress(www.progress * 0.70f);
                    yield return 1;
                }
                if (www.error != null)
                {
                    loadingMap.Remove(f);
                    item.quequeHandler("");
                    yield return StartCoroutine(_LoadRes());
                    www.Dispose();
                    www = null;
                    yield break;
                }
                if (www.isDone)
                {
                    yield return 1;
                    File.WriteAllBytes(localfile, www.bytes);
                    yield return StartCoroutine(_LoadResManifest(item));
                    loadingMap.Remove(item.abName);
                    PlayerPrefs.SetString(item.abName, remoteKey);//记录是否下载了最新key
                    item.quequeHandler(item.resId);
                    www.Dispose();
                    www = null;
                }
            }
            yield return StartCoroutine(_LoadRes());
        }
        IEnumerator _LoadResManifest(ResLoader item)
        {
            string f = item.abName + ".manifest";
            fileUrl = webUrl + f + "?v=" + random; //接取服务器资源
            localfile = (dataPath + f).Trim();

            string path;
            string localKey;
            LoaderInfo loaderInfo = assetBundleLoaderMap[f];
            string remoteKey = loaderInfo.key;

            if (!assetBundleLoaderMap.ContainsKey(f))
            {
                Debug.LogError("不存在资源:" + f);
                item.quequeHandler("");
                yield break;
            }

            bool canUpdate = !File.Exists(localfile);// 是否需要更新
            path = Path.GetDirectoryName(localfile);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!canUpdate) //检查是否更新
            {
                localKey = PlayerPrefs.GetString(f);
                canUpdate = !remoteKey.Equals(localKey);
                if (canUpdate)
                    File.Delete(localfile);
            }

            if (canUpdate)
            { //更新或新增文件
                WWW www = new WWW(fileUrl);
                while (!www.isDone)
                {
                    item.CallProgress(www.progress * 0.1f + 0.70f);
                    yield return 1;
                }
                if (www.error != null)
                {
                    item.quequeHandler("");
                    www.Dispose();
                    www = null;
                    yield break;
                }
                if (www.isDone)
                {
                    yield return 1;
                    File.WriteAllBytes(localfile, www.bytes);
                    PlayerPrefs.SetString(f, remoteKey);
                    www.Dispose();
                    www = null;
                    yield break;
                }
            }
            yield return 0;
        }
        /// <summary>是否已经下载过的资源到本地  </summary>
        public bool IsLoaded(string abName, string remoteKey)
        {
            string key = PlayerPrefs.GetString(abName, null);
            if (key == remoteKey)
                return true;
            return false;
        }
        /// <summary>记录动态加载的资源  </summary>
        public void CacheAssetBundleLoaderData(string abName, string key)
        {
            if (abName != null && key != null)
            {
                // Trace.L("记录动态加载的资源: "+abName);
                if (assetBundleLoaderMap.ContainsKey(abName))
                {
                    //Trace.W("已经加载过的资源: "+abName);
                    assetBundleLoaderMap[abName].key = key;
                }
                else
                {
                    assetBundleLoaderMap.Add(abName, new LoaderInfo(abName, key));
                }
            }
        }

        /// <summary> 资源单元加载器  </summary>
        class ResLoader
        {
            internal string abName = null; //assetbundleName assetbundle资源名
            internal string resId = null; //场景资源id

            internal Action<string> quequeHandler = null;//记录加载完成时的队列回调(不是直接调用callback)
            //C#

            internal Action<string> finishCallback = null;
            internal Action<float> progressCallback = null;
            /// <summary> 资源单元加载器
            /// resId：资源的根目录  
            /// abRoot：unity中 对应打包Packager 打包的资源根目录位置
            /// Action： 加载中的结束及进度回调
            /// </summary>
            internal ResLoader(string resId, string abRoot, Action<string> finish, Action<float> progress, string extName = AppConst.ExtName)
            {
                this.resId = resId;
                this.abName = abRoot + resId + extName;
                this.finishCallback = finish;
                this.progressCallback = progress;
            }

            //Lua
            internal LuaInterface.LuaFunction finishLuaCallback = null;
            internal LuaInterface.LuaFunction progressLuaCallback = null;
            /// <summary> 资源单元加载器
            /// resId：资源的根目录  
            /// abRoot：unity中 对应打包Packager 打包的资源根目录位置
            /// LuaFunction： 加载中的结束及进度回调
            /// </summary>
            internal ResLoader(string resId, string abRoot, LuaInterface.LuaFunction finish, LuaInterface.LuaFunction progress, string extName = AppConst.ExtName)
            {
                this.resId = resId;
                this.abName = abRoot + resId + extName;
                this.finishLuaCallback = finish;
                this.progressLuaCallback = progress;
            }

            internal void CallFinish(string sceneId)
            {
                if (finishCallback != null)
                {
                    finishCallback(sceneId);
                    finishCallback = null;
                    progressCallback = null;
                }
                else if (finishLuaCallback != null)
                {
                    finishLuaCallback.Call(sceneId);
                    finishLuaCallback.Dispose();
                    finishLuaCallback = null;
                }
                if (progressLuaCallback != null)
                {
                    progressLuaCallback.Dispose();
                    progressLuaCallback = null;
                }
                quequeHandler = null;
            }
            internal void CallProgress(float v)
            {
                if (progressCallback != null)
                    progressCallback(v);

                if (progressLuaCallback != null)
                    progressLuaCallback.Call(v);
            }
        }

        /// <summary> 加载资源单元信息  </summary>
        class LoaderInfo
        {
            internal string abName;
            internal string key;
            /// <summary> 加载资源单元信息  </summary>
            internal LoaderInfo(string name, string local)
            {
                this.abName = name;
                this.key = local;
            }
        }
    }
}