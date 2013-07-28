using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace bmcl.lang
{
    [Serializable]
    public class frmCheckRes
    {
        public string buttonCheck, buttonSync, columnName, columnLastMod, columnSize, columnStatus;
        public frmCheckRes()
        {
            buttonCheck = "对比本地";
            buttonSync = "同步";
            columnName = "文件名";
            columnLastMod = "最后修改时间";
            columnSize = "大小";
            columnStatus = "状态";
        }
    }
}
