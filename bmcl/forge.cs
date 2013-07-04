using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace bmcl
{
    class forge
    {
        static string normalprofile = "{\"selectedProfile\": \"(Default)\",\"profiles\": {\"Forge\": {\"name\": \"Forge\",\"lastVersionId\": \"Forge8.9.0.762\"},\"(Default)\": {\"authentication\": {\"username\": \"sylqiuyifeng@yahoo.com.hk\",\"accessToken\": \"d804d5b4b9ad40f88d08bb917f9fc874\",\"uuid\": \"56e38cbd07c14613bd010822786e21ad\",\"displayName\": \"SYL_qiuyifeng\"},\"name\": \"(Default)\",\"gameDir\": \"F:\\craft\\1.6\"}},\"clientToken\": \"1ed168f6-6e28-4be2-8938-84b72d40c054\"}";
        public forge()
        {
        }
        static public void writeprofile()
        {
            StreamWriter profile = new StreamWriter(".minecraft\\launcher_profiles.json");
            profile.WriteLine(resource.normalprofile.NormalProfile);
            profile.Close();
        }
    }
}
