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
        [Description("AD Source")]
        public string SourceName { set; get; }

        [ReportVisibility(true)]
        [Description("Group Name")]
        public string Name { set; get; }

        [ReportVisibility(true)]
        [Description("Group Description")]
        public string Description { set; get; }

        [ReportVisibility(false)]
        public string DistinguishedName { set; get; }

        [ReportVisibility(false)]
        public List<string> Members { set; get; }

        [ReportVisibility(true)]
        [Description("Group Members")]
        public string MembersInOneLine => Members != null ? Members.Count != 0 ? string.Join("\n", Members) : string.Empty : string.Empty;

        public ActiveDirectoryGroupInfo()
        {
            Id = Guid.NewGuid();
            SourceName = string.Empty;

            Name = string.Empty;
            Description = string.Empty;

            DistinguishedName = string.Empty;
            Members = new List<string>();
        }
    }
}
