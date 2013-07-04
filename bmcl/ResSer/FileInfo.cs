using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace bmcl.ResSer
{
    [DataContract]
    class FileInfo
    {
        [DataMember(Order = 0, IsRequired = true)]
        public string Key;
        [DataMember(Order = 1, IsRequired = true)]
        public string LastModified;
        [DataMember(Order = 2, IsRequired = true)]
        public string ETag;
        [DataMember(Order = 3, IsRequired = true)]
        public long Size;
        [DataMember(Order = 4, IsRequired = true)]
        public string StorageClass;

    }
}
