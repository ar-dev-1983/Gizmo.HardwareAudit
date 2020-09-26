using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;

namespace Gizmo.HardwareAuditClasses
{
    public class NetworkAdapter
    {
        [Description("Сетевой адаптер: имя")]
        public string Adapter { set; get; }

        [Description("Сетевой адаптер: физический адрес")]
        public string MAC { set; get; }

        [Description("Сетевой адаптер: IP-адрес")]
        public string IPAddress { set; get; }

        [Description("Сетевой адаптер: шлюз по-умолчанию")]
        public string DefaultGeteway { set; get; }

        [Description("Сетевой адаптер: маска подсети")]
        public string SubnetMasks { set; get; }

        [Description("Сетевой адаптер: DHCP разрешен")]
        public string DHCP_Enabled { set; get; }

        [Description("Сетевой адаптер: DHCP сервер")]
        public string DHCP_ServerIP { set; get; }

        public NetworkAdapter()
        {
            Adapter = string.Empty;
            IPAddress = string.Empty;
            DefaultGeteway = string.Empty;
            SubnetMasks = string.Empty;
            DHCP_Enabled = string.Empty;
            DHCP_ServerIP = string.Empty;
            MAC = string.Empty;
        }

        public static List<NetworkAdapter> Enumerate(ManagementScope Scope)
        {
            var result = new List<NetworkAdapter>();
            try
            {
                Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                Scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject m in queryCollection)
                {
                    if (m["IPEnabled"] != null && StringFormatting.CleanInvalidXmlChars(m["IPEnabled"].ToString()).ToLower() != "false")
                    {
                        result.Add(new NetworkAdapter
                        {
                            Adapter = m["Description"] != null ? StringFormatting.CleanInvalidXmlChars(m["Description"].ToString()).TrimStart().TrimEnd() : string.Empty,
                            MAC = m["MACAddress"] != null ? StringFormatting.CleanInvalidXmlChars(m["MACAddress"].ToString()).TrimStart().TrimEnd() : string.Empty,
                            DHCP_Enabled = m["DHCPEnabled"] != null ? StringFormatting.CleanInvalidXmlChars(m["DHCPEnabled"].ToString()).TrimStart().TrimEnd() : string.Empty,
                            IPAddress = m["IPAddress"] != null ? StringFormatting.CleanInvalidXmlChars(((Array)m["IPAddress"]).GetValue(0).ToString()).TrimStart().TrimEnd() : string.Empty,
                            SubnetMasks = m["IPSubnet"] != null ? StringFormatting.CleanInvalidXmlChars(((Array)m["IPSubnet"]).GetValue(0).ToString()).TrimStart().TrimEnd() : string.Empty,
                            DefaultGeteway = m["DefaultIPGateway"] != null ? StringFormatting.CleanInvalidXmlChars(((Array)m["DefaultIPGateway"]).GetValue(0).ToString()).TrimStart().TrimEnd() : string.Empty,
                            DHCP_ServerIP = m["DHCPServer"] != null ? StringFormatting.CleanInvalidXmlChars(m["DHCPServer"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        });
                    }
                }
            }
            catch (Exception) { }
            if (result.Count > 0)
            {
                return result;
            }
            else
            {
                return new List<NetworkAdapter>();
            }
        }
    }
}
