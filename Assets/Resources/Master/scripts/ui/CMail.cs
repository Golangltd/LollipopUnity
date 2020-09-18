using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LollipopUinty.CGO;
using LollipopUnity.GST;
using XLua;


public class CMail : CGameObject
{
    private MailSt  mailinfo;
    private LuaEnv luaenv;

    public CMail()
    {
        LuaEnv luaenv = new LuaEnv();
        this.mailinfo = new MailSt();
    }

     ~CMail()
    { 
        this.luaenv.Dispose();
    }

    public void SetMailInfo(MailSt maildata)
    {
       this.mailinfo = maildata;
    }

    public MailSt GetMailInfo()
    {
        return this.mailinfo;
    }

}
