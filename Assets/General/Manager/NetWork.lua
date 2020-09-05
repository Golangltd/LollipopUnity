NetWork={}
NetWork.OpenID = nil
NetWork.Token = nil
local MsgCallFun = {}
local proto = require("config.proto")

-- INIT               = iota //  INIT == 0
GameDataProto = 1     --游戏代理的主协议       proxy server 协议
GameDataDBProto = 2   --游戏的DB的主协议       db server 协议
GameNetProto = 3      --游戏的NET主协议        心跳等
GErrorProto = 4       --游戏的错误处理         统一错误处理
GGateWayProto = 5     --网关协议              暂时不用
GGameHallProto = 6    --游戏主场景协议         game server 协议
GGameLoginProto = 7   --登录服务器协议         登录服务器主协议
GGameGMProto = 8      --游戏GM管理系统
GGamePayProto = 9     --游戏支付系统
GGameBattleProto = 10 --游戏战斗系统
GGameConfigProto = 11 --获取游戏配置  其他服务器从game游戏主逻辑服务器获取数据


--子協議
C2GSUserPlayProto2 = 26 --游戏客户端点击【Play】开始战斗，战斗服务器回复战斗数据了

S2CActionBeginProto2 = 91 --服务发送给客户端的卡牌行动的消息
S2CSettleMsgProto2 = 94 --
PS2BSUserInfo = 95 --玩家开始战斗

local ServerId =  {
    ProxyServerId = 1  ,--代理服务器
    GameServerId   = 2 ,--游戏主逻辑服务器
    BattleServerId = 3 ,--战斗服务器
    GMServerId = 4     ,--GM服务器
    DBServerId = 5     ,--DB代理服务器
    CenterServerId = 6 ,--中心服服务器
} 

function NetWork.SetToken( token )
	NetWork.Token = token
end

--注册
function NetWork.RegisterAccount( fun )
	local data = {}
    data.Protocol = 1
    data.Protocol2 = 7
    NetWork.RegisterProto( "Proxy2C_ConnDataProto",function ( serverData )
            NetWork.OpenID = serverData.OpenID
            if fun then
                fun()
            end
        end )
    WebSocketManager.Instance:SendMessage(Json:encode(data))
end

function NetWork.ReceiveMsg( jsonStr )
	local serverData = Json:decode(jsonStr)
    local Protocol = serverData.Protocol
    local Protocol2 = serverData.Protocol2
    if proto[Protocol] and proto[Protocol][Protocol2] then
        local fun = MsgCallFun[proto[Protocol][Protocol2]]
    	if fun then
    		fun(serverData)
    	end
    end
end

--注册协议消息返回函数
function NetWork.RegisterProto( protoName,fun,isSend )
    if MsgCallFun[protoName] == nil then
        MsgCallFun[protoName] = fun
    elseif not isSend then
        logError("重复注册消息"..protoName)
    end
end

function NetWork.Send(protoName,jsonTable,fun)
    local data = {}
    data.Protocol = 1
    data.Protocol2 = 1
    data.ServerID = Util.GetMD5(tostring(ServerId.GameServerId)) 
    jsonTable.Token = NetWork.Token
    print("NetWork.OpenID:",NetWork.OpenID)
    jsonTable.OpenId = NetWork.OpenID
    data.Data = jsonTable
    NetWork.RegisterProto( protoName,fun,true )
	WebSocketManager.Instance:SendMessage(Json:encode(data))
end


--连接服务器
function NetWork.ConnectServer( serverIp,Call )
	WebSocketManager.Instance:InitSocket(serverIp,Call)
    WebSocketManager.Instance:Open(NetWork.ReceiveMsg)
end

--连接游戏服
function NetWork.ConnectGamServer( fun ) 
    NetWork.Send("GS2CUserLoginProto2",{Protocol = 6,Protocol2 = 1},fun)
end