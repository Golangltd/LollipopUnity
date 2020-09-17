
require("Core.Util.md5")
require("../Util/Util")
-- 作者: 彬哥
-- E-mail: 1528884535@qq.com
-- 创建时间：2020年9月16日


--[[
	ProxyServerId   // ProxyServerId == 1
	GameServerId    // GameServerId   == 2
	BattleServerId  // BattleServerId == 3
	GMServerId      // GMServerId == 4
	DBServerId      // DBServerId == 5
	CenterServerId  // CenterServerId == 6
--]]

ServerInfo = {}

ServerId =
{
    "ProxyServerId",
    "GameServerId",
	"BattleServerId",
	"GMServerId",
	"DBServerId",
	"CenterServerId",
}


-- 生成ServerId的MD5数据
function ServerInfo.CreatServerIdMD5(serverId)
    print("CreatServerIdMD5:",serverId);
	ServerIDEunm = CreatEnumTable(ServerId,1);
	if (serverId == ServerIDEunm.ProxyServerId) then
		return md5.sumhexa(ServerIDEunm.ProxyServerId);
	elseif (serverId == ServerIDEunm.GameServerId) then
		return md5.sumhexa(ServerIDEunm.GameServerId);
	elseif (serverId == ServerIDEunm.BattleServerId) then
		return md5.sumhexa(ServerIDEunm.BattleServerId);
	elseif (serverId == ServerIDEunm.GMServerId) then
		return md5.sumhexa(ServerIDEunm.GMServerId);
	elseif (serverId == ServerIDEunm.DBServerId) then
		return md5.sumhexa(ServerIDEunm.DBServerId);
	elseif (serverId == ServerIDEunm.CenterServerId) then
		return md5.sumhexa(ServerIDEunm.CenterServerId);
	end
	return 0;
end


function ServerInfo.Testbinge()
	print("Testbinge:",ServerInfo.CreatServerIdMD5(1));
end

return ServerInfo




