using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackList
{
    public class Settings
    {
        public String operatorName { get; set; }
        public String inn { get; set; }
        public String ogrn { get; set; }
        public String email { get; set; }

        public String OpenSSLPath { get; set; }
        public String KeyPEM { get; set; }

        public String ip { get; set; }
        public String username { get; set; } 
        public String password { get; set; }

        public String SRCAddress { get; set; }

        public Int64 LastDumpDate { get; set; }

        public String NameEventLog { get; set; }
    }
}
