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
         *    <decision date="2013-11-03" number="МИ-6" org="РосКосМопсПопс"/>
         *    <url><![CDATA[http://habrahabr.ru/post/187574/]]></url>
         *    <ip>123.45.67.89</ip>
         * </content>
         * <content id="69" includeTime="2013-05-12T12:43:34">
         *    <decision date="2013-10-02" number="ФБИ" org="СФНК"/>
         *    <domain><![CDATA[chelaxe.ru]]></domain>
         *    <ip>123.45.67.89</ip>
         *    <ip>87.65.43.210</ip>
         * </content>
         */

        public String id { get; set; }
        public String includeTime { get; set; }

        public String date { get; set; }
        public String number { get; set; }
        public String org { get; set; }

        public List<String> url { get; set; }
        public List<String> domain { get; set; }
        public List<String> ip { get; set; }

        public ItemRegisterDump()
        {
            id = String.Empty;
            includeTime = String.Empty;

            date = String.Empty;
            number = String.Empty;
            org = String.Empty;

            url = new List<string>();
            domain = new List<string>();
            ip = new List<string>();
        }
    }
}
