using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp()]
public class FatherClass
{
    public string BaseField = "BaseField";
    private string _BaseProperty = "BaseProperty";

    public string BaseProperty {
        get { return _BaseProperty; }
        set { _BaseProperty = value; }
    }
    public FatherClass()
    {
        Debug.Log("BaseClass 构造方法");
    }
    public void BaseFunc()
    {
        Debug.Log("BaseFunc");
    }
    public static string BaseStaticField = "BaseStaticField";
    public static void BaseStaticFunc()
    {
        Debug.Log("BaseStaticFunc");
    }
}
[LuaCallCSharp]
public class SonClass:FatherClass
{
    public string SonField = "SonField";
    public static string SonStaticField = "SonStaticField";

    public void SonPrint()
    {
        Debug.Log("SonClass");
    }

    public static void OnStaticPrint()
    {
        Debug.Log("OnStaticPrint");
    }
}
