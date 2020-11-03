using System;

namespace Gizmo.HardwareAudit.Classes.Helpers
{
    public class EnumProperies
    {
        public string Description { set; get; }
        public Enum Value { set; get; }


        public EnumProperies()
        {
            Description = string.Empty;
            Value = null;
        }
    }

}
