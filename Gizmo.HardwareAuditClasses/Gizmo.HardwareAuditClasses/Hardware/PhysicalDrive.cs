using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.PhysicalDrive)]
    public class PhysicalDrive
    {
        [Description("Physical Drive Model")]
        [ReportVisibility(true)]
        public string Model { set; get; }

        [Description("Physical Drive Serial Number")]
        [ReportVisibility(true)]
        public string SerialNumber { set; get; }

        [Description("Physical Drive Size")]
        [ReportVisibility(true)]
        public string Size { set; get; }

        public PhysicalDrive()
        {
            Model = string.Empty;
            SerialNumber = string.Empty;
            Size = string.Empty;
        }

        public static List<PhysicalDrive> Enumerate(ManagementScope Scope)
        {
            var result = new List<PhysicalDrive>();
            try
            {
                Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                Scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_DiskDrive");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject m in queryCollection)
                {
                    result.Add(new PhysicalDrive
                    {
                        Model = m["Model"] != null ? StringFormatting.CleanInvalidXmlChars(m["Model"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        SerialNumber = m["SerialNumber"] != null ? StringFormatting.CleanInvalidXmlChars(m["SerialNumber"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        Size = m["Size"] != null ? StringFormatting.CleanInvalidXmlChars((((UInt64)m["Size"]) / 1024 / 1024 / 1024).ToString("0 Gb")).TrimStart().TrimEnd() : string.Empty
                    });
                }
            }
            catch (Exception) { }
            if (result.Count > 0)
            {
                return result;
            }
            else
            {
                return new List<PhysicalDrive>();
            }
        }
    }
}
