using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses.Enums
{
    public enum ComponentGroupingTypeEnum
    {
        [Description("")]
        None = 0,
        [Description("Group by Source Container")]
        ByContainer = 1,
        [Description("Group by Instance")]
        ByInstance = 2
    }
}
