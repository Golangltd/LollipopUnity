NetWork={}
NetWork.OpenID = nil
NetWork.Token = nil
local MsgCallFun = {}
--local proto = require("config.proto")

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
GS2CUserPlayRetProto2 = 38 --C2GSUserPlayProto2 == 38 游戏客户端点击【Play】开始战斗，战斗服务器回复战斗数据了,返回战斗结算数据
S2CActionBeginProto2 = 91 --服务发送给客户端的卡牌行动的消息
C2SAttackMsgProto2 = 92 --客户端发送给服务器的玩家攻击消息
S2CActionEndProto2 = 93 --每次出手攻击结束
S2CSettleMsgProto2 = 94 --服务器发送给客户端的结算消息
PS2BSUserInfo = 95 --玩家开始战斗
S2CskillEffectMsgProto2 = 96 --服务器发送给客户端的技能效果消息

local IDForName = {}
--1 登录 服务器 主协议Id
IDForName[1] ={}
IDForName[1][1] = "C2GSUserLoginProto2"--客户端发送协议
IDForName[1][2] = "Proxy2C_SendDataProto"--游戏客户端选择章节
IDForName[1][3] = "C2GSUserChooseRoundProto2"--游戏客户端选择关卡
IDForName[1][4] = "C2GSUserStartBattleProto2"--游戏客户端开始战斗
IDForName[1][5] = "C2Proxy_ConnDataProto"
IDForName[1][8] = "Proxy2C_ConnDataProto"--服务器返回注册消息
--6 Game 服务器 主协议Id

--服务器
IDForName[6] ={}
--玩家
IDForName[6][2] = "GS2CUserLoginProto2"--服务器返回给游戏客户端玩家数据
--章节
IDForName[6][6] = "GS2CUserChooseMapProto2"--服务器返回给游戏客户端选择章节的数据结构
--关卡
IDForName[6][10] = "GS2CUserChooseRoundProto2"--服务器返回游戏客户端选择关卡的数据结构

--结算
IDForName[6][38] ="C2GSUserPlayProto2"

--创建角色
IDForName[6][22] = "GS2CUserRegisterProto2"--创建角色
--背包增加
IDForName[6][23] = "GS2CUserOpAddItem"--背包增加
--背包删除
IDForName[6][24] = "GS2CUserOpDelItem"--背包删除
--背包改变
IDForName[6][25] = "GS2CUserOpChangeItem"--背包改变
--随机名字
IDForName[6][28] = "GS2CUserGetNameProto2"--请求随机名字
--挂机
IDForName[6][16] = "GS2CUserOffLineBattleProto2"--服务器返回挂机协议
--挂机收益
IDForName[6][35] = "GS2CUserClickOnOffLineProto2"--挂机获取收益

-- --战斗 --测试战斗的回调
IDForName[6][91] = "S2CBattleProtoCallback"--服务发送给客户端的卡牌行动的消息
IDForName[6][93] = "S2CBattleProtoCallback"--每次出手攻击结束
IDForName[6][94] = "S2CBattleProtoCallback"--服务器发送给客户端的结算消息
IDForName[6][96] = "S2CBattleProtoCallback"--服务器发送给客户端的技能效果消息

--获取公共数据类

--获取关卡的数据
IDForName[6][10008] = "C2GS_GetFunctionDataProto2"

local _c2sData = {}
_c2sData.UserA = {}
_c2sData.UserB = {}

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
    NetWork.RegisterProto("Proxy2C_ConnDataProto",function (serverData)
            NetWork.OpenID = serverData.OpenID
            if fun then
                fun()
            end
    end)
    WebSocketManager.Instance:SendMessage(Json:encode(data))
end

function NetWork.ReceiveMsg( jsonStr )
    print("ReceiveMsg", jsonStr)
	local serverData = Json:decode(jsonStr)
    local Protocol = serverData.Protocol
    local Protocol2 = serverData.Protocol2
    if IDForName[Protocol] and IDForName[Protocol][Protocol2] then
        local fun = MsgCallFun[IDForName[Protocol][Protocol2]]
    	if fun then
    		fun(serverData)
    	end
    end
end

--注册协议消息返回函数
function NetWork.RegisterProto(protoName, fun, isSend)
    print("RegisterProto", protoName)
    if MsgCallFun[protoName] == nil and fun ~= nil then
        MsgCallFun[protoName] = fun
    elseif not isSend then
        print("重复注册消息"..protoName)
    end
end

function NetWork.Send(protoName,jsonTable,fun)
    local data = {}
    data.Protocol = 1
    data.Protocol2 = 1
    data.ServerID = Util.GetMD5(tostring(ServerId.GameServerId)) 
    jsonTable.Token = NetWork.Token
    jsonTable.OpenId = NetWork.OpenID
    data.Data = jsonTable
    NetWork.RegisterProto( protoName,fun,true )
	WebSocketManager.Instance:SendMessage(Json:encode(data))
end

function NetWork.SendBattle(protoName,jsonTable,fun)
    local data = {}
    data.Protocol = 1
    data.Protocol2 = 1
    data.ServerID = Util.GetMD5(tostring(ServerId.BattleServerId)) 
    jsonTable.Token = NetWork.Token
    jsonTable.OpenId = NetWork.OpenID
    data.Data = jsonTable
    NetWork.RegisterProto(protoName, fun, true)
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

-- NetWork.ConnectGamServer(function (serverData)
-- 	NetWork.Send("PS2BSUserInfo", _c2sData, function (serverData)
-- 	end)
-- end)
NetWork.RoomID = 0
NetWork.ActionBegin = {}
NetWork.SkillEffectMsg = {}
NetWork.ActionEnd   = {}
function NetWork.S2CBattleProtoCallback(serverData)
    dump(serverData)
    if serverData.Protocol2 == S2CActionBeginProto2 then --服务发送给客户端的卡牌行动的消息协议
        -- //每个子回合开始发送的消息(行动开始)
        -- type ActionBegin struct {
        --     Protocol    int               // 主协议
        --     Protocol2   int               // 子协议
        --     RoomID      UUID              //战斗房间ID
        -- 	ParentRound int               // 当前父回合
        -- 	ChildRound  int               // 当前子回合
        -- 	PlayList    []*PlayCard       // 战斗卡牌列表
        -- 	AttackCard  *AttackCard       // 攻击卡牌
        -- }
        -- //战斗的卡牌
        -- type PlayCard struct {
        -- 	IsMy   bool // 是否是我方
        -- 	RoleID int  // 角色ID
        -- }
        -- //攻击卡牌
        -- type AttackCard struct {
        -- 	OpenId   string // 用户ID
        -- 	Position int    // 卡牌位置
        -- }
        NetWork.ActionBegin = serverData
        NetWork.RoomID = NetWork.ActionBegin.RoomID
    elseif serverData.Protocol2 == C2SAttackMsgProto2 then --客户端发送给服务器的玩家攻击消息

    elseif serverData.Protocol2 == S2CActionEndProto2 then --每次出手攻击结束
        -- type ActionEnd struct {
        --     Protocol       int              // 主协议
        --     Protocol2      int              // 子协议
        --     RoomID     UUID    //战斗房间ID
        --     ACard          *ActionResult    // 攻击卡牌
        --     BeAttackCards  []*ActionResult  // 被攻击的卡牌
        -- }
        -- //每次卡牌行动完后发送回的结果消息
        -- type ActionResult struct {
        --     OpenId    string     // 用户唯一索引
        --     Position  int        // 卡牌站位
        --     CommonMsg *CommonMsg // 根据行动后，变化的一些属性
        -- }
        -- type CommonMsg struct {
        --     IsHit     bool       // 是否被击中
        --     IsCrit    bool       // 是否暴击
        --     HitResult *HitResult // 击中结果
        -- }
        -- //击中结果
        -- type HitResult struct {
        --     ReduceBlood  int // 减去的血量值
        --     RestoreBlood int // 恢复血量值
        --     ReduceSP     int // 减去的蓝量值
        --     RestoreSP    int // 恢复蓝量值
        -- }        
        NetWork.ActionEnd = serverData
    elseif serverData.Protocol2 == S2CSettleMsgProto2 then --服务器发送给客户端的结算消息
        -- type SettleMsg struct {
        --     Protocol      int             // 主协议
        --     Protocol2     int             // 子协议
        --     RoomID     UUID    //战斗房间ID
        --     Result        map[string]bool // true就为胜利者 false为失败者
        -- }
        NetWork.SettleMsg = serverData
    elseif serverData.Protocol2 == S2CskillEffectMsgProto2 then--服务器发送给客户端的技能效果消息
        -- type SkillEffectMsg struct {
        --     Protocol    int               // 主协议
        --     Protocol2   int               // 子协议
        --     RoomID      UUID              //战斗房间ID
        --     OtherEffects []*EffectMsg // 其他特殊效果触发列表
        --     NumEffects   []*EffectMsg // 数值效果触发列表
        -- }
        -- type EffectMsg struct {
        --     EffectType int           // 效果类型
        --     EffectId   int           // 特殊效果ID
        --     Adder      *Caster // 施放者
        --     BeAdder    *Caster // 被施放者
        --     Rounds     int           // 持续回合数
        --     NumUpdate  map[int]int   // 数值作用(攻击力,生命值，蓝量值等,根据属性ID来进行赋值)
        -- }
        -- type Caster struct{
        --     UserID string //用户ID
        --     Position int //站位
        -- }
        NetWork.SkillEffectMsg = serverData
    else
        dump(serverData, "not wanted proto")
    end
    Event:Call(NetworkEvent.RecvBattleData, true)
end

function NetWork.RegisterBattleProto()
    NetWork.RegisterProto("S2CBattleProtoCallback", NetWork.S2CBattleProtoCallback)
end

-- type UserInfo struct {
-- 	 string
-- 	Level  int
-- 	Cards  map[int]*CardInfo // 卡牌信息
-- }
-- type CardInfo struct {
-- 	CardID   uint64
-- 	Level    int
-- 	RoleID   int
-- 	Position int
-- 	Skills   []int
-- }
function NetWork.C2SSendCampData()
	_c2sData.Protocol = GGameBattleProto
	_c2sData.Protocol2 = PS2BSUserInfo
	_c2sData.IsPvP = false
	_c2sData.UserA = {}
	_c2sData.UserB = {}

	_c2sData.UserA.OpenId = NetWork.OpenID
	_c2sData.UserA.Level = 1
	_c2sData.UserA.Cards = {}
	NetWork.InitUserInfo(_c2sData.UserA, false)

	_c2sData.UserB.OpenId = "yyyyyyyyyyyy"
	_c2sData.UserB.Level = 1
	_c2sData.UserB.Cards = {}

	NetWork.InitUserInfo(_c2sData.UserB, true)

	-- NetWork.ConnectServer("ws://"..battleServerUrl.."/BaBaLiuLiu?data={ID:1}",Connected)
	-- NetWork.SendBattle("PS2BSUserInfo", _c2sData, function (serverData)
	-- 	print("啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊啊")
	-- 	dump(serverData)
	-- end)
	NetWork.SendBattle("PS2BSUserInfo", _c2sData, nil)
end

function NetWork.C2SAttackMsgProto2(BeAttackID, Position, SkillID)
    local AttackMsg = {}
    AttackMsg.Protocol = GGameBattleProto
    AttackMsg.Protocol2= PS2BSUserInfo
    AttackMsg.RoomID   = NetWork.RoomID
    AttackMsg.BeAttackID= BeAttackID
    AttackMsg.Position = Position
    AttackMsg.SkillID  = SkillID
    NetWork.SendBattle("PS2BSUserInfo", _c2sData, nil)
end

function NetWork.InitUserInfo(userInfo, isEnemy)
    local campData
    if isEnemy then
        campData = DataManager.GetCampInfoEnemy()
    else
        campData = DataManager.GetCampInfo()
    end
	for i =1, #campData do
		local skills = DataManager.GetCampInfoSkillIds(campData[i])
		NetWork.InsertCardInfo(userInfo, campData[i], 1, campData[i] + i, i, skills)
	end
end

function NetWork.InsertCardInfo(userInfo, cardId, level, roleId, pos, skill)
	local cardInfo = {}
	cardInfo.CardID= cardId
	cardInfo.Level = level
	cardInfo.RoleID= roleId
	cardInfo.Position=pos
	cardInfo.Skills= skill
	dump(cardInfo,"cardInfo")
	table.insert(userInfo.Cards, cardInfo)
end
