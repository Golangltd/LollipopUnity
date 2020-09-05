﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class WebSocketManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(WebSocketManager), typeof(MonoSingleton<WebSocketManager>));
		L.RegFunction("GetInst", GetInst);
		L.RegFunction("InitSocket", InitSocket);
		L.RegFunction("Open", Open);
		L.RegFunction("SendMessage", SendMessage);
		L.RegFunction("BeClose", BeClose);
		L.RegFunction("HttpRequest", HttpRequest);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("progress", get_progress, set_progress);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetInst(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			WebSocketManager obj = (WebSocketManager)ToLua.CheckObject<WebSocketManager>(L, 1);
			WebSocketManager o = obj.GetInst();
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int InitSocket(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			WebSocketManager obj = (WebSocketManager)ToLua.CheckObject<WebSocketManager>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			LuaFunction arg1 = ToLua.CheckLuaFunction(L, 3);
			obj.InitSocket(arg0, arg1);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Open(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			WebSocketManager obj = (WebSocketManager)ToLua.CheckObject<WebSocketManager>(L, 1);
			LuaFunction arg0 = ToLua.CheckLuaFunction(L, 2);
			obj.Open(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SendMessage(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2)
			{
				WebSocketManager obj = (WebSocketManager)ToLua.CheckObject<WebSocketManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				obj.SendMessage(arg0);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<UnityEngine.SendMessageOptions>(L, 3))
			{
				WebSocketManager obj = (WebSocketManager)ToLua.CheckObject<WebSocketManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				UnityEngine.SendMessageOptions arg1 = (UnityEngine.SendMessageOptions)ToLua.ToObject(L, 3);
				obj.SendMessage(arg0, arg1);
				return 0;
			}
			else if (count == 3 && TypeChecker.CheckTypes<object>(L, 3))
			{
				WebSocketManager obj = (WebSocketManager)ToLua.CheckObject<WebSocketManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				obj.SendMessage(arg0, arg1);
				return 0;
			}
			else if (count == 4)
			{
				WebSocketManager obj = (WebSocketManager)ToLua.CheckObject<WebSocketManager>(L, 1);
				string arg0 = ToLua.CheckString(L, 2);
				object arg1 = ToLua.ToVarObject(L, 3);
				UnityEngine.SendMessageOptions arg2 = (UnityEngine.SendMessageOptions)ToLua.CheckObject(L, 4, typeof(UnityEngine.SendMessageOptions));
				obj.SendMessage(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: WebSocketManager.SendMessage");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int BeClose(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			WebSocketManager obj = (WebSocketManager)ToLua.CheckObject<WebSocketManager>(L, 1);
			obj.BeClose();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int HttpRequest(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 5);
			WebSocketManager obj = (WebSocketManager)ToLua.CheckObject<WebSocketManager>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			string arg1 = ToLua.CheckString(L, 3);
			object[] arg2 = ToLua.CheckObjectArray(L, 4);
			LuaFunction arg3 = ToLua.CheckLuaFunction(L, 5);
			obj.HttpRequest(arg0, arg1, arg2, arg3);
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
	static int get_progress(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			WebSocketManager obj = (WebSocketManager)o;
			float ret = obj.progress;
			LuaDLL.lua_pushnumber(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index progress on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_progress(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			WebSocketManager obj = (WebSocketManager)o;
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.progress = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index progress on a nil value");
		}
	}
}
