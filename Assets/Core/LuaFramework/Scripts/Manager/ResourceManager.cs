using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using LuaInterface;
// //using FairyGUI;
using UObject = UnityEngine.Object;

public class AssetBundleInfo {
    public AssetBundle m_AssetBundle;
    public int m_ReferencedCount;

    public AssetBundleInfo(AssetBundle assetBundle) {
        m_AssetBundle = assetBundle;
        m_ReferencedCount = 0;
    }
}

namespace LuaFramework {

    public class ResourceManager : Manager {
        string m_BaseDownloadingURL = "";
        string[] m_AllManifest = null;
        AssetBundleManifest m_AssetBundleManifest = null;
        Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();
        Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();
        Dictionary<string, List<LoadAssetRequest>> m_LoadRequests = new Dictionary<string, List<LoadAssetRequest>>();

        Dictionary<string, bool> m_UIABLoadedList = new Dictionary<string, bool>();
        Dictionary<string, AssetBundle> m_SceneABLoadedList = new Dictionary<string, AssetBundle>();

        class LoadAssetRequest {
            public Type assetType;
            public string[] assetNames;
            public LuaFunction luaFunc;
            public Action<UObject[]> sharpFunc;
            
        }

        public static GameObject LoadUI(string path)
        {
            UnityEngine.Object go = Resources.Load(path);
            GameObject pageObject = GameObject.Instantiate(go) as GameObject;
            return pageObject;

        }

        /// 获得指定名字的UI Ab(位于本地数据)
        public bool AddUIAB(string uiMoudleName)
        {
            string uiABName = Util.DataPath + "ui/" + uiMoudleName.ToLower();
            if (m_UIABLoadedList.ContainsKey(uiABName)) return true;
			AssetBundle ab_desc = null;
			AssetBundle ab_res = null;

			ab_desc = AssetBundle.LoadFromFile(uiABName + AppConst.uiDataSubfix);
			ab_res = AssetBundle.LoadFromFile(uiABName + AppConst.uiResSubfix);

            if (ab_desc != null && ab_res != null)
            {
                Debug.Log("FairyGUI AddPackage name " + uiABName);
                // FairyGUI.UIPackage.AddPackage(ab_desc, ab_res);
                m_UIABLoadedList.Add(uiABName, true);
                return true;
            }
        
            
            return false;
        }

        public static void LoadSpriteImage(Image image, string imagePath)
        {
            Sprite sp = Resources.Load(imagePath, typeof(Sprite)) as Sprite;
            image.sprite = sp;
        }

        /// 获得指定名字的场景 Ab(位于本地数据)
        public AssetBundle GetSceneAB(string id)
        {
            string path = Util.DataPath + "scene/" + id.ToLower() + AppConst.ExtName;
            AssetBundle ab;
            if (m_SceneABLoadedList.TryGetValue(path, out ab))
                return ab;
            ab = AssetBundle.LoadFromFile(path);
            m_SceneABLoadedList.Add(path, ab);
            return ab;
        }
        public void UnloadSceneAB(string id, bool unloadAllLoadedObjs=true)
        {
            string path = Util.DataPath + "scene/" + id.ToLower() + AppConst.ExtName;
            AssetBundle ab;
            if (m_SceneABLoadedList.TryGetValue(path, out ab))
            {
                ab.Unload(unloadAllLoadedObjs);
                m_SceneABLoadedList.Remove(path);
            }
            //Resources.UnloadUnusedAssets();
        }

        /// 获得指定位置Ab资源
        public AssetBundle GetResAB(string abName)
        {
            string path = Util.DataPath + abName.ToLower();
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            return ab;
        }

        // Load AssetBundleManifest.
        public void Initialize(string manifestName, Action initOK) {
            m_BaseDownloadingURL = Util.GetRelativePath();
            LoadAsset<AssetBundleManifest>(manifestName, new string[] { "AssetBundleManifest" }, delegate(UObject[] objs) {
                if (objs.Length > 0) {
                    m_AssetBundleManifest = objs[0] as AssetBundleManifest;
                    m_AllManifest = m_AssetBundleManifest.GetAllAssetBundles();
                }
                if (initOK != null) initOK();
            });
        }

        // public void LoadPrefab(string abName, string assetName, Action<UObject[]> func) {
        //     LoadAsset<GameObject>(abName, new string[] { assetName }, func);
        // }

        // public void LoadPrefab(string abName, string[] assetNames, Action<UObject[]> func) {
        //     LoadAsset<GameObject>(abName, assetNames, func);
        // }

        public void LoadPrefab(string abName, string[] assetNames, LuaFunction func) {
            LoadAsset<GameObject>(abName, assetNames, null, func);
        }
        public void LoadPrefab(string res, LuaFunction func)
        {
            StartCoroutine(AsyncLoadPrefab<GameObject>(res, func));
        }
        IEnumerator AsyncLoadPrefab<T>(string res, LuaFunction func) where T : UObject
        {
            ResourceRequest rr = Resources.LoadAsync<T>(res);
            yield return rr;
            if(rr.isDone)
            {
                func.Call(rr.asset);
                func.Dispose();
                func = null;
            }
        }
        IEnumerator AsyncLoadPrefab<T>(string res, Action<UObject> func) where T : UObject
        {
            ResourceRequest rr = Resources.LoadAsync<T>(res);
            yield return rr;
            if (rr.isDone)
            {
                func.Invoke(rr.asset);
            }
        }

        //public void LoadPrefab(string res, LuaFunction func)
        //{
        //    GameObject prefab = Resources.Load<GameObject>(res);
        //    if (func != null)
        //    {
        //        func.Call(prefab);
        //        func.Dispose();
        //        func = null;
        //    }
        //}

        string GetRealAssetPath(string abName) {
            if (abName.Equals(AppConst.AssetDir)) {
                return abName;
            }
            abName = abName.ToLower();
            if (!abName.EndsWith(AppConst.ExtName)) {
                abName += AppConst.ExtName;
            }
            if (abName.Contains("/")) {
                return abName;
            }
            //string[] paths = m_AssetBundleManifest.GetAllAssetBundles();  产生GC，需要缓存结果
            for (int i = 0; i < m_AllManifest.Length; i++) {
                int index = m_AllManifest[i].LastIndexOf('/');  
                string path = m_AllManifest[i].Remove(0, index + 1);    //字符串操作函数都会产生GC
                if (path.Equals(abName)) {
                    return m_AllManifest[i];
                }
            }
            Debug.LogError("GetRealAssetPath Error:>>" + abName);
            return null;
        }

        /// <summary>
        /// 载入素材
        /// </summary>
        void LoadAsset<T>(string abName, string[] assetNames, Action<UObject[]> action = null, LuaFunction func = null) where T : UObject {
            abName = GetRealAssetPath(abName);

            LoadAssetRequest request = new LoadAssetRequest();
            request.assetType = typeof(T);
            request.assetNames = assetNames;
            request.luaFunc = func;
            request.sharpFunc = action;

            List<LoadAssetRequest> requests = null;
            if (!m_LoadRequests.TryGetValue(abName, out requests)) {
                requests = new List<LoadAssetRequest>();
                requests.Add(request);
                m_LoadRequests.Add(abName, requests);
                StartCoroutine(OnLoadAsset<T>(abName));
            } else {
                requests.Add(request);
            }
        }

        IEnumerator OnLoadAsset<T>(string abName) where T : UObject {
            AssetBundleInfo bundleInfo = GetLoadedAssetBundle(abName);
            if (bundleInfo == null) {
                yield return StartCoroutine(OnLoadAssetBundle(abName, typeof(T)));

                bundleInfo = GetLoadedAssetBundle(abName);
                if (bundleInfo == null) {
                    m_LoadRequests.Remove(abName);
                    Debug.LogError("OnLoadAsset--->>>" + abName);
                    yield break;
                }
            }

            List<LoadAssetRequest> list = null;
            if (!m_LoadRequests.TryGetValue(abName, out list)) {
                m_LoadRequests.Remove(abName);
                yield break;
            }
            for (int i = 0; i < list.Count; i++) {
                string[] assetNames = list[i].assetNames;
                List<UObject> result = new List<UObject>();

                AssetBundle ab = bundleInfo.m_AssetBundle;
                for (int j = 0; j < assetNames.Length; j++) {
                    string assetPath = assetNames[j];
                    AssetBundleRequest request = ab.LoadAssetAsync(assetPath, list[i].assetType);
                    yield return request;
                    result.Add(request.asset);

                    //T assetObj = ab.LoadAsset<T>(assetPath);
                    //result.Add(assetObj);
                }
                if (list[i].sharpFunc != null) {
                    list[i].sharpFunc(result.ToArray());
                    list[i].sharpFunc = null;
                }
                if (list[i].luaFunc != null) {
                    list[i].luaFunc.Call((object)result.ToArray());
                    list[i].luaFunc.Dispose();
                    list[i].luaFunc = null;
                }
                bundleInfo.m_ReferencedCount++;
            }
            m_LoadRequests.Remove(abName);
        }

        IEnumerator OnLoadAssetBundle(string abName, Type type) {
            string url = m_BaseDownloadingURL + abName;

            WWW download = null;
            if (type == typeof(AssetBundleManifest))
                download = new WWW(url);
            else {
                string[] dependencies = m_AssetBundleManifest.GetAllDependencies(abName);
                if (dependencies.Length > 0) {
                    m_Dependencies.Add(abName, dependencies);
                    for (int i = 0; i < dependencies.Length; i++) {
                        string depName = dependencies[i];
                        AssetBundleInfo bundleInfo = null;
                        if (m_LoadedAssetBundles.TryGetValue(depName, out bundleInfo)) {
                            bundleInfo.m_ReferencedCount++;
                        } else if (!m_LoadRequests.ContainsKey(depName)) {
                            yield return StartCoroutine(OnLoadAssetBundle(depName, type));
                        }
                    }
                }
                download = WWW.LoadFromCacheOrDownload(url, m_AssetBundleManifest.GetAssetBundleHash(abName), 0);
            }
            yield return download;

            AssetBundle assetObj = download.assetBundle;
            if (assetObj != null) {
                m_LoadedAssetBundles.Add(abName, new AssetBundleInfo(assetObj));
            }
        }

        AssetBundleInfo GetLoadedAssetBundle(string abName) {
            AssetBundleInfo bundle = null;
            m_LoadedAssetBundles.TryGetValue(abName, out bundle);
            if (bundle == null) return null;

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
                return bundle;

            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies) {
                AssetBundleInfo dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (dependentBundle == null) return null;
            }
            return bundle;
        }

        /// <summary>
        /// 此函数交给外部卸载专用，自己调整是否需要彻底清除AB
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="isThorough"></param>
        public void UnloadAssetBundle(string abName, bool isThorough = false) {
            abName = GetRealAssetPath(abName);
            //Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory before unloading " + abName);
            UnloadAssetBundleInternal(abName, isThorough);
            UnloadDependencies(abName, isThorough);
            //Debug.Log(m_LoadedAssetBundles.Count + " assetbundle(s) in memory after unloading " + abName);
        }

        void UnloadDependencies(string abName, bool isThorough) {
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(abName, out dependencies))
                return;

            //Loop dependencies.
            foreach (var dependency in dependencies) {
                UnloadAssetBundleInternal(dependency, isThorough);
            }
            m_Dependencies.Remove(abName);
        }

        void UnloadAssetBundleInternal(string abName, bool isThorough) {
            AssetBundleInfo bundle = GetLoadedAssetBundle(abName);
            if (bundle == null) return;
            if (--bundle.m_ReferencedCount <= 0) {
                if (m_LoadRequests.ContainsKey(abName)) {
                    return;     //如果当前AB处于Async Loading过程中，卸载会崩溃，只减去引用计数即可
                }
                bundle.m_AssetBundle.Unload(isThorough);
                m_LoadedAssetBundles.Remove(abName);
                //Debug.Log(abName + " has been unloaded successfully");
            }
        }

        //载入音效资源 
        public void LoadAudioClip(string assetName, Action<UObject> func)
        {
            //LoadAsset<AudioClip>(abName, new string[] { assetName }, func);
            StartCoroutine(AsyncLoadPrefab<AudioClip>(assetName, func));
        }
    }
}

