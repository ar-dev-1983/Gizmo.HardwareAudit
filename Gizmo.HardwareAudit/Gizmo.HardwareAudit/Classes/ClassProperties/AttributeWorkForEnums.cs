using Gizmo.HardwareAuditClasses.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Gizmo.HardwareAudit.Classes.Helpers
{
    public static class AttributeWorkForEnums
    {
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : Attribute
        {
            var memberInfos = enumVal.GetType().GetMember(enumVal.ToString());
            var attributes = memberInfos[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
        public static List<T> GetAttributesOfType<T>(this Enum enumVal) where T : Attribute
        {
            var result = new List<T>();
            var memberInfos = enumVal.GetType().GetMember(enumVal.ToString());
            var attributes = memberInfos[0].GetCustomAttributes(typeof(T), false);
            foreach (var node in attributes)
            {
                result.Add((T)node);
            }
            return result;
        }

        public static ObservableCollection<EnumProperies> Enumerate(Type type, object excludeValue)
        {
            var result = new ObservableCollection<EnumProperies>();
            foreach (var node in type.GetEnumValues())
            {
                if (!Object.Equals(node, excludeValue))
                {
                    if ((node as Enum).GetAttributeOfType<DescriptionAttribute>() != null)
                    {
                        result.Add(new EnumProperies() { Value = (Enum)node, Description = (node as Enum).GetAttributeOfType<DescriptionAttribute>().Description });
                    }
                }
            }
            return result;
        }
        public static ObservableCollection<EnumProperies> Enumerate(Type type, object excludeValue, object filterValue)
        {
            var result = new ObservableCollection<EnumProperies>();
            foreach (var node in type.GetEnumValues())
            {
                if (!Object.Equals(node, excludeValue))
                {
                    if ((node as Enum).GetAttributeOfType<DescriptionAttribute>() != null)
                    {
                        if ((node as Enum).GetAttributesOfType<ComponentGroupingTypeAttribute>() != null)
                        {
                            if ((node as Enum).GetAttributesOfType<ComponentGroupingTypeAttribute>().Count != 0)
                            {
                                if ((node as Enum).GetAttributesOfType<ComponentGroupingTypeAttribute>().Where(x => Object.Equals(x.ComponentGroupingType, filterValue)).Count() != 0)
                                {
                                    result.Add(new EnumProperies() { Value = (Enum)node, Description = (node as Enum).GetAttributeOfType<DescriptionAttribute>().Description });
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
