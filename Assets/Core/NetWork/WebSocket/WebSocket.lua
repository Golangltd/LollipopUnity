
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
	data.ServerID = md5.sumhexa("1")
	data.Data = jsonTable
end

--C#���յ������ɷ�
--��Ӧ��lua����
--����ʵ���ɷ�����
function WebSocket.OnMessage()
end

--ֱ�ӵ���C#��
function WebSocket.Open()
end

--ֱ��C#��ʾ,����תlua����
function WebSocket.Error()
end

--ֱ��C#�ر�,����תlua����
function WebSocket.OnClose()
end

return WebSocket
