
local skill = require "Assets.General.Config.skill" -- 加载配置文件

ServerInfo =
{
  LoginServerUrl = "192.168.2.115:7601",
  ProxyServerUrl = "192.168.2.115:8888",
  ResServerUrl = "192.168.2.115:8080",
}

-- lua table 怎么建立
print("----000",skill[1].name)

return ServerInfo
