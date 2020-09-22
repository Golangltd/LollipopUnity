HandBookManager={}

local handbookhero_proto = require("Config.handbookhero_proto")
local word_proto = require("Config.word_proto")
local character = require("Config.character")
local variable_proto = require("Config.variable_proto")

local qualityImagePath = nil
local qualityColor = nil
local qualityTeamColor = nil
local characterQualityImagePath = nil
local tools = require("Common.Tools.Tools")

local handBookClickId = 0

---vocation 或者 job 都是职业名
local jobImagePath = nil
---clamp 或者 team 都是团队名
local teamImagePath = nil


function HandBookManager.InitData()   
    local teamDatas = 
    {
        [1] = {},
        [2] = {},
        [3] = {},
        [4] = {},
        [5] = {},
        [6] = {},
    }

    local useDatas = {}

    for k,v in ipairs(handbookhero_proto) do
        local camp =character[v.character_id[1]].camp
        table.insert(teamDatas[camp],k) 
    end


    for k,v in ipairs(teamDatas) do
        table.sort(v,function(a,b)
            return a > b
        end)
    end

    dump(teamDatas)

    for k,v in ipairs(teamDatas) do

        print(k,"teamType")
        local startIndex = #useDatas + 1
        for i = 1 , 5 do
            table.insert(useDatas,startIndex,0)
        end 

        for k1,v1  in ipairs(v) do
            table.insert(useDatas,v1)
        end

        local addGrid = (5 - (#v) % 5)
        if  addGrid ~= 0 then
            for i = 1 , addGrid do
                table.insert(useDatas,0)
            end
        end
    end

    return teamDatas,useDatas
end

--获取图鉴名字 角色名字
function HandBookManager.GetHandBookName(id)
    local wordId = character[handbookhero_proto[id].character_id[1]].name
    return string.format(HandBookManager.GetNameColorString(id),word_proto[wordId].data1)  
end

function HandBookManager.GetCharacterId(id)
    return handbookhero_proto[id].character_id[1]
end

--获取模型
function HandBookManager.GetModelId(id)  
    return character[handbookhero_proto[id].character_id[1]].resouce
end

--获取描述
function HandBookManager.GetDes(id)
    local wordId = character[handbookhero_proto[id].character_id[1]].description
    return word_proto[wordId].data1

end

--获取图标路径
function HandBookManager.GetHandBookIconPath(id)
    local iconPath = character[handbookhero_proto[id].character_id[1]].icon
    return "Icon/head/icon_head_" .. iconPath
end

--获取品质框路径
function HandBookManager.GetHandBookQualityPath(id)
    local quality = character[handbookhero_proto[id].character_id[1]].maxquality

    if qualityImagePath ~= nil and qualityImagePath[quality] then
        return qualityImagePath[quality]
    else
        qualityImagePath = {}
        if quality == 1 then
            qualityImagePath[quality] = "UI/Common/cmm_hero_bg_01"
        elseif quality == 2 then
            qualityImagePath[quality] = "UI/Common/cmm_hero_bg_02"
        elseif quality == 3 then
            qualityImagePath[quality] = "UI/Common/cmm_hero_bg_03"
        elseif quality > 3 and quality < 7 then
            qualityImagePath[quality] = "UI/Common/cmm_hero_bg_02"
        elseif quality >= 7 and quality < 10 then
            qualityImagePath[quality] = "UI/Common/cmm_hero_bg_03"
        elseif quality >= 10 and quality < 13 then
            qualityImagePath[quality] = "UI/Common/cmm_hero_bg_04"
        elseif quality >= 13 and quality < 16 then
            qualityImagePath[quality] = "UI/Common/cmm_hero_bg_05"
        elseif quality >= 16 and quality < 19 then
            qualityImagePath[quality] = "UI/Common/cmm_hero_bg_06"
        end
    end
    return qualityImagePath[quality]
end

--获取名字颜色
function HandBookManager.GetNameColorString(id)
    local quality = character[handbookhero_proto[id].character_id[1]].maxquality
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

--获取名字颜色
function HandBookManager.GetTeamNameColorString(camp)
    if qualityTeamColor ~= nil and qualityTeamColor[camp] then
        return qualityTeamColor[camp]
    else
        qualityTeamColor = {}
        if camp == 1 then
            qualityTeamColor[camp] = "<color=#99FCA1>%s</color>"
        elseif camp == 2 then
            qualityTeamColor[camp] = "<color=#76B3FF>%s</color>"
        elseif camp == 3 then
            qualityTeamColor[camp] = "<color=#FFA44C>%s</color>"
        elseif camp == 4 then
            qualityTeamColor[camp] = "<color=#FF5555>%s</color>"
        elseif camp == 5 then
            qualityTeamColor[camp] = "<color=#FEE351>%s</color>"
        elseif camp == 6 then
            qualityTeamColor[camp] = "<color=#FEE351>%s</color>"    
        end   
    end
    return qualityTeamColor[camp]
end

--获取最大等级
function HandBookManager.GetMaxLevel(id)

    local maxquality = character[handbookhero_proto[id].character_id[1]].maxquality
    --125为最大品质对应等级   
    return variable_proto[125].data1[maxquality][2]

end

--获取职业图标路径
function HandBookManager.GetJobIconPath(id)
    local vocation = character[handbookhero_proto[id].character_id[1]].vocation

    if jobImagePath ~= nil and jobImagePath[vocation] then
        return jobImagePath[vocation]
    else
        jobImagePath = {}
        if vocation == 1 then
            jobImagePath[vocation] = "Icon/icon_vocation_01"
        elseif vocation == 2 then
            jobImagePath[vocation] = "Icon/icon_vocation_02"
        elseif vocation == 3 then
            jobImagePath[vocation] = "Icon/icon_vocation_03"
        elseif vocation == 4 then
            jobImagePath[vocation] = "Icon/icon_vocation_04"
        end
    end
    return jobImagePath[vocation]
end

--获取阵营图标路径
function HandBookManager.GetTeamIconPath(id)
    local camp = character[handbookhero_proto[id].character_id[1]].camp
    if teamImagePath ~= nil and teamImagePath[camp] then
        return teamImagePath[camp]
    else
        teamImagePath = {}
        if camp == 1 then
            teamImagePath[camp] = "Icon/icon_houses_01"
        elseif camp == 2 then
            teamImagePath[camp] = "Icon/icon_houses_02"
        elseif camp == 3 then
            teamImagePath[camp] = "Icon/icon_houses_03"
        elseif camp == 4 then
            teamImagePath[camp] = "Icon/icon_houses_04"
        elseif camp == 5 then
            teamImagePath[camp] = "Icon/icon_houses_05"
        elseif camp == 6 then
            teamImagePath[camp] = "Icon/icon_houses_06"
        end
    end
    return teamImagePath[camp]
end

--获取阵营名称
function HandBookManager.GetTeamName(camp)
    local world
    if camp == 1 then
        world = "阵营1"
    elseif camp == 2 then
        world = "阵营2"
    elseif camp == 3 then
        world = "阵营3"
    elseif camp == 4 then
        world = "阵营4"
    elseif camp == 5 then
        world = "阵营5"
    elseif camp == 6 then
        world = "阵营6"
    end

    return string.format(HandBookManager.GetTeamNameColorString(camp),world)  
end

--获取角色品质路径
function HandBookManager.GetCharacterQualityImagePath(id)
    local quality = character[handbookhero_proto[id].character_id[1]].maxquality

    if characterQualityImagePath ~= nil and characterQualityImagePath[quality] then
        return characterQualityImagePath[quality]
    else
        characterQualityImagePath = {}
        if quality == 1 then
            characterQualityImagePath[quality] = "Icon/icon_ui_pz_01"
        elseif quality == 2 then
            characterQualityImagePath[quality] = "Icon/icon_ui_pz_01"
        elseif quality == 3 then
            characterQualityImagePath[quality] = "Icon/icon_ui_pz_01"
        elseif quality > 3 and quality < 7 then
            characterQualityImagePath[quality] = "Icon/icon_ui_pz_01"
        elseif quality >= 7 and quality < 10 then
            characterQualityImagePath[quality] = "Icon/icon_ui_pz_01"
        elseif quality >= 10 and quality < 13 then
            characterQualityImagePath[quality] = "Icon/icon_ui_pz_01"
        elseif quality >= 13 and quality < 16 then
            characterQualityImagePath[quality] = "Icon/icon_ui_pz_01"
        elseif quality >= 16 and quality < 19 then
            characterQualityImagePath[quality] = "Icon/icon_ui_pz_01"
        end
    end
    return characterQualityImagePath[quality]
end

return HandBookManager