using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.MemoryDevice)]
    public class MemoryDevice
    {
        [ReportVisibility(true)]
        [Description("RAM Device Locator")]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string DeviceLocator { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Bank Locator")]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string BankLocator { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Manufacturer")]
        [FieldType(FieldTypeEnum.KeyToGroupAndSort)]
        public string ManufacturerName { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Serial Number")]
        [FieldType(FieldTypeEnum.KeyToIgnore)]
        public string SerialNumber { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Part Number")]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string PartNumber { set; get; }

        [ReportVisibility(true)]
        [Description("RAM Speed")]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public int Speed { set; get; }

        [ReportVisibility(false)]
        public int MemoryType { set; get; }

        [JsonIgnore]
        [ReportVisibility(true)]
        [Description("RAM Memory Type")]
        [FieldType(FieldTypeEnum.KeyToGroupAndSort)]
        public string MemoryTypeString
        {
            get
            {
                var mt = new List<string>() { "Unknown", "Other", "Unknown", "DRAM", "EDRAM", "VRAM", "SRAM", "RAM", "ROM", "FLASH", "EEPROM", "FEPROM", "EPROM", "CDRAM", "3DRAM", "SDRAM", "SGRAM", "RDRAM", "DDR", "DDR2", "DDR2 FB-DIMM", "Reserved", "Reserved", "Reserved", "DDR3", "FBD2", "DDR4", "LPDDR", "LPDDR2", "LPDDR3", "LPDDR4" };
                return Size != 0 ? mt[MemoryType] : string.Empty;
            }
        }

        [ReportVisibility(false)]
        public int FormFactor { set; get; }

        [JsonIgnore]
        [ReportVisibility(true)]
        [Description("RAM Form Factor")]
        [FieldType(FieldTypeEnum.KeyToGroupAndSort)]
        public string FormFactorString
        {
            get
            {
                var ff = new List<string>() { "Unknown", "Other", "Unknown", "SIMM", "SIP", "Chip", "DIP", "ZIP", "Proprietary Card", "DIMM", "TSOP", "Row of chips", "RIMM", "SODIMM", "SRIMM", "FB-DIMM" };
                return Size != 0 ? ff[FormFactor] : string.Empty;
            }
        }

        [ReportVisibility(false)]
        public int Size { set; get; }

        [JsonIgnore]
        [ReportVisibility(true)]
        [Description("RAM Size")]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string SizeString
        {
            get
            {
                return Size != 0 ? (Size / 1024) + " Gb" : string.Empty;
            }
        }

        [ReportVisibility(true)]
        [Description("RAM Asset Tag")]
        [FieldType(FieldTypeEnum.KeyToSort)]
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
