using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackList
{
    public class RegisterDump
    {
        /*
         * <reg:register updateTime="2013-07-15T10:05:00+04:00" xmlns:reg="http://rsoc.ru" xmlns:tns="http://rsoc.ru">
         *    <content></content>
         *    <content></content>
         *       ...
         *    <content></content>
         * </reg:register>
         */

        public List<ItemRegisterDump> Items { get; set; }
        public String UpdateTime { get; set; }

        public RegisterDump()
        {
            this.Items = new List<ItemRegisterDump>();
            this.UpdateTime = String.Empty;
        }

        public RegisterDump(String UpdateTime, List<ItemRegisterDump> Items)
        {
            this.Items = Items;
            this.UpdateTime = UpdateTime;
        }
    }

    public class ItemRegisterDump
    {
        /*
         * <content id="60" includeTime="2013-01-12T16:33:38">
         *    <decision date="2012-11-03" number="14-РИ" org="Роскомнадзор"/>
         *    <url><![CDATA[http://s4.artemisweb.jp/settoto/]]></url>
         *    <domain><![CDATA[s4.artemisweb.jp]]></domain>
         *    <ip>124.35.0.146</ip>
         * </content>
         */

        public String id { get; set; }
        public String includeTime { get; set; }

        public String date { get; set; }
        public String number { get; set; }
        public String org { get; set; }

        public String url { get; set; }
        public String domain { get; set; }
        public String ip { get; set; }

        public ItemRegisterDump()
        {
            id = String.Empty;
            includeTime = String.Empty;

            date = String.Empty;
            number = String.Empty;
            org = String.Empty;

            url = String.Empty;
            domain = String.Empty;
            ip = String.Empty;
        }
    }
}
