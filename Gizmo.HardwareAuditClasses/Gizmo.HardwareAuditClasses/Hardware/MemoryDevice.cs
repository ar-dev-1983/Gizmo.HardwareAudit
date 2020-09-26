using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Gizmo.HardwareAuditClasses
{
    public class MemoryDevice
    {
        [Category("Memory")]
        [Description("Оперативная память: источник")]
        public string DeviceLocator { set; get; }

        [Category("Memory")]
        [Description("Оперативная память: слот")]
        public string BankLocator { set; get; }

        [Category("Memory")]
        [Description("Memory: производитель")]
        public string ManufacturerName { set; get; }

        [Category("Memory")]
        [Description("Оперативная память: серийный номер")]
        public string SerialNumber { set; get; }

        [Category("Memory")]
        [Description("Оперативная память: номер партии")]
        public string PartNumber { set; get; }

        [Category("Memory")]
        [Description("Оперативная память: частота")]
        public int Speed { set; get; }

        public int MemoryType { set; get; }

        [JsonIgnore]
        [Category("Memory")]
        [Description("Оперативная память: тип памяти")]
        public string MemoryTypeString
        {
            get
            {
                var mt = new List<string>() { "Unknown", "Other", "Unknown", "DRAM", "EDRAM", "VRAM", "SRAM", "RAM", "ROM", "FLASH", "EEPROM", "FEPROM", "EPROM", "CDRAM", "3DRAM", "SDRAM", "SGRAM", "RDRAM", "DDR", "DDR2", "DDR2 FB-DIMM", "Reserved", "Reserved", "Reserved", "DDR3", "FBD2", "DDR4", "LPDDR", "LPDDR2", "LPDDR3", "LPDDR4" };
                return Size != 0 ? mt[MemoryType] : string.Empty;
            }
        }

        public int FormFactor { set; get; }

        [JsonIgnore]
        [Category("Memory")]
        [Description("Оперативная память: тип")]
        public string FormFactorString
        {
            get
            {
                var ff = new List<string>() { "Unknown", "Other", "Unknown", "SIMM", "SIP", "Chip", "DIP", "ZIP", "Proprietary Card", "DIMM", "TSOP", "Row of chips", "RIMM", "SODIMM", "SRIMM", "FB-DIMM" };
                return Size != 0 ? ff[FormFactor] : string.Empty;
            }
        }

        public int Size { set; get; }

        [JsonIgnore]
        [Category("Memory")]
        [Description("Оперативная память: объем")]
        public string SizeString
        {
            get
            {
                return Size != 0 ? (Size / 1024) + " Gb" : string.Empty;
            }
        }

        [Category("Memory")]
        [Description("Оперативная память: модель")]
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
