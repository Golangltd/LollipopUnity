require"UILogic.UIBag.UIItemTip"
TipsManager = {}

--onlyShow 为true时紧紧展示，无任何按钮
function TipsManager.Open( itemtype,itemid,onlyShow,data )
	log(itemtype,itemid,onlyShow)
    if itemtype == ItemManager.ItemType.item then--道具
        UIItemTip.Open(itemtype,itemid,onlyShow,data)
    elseif itemtype == ItemManager.ItemType.equip then--装备

    else

    end
end
