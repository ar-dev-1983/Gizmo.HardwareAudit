using Gizmo.HardwareAuditClasses.Enums;
using System;

namespace Gizmo.HardwareAuditClasses.Helpers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ComponentTypeAttribute : Attribute
    {
        protected ComponentTypeEnum ComponentTypeValue { get; set; }
        public virtual ComponentTypeEnum ComponentType => ComponentTypeValue;

        public ComponentTypeAttribute()
        {
            ComponentTypeValue = 0;
        }
        public ComponentTypeAttribute(ComponentTypeEnum componentTypeValue)
        {
            ComponentTypeValue = componentTypeValue;
        }
    }
}
