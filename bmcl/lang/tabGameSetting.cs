using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace bmcl.lang
{
    [Serializable]
    public class tabGameSetting
    {
        public string llabver, llabTime, llabReltime, llabType, btnDelete, btnExportOfficial, btnImportOldVer, btnPackUp, btnChangeName, btnMod, btnCoreMod, btnModConfig, btnModDir,tipModDir;
        public tabGameSetting()
        {
            llabver = "游戏版本";
            llabTime = "上次打开时间";
            llabReltime = "发布时间";
            llabType = "发布类型";
            btnDelete = "删除";
            btnExportOfficial = "导出到正版启动器";
            btnImportOldVer = "导入旧版Minecraft";
            btnPackUp = "打包";
            btnChangeName = "重命名";
            btnMod = "Mod管理";
            btnCoreMod = "CoreMod管理";
            btnModConfig = "ModConfig管理";
            btnModDir = "Mod独立文件夹管理";
            tipModDir = "如果MOD在.minecraft下创建了文件夹（例如Flan's mod,Custom NPC等），请使用这个管理";
        }
    }
}
