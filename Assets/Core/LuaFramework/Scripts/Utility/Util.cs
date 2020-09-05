using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using LuaInterface;
using LuaFramework;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace LuaFramework { 
    public class Util {
        private static List<string> luaPaths = new List<string>();
        public static int Int(object o) {
            return Convert.ToInt32(o);
        }

        public static float Float(object o) {
            return (float)Math.Round(Convert.ToSingle(o), 2);
        }

        public static long Long(object o) {
            return Convert.ToInt64(o);
        }

        public static int Random(int min, int max) {
            return UnityEngine.Random.Range(min, max);
        }

        public static float Random(float min, float max) {
            return UnityEngine.Random.Range(min, max);
        }

        // 返回半径为1的圆内的一个随机点
        public static Vector2 RandomInsideUnitCircle() {
            return UnityEngine.Random.insideUnitCircle;
        }
        //返回半径为1的球体内的一个随机点。
        public static Vector3 RandomInsideUnitSphere() {
            return UnityEngine.Random.insideUnitSphere;
        }
        // 返回半径为1的球体在表面上的一个随机点
        public static Vector3 RandomOnUnitSphere() {
            return UnityEngine.Random.onUnitSphere;
        }
        // 返回一个随机旋转角度
        public static Quaternion RandomRotation() {
            return UnityEngine.Random.rotation;
        }
        //设置文本的缩放模式
        // public static void SetTextFileAutoSize(GTextField tf, int type)
        // {
        //     if (tf != null)
        //     tf.autoSize = (AutoSizeType)type;
        // }

        /// <summary>
        /// 缩放粒子
        /// </summary>
        /// <param name="gameObj">粒子节点</param>
        /// <param name="scale">缩放系数</param>
        public static void ScaleParticleSystem(GameObject gameObj, float scale)
        {
            var hasParticleObj = false;
            var particles = gameObj.GetComponentsInChildren<ParticleSystem>(true);
            //if (particles == null || particles.Length==0){
            //    gameObj.transform.localScale *= scale;
            //    return;
            //}
            var max = particles.Length;
            for (int idx = 0; idx < max; idx++){
                var particle = particles[idx];
                if (particle == null) continue;
                hasParticleObj = true;
                particle.startSize *= scale;
                particle.startSpeed *= scale;
                particle.startRotation *= scale;
                //particle.transform.localScale *= scale;
            } 
            if (hasParticleObj)
                gameObj.transform.localScale = new Vector3(scale, scale, 1);
        }

        /// <summary>
        /// 播放现有的粒子系统
        /// </summary>
        public static void PlayParticleSystem(GameObject gameObj)
        {
            var particles = gameObj.GetComponentsInChildren<ParticleSystem>(true);
            if (particles.Length == 0) return;
            var max = particles.Length;
            for (int idx = 0; idx < max; idx++){
                var particle = particles[idx];
                if (particle == null) continue;
                
                particle.Stop(true);
                particle.Play(true);
            }
        }
        /// <summary>
        /// 设置粒子系统速度
        /// </summary>
        public static void SetParticleSystemSpeed(GameObject gameObj, float speed)
        {
            var particles = gameObj.GetComponentsInChildren<ParticleSystem>(true);
            if (particles.Length == 0) return;
            var max = particles.Length;
            for (int idx = 0; idx < max; idx++)
            {
                var particle = particles[idx];
                if (particle == null) continue;
                if (speed != -1.0f) particle.startSpeed = speed;
            }
        }

        ///  设置是否active...
        public static void SetActive(GameObject obj, bool show)
        {
            if (show ^ obj.activeSelf)
                obj.SetActive(show);
        }

        public static void SetActive(Component obj, bool show)
        {
            if (show ^ obj.gameObject.activeSelf)
                obj.gameObject.SetActive(show);
        }

        /// <summary>
        ///  找骨骼(gameobject)节点..
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static GameObject GetBone(string name, GameObject parent)
        {
            if(parent == null) return null;
            bool b = false;
            if (!parent.activeSelf)
            {
                b = true;
                SetActive(parent, true);
            }
            Transform[] allTran = parent.GetComponentsInChildren<Transform>();
            if (b) SetActive(parent, false);//激活后再复原
            for (int i = 0; i < allTran.Length; i++)
            {
                Transform t = allTran[i];
                if (t != null && t.name == name)
                    return t.gameObject;
            }
            return null;
        }

        /// 绑定骨骼
        public static void BindBone(GameObject node, string boneName, GameObject targetRoot, Vector3 pos)
        {
            GameObject bone = GetBone(boneName, targetRoot);
            if (bone != null && node != null)
            {
                Transform transform = bone.transform;
                node.transform.parent = transform;
                if(pos != null)
                    node.transform.localPosition = pos;
                node.transform.localScale = Vector3.one;
                node.transform.rotation = transform.rotation;
            }
        }

        #region 设置位置 朝向 缩放

        public static void GetPosition(GameObject target, out float x, out float y, out float z)
        {

            x = target.transform.localPosition.x;
            y = target.transform.localPosition.y;
            z = target.transform.localPosition.z;
        }

        public static void GetPosition(Transform target, out float x, out float y, out float z)
        {

            x = target.localPosition.x;
            y = target.localPosition.y;
            z = target.localPosition.z;
        }

        public static void GetPositionY(Transform target, out float y)
        {

            y = target.localPosition.y;
        }

        public static void GetPositionY(GameObject target, out float y)
        {

            y = target.transform.localPosition.y;
        }

        public static void SetPosition(GameObject target, float x, float y, float z)
        {
            if (target == null) return;
            target.transform.position = new Vector3(x, y, z);
        }
        public static void SetLocalPosition(GameObject target, float x, float y, float z)
        {
            if (target == null) return;
            target.transform.localPosition = new Vector3(x, y, z);
        }
        public static void SetPosition(Transform target, float x, float y, float z)
        {
            if (target == null) return;
            target.position = new Vector3(x, y, z);
        }
        public static void SetLocalPosition(Transform target, float x, float y, float z)
        {
            if (target == null) return;
            target.localPosition = new Vector3(x, y, z);
        }
        public static void SetRot(GameObject target, float x, float y, float z)
        {
            if (target == null) return;
            target.transform.eulerAngles = new Vector3(x, y, z);
        }
        public static void SetLocalRot(GameObject target, float x, float y, float z)
        {
            if (target == null) return;
            target.transform.localEulerAngles = new Vector3(x, y, z);
        }
        public static void SetRot(Transform target, float x, float y, float z)
        {
            if (target == null) return;
            target.eulerAngles = new Vector3(x, y, z);
        }
        public static void SetLocalRot(Transform target, float x, float y, float z)
        {
            if (target == null) return;
            target.localEulerAngles = new Vector3(x, y, z);
        }
        
        #endregion

        /// 获取游戏对象的大小
        public static Vector3 GetObjectSize(GameObject go)  
        {
           Vector3 realSize = Vector3.zero;
           Renderer render = go.GetComponent<Renderer>();
           if (render == null) return realSize;
           Vector3 meshSize = render.bounds.size; // 模型网格的大小
           Vector3 scale = go.transform.parent.lossyScale;//go.transform.lossyScale;  // 放缩  
           realSize = new Vector3(meshSize.x * scale.x, meshSize.y * scale.y, meshSize.z * scale.z);// 游戏中的实际大小
           return realSize;  
        }

        public static string Uid(string uid) {
            int position = uid.LastIndexOf('_');
            return uid.Remove(0, position + 1);
        }

        public static long GetTime() {
            TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks);
            return (long)ts.TotalMilliseconds;
        }

        /// <summary>
        /// 搜索子物体组件-GameObject版
        /// </summary>
        public static T Get<T>(GameObject go, string subnode) where T : Component {
            if (go != null) {
			Transform sub = go.transform.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Transform版
        /// </summary>
        public static T Get<T>(Transform go, string subnode) where T : Component {
            if (go != null) {
			Transform sub = go.Find(subnode);
                if (sub != null) return sub.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 搜索子物体组件-Component版
        /// </summary>
        public static T Get<T>(Component go, string subnode) where T : Component {
		return go.transform.Find(subnode).GetComponent<T>();
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(GameObject go) where T : Component {
            if (go != null) {
                T[] ts = go.GetComponents<T>();
                for (int i = 0; i < ts.Length; i++) {
                    if (ts[i] != null) GameObject.Destroy(ts[i]);
                }
                return go.gameObject.AddComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// 添加组件
        /// </summary>
        public static T Add<T>(Transform go) where T : Component {
            return Add<T>(go.gameObject);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(GameObject go, string subnode) {
            return Child(go.transform, subnode);
        }

        /// <summary>
        /// 查找子对象
        /// </summary>
        public static GameObject Child(Transform go, string subnode) {
		Transform tran = go.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(GameObject go, string subnode) {
            return Peer(go.transform, subnode);
        }

        /// <summary>
        /// 取平级对象
        /// </summary>
        public static GameObject Peer(Transform go, string subnode) {
		Transform tran = go.parent.Find(subnode);
            if (tran == null) return null;
            return tran.gameObject;
        }
        //根据名字获取子节点
        public static Transform GetChild(Transform root,string bone)
        {
            if (root.name == bone)
            {
                return root;
            }
            foreach (Transform t in root)
            {
                Transform f = GetChild(t, bone);
                if (f != null)
                {
                    return f;
                }
            }
            return null;
        }

        /// <summary>
        /// 计算字符串的MD5值
        /// </summary>
        public static string md5(string source) {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++) {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            destString = destString.PadLeft(32, '0');
            return destString;
        }

        /// <summary>
        /// 计算文件的MD5值
        /// </summary>
        public static string md5file(string file) {
            try {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++) {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            } catch (Exception ex) {
                throw new Exception("md5file() fail, error:" + ex.Message);
            }
        }
        //预加载特效处理使用(lua中)
        public static void GetAllEffectName(LuaFunction callback)
        {
            try
            {
                string root = Util.DataPath + "effect/";
                string[] files = Directory.GetFiles(root);
                string result = "";
                for (int i = 0; i < files.Length; i++)
                {
                    string item = files[i];
                    string p = item.Replace('\\', '/');
                    if (p.EndsWith(".unity3d") || p.EndsWith(".prefab"))
                    {
                        string assetName = p.Replace(root, "");
                        assetName = assetName.Replace(AppConst.ExtName, "");
                        result += assetName + ",";
                    }
                }
                result = result.Substring(0, result.Length - 1);
                if(callback!=null)
                    callback.Call(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Util.GetAllEffectName error:" + ex.Message);
            }
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public static void ClearChild(Transform go) {
            if (go == null) return;
            for (int i = go.childCount - 1; i >= 0; i--) {
                GameObject.Destroy(go.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// 清理内存
        /// </summary>
        public static void ClearMemory() {
            GC.Collect(); Resources.UnloadUnusedAssets();
            LuaManager mgr = AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua);
            if (mgr != null) mgr.LuaGC();
        }

        /// <summary>
        /// 取得数据存放目录
        /// </summary>
        public static string DataPath {
            get {
                string game = AppConst.AppName.ToLower();
                if (Application.isMobilePlatform) {
                    Debug.LogWarning("isMobilePlatform DataPath " + Application.persistentDataPath + "/" + game + "/");
                    return Application.persistentDataPath + "/" + game + "/";
                }
                if (AppConst.DebugMode) {
                    return Application.dataPath + "/" + AppConst.AssetDir + "/";
                }
                if (Application.platform == RuntimePlatform.OSXEditor) {
                    int i = Application.dataPath.LastIndexOf('/');
                    return Application.dataPath.Substring(0, i + 1) + game + "/";
                }
                Debug.LogWarning("DataPath " + "out/" + game + "/");
                return "out/" + game + "/";
            }
        }

    public static string GetDataPath(string folder)
    {
        folder = folder.ToLower();
        if (Application.isMobilePlatform)
        {
            Debug.LogWarning("isMobilePlatform GetDataPath " + Application.persistentDataPath + "/" + folder + "/");
            return Application.persistentDataPath + "/" + folder + "/";
        }
        Debug.LogWarning("GetDataPath " + "out/" + folder + "/");
        return "out/" + folder + "/";
    }

        public static string GetRelativePath() {
            if (Application.isEditor)
                return "file://" + System.Environment.CurrentDirectory.Replace("\\", "/") + "/Assets/" + AppConst.AssetDir + "/";
            else if (Application.isMobilePlatform || Application.isConsolePlatform)
                return "file:///" + DataPath;
            else // For standalone player.
                return "file://" + Application.streamingAssetsPath + "/";
        }

        /// <summary>
        /// 取得行文本
        /// </summary>
        public static string GetFileText(string path) {
            return File.ReadAllText(path);
        }

        /// <summary>
        /// 网络可用
        /// </summary>
        public static bool NetAvailable {
            get {
                return Application.internetReachability != NetworkReachability.NotReachable;
            }
        }

        /// <summary>
        /// 是否是无线
        /// </summary>
        public static bool IsWifi {
            get {
                return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
            }
        }

        /// <summary>
        /// 应用程序内容路径
        /// </summary>
        public static string AppContentPath() {
            string path = string.Empty;
            switch (Application.platform) {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/";
                break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "/Raw/";
                break;
                default:
                    path = Application.dataPath + "/" + AppConst.AssetDir + "/";
                break;
            }
            return path;
        }

        public static void Log(string str) {
            Debug.Log(str);           
        }

        public static void LogWarning(string str) {
            Debug.LogWarning(str);
        }

        public static void LogError(string str) {
            Debug.LogError(str);
        }

        /// <summary>
        /// 防止初学者不按步骤来操作
        /// </summary>
        /// <returns></returns>
        public static int CheckRuntimeFile() {
            if (!Application.isEditor) return 0;
            string streamDir = Application.dataPath + "/StreamingAssets/";
            if (!Directory.Exists(streamDir)) {
                return -1;
            } else {
                string[] files = Directory.GetFiles(streamDir);
                if (files.Length == 0) return -1;

                if (!File.Exists(streamDir + "files.txt")) {
                    return -1;
                }
            }
            string sourceDir = AppConst.FrameworkRoot + "/ToLua/Source/Generate/";
            if (!Directory.Exists(sourceDir)) {
                return -2;
            } else {
                string[] files = Directory.GetFiles(sourceDir);
                if (files.Length == 0) return -2;
            }
            return 0;
        }

        /// <summary>
        /// 执行Lua方法
        /// </summary>
        public static object[] CallMethod(string module, string func, params object[] args) {
            LuaManager luaMgr = AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua);
            if (luaMgr == null) return null;
            return luaMgr.CallFunction(module + "." + func, args);
        }

        /// <summary>
        /// 检查运行环境
        /// </summary>
        public static bool CheckEnvironment() {
#if UNITY_EDITOR
            int resultId = Util.CheckRuntimeFile();
            if (resultId == -1) {
                Debug.LogError("没有找到框架所需要的资源，单击Game菜单下Build xxx Resource生成！！");
                EditorApplication.isPlaying = false;
                return false;
            } else if (resultId == -2) {
                Debug.LogError("没有找到Wrap脚本缓存，单击Lua菜单下Gen Lua Wrap Files生成脚本！！");
                EditorApplication.isPlaying = false;
                return false;
            }
#endif
            return true;
        }

        /**版本号 t 可选为 0，1，2, 3 分别表示获得 v1, v2, v3, v4 版号*/
        public static string GetVersion(string ver, int t = 0)
        {
            return VersionUtil.GetVersion(ver, t);
        }

        public static LuaByteBuffer GetVoiceData()
        {
            byte[] bs = new byte[Util.Random(10000, 40000)];
            for (int i = 0; i < bs.Length; i++)
            {
                bs[i] = (byte)Util.Random(-128, 127);
            }
            Debug.Log("发送起始字节："+bs[0]+"| 最后个字节："+bs[bs.Length-1]+"|长度 "+bs.Length);
            LuaByteBuffer bytes = new LuaByteBuffer(bs);
            return bytes;
        }
        public static void SetVoiceData(byte[] bs)
        {
            Debug.Log("收到起始字节："+bs[0]+"| 最后个字节："+bs[bs.Length-1]+"|长度 "+bs.Length);
        }

        //base64转换
        public static string BaseToString(string str)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(str.Substring(1, str.Length - 2));
            decode = System.Text.Encoding.UTF8.GetString(bytes);
            return decode;
        }

        //MD5加密
        public static string GetMD5(string str)
        {
            MD5 mD5 = new MD5CryptoServiceProvider();
            byte[] formData = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] targetData = mD5.ComputeHash(formData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }



        //切换场景
        public static void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }

        private static void asdas(Scene arg0, LoadSceneMode arg1)
        {
            throw new NotImplementedException();
        }

        public static void SetParent(GameObject tra, GameObject parent)
        {
            tra.transform.SetParent(parent.transform);
            tra.transform.localPosition = Vector3.zero;
            tra.transform.localScale = new Vector3(1, 1, 1);
        }

        public static void SetParent(Transform tra, Transform parent)
        {
            tra.SetParent(parent);
            tra.localPosition = Vector3.zero;
            tra.localScale = new Vector3(1, 1, 1);
        }

        public static void SetUIAnchor(GameObject obj)
        {
            SetUIAnchor(obj.transform);
        }

        public static void SetUIAnchor(Transform tra)
        {
            RectTransform rectTransform = tra.GetComponent<RectTransform>();
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
        }


        public static void SetActive(Transform tra, bool active)
        {
            tra.gameObject.SetActive(active);
        }

        public static GameObject Clone(GameObject obj)
        {
            GameObject cloneObj = GameObject.Instantiate(obj);
            cloneObj.transform.parent = obj.transform.parent;
            cloneObj.transform.localPosition = Vector3.zero;
            cloneObj.transform.localScale = Vector3.one;
            return cloneObj;
        }

        public static GameObject Clone(Transform tra)
        {
            GameObject cloneObj = Clone(tra.gameObject);
            return cloneObj;
        }

        public static void DestroyObj(GameObject obj)
        {
            GameObject.DestroyImmediate(obj);
        }

        public static string GetCurSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        public static void AddPositionY(GameObject obj,float y)
        {
            obj.transform.localPosition = new Vector3 (0,obj.transform.localPosition.y + y,0);
        }

        public static void SetPositionZzero(GameObject obj)
        {
            obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y , 0);
        }
    }
}