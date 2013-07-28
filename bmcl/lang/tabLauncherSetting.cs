using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace bmcl.lang
{
    [Serializable]
    public class tabLauncherSetting
    {
        public string labUsername, labPwd, labJavaw, labJavaXmx, checkAutoStart, labJavaArg, groupAuth, noAuth, btnSaveConfig;
        public tabLauncherSetting()
        {
            labUsername = "用户名";
            labPwd = "密码";
            labJavaw = "javaw.exe路径";
            labJavaXmx = "Java运行内存";
            checkAutoStart = "下次自动启动";
            labJavaArg = "自定义Java Arguments";
            groupAuth = "登录验证方式";
            noAuth = "啥都没有";
            btnSaveConfig = "保存配置";
        }
    }
}
