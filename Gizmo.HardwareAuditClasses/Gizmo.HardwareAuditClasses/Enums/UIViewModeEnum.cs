using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses.Enums
{
    public enum UIViewModeEnum
    {
        [Description("Show full scan")]
        All,
        [Description("Show System Enclosure")]
        SystemEnclosure,
        [Description("Show CPU")]
        CPUs,
        [Description("Show Memory Devices")]
        MemoryDevices,
        [Description("Show Video Controllers")]
        VideoControllers,
        [Description("Show Displays")]
        Displays,
        [Description("Show Network Adapters")]
        NetworkAdapters,
        [Description("Show Physical Drives")]
        PhysicalDrives,
        [Description("Show Partitions")]
        Partitions,
        [Description("Show Licenses")]
        Licenses,
        [Description("Show Printers")]
        Printers,
        [Description("Show Local Users")]
        LocalUsers,
        [Description("Show Local Groups")]
        LocalGroups
    }
}
