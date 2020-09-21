
require("md5")

Util = {}

-- 创建枚举
function CreatEnumTable(tbl, index)
   -- assert(IsTable(tbl))
    local enumtbl = {}
    local enumindex = index or 0
    for i, v in ipairs(tbl) do
        enumtbl[v] = enumindex + i
    end
    return enumtbl
end

--[[
使用方法：

EnumTable =
{
    "ET1",
    "ET2",
}

EnumTable = CreatEnumTable(EnumTable)
print(EnumTable.ET1)
print(EnumTable.ET2)
输出为：
1
2

比如从10开始，只要把上面创建枚举表的语句换成下面这样就可以了：
EnumTable = CreatEnumTable(EnumTable, 10)

path=lfs.currentdir()
print(path)

package.path = package.path.."../Util/?.lua"
print(package.path)

--]]


ServerIDEunm =
{
    "ProxyServerId",
    "GameServerId",
	"BattleServerId",
	"GMServerId",
	"DBServerId",
	"CenterServerId",
}


-- 生成ServerId的MD5数据
function CreatServerIdMD5(serverId)
    print("CreatServerIdMD5:",serverId);

	ServerIDEunm = CreatEnumTable(ServerIDEunm,0);
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


--print(CreatServerIdMD5(1))

return Util
