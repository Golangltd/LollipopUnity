LanguageManager = {}
--后续加入多语言版本  old
function LanguageManager.GetString(str,...)
	local temp = {...}
	if #temp > 0 then
		if wordClient_proto[str].content then 
		return string.format(wordClient_proto[str].content,...)
		else
			logError("数据未找到："..temp.."未找到")
		end
	else
    	return wordClient_proto[str].content
	end	
end


--获取文本   根据默认的指定类型
function LanguageManager.GetStringForType(str)
	if wordClient_proto[str].NowLanguageType then
	return wordClient_proto[str].NowLanguageType
	end
end


--设置按钮的文本  将按钮传进来即可
function LanguageManager.SetButtonObjText(btn)
	local indexText = btn.transform.name
	local txtName = btn.transform:Find("Text")
	txtName.text=wordClient_proto[indexText].NowLanguageType
end


--获取文本ID对应的内容（策划配置）
function LanguageManager.GetWordStringByID( id )
	if word_proto[id] then
    	return word_proto[id].data1
    else
    	return string.format("未找到配置内容:%s",id) 
    end
end


--获取随机姓名表（姓）
function LanguageManager.GetName(  )
	local surname = require("config.surname")
    local index = Random(1,#surname,7)
	return surname[index].chs_surname
end


--获取随机姓名表（名）
function LanguageManager.GetPlayerName( index,sex )
	local playername = require("config.playername")
    local index = Random(1,#playername,20)
	if sex == 0 then--男
		return playername[index].chs_M_name
	else
		return playername[index].chs_M_name
	end
end

