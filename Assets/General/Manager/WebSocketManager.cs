/*using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using SuperSocket.ClientEngine;
using UnityEngine;
using UnityEngine.Networking;
using WebSocket4Net;

public class WebSocketManager: MonoSingleton<WebSocketManager>
{
    LuaFunction OpenCallBack;
    LuaFunction ReceiveMsg;
    WebSocket webSocket;
    bool openState = false;
    WebSocketManager _inst;
    Queue<string> receiveMsg = new Queue<string>();

    public WebSocketManager GetInst(){
        if(_inst != null){
            return _inst;
        }
        _inst = new WebSocketManager();
        return _inst;
    }

    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);
        _inst = this;
    }

    public void InitSocket(string url, LuaFunction _openCallBack)
    {
        if (webSocket == null)
        {
            OpenCallBack = _openCallBack;
            webSocket = new WebSocket(url);
            webSocket.Opened += Opened;
            webSocket.MessageReceived += MessageReceived;
            webSocket.Closed += Closed;
            webSocket.Error += Error;
        }
    }

    private void Update()
    {
        if (openState == true)
        {
            OpenCallBack.Call();
            openState = false;
        }

        if (receiveMsg.Count > 0)
        {
            ReceiveMsg.Call(receiveMsg.Dequeue());
        }
    }

    public void Open(LuaFunction _receiveMsg)
    {
        if (webSocket.State == WebSocketState.Open)
        {
            return;
        }
        webSocket.Open();
        ReceiveMsg = _receiveMsg;
    }

    public void SendMessage(string str)
    {
        if (webSocket.State == WebSocketState.Open)
        {
            Debug.Log("发送消息:" + str);
            webSocket.Send(str);
        }
    }

    private void Error(object sender, ErrorEventArgs e)
    {
        Debug.LogWarning("WebSocketError " + e.ToString());
    }

    private void Closed(object sender, EventArgs e)
    {
        Debug.LogWarning("链接关闭 " + e.ToString());
    }

    private void MessageReceived(object sender, MessageReceivedEventArgs e)
    {
        if ( webSocket.State == WebSocketState.Open)
        {
            string decode = LuaFramework.Util.BaseToString(e.Message);
            Debug.Log("收到消息："+ decode);
            receiveMsg.Enqueue(decode);
        }
    }

    private void Opened(object sender, EventArgs e)
    {
        Debug.Log("已连接" + webSocket.State);
        openState = true;
    }

    private void OnDestroy()
    {
        BeClose();
    }

    public void BeClose()
    {
        if (webSocket != null)
        {
            webSocket.Close();
            webSocket = null;
        }
    }

}*/