using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;

public class LollipopUnityX: MonoBehaviour
{
    LuaEnv luaenv = new LuaEnv();
    void Start()
    {
        luaenv.AddLoader(CustomLoader);
        // 加载数据lua  数据
        luaenv.DoString("require 'LollipopUnity'");
        GetVersion();
    }

    #region 0. 获取版本号
    void GetVersion()
    {
        string strversion = luaenv.Global.Get<string>("version");
        Debug.Log("LollipopUnity " + strversion);
    }
    #endregion

    #region 1. 加载lua路径
    private byte[] CustomLoader(ref string filepath)
    {
        string str = Application.dataPath + "/Scenes/" + filepath.Replace('.', '/') + ".lua";
        Debug.Log("LollipopUnity str " + str);
        if (File.Exists(str))
            return File.ReadAllBytes(str);
        return null;
    }
    #endregion


    private void OnDestroy()
    {
        luaenv.Dispose();
        luaenv = null;
    }

}
