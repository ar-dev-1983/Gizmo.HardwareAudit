using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.MotherBoardInformation)]
    public class MotherBoardInformation
    {
        [Description("MotherBoard Manufacturer")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToGroupAndSort)]
        public string ManufacturerName { set; get; }

        [Description("MotherBoard Product Name")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToGroupAndSort)]
        public string ProductName { set; get; }

        [Description("MotherBoard Version")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string Version { set; get; }

        [Description("MotherBoard Serial Number")]
        [ReportVisibility(true)]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string SerialNumber { set; get; }

        public MotherBoardInformation()
        {
            ManufacturerName = string.Empty;
            ProductName = string.Empty;
            Version = string.Empty;
            SerialNumber = string.Empty;
        }
    }
}
