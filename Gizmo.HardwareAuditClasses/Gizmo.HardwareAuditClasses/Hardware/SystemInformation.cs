using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.SystemInformation)]
    public class SystemInformation
    {
        [Description("System Vendor")]
        [ReportVisibility(true)]
        public string ManufacturerName { set; get; }

        [Description("System Name")]
        [ReportVisibility(true)]
        public string ProductName { set; get; }

        [Description("System Version")]
        [ReportVisibility(true)]
        public string Version { set; get; }

        [Description("System Serial Number")]
        [ReportVisibility(true)]
        public string SerialNumber { set; get; }

        [Description("System Family")]
        [ReportVisibility(true)]
        public string Family { set; get; }

        public SystemInformation()
        {
            ManufacturerName = string.Empty;
            ProductName = string.Empty;
            Version = string.Empty;
            SerialNumber = string.Empty;
            Family = string.Empty;
        }
    }
}
