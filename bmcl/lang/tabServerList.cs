using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace bmcl.lang
{
    [Serializable]
    public class tabServerList
    {
        public string btnReflushServer, btnAddServer, btnDeleteServer, btnEditServer, columnName, columnHide, columnAddress, columnMotd, columnVer, columnOnline, columnDelay;
        public tabServerList()
        {
            btnReflushServer = "刷新服务器";
            btnAddServer = "添加服务器";
            btnDeleteServer = "删除服务器";
            btnEditServer = "编辑服务器";
            columnName = "服务器名称";
            columnHide = "隐藏地址";
            columnAddress = "地址";
            columnMotd = "服务器简介";
            columnVer = "版本";
            columnOnline = "在线人数";
        }
    }
}
