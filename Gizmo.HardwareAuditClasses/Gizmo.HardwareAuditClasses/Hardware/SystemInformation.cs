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
        [FieldType(FieldTypeEnum.KeyToGroupAndSort)]
        public string ManufacturerName { set; get; }

        [Description("System Name")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToGroupAndSort)]
        public string ProductName { set; get; }

        [Description("System Version")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string Version { set; get; }

        [Description("System Serial Number")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string SerialNumber { set; get; }

        [Description("System Family")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
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
