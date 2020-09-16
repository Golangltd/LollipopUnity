using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System.IO;  //引入这个空间是为了输入路径用

public class LollipopUnity : MonoBehaviour
{
    //创建Lua虚拟机
    LuaEnv luaenv = new LuaEnv();
    // Start is called before the first frame update
    void Start()
    {
        luaenv.AddLoader(CustomLoader);
        luaenv.DoString("require 'LollipopUnity'");

        GetGlobalData();

    
    }

    #region 1.C#访问Lua中的全局基本数据类型
    void GetGlobalData()
    {
        double a = luaenv.Global.Get<double>("a");
        print("a = " + a);//a = 100.1
        string str = luaenv.Global.Get<string>("str");
        print("str = " + str);//str = hahaha
        bool isDie = luaenv.Global.Get<bool>("isDie");
        print("isDie = " + isDie);//isDie = False

        a = 10;
        a = luaenv.Global.Get<double>("a");
        print(a);//a = 100.1  说明在这种直接获取的情况下不能改变Lua脚本中的变量值，只能获取
    }
    #endregion


    private byte[] CustomLoader(ref string filepath)      //这个是在网上查的，可以记下来作为一个API来使用，这个函数返回一个自定义路径
    {
        //重定向  可以在AB包里去读取 也可以指定 本地的一个路径
        string str = Application.dataPath + "/Scenes/" + filepath + ".lua";
        Debug.Log(str);
        if (File.Exists(str))
            return File.ReadAllBytes(str);
        return null;
    }
}
