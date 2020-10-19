using System;
using System.Collections.Generic;
using System.Text;

namespace Gizmo.HardwareAudit.Classes.Helpers
{
    public static class EnumAttributeHelper
    {
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var memberInfos = enumVal.GetType().GetMember(enumVal.ToString());
            var attributes = memberInfos[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
