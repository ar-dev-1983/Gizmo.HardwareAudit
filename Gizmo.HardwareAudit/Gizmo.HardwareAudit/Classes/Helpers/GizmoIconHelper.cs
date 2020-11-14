using Gizmo.HardwareAuditWPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gizmo.HardwareAudit.Classes.Helpers
{
    public class GizmoIconHelper
    {
        public static List<GizmiComputerHardwareIconsEnum> GetIconsByCategory(string categoryName)
        {
            var result = new List<GizmiComputerHardwareIconsEnum>();
            foreach (var node in Enum.GetValues(typeof(GizmiComputerHardwareIconsEnum)))
            {
                if (((GizmiComputerHardwareIconsEnum)node).GetAttributeOfType<CategoryAttribute>().Category == categoryName)
                {
                    result.Add((GizmiComputerHardwareIconsEnum)node);
                }
            }
            return result;
        }
    }
}
