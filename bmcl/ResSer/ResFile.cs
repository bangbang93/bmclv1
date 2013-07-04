using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace bmcl.ResSer
{
    [DataContract(Namespace = "http://s3.amazonaws.com/doc/2006-03-01/")]
    class ResFile
    {
        [DataMember(Order = 0, IsRequired = true)]
        public string Name = "";
        [DataMember(Order = 1, IsRequired = true)]
        public string Prefix = "";
        [DataMember(Order = 2, IsRequired = true)]
        public string Marker = "";
        [DataMember(Order = 3, IsRequired = true)]
        public int MaxKeys = 0;
        [DataMember(Order = 4, IsRequired = true)]
        public bool IsTruncated = false;
        [DataMember(Order = 5, IsRequired = true)]
        public FileInfo[] Contents;

    }
}
