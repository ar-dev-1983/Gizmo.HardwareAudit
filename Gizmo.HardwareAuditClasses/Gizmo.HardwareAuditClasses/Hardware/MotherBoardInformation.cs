using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    public class MotherBoardInformation
    {
        [Category("MotherBoard")]
        [Description("MotherBoard: производитель")]
        public string ManufacturerName { set; get; }

        [Category("MotherBoard")]
        [Description("MotherBoard: имя")]
        public string ProductName { set; get; }

        [Category("MotherBoard")]
        [Description("MotherBoard: версия")]
        public string Version { set; get; }

        [Category("MotherBoard")]
        [Description("MotherBoard: серийный номер")]
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
