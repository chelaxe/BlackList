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
                    ItemRegisterDump item = new ItemRegisterDump();

                    item.id = content[i].Attributes.GetNamedItem("id").InnerText;
                    item.includeTime = content[i].Attributes.GetNamedItem("includeTime").InnerText;

                    foreach (XmlNode node in content[i].ChildNodes)
                    { 
                        switch(node.Name)
                        {
                            case "decision":
                                item.date = node.Attributes.GetNamedItem("date").InnerText;
                                item.number = node.Attributes.GetNamedItem("number").InnerText;
                                item.org = node.Attributes.GetNamedItem("org").InnerText;
                                break;
                            case "url":
                                item.url.Add(node.InnerText);
                                break;
                            case "domain":
                                item.domain.Add(node.InnerText);
                                break;
                            case "ip":
                                item.ip.Add(node.InnerText);
                                break;
                        }
                    }

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
