
require("md5")

--WebSocket网络接口，数据收发等
--与C#交互逻辑

WebSocket={}

-- 发送函数
-- 发送给代理服务器
function WebSocket.Send(protoName,jsonTable,fun)
	local data = {}
	data.Protocol = 1
	data.Protocol2 = 1
	data.ServerID = md5.sumhexa("1")
	data.Data = jsonTable
end

--C#接收到数据派发
--相应的lua处理
--可以实现派发机制
function WebSocket.OnMessage()
end

--直接调用C#的
function WebSocket.Open()
end

--直接C#提示,无需转lua调用
function WebSocket.Error()
end

--直接C#关闭,无需转lua调用
function WebSocket.OnClose()
end

return WebSocket
