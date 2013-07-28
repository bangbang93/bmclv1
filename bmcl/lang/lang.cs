using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace bmcl.lang
{
    [Serializable]
    public class lang
    {
        #region 语言内容
        //窗体下方的公共部分
        public string buttonStart;
        public string btnlanguage;
        public tabGameSetting tabgamesetting;
        public tabLauncherSetting tablaunchersetting;
        public tabVerManage tabvermanage;
        public tabForge tabforge;
        public tabServerList tabserverlist;
        public frmCheckRes frmcheckres;
        #endregion
        public string Name;//语言名
        public int Ver;//语言对应的版本
        public lang()
        {
            buttonStart = "开始游戏";
            btnlanguage = "语言\nlanguage";
            tabgamesetting = new tabGameSetting();
            tablaunchersetting = new tabLauncherSetting();
            tabvermanage=new tabVerManage();
            tabforge=new tabForge();
            tabserverlist=new tabServerList();
            frmcheckres=new frmCheckRes();
            Name = "简体中文";
            Ver = 1;
        }
    }
}
