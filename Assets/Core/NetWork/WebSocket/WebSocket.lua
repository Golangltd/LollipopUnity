
require("md5")

--WebSocket����ӿڣ������շ���
--��C#�����߼�

WebSocket={}

-- ���ͺ���
-- ���͸����������
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
