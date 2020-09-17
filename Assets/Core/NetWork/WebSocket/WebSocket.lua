
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
	data.ServerID = md5.sumhexa()
	data.Data = jsonTable
end


function OnMessage()
end

function Open()
end

function Error()
end

function OnClose()
end

function NewWebsocket()
end
