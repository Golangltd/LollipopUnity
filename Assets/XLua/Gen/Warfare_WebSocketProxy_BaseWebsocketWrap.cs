#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class WarfareWebSocketProxyBaseWebsocketWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(Warfare.WebSocketProxy.BaseWebsocket);
			Utils.BeginObjectRegister(type, L, translator, 0, 6, 2, 2);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "BaseWebsocketOpen", _m_BaseWebsocketOpen);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Recv", _m_Recv);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Connected", _m_Connected);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ConnectedState", _m_ConnectedState);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Websocket_SendMessage", _m_Websocket_SendMessage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CloseConnect", _m_CloseConnect);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "onWebsocket_Open", _g_get_onWebsocket_Open);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "onWebsocket_Close", _g_get_onWebsocket_Close);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "onWebsocket_Open", _s_set_onWebsocket_Open);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "onWebsocket_Close", _s_set_onWebsocket_Close);
            
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 3 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && (LuaAPI.lua_isnil(L, 3) || LuaAPI.lua_type(L, 3) == LuaTypes.LUA_TSTRING))
				{
					string __url_Port = LuaAPI.lua_tostring(L, 2);
					string __data = LuaAPI.lua_tostring(L, 3);
					
					var gen_ret = new Warfare.WebSocketProxy.BaseWebsocket(__url_Port, __data);
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to Warfare.WebSocketProxy.BaseWebsocket constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_BaseWebsocketOpen(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Warfare.WebSocketProxy.BaseWebsocket gen_to_be_invoked = (Warfare.WebSocketProxy.BaseWebsocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.BaseWebsocketOpen(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Recv(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Warfare.WebSocketProxy.BaseWebsocket gen_to_be_invoked = (Warfare.WebSocketProxy.BaseWebsocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.Recv(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Connected(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Warfare.WebSocketProxy.BaseWebsocket gen_to_be_invoked = (Warfare.WebSocketProxy.BaseWebsocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.Connected(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ConnectedState(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Warfare.WebSocketProxy.BaseWebsocket gen_to_be_invoked = (Warfare.WebSocketProxy.BaseWebsocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.ConnectedState(  );
                        translator.Push(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Websocket_SendMessage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Warfare.WebSocketProxy.BaseWebsocket gen_to_be_invoked = (Warfare.WebSocketProxy.BaseWebsocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _msg = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.Websocket_SendMessage( _msg );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CloseConnect(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                Warfare.WebSocketProxy.BaseWebsocket gen_to_be_invoked = (Warfare.WebSocketProxy.BaseWebsocket)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.CloseConnect(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onWebsocket_Open(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Warfare.WebSocketProxy.BaseWebsocket gen_to_be_invoked = (Warfare.WebSocketProxy.BaseWebsocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onWebsocket_Open);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_onWebsocket_Close(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Warfare.WebSocketProxy.BaseWebsocket gen_to_be_invoked = (Warfare.WebSocketProxy.BaseWebsocket)translator.FastGetCSObj(L, 1);
                translator.Push(L, gen_to_be_invoked.onWebsocket_Close);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onWebsocket_Open(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Warfare.WebSocketProxy.BaseWebsocket gen_to_be_invoked = (Warfare.WebSocketProxy.BaseWebsocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onWebsocket_Open = translator.GetDelegate<Warfare.WebSocketProxy.BaseWebsocket.OnWebsocketTigger>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_onWebsocket_Close(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                Warfare.WebSocketProxy.BaseWebsocket gen_to_be_invoked = (Warfare.WebSocketProxy.BaseWebsocket)translator.FastGetCSObj(L, 1);
                gen_to_be_invoked.onWebsocket_Close = translator.GetDelegate<Warfare.WebSocketProxy.BaseWebsocket.OnWebsocketTigger>(L, 2);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
