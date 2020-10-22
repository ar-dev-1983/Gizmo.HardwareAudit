using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.ActiveDirectoryGroupInfo)]
    public class ActiveDirectoryGroupInfo
    {

        [ReportVisibility(false)]
        public Guid Id { set; get; }

        [ReportVisibility(true)]
        [Description("Имя группы")]
        public string Name { set; get; }

        [ReportVisibility(true)]
        [Description("Описание группы")]
        public string Description { set; get; }

        [ReportVisibility(false)]
        public string DistinguishedName { set; get; }

        [ReportVisibility(true)]
        [Description("Члены группы")]
        public List<string> Members { set; get; }

        public ActiveDirectoryGroupInfo()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;

            DistinguishedName = string.Empty;
            Members = new List<string>();
        }
    }
}
