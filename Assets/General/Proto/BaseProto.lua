require "Assets.Core.Util.Util"

--[[
    LollipopGo开源游戏服务器主协议
    https://github.com/Golangltd/LollipopGo
]]

BaseProto =
{
    "GameDataProto",   -- GameDataProto == 1 游戏代理主协议 proxy server 协议
    "GameDataDBProto", -- GameDataDBProto == 2 游戏DB的主协议 db server 协议
	"GameNetProto",    -- GameNetProto == 3 游戏网络主协议 心跳等
	"GErrorProto",     -- GErrorProto == 4 游戏错误处理 统一错误处理
	"GGateWayProto",   -- GGateWayProto == 5 游戏网关协议，暂时废弃
    "GGameHallProto",  -- GGameHallProto == 6 游戏大厅主协议
    "GGameLoginProto", -- GGameLoginProto == 7 游戏登录主协议
    "GGameGMProto",    -- GGameGMProto == 8 游戏GM主协议
    "GGamePayProto",   -- GGamePayProto == 9 支付协议
    "GGameBattleProto",-- GGameBattleProto == 10 战斗主协议
    "GGameConfigProto",-- GGameConfigProto == 11 配置数据主协议
    "GGameCenterProto",-- GGameCenterProto == 12 中心服主协议
}


-- 获取主协议的枚举值, 从数字1开始
function GetBaseProtoNum()
    ProtoIDEunm = Util.CreatEnumTable(BaseProto,0);
    return ProtoIDEunm
end

return BaseProto