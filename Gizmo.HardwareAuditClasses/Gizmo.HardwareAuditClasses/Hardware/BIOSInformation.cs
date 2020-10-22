using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.BIOSInformation)]
    public class BIOSInformation
    {
        [Description("BIOS Vendor")]
        [ReportVisibility(true)]
        public string Vendor { set; get; }
        [Description("BIOS Version")]
        [ReportVisibility(true)]
        public string Version { set; get; }

        public BIOSInformation()
        {
            Vendor = string.Empty;
            Version = string.Empty;
        }
    }

}
