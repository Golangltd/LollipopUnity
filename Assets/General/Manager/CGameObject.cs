using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LollipopUnity.GST;
using XLua;

/*
    LollipopUnity 游戏框架基类
    其他模块继承本类
 */

namespace LollipopUinty.CGO
{
    public class CGameObject : MonoBehaviour
    { 
        private string LoginUrl = "";
        private string ProxyUrl = "";
        private string Token = "";

        public string OpenId = "";
        public UserInfo userInfo;

        public CGameObject()
        {

        }

        ~CGameObject()
        {
            this.userInfo = new UserInfo();
            this.LoginUrl = "";
            this.ProxyUrl = "";
            this.Token = "";
            this.OpenId = "";
        }

        public string GetLoginUrlInfo()
        {
            return this.LoginUrl;
        }

        public void SetUserInfo(UserInfo userInfo)
        {
            this.userInfo = userInfo;
        }

        public UserInfo GetUserInfo()
        {
            return this.userInfo;
        }

        public void SetLoginUrl(string url)
        {
            this.LoginUrl = url;
        }

        public string GetLoginUrl()
        {
            return this.LoginUrl;
        }

        public void SetProxyUrl(string url)
        {
            this.ProxyUrl = url;
        }

        public string GetProxyUrl()
        {
            return this.ProxyUrl;
        }
    }
}
