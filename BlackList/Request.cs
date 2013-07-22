using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackList
{
    public class Request
    {
        public static String GeneratingRequest(String operatorName, String inn, String ogrn, String email)
        {
            String result = String.Empty;

            result = "<?xml version=\"1.0\" encoding=\"windows-1251\"?>";
            result += "<request>";
            result += "<requestTime>";
            result += DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz");
            result += "</requestTime>";
            result += "<operatorName>";
            result += "<![CDATA[" + operatorName + "]]>";
            result += "</operatorName>";
            result += "<inn>";
            result += inn;
            result += "</inn>";
            result += "<ogrn>";
            result += ogrn;
            result += "</ogrn>";
            result += "<email>";
            result += email;
            result += "</email>";
            result += "</request>";

            return result;
        }
    }
}
