--管理器--
LoginManager = {}
local serverList = nil
local defaultServer = nil --默认选中的服务器
local selectServer = nil --当前选择的服务器
LoginManager.serverAreaCount = 2 --分区的数量
-- local webSocketManager = nil
	
function LoginManager.Init(  )
	LoginManager.RequestServerList()
end

--注册账号
function LoginManager.RegisterAccount( name,Call )
    local version = require("config.version")
	local Ip = string.format(version.registerIp,name)
	print(version.serverListIp)
	WebSocketManager.Instance:HttpRequest(0,Ip, nil, function ( _type,str )
		if str == nil or #str < 1 then
			return
		end
    	local js = Json:decode(str)
    	NetWork.SetToken( js.Token )
    	if Call then
    		Call()
    	end
    end)
end

--请求服务器列表
function LoginManager.RequestServerList()
	-- local luacher = GameObject.Find("lanuch")
    -- webSocketManager = luacher.transform:GetComponent("WebSocketManager")
	local version = require("config.version")
	print(version.serverListIp)
    WebSocketManager.Instance:HttpRequest(0,version.serverListIp, nil, function ( _type,str )
		print("str: ",str)
		if not str then
			return
		end
    	serverList = Json:decode(str).SeverList
    	table.insert(serverList,serverList[1])
    	table.insert(serverList,serverList[1])
    	table.insert(serverList,serverList[1])
    	table.insert(serverList,serverList[1])
    	table.insert(serverList,serverList[1])
    	table.insert(serverList,serverList[1])
    	table.insert(serverList,serverList[1])
    	for i,v in ipairs(serverList) do
    		if v.state == 5 then --最近登录的服务器    
    			defaultServer = v
    			break
    		end
    	end
    	defaultServer = defaultServer or serverList[2]
		dump(serverList,"服务器列表:")
		require("UILogic.Login.UILogin") 
		UILogin:SetVisible(true)
    end)
end

--获取服务器列表
function LoginManager.GetServerList(  )
	return serverList
end

--获取默认选中的服务器
function LoginManager.GetDefaultServer(  )
	return defaultServer
end

--获取已分区的服务器列表
function LoginManager.GetAreaServerList(  )
	local temp = {}
	local index = 1
	local count = 1
	for i,v in ipairs(serverList) do
		temp[index] = temp[index] or {}
		table.insert(temp[index],v)
		count = count + 1
		if count > LoginManager.serverAreaCount then
			index = index + 1
			count = 1
		end
	end
	return temp
end

function LoginManager.SetSelectServer( server )
	selectServer = server
end

function LoginManager.GetSelectServer()
	return selectServer or defaultServer
end
