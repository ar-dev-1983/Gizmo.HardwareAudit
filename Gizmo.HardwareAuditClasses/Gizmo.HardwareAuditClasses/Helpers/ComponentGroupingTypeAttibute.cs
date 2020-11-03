using Gizmo.HardwareAuditClasses.Enums;
using System;

namespace Gizmo.HardwareAuditClasses.Helpers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ComponentGroupingTypeAttribute : Attribute
    {
        protected ComponentGroupingTypeEnum ComponentGroupingTypeValue { get; set; }
        public virtual ComponentGroupingTypeEnum ComponentGroupingType => ComponentGroupingTypeValue;

        public ComponentGroupingTypeAttribute()
        {
            ComponentGroupingTypeValue = 0;
        }
        public ComponentGroupingTypeAttribute(ComponentGroupingTypeEnum componentGroupingTypeValue)
        {
            ComponentGroupingTypeValue = componentGroupingTypeValue;
        }
    }
}
