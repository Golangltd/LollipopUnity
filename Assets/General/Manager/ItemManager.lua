ItemManager={}
require("config.FuncId")
local itemtype_proto = require("config.itemtype_proto")
local qualityImagePath = nil
local qualityColor = nil

function ItemManager.GetQualityImagePath(quality)
    if qualityImagePath ~= nil and qualityImagePath[quality] then
        return qualityImagePath[quality]
    else
        qualityImagePath = {}
        if quality == 1 then
            qualityImagePath[quality] = "UIRes/Common/cmm_bag_bg_01"
        elseif quality == 2 then
            qualityImagePath[quality] = "UIRes/Common/cmm_bag_bg_02"
        elseif quality == 3 then
            qualityImagePath[quality] = "UIRes/Common/cmm_bag_bg_03"
        elseif quality > 3 and quality < 7 then
            qualityImagePath[quality] = "UIRes/Common/cmm_bag_bg_02"
        elseif quality >= 7 and quality < 10 then
            qualityImagePath[quality] = "UIRes/Common/cmm_bag_bg_03"
        elseif quality >= 10 and quality < 13 then
            qualityImagePath[quality] = "UIRes/Common/cmm_bag_bg_04"
        elseif quality >= 13 and quality < 16 then
            qualityImagePath[quality] = "UIRes/Common/cmm_bag_bg_05"
        elseif quality >= 16 and quality < 19 then
            qualityImagePath[quality] = "UIRes/Common/cmm_bag_bg_06"
        end
    end
    return qualityImagePath[quality]
end

function ItemManager.GetNameColor( quality )
    if qualityColor ~= nil and qualityColor[quality] then
        return qualityColor[quality]
    else
        qualityColor = {}
        if quality == 1 then
            qualityColor[quality] = "<color=#99FCA1>%s</color>"
        elseif quality == 2 then
            qualityColor[quality] = "<color=#99FCA1>%s</color>"
        elseif quality == 3 then
            qualityColor[quality] = "<color=#99FCA1>%s</color>"
        elseif quality > 3 and quality < 7 then
            qualityColor[quality] = "<color=#76B3FF>%s</color>"
        elseif quality >= 7 and quality < 10 then
            qualityColor[quality] = "<color=#C574FF>%s</color>"
        elseif quality >= 10 and quality < 13 then
            qualityColor[quality] = "<color=#FFA44C>%s</color>"
        elseif quality >= 13 and quality < 16 then
            qualityColor[quality] = "<color=#FF5555>%s</color>"
        elseif quality >= 16 and quality < 19 then
            qualityColor[quality] = "<color=#FEE351>%s</color>"
        end
    end
    return qualityColor[quality]
end

--配置表
--*[itemtype] = config
ItemManager.Config = {}
ItemManager.FuncidForItemtype = {}

ItemManager.ItemType = {
    item = 1,--道具
    equip = 2,--装备
}

--数据
--[funcid] = {}
local bagData = {}--（超过最大堆叠数会重新生成格子数据）
--[funcid][itemtype][itemid] = {}
local bagDataDic = {}--（超过最大堆叠数会重新生成格子数据）
--[itemtype][itemid]
local bagDataTypeId = {}--（超过最大堆叠数不会会重新生成格子数据,也就是总数量）

--初始化
function ItemManager.InitData( list )
    ItemManager.InitConfig()
    ItemManager.InitProto()
    local _config = nil
    local config = nil
    local funcId = nil
    local gridNum = nil
    local remainder = nil
    logTable(list,"list")
    local ItemType = nil
    local temp = {}
    local ItemId = nil
    for i,itemList in ipairs(list) do
        funcId = itemList.FunctionId
        ItemType = ItemManager.FuncidForItemtype[funcId]
        _config = ItemManager.Config[ItemType]
        bagData[funcId] = bagData[funcId] or {}
        bagDataDic[funcId] = bagDataDic[funcId] or {}
        bagDataDic[funcId][ItemType] = bagDataDic[funcId][ItemType] or {}
        bagDataTypeId[ItemType] = bagDataTypeId[ItemType] or {}

        for j,itemdata in ipairs(itemList.ItemData) do
            ItemId = itemdata.ItemId
            config = _config[ItemId]
            log("ItemId:",ItemId,ItemType)
            if not config then
                return
            end
            if (config.num_max or 1) >= itemdata.ItemNum then
                itemdata.config = config
                itemdata.ItemType = ItemType
                bagDataDic[funcId][ItemType][ItemId] = itemdata
                table.insert(bagData[funcId],itemdata)
            else
                gridNum = math.floor(itemdata.ItemNum/(config.num_max or 1))
                for i=1,gridNum do
                    local temp = {ItemId = ItemId,ItemNum = (config.num_max or 1),config = config
                        ,ItemType = ItemType}
                    table.insert(bagData[funcId],temp)
                    bagDataDic[funcId][ItemType][ItemId] = temp
                end
                remainder = itemdata.ItemNum%(config.num_max or 1)
                if remainder > 0 then
                    local temp = {ItemId = ItemId,ItemNum = remainder,config = config
                        ,ItemType = ItemType}
                    table.insert(bagData[funcId],temp)
                    bagDataDic[funcId][ItemType][ItemId] = temp
                end
            end
            bagDataTypeId[ItemType][ItemId] = {ItemId = ItemId,ItemNum = itemdata.ItemNum,config = config
                        ,ItemType = ItemType}
        end
    end
    logTable(bagData,"初始化背包数据")
end

--获取功能对应的数据
function ItemManager.GetData( _funcid )
    if _funcid ~= -1 then
        return bagData[_funcid] or {}
    else
        logTable(bagData,"bagData")
        local temp = {}
        for k,v in pairs(bagData) do
            for j,k in ipairs(v) do
                table.insert(temp,k)
            end
        end
        logTable(temp,"temp")
        return temp
    end
end

--获取总的数据或者Icon需要显示的数据格式
function ItemManager.GetItemData( itemtype,itemid,itemnum )
    logTable(bagDataDic,"bagDataDic")
    if not funcid then
        for _funcid,_itemtype in pairs(ItemManager.FuncidForItemtype) do
            if itemType == _itemtype then
                funcid = _funcid
            end
        end
    end
    if bagDataTypeId[itemtype] and bagDataTypeId[itemtype][itemid] then
        return bagDataTypeId[itemtype][itemid]
    else
        return {ItemType = itemtype,ItemId = itemid,ItemNum = itemnum or 0,config = ItemManager.GetConfig( itemtype,itemid )}
    end
end

function ItemManager.InitConfig(  )
    if next(ItemManager.FuncidForItemtype) then return end
    for itemtype,v in ipairs(itemtype_proto) do
        ItemManager.FuncidForItemtype[v.funcid] = itemtype
        ItemManager.Config[itemtype] = require("config."..v.configname)
    end
end

--背包增加物品
local function GS2CUserOpAddItem( data )
    logTable(bagData,"bagData")
    logTable(bagDataDic,"bagDataDic")
    logTable(bagDataTypeId,"bagDataTypeId")
    local funcId = data.FunctionId
    local ItemType = ItemManager.FuncidForItemtype[funcId]
    local _config = ItemManager.Config[ItemType]
    local ItemId = data.ItemId
    bagData[funcId] = bagData[funcId] or {}
    bagDataDic[funcId] = bagDataDic[funcId] or {}
    bagDataDic[funcId][ItemType] = bagDataDic[funcId][ItemType] or {}
    bagDataTypeId[ItemType] = bagDataTypeId[ItemType] or {}
    local config = _config[ItemId]
    config.num_max = 2
    if (config.num_max or 1) >= data.ItemNum then
        data.config = config
        data.ItemType = ItemType
        bagDataDic[funcId][ItemType][ItemId] = data
        table.insert(bagData[funcId],data)
    else
        local gridNum = math.floor(data.ItemNum/(config.num_max or 1))
        for i=1,gridNum do
            local temp = {ItemId = ItemId,ItemNum = (config.num_max or 1),config = config
                ,ItemType = ItemType}
            table.insert(bagData[funcId],temp)
            bagDataDic[funcId][ItemType][ItemId] = temp
        end
        local remainder = data.ItemNum%(config.num_max or 1)
        if remainder > 0 then
            local temp = {ItemId = ItemId,ItemNum = remainder,config = config
                ,ItemType = ItemType}
            table.insert(bagData[funcId],temp)
            bagDataDic[funcId][ItemType][ItemId] = temp
        end
    end
    bagDataTypeId[ItemType][ItemId] = {ItemId = ItemId,ItemNum = data.ItemNum,config = config
                        ,ItemType = ItemType}
    logTable(bagData,"bagData")
    logTable(bagDataDic,"bagDataDic")
    logTable(bagDataTypeId,"bagDataTypeId")
end

--背包删除物品
local function GS2CUserOpDelItem( data )
    logTable(bagData,"bagData")
    logTable(bagDataDic,"bagDataDic")
    logTable(bagDataTypeId,"bagDataTypeId")
    local funcId = data.FunctionId
    local ItemType = ItemManager.FuncidForItemtype[funcId]
    local ItemId = data.ItemId
    if bagData[funcId] then
        for i,v in ipairs(bagData[funcId]) do
            if v.ItemId == ItemId and v.ItemType == ItemType then
                table.remove(bagData[funcId],i)
                break
            end
        end
    end
    if bagDataDic[funcId] and bagDataDic[funcId][ItemType] then
        bagDataDic[funcId][ItemType][ItemId] = nil
    end
    if bagDataTypeId[ItemType] then
        bagDataTypeId[ItemType][ItemId] = nil
    end
    logTable(bagData,"bagData")
    logTable(bagDataDic,"bagDataDic")
    logTable(bagDataTypeId,"bagDataTypeId")
end

--背包改变物品
local function GS2CUserOpChangeItem( data )
    logTable(bagData,"bagData")
    logTable(bagDataDic,"bagDataDic")
    logTable(bagDataTypeId,"bagDataTypeId")
    local funcId = data.FunctionId
    local ItemType = ItemManager.FuncidForItemtype[funcId]
    local ItemId = data.ItemId
    if bagData[funcId] then
        for i,v in ipairs(bagData[funcId]) do
            if v.ItemId == ItemId and v.ItemType == ItemType then
                v.ItemNum = v.ItemNum + data.ItemNum
                break
            end
        end
    end
    if bagDataTypeId[ItemType] and bagDataTypeId[ItemType][ItemId] then
        bagDataTypeId[ItemType][ItemId].ItemNum = bagDataTypeId[ItemType][ItemId].ItemNum + data.ItemNum
    end
    logTable(bagData,"bagData")
    logTable(bagDataDic,"bagDataDic")
    logTable(bagDataTypeId,"bagDataTypeId")
end

--获取道具对应的数量
function ItemManager.GetItemNum( ItemType,ItemId )
    if bagDataTypeId[ItemType] and bagDataTypeId[ItemType][ItemId] then
        return bagDataTypeId[ItemType][ItemId].ItemNum or 0
    end
    return 0
end

--初始化协议
function ItemManager.InitProto(  )
    NetWork.RegisterProto( "GS2CUserOpAddItem",GS2CUserOpAddItem )
    NetWork.RegisterProto( "GS2CUserOpDelItem",GS2CUserOpDelItem )
    NetWork.RegisterProto( "GS2CUserOpChangeItem",GS2CUserOpChangeItem )
end



--获取配置表
function ItemManager.GetConfig( itemtype,itemid )
    return ItemManager.Config[itemtype][itemid]
end