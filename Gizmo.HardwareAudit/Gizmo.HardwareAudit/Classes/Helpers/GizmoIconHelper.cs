using Gizmo.HardwareAuditWPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gizmo.HardwareAudit.Classes.Helpers
{
    public class GizmoIconHelper
    {
        public static List<GizmoComputerHardwareIconsEnum> GetIconsByCategory(string categoryName)
        {
            var result = new List<GizmoComputerHardwareIconsEnum>();
            foreach (var node in Enum.GetValues(typeof(GizmoComputerHardwareIconsEnum)))
            {
                if (((GizmoComputerHardwareIconsEnum)node).GetAttributeOfType<CategoryAttribute>().Category == categoryName)
                {
                    result.Add((GizmoComputerHardwareIconsEnum)node);
                }
            }
            return result;
        }
    }
}
