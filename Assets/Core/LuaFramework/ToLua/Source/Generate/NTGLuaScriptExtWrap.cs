﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class NTGLuaScriptExtWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(NTGLuaScriptExt), typeof(NTGLuaScript));
		L.RegFunction("LateUpdate", LateUpdate);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("eventUpdate", get_eventUpdate, set_eventUpdate);
		L.RegVar("eventFixedUpdate", get_eventFixedUpdate, set_eventFixedUpdate);
		L.RegVar("eventLateUpdate", get_eventLateUpdate, set_eventLateUpdate);
		L.RegVar("eventOnTriggerStay", get_eventOnTriggerStay, set_eventOnTriggerStay);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LateUpdate(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			NTGLuaScriptExt obj = (NTGLuaScriptExt)ToLua.CheckObject<NTGLuaScriptExt>(L, 1);
			obj.LateUpdate();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_eventUpdate(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NTGLuaScriptExt obj = (NTGLuaScriptExt)o;
			bool ret = obj.eventUpdate;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index eventUpdate on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_eventFixedUpdate(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NTGLuaScriptExt obj = (NTGLuaScriptExt)o;
			bool ret = obj.eventFixedUpdate;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index eventFixedUpdate on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_eventLateUpdate(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NTGLuaScriptExt obj = (NTGLuaScriptExt)o;
			bool ret = obj.eventLateUpdate;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index eventLateUpdate on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_eventOnTriggerStay(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NTGLuaScriptExt obj = (NTGLuaScriptExt)o;
			bool ret = obj.eventOnTriggerStay;
			LuaDLL.lua_pushboolean(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index eventOnTriggerStay on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_eventUpdate(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NTGLuaScriptExt obj = (NTGLuaScriptExt)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.eventUpdate = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index eventUpdate on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_eventFixedUpdate(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NTGLuaScriptExt obj = (NTGLuaScriptExt)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.eventFixedUpdate = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index eventFixedUpdate on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_eventLateUpdate(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NTGLuaScriptExt obj = (NTGLuaScriptExt)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.eventLateUpdate = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index eventLateUpdate on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_eventOnTriggerStay(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			NTGLuaScriptExt obj = (NTGLuaScriptExt)o;
			bool arg0 = LuaDLL.luaL_checkboolean(L, 2);
			obj.eventOnTriggerStay = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index eventOnTriggerStay on a nil value");
		}
	}
}

