using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Gizmo.HardwareAudit.Classes.Helpers
{
    public class AttributeWorkForClass
    {
        public static ObservableCollection<ClassProperties> Enumerate(ComponentTypeEnum component)
        {
            Assembly assembly = typeof(ComponentTypeAttribute).Assembly;
            var result = new ObservableCollection<ClassProperties>();
            foreach (var typeNode in assembly.DefinedTypes)
            {
                if (typeNode.GetCustomAttribute(typeof(ComponentTypeAttribute)) != null)
                {
                    if ((ComponentTypeEnum)typeNode.CustomAttributes.Where(x => x.AttributeType == typeof(ComponentTypeAttribute)).First().ConstructorArguments[0].Value == component)
                    {
                        var Properties = typeNode.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var node in Properties)
                        {
                            if (node.GetCustomAttribute(typeof(ReportVisibilityAttribute)) != null)
                            {
                                if (!(bool)node.GetCustomAttributesData().Where(x => x.AttributeType == typeof(ReportVisibilityAttribute)).First().ConstructorArguments[0].Value)
                                {
                                    continue;
                                }
                                else
                                {
                                    var property = new ClassProperties()
                                    {
                                        SourceClassTypeName = typeNode.Name,
                                        PropertyKey = node.Name,
                                        PropertyBindingKey = typeNode.Name + "_" + node.Name,
                                        PropertyValueType = node.PropertyType.Name
                                    };
                                    foreach (var anode in node.CustomAttributes)
                                    {
                                        if (anode.AttributeType == typeof(DescriptionAttribute))
                                        {
                                            property.PropertyDescription = anode.ConstructorArguments[0].Value.ToString();
                                        }
                                        else if (anode.AttributeType == typeof(FieldTypeAttribute))
                                        {
                                            property.FieldType = (FieldTypeEnum)anode.ConstructorArguments[0].Value;
                                        }
                                    }
                                    result.Add(property);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static List<ClassProperties> Enumerate(Type TypeName)
        {
            var result = new List<ClassProperties>();
            var Properties = TypeName.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var node in Properties)
            {
                foreach (var anode in node.CustomAttributes)
                {
                    if (anode.AttributeType == typeof(DescriptionAttribute))
                    {
                        result.Add(new ClassProperties()
                        {
                            SourceClassTypeName = TypeName.Name,
                            PropertyDescription = anode.ConstructorArguments[0].Value.ToString(),
                            PropertyKey = node.Name,
                            PropertyBindingKey = TypeName.Name + "_" + node.Name,
                            PropertyValueType = node.PropertyType.Name
                        });
                    }
                }

            }
            return result;
        }

        public static List<ClassProperties> Enumerate(Type TypeName, bool selected)
        {
            var result = new List<ClassProperties>();
            var Properties = TypeName.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var node in Properties)
            {
                foreach (var anode in node.CustomAttributes)
                {
                    if (anode.AttributeType == typeof(DescriptionAttribute))
                    {
                        result.Add(new ClassProperties()
                        {
                            IsSelected = selected,
                            SourceClassTypeName = TypeName.Name,
                            PropertyDescription = anode.ConstructorArguments[0].Value.ToString(),
                            PropertyKey = node.Name,
                            PropertyBindingKey = TypeName.Name + "_" + node.Name,
                            PropertyValueType = node.PropertyType.Name
                        });
                    }
                }

            }
            return result;
        }

        public static List<ClassProperties> Enumerate(Type TypeName, bool selected, int MaxElements)
        {
            var result = new List<ClassProperties>();
            var Properties = TypeName.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            for (int i = 0; i < MaxElements; i++)
            {
                foreach (var node in Properties)
                {
                    foreach (var anode in node.CustomAttributes)
                    {
                        if (anode.AttributeType == typeof(DescriptionAttribute))
                        {
                            result.Add(new ClassProperties()
                            {
                                IsSelected = selected,
                                SourceClassTypeName = TypeName.Name,
                                PropertyDescription = anode.ConstructorArguments[0].Value.ToString() + " №" + (i + 1).ToString(),
                                PropertyKey = node.Name,
                                PropertyBindingKey = TypeName.Name + "_" + node.Name + " №" + (i + 1).ToString(),
                                PropertyValueType = node.PropertyType.Name
                            });
                        }
                    }
                }
            }
            return result;
        }
    }

}
