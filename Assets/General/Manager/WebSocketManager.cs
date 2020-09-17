using UnityEngine;
using WebSocket4Net;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using LitJson;
namespace Warfare.WebSocketProxy
{
	public class BaseWebsocket
	{

		public delegate void OnWebsocketTigger();
		/// <summary>
		/// websocket Open 事件
		/// </summary>
		public OnWebsocketTigger onWebsocket_Open;

		public OnWebsocketTigger onWebsocket_Close;

		private Queue<string> m_RecvQueue = new Queue<string>();
		/// <summary>
		/// websocket的实例
		/// </summary>
		protected WebSocket ws;
		/// <summary>
		/// 服务器地址 和 协议
		/// </summary>
		protected string networkAddress = "";

		protected bool IsConnect = false;

		/// <summary>
		/// 接收数据队列同步锁。
		/// </summary>
		private readonly object m_RecvQueueLocker = new object();

		public BaseWebsocket(string _url_Port, string _data)
		{
			networkAddress = string.Format("ws://{0}/RuiLiDe?data={1}", _url_Port, _data);
			Connect();
#if UNITY_EDITOR
			Debug.Log(networkAddress);
#endif

		}
		/// <summary>
		/// 连接后端
		/// </summary>
		void Connect()
		{
			ws = new WebSocket(networkAddress);

			ws.Opened += new EventHandler(Websocket_Opened);
			ws.Closed += new EventHandler(Websocket_Closed);
			ws.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(Websocket_Error);

			ws.EnableAutoSendPing = false;
			ws.MessageReceived += new EventHandler<MessageReceivedEventArgs>(Websocket_MessageReceived);
		}

		public void BaseWebsocketOpen()
		{
			try
			{
#if UNITY_EDITOR
				Debug.Log(ws.Handshaked);
				Debug.Log(ws.LastActiveTime);
#endif

				ws.Open();
			}
#if UNITY_EDITOR
			catch(Exception e )
#else
			catch
#endif
			{
#if UNITY_EDITOR
				Debug.LogError("ReConnectWebSocket "+e.Message);
#endif
			}
		}

		/// <summary>
		///提供给上层的接受函数， 仅从接受队列获取一个数据包
		///空队列返回 null 
		/// </summary>
		public string[] Recv()
		{
			if (m_RecvQueue.Count > 0)
			{

				string[] res;
				lock (m_RecvQueueLocker)
				{
					res = m_RecvQueue.ToArray();
					m_RecvQueue.Clear();
				}

				return res;

			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 判断连接是否存在
		/// </summary>
		public bool Connected()
		{
			if (ws == null)
				IsConnect = false;
			else
			{
				switch (ws.State)
				{
					case WebSocketState.None:
						IsConnect = false;
						break;
					case WebSocketState.Closing:
						IsConnect = false;
						break;
					case WebSocketState.Connecting:
						IsConnect = true;
						break;
					case WebSocketState.Closed:
						IsConnect = false;
						break;
					case WebSocketState.Open:
						IsConnect = true;
						break;
				}
			}
			return IsConnect;
		}


		public WebSocketState ConnectedState()
		{
			if (ws == null)
				return WebSocketState.None;
			else
				return ws.State;
		}
		//	int protocl_Int =0;
		//	int protocl2_Int =0;
		private void Websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
		{
			string _eMessage = e.Message;

			string decodeMessage;//= UnicodeConverter.base64decode(_eMessage);
			string utf8Message =""; //= UnicodeConverter.utf8to16(decodeMessage);

			lock (m_RecvQueueLocker)
			{
				m_RecvQueue.Enqueue(utf8Message);
			}

		}

		public bool Websocket_SendMessage(string msg)
		{
			try
			{

				if (ws != null && WebSocketState.Open == ws.State)
				{
					ws.Send(msg);

					return true;
				}
			}
			catch
			{

			}

			return false;
		}
		///一旦服务端响应WebSocket连接请求，就会触发open事件
		private void Websocket_Opened(object sender, EventArgs e)
		{
#if UNITY_EDITOR
			Debug.Log("Websocket_Opened");
#endif

			if (onWebsocket_Open != null)
				onWebsocket_Open();
		}

		private void Websocket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
		{
#if UNITY_EDITOR
			Debug.Log("连接出现错误 "+e.Exception.Message+" "+sender.GetType ());

#endif

		}


		private void Websocket_Closed(object sender, EventArgs e)
		{
#if UNITY_EDITOR
			Debug.Log("Websocket_Closed 关闭 "+e.GetType().ToString() +" "+e.GetType().FullName);

#endif
		}

		public void CloseConnect()
		{
			try
			{
#if UNITY_EDITOR
				if(ws !=null)
					Debug.Log("yyWebSocket.State "+ws.State);
				else
					Debug.Log("yyWebSocket.State null ");
#endif

				lock (m_RecvQueueLocker)
					m_RecvQueue.Clear();
				if (ws != null && WebSocketState.Closed != ws.State)
				{
					ws.Close();
				}
			}
			catch
			{
#if UNITY_EDITOR
				Debug.LogError("断开失败 CloseConnect");
#endif
			}

			System.GC.Collect();
		}

	}
}
