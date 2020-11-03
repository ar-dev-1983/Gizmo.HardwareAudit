using Gizmo.HardwareAuditClasses.Enums;

namespace Gizmo.HardwareAudit.Classes.Helpers
{
    public class ClassProperties
    {
        public bool IsSelected { set; get; }
        public string SourceClassTypeName { set; get; }
        public string PropertyDescription { set; get; }
        public string PropertyKey { set; get; }
        public string PropertyBindingKey { set; get; }
        public string PropertyValueType { set; get; }
        public FieldTypeEnum FieldType { set; get; }

        public ClassProperties()
        {
            IsSelected = true;
            SourceClassTypeName = string.Empty;
            PropertyDescription = string.Empty;
            PropertyKey = string.Empty;
            PropertyBindingKey = string.Empty;
            PropertyValueType = string.Empty;
            FieldType = FieldTypeEnum.None;
        }
    }
}
