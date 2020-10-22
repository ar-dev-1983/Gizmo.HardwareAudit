using System;

namespace Gizmo.HardwareAuditClasses.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ReportVisibilityAttribute : Attribute
    {
        protected bool VisibleValue { get; set; }
        public virtual bool Visible => VisibleValue;

        public ReportVisibilityAttribute()
        {
            VisibleValue = false;
        }
        public ReportVisibilityAttribute(bool visibleValue)
        {
            VisibleValue = visibleValue;
        }
    }
}
