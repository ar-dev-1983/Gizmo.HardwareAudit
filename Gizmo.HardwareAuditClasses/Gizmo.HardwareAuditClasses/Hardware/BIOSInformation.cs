using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    public class BIOSInformation
    {
        [Category("BIOS")]
        [Description("BIOS: производитель")]
        public string Vendor { set; get; }
        [Category("BIOS")]
        [Description("BIOS: версия")]
        public string Version { set; get; }

        public BIOSInformation()
        {
            Vendor = string.Empty;
            Version = string.Empty;
        }
    }

}
