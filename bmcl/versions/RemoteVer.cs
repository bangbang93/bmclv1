using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace bmcl.versions
{
    [DataContract]
    class RemoteVer
    {
        [DataMember(Order = 0, IsRequired = true)]
        public string id;
        [DataMember(Order = 1, IsRequired = true)]
        public string time;
        [DataMember(Order = 2, IsRequired = true)]
        public string releaseTime;
        [DataMember(Order = 3, IsRequired = true)]
        public string type;

        
    }
}
