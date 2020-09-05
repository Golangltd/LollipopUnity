RoleBagManager={}
local roleCamps = nil 
local roleData = nil
local roleDataDic = nil

local function Sort( a,b )
	return a < b
end

--获取角色的所有阵营类型
function RoleBagManager.GetRoleCamps(  )
	if not roleCamps then
		roleCamps = {}
		local character = require("config.character")
		local camp = nil
		for k,v in pairs(character) do
			camp = v.camp
			if not roleCamps[camp] then
				roleCamps[camp] = camp
			end
		end
		--加入全部分类
		table.sort( roleCamps,Sort )
		table.insert(roleCamps,0,-1)
	end
	return roleCamps
end

function RoleBagManager.GetData( _camp )
	if not roleData then
		roleDataDic = {}
		roleData = ItemManager.GetData( FuncId.roleBag )
		local config = nil
		local camp = nil
		for i,v in ipairs(roleData) do
			config = v.config
			camp = config.camp
			roleDataDic[camp] = roleDataDic[camp] or {}
			table.insert(roleDataDic[camp],v)
		end
	end
	if _camp ~= -1 then
		return roleDataDic[_camp] or {}
	else--全部
		return roleData or {}
	end
end


