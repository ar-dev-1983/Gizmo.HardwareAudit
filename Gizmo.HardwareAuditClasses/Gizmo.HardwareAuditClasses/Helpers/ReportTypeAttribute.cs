using Gizmo.HardwareAuditClasses.Enums;
using System;

namespace Gizmo.HardwareAuditClasses.Helpers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ReportTypeAttribute : Attribute
    {
        protected ReportTypeEnum ReportTypeValue { get; set; }
        public virtual ReportTypeEnum ReportType => ReportTypeValue;

        public ReportTypeAttribute()
        {
            ReportTypeValue = 0;
        }
        public ReportTypeAttribute(ReportTypeEnum reportTypeValue)
        {
            ReportTypeValue = reportTypeValue;
        }
    }
}
