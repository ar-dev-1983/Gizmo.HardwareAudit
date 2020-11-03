using Gizmo.HardwareAuditClasses.Helpers;
using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses.Enums
{
    public enum ComponentTypeEnum
    {
        [Description("")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.None)]
        None = 0,

        [Description("BIOS Version")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByInstance)]
        BIOSInformation = 1,

        [Description("CPU Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByInstance)]
        CPUInformation = 2,

        [Description("Memory Device Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByInstance)]
        MemoryDevice = 3,

        [Description("MotherBoard Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByInstance)]
        MotherBoardInformation = 4,

        [Description("Network Adapter Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByInstance)]
        NetworkAdapter = 5,

        [Description("Video Controller Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByInstance)]
        VideoController = 6,

        [Description("Physical Drice Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByInstance)]
        PhysicalDrive = 7,

        [Description("Partition Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        LogicalDrive = 8,

        [Description("Monitor Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByInstance)]
        Monitor = 9,

        [Description("Printer Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        Printer = 10,

        [Description("System Enclosure Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByInstance)]
        SystemInformation = 11,

        [Description("Windows OS Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByInstance)]
        WindowsInformation = 12,

        [Description("Windows Local User Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        WindowsLocalUser = 13,

        [Description("Windows Local Group Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        WindowsLocalGroup = 14,

        [Description("Microsoft Product Licensing Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByInstance)]
        SoftwareLicensingProduct = 15,

        [Description("AD Computer Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        ActiveDirectoryComputerInfo = 16,

        [Description("AD Security Group Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        ActiveDirectoryGroupInfo = 17,

        [Description("AD User Information")]
        [ComponentGroupingType(ComponentGroupingTypeEnum.ByContainer)]
        ActiveDirectoryUserInfo = 18,

        //[Description("Linux OS Information")]
        //[ComponentGroupingType(ComponentGroupingTypeEnum.None)]
        //LinuxInformation = 19,

        //[Description("Linux Local User Information")]
        //[ComponentGroupingType(ComponentGroupingTypeEnum.None)]
        //LinuxLocalUser = 20,

        //[Description("Linux Local Group Information")]
        //[ComponentGroupingType(ComponentGroupingTypeEnum.None)]
        //LinuxLocalGroup = 21
    }
}
