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
        [FieldType(FieldTypeEnum.KeyToGroupAndSort)]
        public string Vendor { set; get; }

        [Description("BIOS Version")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string Version { set; get; }

        public BIOSInformation()
        {
            Vendor = string.Empty;
            Version = string.Empty;
        }
    }

}
