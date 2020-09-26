using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    public class SystemInformation
    {
        [Category("System Information")]
        [Description("System: производитель")]
        public string ManufacturerName { set; get; }

        [Category("System Information")]
        [Description("System: имя")]
        public string ProductName { set; get; }

        [Category("System Information")]
        [Description("System: версия")]
        public string Version { set; get; }

        [Category("System Information")]
        [Description("System: серийный номер")]
        public string SerialNumber { set; get; }

        [Category("System Information")]
        [Description("System: семейство")]
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
