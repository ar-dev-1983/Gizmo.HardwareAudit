using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses.Enums
{
    public enum ReportTypeEnum
    {
        [Description("")]
        None = 0,
        [Description("Computer information report")]
        ComputerInformationReport = 1,
        [Description("Computer components report")]
        ComputerComponentsReport = 2,
        [Description("Computer components quantity report")]
        ComputerComponentsQuantityReport = 3,
        [Description("Active Directory information report")]
        ActiveDirectoryInformationReport = 4,
    }
}
