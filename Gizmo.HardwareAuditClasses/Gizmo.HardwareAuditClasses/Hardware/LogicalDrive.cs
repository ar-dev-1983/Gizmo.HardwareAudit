using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.LogicalDrive)]
    public class LogicalDrive
    {
        [ReportVisibility(true)]
        [Description("Partition Letter")]
        public string Letter { set; get; }

        [ReportVisibility(true)]
        [Description("Partition Size")]
        public string TotalSize { set; get; }

        [ReportVisibility(true)]
        [Description("Partition Aviailable Size")]
        public string AviailableSize { set; get; }

        public LogicalDrive()
        {
            Letter = string.Empty;
            TotalSize = string.Empty;
            AviailableSize = string.Empty;
        }

        public static List<LogicalDrive> Enumerate(ManagementScope Scope)
        {
            var result = new List<LogicalDrive>();
            try
            {
                Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                Scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_LogicalDisk");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject m in queryCollection)
                {
                    if (m["Description"] != null)
                    {
                        if (m["Description"].ToString() == "Local Fixed Disk" || m["Description"].ToString() == "Локальный несъемный диск")
                        {
                            result.Add(new LogicalDrive
                            {
                                Letter = m["Name"] != null ? StringFormatting.CleanInvalidXmlChars(m["Name"].ToString()).TrimStart().TrimEnd() : string.Empty,
                                AviailableSize = m["FreeSpace"] != null ? StringFormatting.CleanInvalidXmlChars((((UInt64)m["FreeSpace"]) / 1024 / 1024 / 1024).ToString("0 Gb")).TrimStart().TrimEnd() : string.Empty,
                                TotalSize = m["Size"] != null ? StringFormatting.CleanInvalidXmlChars((((UInt64)m["Size"]) / 1024 / 1024 / 1024).ToString("0 Gb")).TrimStart().TrimEnd() : string.Empty
                            });
                        }
                    }
                }
            }
            catch (Exception) { }
            return result.OrderBy(x => x.Letter).ToList();
        }
    }
}
