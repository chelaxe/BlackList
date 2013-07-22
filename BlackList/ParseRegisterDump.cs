using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BlackList
{
    public class ParseRegisterDump
    {
        public static Boolean Parse(out RegisterDump Register, Byte[] registerZipArchive, String NameEventLog)
        {
            Boolean ret = true;
            Register = new RegisterDump();
            String registerZipArchivePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\register.zip";
            String UnZIPPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\register";
            String dumpfile = UnZIPPath + @"\dump.xml";
            String signdumpfile = UnZIPPath + @"\dump.xml.sig";

            try
            {
                File.WriteAllBytes(registerZipArchivePath, registerZipArchive);

                ZipFile.ExtractToDirectory(registerZipArchivePath, UnZIPPath);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(dumpfile);

                Register.UpdateTime = xmlDoc.GetElementsByTagName("reg:register")[0].Attributes.GetNamedItem("updateTime").InnerText;
                XmlNodeList content = xmlDoc.GetElementsByTagName("content");

                for (int i = 0; i < content.Count; i++)
                {
                    XmlNodeList nodechild = content[i].ChildNodes;

                    ItemRegisterDump item = new ItemRegisterDump();

                    item.id = content[i].Attributes.GetNamedItem("id").InnerText;
                    item.includeTime = content[i].Attributes.GetNamedItem("includeTime").InnerText;

                    item.date = nodechild[0].Attributes.GetNamedItem("date").InnerText;
                    item.number = nodechild[0].Attributes.GetNamedItem("number").InnerText;
                    item.org = nodechild[0].Attributes.GetNamedItem("org").InnerText;

                    item.url = nodechild[1].InnerText;
                    item.domain = nodechild[2].InnerText;
                    item.ip = nodechild[3].InnerText;

                    Register.Items.Add(item);
                }
                                
                Directory.Delete(UnZIPPath, true);
                File.Delete(registerZipArchivePath);                
            }
            catch (Exception error)
            {
                EventLog.WriteEntry(NameEventLog, "Ошибка парсера: " + error.Message, EventLogEntryType.Error, 200, 003);
                ret = false;
            }

            return ret;
        }
    }
}
