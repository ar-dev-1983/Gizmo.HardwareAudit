using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    public class CPUInformation
    {
        public string SlotLocator { set; get; }

        [Category("CPU")]
        [Description("CPU: производитель")]
        public string ManufacturerName { set; get; }

        [Category("CPU")]
        [Description("CPU: версия")]
        public string Version { set; get; }

        [Category("CPU")]
        [Description("CPU: кол-во ядер")]
        public int CoreCount { set; get; }

        [Category("CPU")]
        [Description("CPU: кол-во ядер включено")]
        public int CoreEnabled { set; get; }

        [Category("CPU")]
        [Description("CPU: кол-во потоков")]
        public int ThreadCount { set; get; }

        [Category("CPU")]
        [Description("CPU: частота")]
        public int ExternalClock { set; get; }

        public CPUInformation()
        {
            SlotLocator = string.Empty;
            ManufacturerName = string.Empty;
            Version = string.Empty;
            CoreCount = 0;
            CoreEnabled = 0;
            ThreadCount = 0;
            ExternalClock = 0;
        }
    }

}
