using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackList
{
    public class GetRegister
    {
        public static Settings Start(Settings options)
        {
            String requestFile;
            String signatureFile;

            Byte[] registerZipArchive;

            RegisterDump dump;
            String resultComment;
            String code;

            Int64 ldd = ZapretSOAPServices.LastDumpDate();

            EventLog.WriteEntry(options.NameEventLog, "Временная метка: " + ldd, EventLogEntryType.Information, 100, 001);

            if (ldd != options.LastDumpDate)
            {
                options.LastDumpDate = ldd;

                EventLog.WriteEntry(options.NameEventLog, "Загружаем новую базу", EventLogEntryType.Information, 100, 002);
                
                if (OpenSSL.SignRequest(options.OpenSSLPath, options.KeyPEM, Request.GeneratingRequest(options.operatorName, options.inn, options.ogrn, options.email), out requestFile, out signatureFile, options.NameEventLog))
                {
                    EventLog.WriteEntry(options.NameEventLog, "Подписать запрос удалось", EventLogEntryType.Information, 100, 003);

                    if (ZapretSOAPServices.SendRequest(out resultComment, out code, File.ReadAllBytes(requestFile), File.ReadAllBytes(signatureFile)))
                    {
                        EventLog.WriteEntry(options.NameEventLog, "Запрос отправлен. Комментарий: " + resultComment + " Code: " + code, EventLogEntryType.Information, 100, 004);

                        File.Delete(requestFile);
                        File.Delete(signatureFile);

                        while (!ZapretSOAPServices.GetResult(out resultComment, out registerZipArchive, code))
                        {
                            if (resultComment != "запрос обрабатывается")
                            {
                                EventLog.WriteEntry(options.NameEventLog, "База не получена. Комментарий: " + resultComment + " Работа преостановлена.", EventLogEntryType.Error, 200, 002);
                                return options;
                            }

                            EventLog.WriteEntry(options.NameEventLog, "База не получена. Комментарий: " + resultComment + " Ждем 5m.", EventLogEntryType.Information, 100, 005);

                            Thread.Sleep(300000);
                        }

                        if (ParseRegisterDump.Parse(out dump, registerZipArchive, options.NameEventLog))
                        {
                            EventLog.WriteEntry(options.NameEventLog, "База разобранна успешно", EventLogEntryType.Information, 100, 006);

                            if (FilterL7RouterOS.AddFilterL7(options.ip, options.username, options.password, dump, options.SRCAddress, options.NameEventLog))
                            {
                                EventLog.WriteEntry(options.NameEventLog, "Правила добавлены успешно", EventLogEntryType.Information, 100, 007);
                            }
                        }                        
                    }
                }
            }

            return options;
        }
    }
}
