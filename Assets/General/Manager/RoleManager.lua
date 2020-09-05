RoleManager={}
RoleManager.inited = false
local attrs = {}
local cards = {}

-- RoleUid    int64         // 玩家唯一Id
-- RoleName   string        // 玩家的名字
-- RoleAvatar int           // 玩家的头像
-- RoleLev    int           // 玩家等级
-- RoleSex    int           // 玩家性别 0男，1女
-- Coin       int64         // 金币
-- Diamond    int64         // 砖石
-- CardList   []*CardSt     // 角色拥有的卡牌
-- LatestArea string        // 上一次的最新登录的区   区的url：ip+port
-- ItemList   []*ItemSt     // 角色身上的道具，包括装备(放在itemmanager里面)
-- ChannelId  int           // 渠道Id
-- ServerList []*ServerList // 整个游戏的所有区列表，从上线开始  1-30  29
function RoleManager.Init( _attrs )
	cards = {}
	attrs = {}
	local PlayerSt = _attrs.PlayerSt
	logTable(PlayerSt,"初始化玩家数据")
	for name,value in pairs(PlayerSt) do
		if name == "ItemList" then
    		ItemManager.InitData( value )
    	elseif name == "CardList" then
    		for i,v in ipairs(value) do
    			cards[v.CardID] = v
    		end
		else
			attrs[name] = value
		end
	end
	logTable(cards,"cards")
    RoleManager.inited = true
end

--获取属性
function RoleManager.GetAttr( attrName,default )
	return attrs[attrName] or default or "nil"
end

--获取卡牌信息
function RoleManager.GetCardInfo( itemid )
	return cards[itemid] or {}
end