using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace bmcl.lang
{
    [Serializable]
    public class tabVerManage
    {
        public string ClmId, ClmRelTime, ClmType, buttonFlush, buttonDownload, buttonCheckRes;
        public tabVerManage()
        {
            ClmId = "版本";
            ClmRelTime = "发布时间";
            ClmType = "发布类型";
            buttonFlush = "刷新版本";
            buttonDownload = "下载";
            buttonCheckRes = "检查资源文件";
        }
    }
}
