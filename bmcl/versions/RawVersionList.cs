using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;

namespace bmcl.versions
{
    [DataContract]
    class RawVersionList
    {
        [DataMember(Order = 0,IsRequired = true)]
        private RemoteVer[] versions;
        [DataMember(Order = 1,IsRequired = true)]
        private latest latest;

        public RemoteVer[] getVersions()
        {
            return versions;
        }

        public latest getLastestVersion()
        {
            return latest;
        }

        public RawVersionList()
        {
            versions = null;
            latest = new latest();
        }
    }
    [DataContract]
    class latest
    {
        [DataMember(Order = 0, IsRequired = true)]
        string snapshot;
        [DataMember(Order = 1, IsRequired = true)]
        string release;

        public latest()
        {
            snapshot = "";

            release = "";
        }
    }
}
