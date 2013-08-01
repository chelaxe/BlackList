using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackList
{
    public class FilterL7RouterOS
    {
        public static Boolean AddFilterL7(String ip, String username, String password, RegisterDump dump, String SRCAddress, String NameEventLog)
        {
            Boolean ret = true;

            try
            {
                MK mikrotik = new MK(IPAddress.Parse(ip).ToString());



                if (mikrotik.Login(username, password))
                {
                    mikrotik.Send("/system/script/add");
                    mikrotik.Send("=name=cleaner");
                    mikrotik.Send("=source=/ip firewall layer7-protocol remove [find comment=register]\n/ip firewall filter remove [find comment=register]", true);

                    mikrotik.Read();

                    mikrotik.Send("/system/script/run");
                    mikrotik.Send("=number=cleaner", true);

                    mikrotik.Read();

                    /* Cleaner
                     * /ip firewall layer7-protocol remove [find comment=register]
                     * /ip firewall filter remove [find comment=register]
                     */

                    foreach (ItemRegisterDump item in dump.Items)
                    {
                        for (Int32 i = 0; i < item.domain.Count; i++ )
                        {
                            mikrotik.Send("/ip/firewall/layer7-protocol/add");
                            mikrotik.Send("=name=" + item.id + "_" + i);
                            mikrotik.Send("=comment=register");
                            mikrotik.Send("=regexp=^.+(" + item.domain[i] + ").*$", true);

                            mikrotik.Read();

                            mikrotik.Send("/ip/firewall/filter/add");
                            mikrotik.Send("=action=drop");
                            mikrotik.Send("=chain=forward");
                            mikrotik.Send("=disabled=no");
                            mikrotik.Send("=dst-port=80");
                            mikrotik.Send("=layer7-protocol=" + item.id + "_" + i);
                            mikrotik.Send("=protocol=tcp");
                            mikrotik.Send("=src-address=" + SRCAddress);
                            mikrotik.Send("=comment=register", true);

                            mikrotik.Read();
                        }
                    }
                }

                mikrotik.Close();
            }
            catch (Exception error)
            {
                EventLog.WriteEntry(NameEventLog, "Ошибка добавления правил: " + error.Message, EventLogEntryType.Error, 200, 004);
                ret = false;
            }

            return ret;
        }
    }
}
