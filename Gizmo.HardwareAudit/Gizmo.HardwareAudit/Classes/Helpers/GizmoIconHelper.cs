using Gizmo.HardwareAudit.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gizmo.HardwareAudit.Classes.Helpers
{
    public class GizmoIconHelper
    {
        public static List<GizmoIconEnum> GetIconsByCategory(string categoryName)
        {
            var result = new List<GizmoIconEnum>();
            foreach (var node in Enum.GetValues(typeof(GizmoIconEnum)))
            {
                if (((GizmoIconEnum)node).GetAttributeOfType<CategoryAttribute>().Category == categoryName)
                {
                    result.Add((GizmoIconEnum)node);
                }
            }
            return result;
        }
    }
}
