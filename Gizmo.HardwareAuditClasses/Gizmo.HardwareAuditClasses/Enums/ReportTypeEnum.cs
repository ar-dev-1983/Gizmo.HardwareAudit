using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses.Enums
{
    public enum ReportTypeEnum
    {
        [Description("")]
        None = 0,
        [Description("Computer components report")]
        ComputerComponentsReport = 1,
        [Description("Computer components quantity report")]
        ComputerComponentsQuantityReport = 2,
        //[Description("Computer information report")]
        //ComputerInformationReport = 3,
        //[Description("Active Directory information report")]
        //ActiveDirectoryInformationReport = 4,
    }
}
