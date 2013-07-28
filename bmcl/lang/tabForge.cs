using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace bmcl.lang
{
    [Serializable]
    public class tabForge
    {
        public string buttonLastForge, buttonCopyInsPath, btnReForge, labForgeTip;
        public tabForge()
        {
            buttonLastForge = "一键下载最新版forge";
            buttonCopyInsPath = "复制安装路径";
            btnReForge = "刷新Forge列表";
            labForgeTip = "Forge的服务器经常抽风，所以如果获取失败就过一段时间重试\n获取后右边会出现所有Forge的版本，选择一个下载就行，暂不支持早于1.6.1的Forge自动下载";
        }
    }
}
