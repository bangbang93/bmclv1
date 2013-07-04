using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace loginauthmothed
{
    public interface auth
    {
        bool login(string username, string passwd);
        string getname();
        string getsession();
    }
}
