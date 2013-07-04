using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace bmcl.ResSer
{
    
    class root
    {
        [DataMember(Order = 0, IsRequired = true)]
        public ResFile ListBucketResult;
    }
}
