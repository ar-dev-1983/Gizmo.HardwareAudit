using Gizmo.HardwareAuditClasses.Helpers;
using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses.Enums
{
    public enum ComponentTypeEnum
    {
        [Description("")]
        [ReportType(ReportTypeEnum.None)]
        None = 0,

        [Description("BIOS Version")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        [ReportType(ReportTypeEnum.ComputerComponentsQuantityReport)]
        BIOSInformation = 1,

        [Description("CPU Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        [ReportType(ReportTypeEnum.ComputerComponentsQuantityReport)]
        CPUInformation = 2,

        [Description("Memory Device Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        [ReportType(ReportTypeEnum.ComputerComponentsQuantityReport)]
        MemoryDevice = 3,

        [Description("MotherBoard Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        [ReportType(ReportTypeEnum.ComputerComponentsQuantityReport)]
        MotherBoardInformation = 4,

        [Description("Network Adapter Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        [ReportType(ReportTypeEnum.ComputerComponentsQuantityReport)]
        NetworkAdapter = 5,

        [Description("Video Controller Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        [ReportType(ReportTypeEnum.ComputerComponentsQuantityReport)]
        VideoController = 6,

        [Description("Physical Drive Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        [ReportType(ReportTypeEnum.ComputerComponentsQuantityReport)]
        PhysicalDrive = 7,

        [Description("Partition Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        LogicalDrive = 8,

        [Description("Monitor Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        [ReportType(ReportTypeEnum.ComputerComponentsQuantityReport)]
        Monitor = 9,

        [Description("Printer Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        Printer = 10,

        [Description("System Enclosure Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        [ReportType(ReportTypeEnum.ComputerComponentsQuantityReport)]
        SystemInformation = 11,

        [Description("Windows OS Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        [ReportType(ReportTypeEnum.ComputerComponentsQuantityReport)]
        WindowsInformation = 12,

        [Description("Windows Local User Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        WindowsLocalUser = 13,

        [Description("Windows Local Group Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        WindowsLocalGroup = 14,

        [Description("Microsoft Product Licensing Information")]
        [ReportType(ReportTypeEnum.ComputerComponentsReport)]
        [ReportType(ReportTypeEnum.ComputerComponentsQuantityReport)]
        SoftwareLicensingProduct = 15,

        [Description("AD Computer Information")]
        [ReportType(ReportTypeEnum.None)]
        ActiveDirectoryComputerInfo = 16,

        [Description("AD Security Group Information")]
        [ReportType(ReportTypeEnum.None)]
        ActiveDirectoryGroupInfo = 17,

        [Description("AD User Information")]
        [ReportType(ReportTypeEnum.None)]
        ActiveDirectoryUserInfo = 18,

        [Description("Linux OS Information")]
        [ReportType(ReportTypeEnum.None)]
        LinuxInformation = 19,

        [Description("Linux Local User Information")]
        [ReportType(ReportTypeEnum.None)]
        LinuxLocalUser = 20,

        [Description("Linux Local Group Information")]
        [ReportType(ReportTypeEnum.None)]
        LinuxLocalGroup = 21,

        [Description("Computer Information")]
        [ReportType(ReportTypeEnum.None)]
        ComputerInformation = 22
    }
}
