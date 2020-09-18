-- 作者: 彬哥
-- E-mail: 1528884535@qq.com
-- 创建时间：2020年9月16日


do
ServerInfo =
{
  LoginServerUrl = "192.168.2.115:7601", -- 登录服务器地址
  ProxyServerUrl = "192.168.2.115:8888", -- 反向代理服务器或网关地址
}
end

print("加载ServerInfo.lua成功")
return ServerInfo
