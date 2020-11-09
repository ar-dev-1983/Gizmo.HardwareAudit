using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.MemoryDevice)]
    public class MemoryDevice
    {
        private List<string> formFactorStrings = new List<string>() { "Unknown", "Other", "Unknown", "SIMM", "SIP", "Chip", "DIP", "ZIP", "Proprietary Card", "DIMM", "TSOP", "Row of chips", "RIMM", "SODIMM", "SRIMM", "FB-DIMM" };
        private List<string> memoryTypeStrings = new List<string>() { "Unknown", "Other", "Unknown", "DRAM", "EDRAM", "VRAM", "SRAM", "RAM", "ROM", "FLASH", "EEPROM", "FEPROM", "EPROM", "CDRAM", "3DRAM", "SDRAM", "SGRAM", "RDRAM", "DDR", "DDR2", "DDR2 FB-DIMM", "Reserved", "Reserved", "Reserved", "DDR3", "FBD2", "DDR4", "LPDDR", "LPDDR2", "LPDDR3", "LPDDR4" };

        [ReportVisibility(true)]
        [Description("RAM Device Locator")]
        public string DeviceLocator { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Bank Locator")]
        public string BankLocator { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Manufacturer")]
        public string ManufacturerName { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Serial Number")]
        public string SerialNumber { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Part Number")]
        public string PartNumber { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Speed")]
        public int Speed { set; get; }

        [ReportVisibility(false)]
        public int MemoryType { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Memory Type")]
        public string MemoryTypeString
        {
            get => Size != 0 ? memoryTypeStrings[MemoryType] : string.Empty;
            set => _ = Size != 0 ? memoryTypeStrings[MemoryType] : string.Empty;
        }

        [ReportVisibility(false)]
        public int FormFactor { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Form Factor")]
        public string FormFactorString
        {
            get => Size != 0 ? formFactorStrings[FormFactor] : string.Empty;
            set => _ = Size != 0 ? formFactorStrings[FormFactor] : string.Empty;
        }

        [ReportVisibility(false)]
        public int Size { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Size")]
        public string SizeString
        {
            get => Size != 0 ? (Size / 1024) + " Gb" : string.Empty;
            set => _ = Size != 0 ? (Size / 1024) + " Gb" : string.Empty;
        }


        [ReportVisibility(true)]
        [Description("RAM Asset Tag")]
        public string AssetTag { set; get; }

        public MemoryDevice()
        {
            DeviceLocator = string.Empty;
            BankLocator = string.Empty;
            ManufacturerName = string.Empty;
            SerialNumber = string.Empty;
            PartNumber = string.Empty;
            Speed = 0;
            MemoryType = 0;
            FormFactor = 0;
            Size = 0;
            AssetTag = string.Empty;
        }
    }

}
