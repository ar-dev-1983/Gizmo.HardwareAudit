using Gizmo.HardwareAuditClasses.Enums;
using System;

namespace Gizmo.HardwareAuditClasses.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FieldTypeAttribute : Attribute
    {
        protected FieldTypeEnum FieldTypeValue { get; set; }
        public virtual FieldTypeEnum FieldType => FieldTypeValue;

        public FieldTypeAttribute()
        {
            FieldTypeValue = 0;
        }
        public FieldTypeAttribute(FieldTypeEnum fieldTypeValue)
        {
            FieldTypeValue = fieldTypeValue;
        }
    }
}
