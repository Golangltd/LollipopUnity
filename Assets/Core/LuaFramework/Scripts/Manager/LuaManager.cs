using UnityEngine;
using System.Collections;
using LuaInterface;

namespace LuaFramework
{
    public class LuaManager : Manager
    {
        private LuaState lua;
        private LuaLoader loader;
        private LuaLooper loop = null;
        private static LuaManager _instance;
        public static LuaManager Instance
        {
            get { return _instance; }
        }
        // Use this for initialization
        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
            loader = new LuaLoader();
            lua = new LuaState();
            this.OpenLibs();
            lua.LuaSetTop(0);

            LuaBinder.Bind(lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, this);
            this.InitStart();
        }

        void OnDestroy()
        {
            this.Close();
        }

        public void InitStart()
        {
            InitLuaPath();
            InitLuaBundle();
            this.lua.Start();    //启动LUAVM
            this.StartMain();
            this.StartLooper();
        }

        void StartLooper()
        {
            loop = gameObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson()
        {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        void StartMain()
        {
            lua.DoFile("Main.lua");

            LuaFunction main = lua.GetFunction("Main");
            main.Call();
            main.Dispose();
            main = null;
        }

        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        void OpenLibs()
        {
            lua.OpenLibs(LuaDLL.luaopen_pb);
            // lua.OpenLibs(LuaDLL.luaopen_sproto_core);
            // lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_bit);
            lua.OpenLibs(LuaDLL.luaopen_socket_core);

            this.OpenCJson();
        }

        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        void InitLuaPath()
        {
            if (AppConst.DebugMode)
            {
#if UNITY_STANDALONE
                lua.AddSearchPath(Application.streamingAssetsPath + "/Lua");
#endif
                string rootPath = AppConst.FrameworkRoot;
                lua.AddSearchPath(rootPath + "/Lua");
                lua.AddSearchPath(rootPath + "/ToLua/Lua");
            }
            else
            {
                lua.AddSearchPath(Util.DataPath + "lua");
            }
        }

        /// <summary>
        /// 初始化LuaBundle
        /// </summary>
        void InitLuaBundle()
        {
            if (loader.beZip)
            {
                loader.AddBundle("lua/lua.unity3d");
                loader.AddBundle("lua/lua_math.unity3d");
                loader.AddBundle("lua/lua_system.unity3d");
                loader.AddBundle("lua/lua_system_reflection.unity3d");
                loader.AddBundle("lua/lua_unityengine.unity3d");
                loader.AddBundle("lua/lua_common.unity3d");
                loader.AddBundle("lua/lua_logic.unity3d");
                loader.AddBundle("lua/lua_view.unity3d");
                loader.AddBundle("lua/lua_controller.unity3d");
                loader.AddBundle("lua/lua_misc.unity3d");

                loader.AddBundle("lua/lua_protobuf.unity3d");
                loader.AddBundle("lua/lua_3rd_cjson.unity3d");
                loader.AddBundle("lua/lua_3rd_luabitop.unity3d");
                loader.AddBundle("lua/lua_3rd_pbc.unity3d");
                loader.AddBundle("lua/lua_3rd_pblua.unity3d");
                loader.AddBundle("lua/lua_3rd_sproto.unity3d");
            }
        }

        public void DoFile(string filename)
        {
            lua.DoFile(filename);
        }

        // Update is called once per frame
        public object[] CallFunction(string funcName, params object[] args)
        {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null)
            {
                return func.LazyCall(args);
            }
            return null;
        }

        public object[] LuaDoFile(string scriptName)
        {
            if (lua != null)
            {
                return lua.DoFile(NTGResourceController.LuaPath(scriptName));
            }
            return null;
        }

        public object[] LuaCall(string module, string name, params object[] args)
        {
            if (lua != null)
            {
                string funcName = module + "." + name;
                var func = lua.GetFunction(funcName);
                var result = func.Call(args);
                func.Dispose();
                return result;
            }
            return null;
        }

        public LuaFunction LuaGetFunction(string module, string func)
        {
            if (lua != null)
            {
                string funcName = module + "." + func;
                return lua.GetFunction(funcName);
            }
            return null;
        }

        public LuaTable LuaGetTable(string name)
        {
            if (lua != null)
            {
                return lua.GetTable(name);
            }
            return null;
        }

        public void RemoveTableCache(string fullPath)
        {
            if (lua != null)
            {
                lua.RemoveTableCache(fullPath);
            }
        }

        public void LuaReleaseTable(LuaTable table)
        {
            if (lua != null)
            {
                lua.CollectRef(table.GetReference(), table.name);
            }
        }

        public void LuaGC()
        {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close()
        {
            loop.Destroy();
            loop = null;

            lua.Dispose();
            lua = null;
            loader = null;
        }
    }
}