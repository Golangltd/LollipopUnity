
require("md5")

--[[
   网络接口操作

]]

WebSocket={}

-- 发送函数
function WebSocket.Send(protoName,jsonTable,fun)
	local data = {}
	data.Protocol = 1
	data.Protocol2 = 1
	data.ServerID = md5.sumhexa("1")
	data.Data = jsonTable
end

-- 处理消息函数
function WebSocket.OnMessage()

end

-- 创建连接函数 
function WebSocket.Open()

end

-- 错误处理函数 回调
function WebSocket.Error()
	
end

-- 网络关闭函数
function WebSocket.OnClose()
end

return WebSocket
