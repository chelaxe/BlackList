using BlackList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings options;

            try
            {
                Console.WriteLine("Грузим конфиг: " + args[0]);
                using (Stream stream = new FileStream(args[0], FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    options = (Settings)serializer.Deserialize(stream);
                }

                try
                {
                    Console.WriteLine("Проверяем наличие журнала: " + options.NameEventLog);
                    if (!EventLog.SourceExists(options.NameEventLog))
                    {                        
                        EventLog.CreateEventSource(options.NameEventLog, options.NameEventLog);
                        Console.WriteLine("Создаем журнал: " + options.NameEventLog);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Нужны прова администратора на создание журнала");
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.Verb = "runas";
                    processInfo.FileName = Assembly.GetExecutingAssembly().Location;
                    processInfo.Arguments = args[0];
                    try
                    {
                        Process.Start(processInfo);
                        return;
                    }
                    catch (Win32Exception)
                    {
                        return;
                    }
                }

                Console.WriteLine("Процесс пошел...");
                options = GetRegister.Start(options);

                Console.WriteLine("Сохраняем кофиг");
                using (Stream writer = new FileStream(args[0], FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    serializer.Serialize(writer, options);
                }

                Console.ReadKey();
            }
            catch (Exception)
            {
                
            }
        }

        static Settings temp()
        {
            Settings options = new Settings();

            options.operatorName = @"ООО фирма 'Рога и копыта'";
            options.inn = @"0123456789";
            options.ogrn = @"9876543210";
            options.email = @"info@blabla.ru";

            options.NameEventLog = "Zapret";

            options.OpenSSLPath = @"c:\OpenSSL\bin\";
            #region options.KeyPEM = "keykeykey";
            options.KeyPEM = @"keykeykey";
            #endregion

            options.LastDumpDate = 0;

            options.ip = IPAddress.Parse(@"192.168.0.1").ToString();
            options.username = @"admin";
            options.password = @"admin";
            options.SRCAddress = @"192.168.0.0/24";

            return options;
        }
    }
}
