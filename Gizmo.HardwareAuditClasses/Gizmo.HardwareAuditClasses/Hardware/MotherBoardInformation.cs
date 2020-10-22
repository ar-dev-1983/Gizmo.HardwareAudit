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
        public string ManufacturerName { set; get; }

        [Description("MotherBoard Product Name")]
        [ReportVisibility(true)]
        public string ProductName { set; get; }

        [Description("MotherBoard Version")]
        [ReportVisibility(true)]
        public string Version { set; get; }

        [Description("MotherBoard Serial Number")]
        [ReportVisibility(true)]
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
