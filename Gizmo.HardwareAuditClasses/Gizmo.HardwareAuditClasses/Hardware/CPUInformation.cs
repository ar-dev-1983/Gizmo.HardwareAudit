﻿using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.CPUInformation)]
    public class CPUInformation
    {
        [ReportVisibility(true)]
        [Description("CPU Slot")]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public string SlotLocator { set; get; }

        [ReportVisibility(true)]
        [Description("CPU Manufacturer")]
        [FieldType(FieldTypeEnum.KeyToGroupAndSort)]
        public string ManufacturerName { set; get; }

        [ReportVisibility(true)]
        [Description("CPU Version")]
        [FieldType(FieldTypeEnum.KeyToGroupAndSort)]
        public string Version { set; get; }

        [ReportVisibility(true)]
        [Description("CPU Core Count")]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public int CoreCount { set; get; }

        [ReportVisibility(false)]
        [Description("CPU Core Enabled")]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public int CoreEnabled { set; get; }

        [ReportVisibility(true)]
        [Description("CPU Thread Count")]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public int ThreadCount { set; get; }

        [ReportVisibility(false)]
        [Description("CPU External Clock")]
        [FieldType(FieldTypeEnum.KeyToSort)]
        public int ExternalClock { set; get; }

        public CPUInformation()
        {
            SlotLocator = string.Empty;
            ManufacturerName = string.Empty;
            Version = string.Empty;
            CoreCount = 0;
            CoreEnabled = 0;
            ThreadCount = 0;
            ExternalClock = 0;
        }
    }

}
