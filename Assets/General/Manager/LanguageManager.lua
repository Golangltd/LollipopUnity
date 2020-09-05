local wordclient_proto = require("config.wordclient_proto")
local word_proto = require("config.word_proto")
LanguageManager = {}

--后续加入多语言版本
function LanguageManager.GetString( str,... )
	local temp = {...}
	if #temp > 0 then
		return string.format(wordclient_proto[str].content,...)
	else
    	return wordclient_proto[str].content
	end
	
end

--获取文本ID对应的内容（策划配置）
function LanguageManager.GetStringById( id )
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

