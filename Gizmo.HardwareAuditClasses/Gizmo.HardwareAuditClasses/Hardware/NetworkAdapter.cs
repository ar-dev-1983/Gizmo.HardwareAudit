using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.NetworkAdapter)]
    public class NetworkAdapter
    {
        [Description("Network Adapter Name")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToGroupAndSort)]
        public string Adapter { set; get; }

        [Description("Network Adapter MAC")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string MAC { set; get; }

        [Description("Network Adapter IP Address")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string IPAddress { set; get; }

        [Description("Network Adapter Default Geteway")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string DefaultGeteway { set; get; }

        [Description("Network Adapter Subnet Masks")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string SubnetMasks { set; get; }

        [Description("Network Adapter DHCP Enabled")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string DHCP_Enabled { set; get; }

        [Description("Network Adapter DHCP Server IP")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
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
