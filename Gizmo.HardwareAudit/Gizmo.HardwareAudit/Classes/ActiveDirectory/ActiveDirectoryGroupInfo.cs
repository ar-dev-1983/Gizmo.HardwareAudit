using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices;

namespace Gizmo.HardwareAudit
{
    public class ActiveDirectoryGroupInfo
    {
        public Guid Id { set; get; }
        [Description("Имя группы")]
        public string Name { set; get; }
        [Description("Описание группы")]
        public string Description { set; get; }

        public string DistinguishedName { set; get; }

        public List<string> Members { set; get; }

        public ActiveDirectoryGroupInfo()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Description = string.Empty;

            DistinguishedName = string.Empty;
            Members = new List<string>();
        }

        public ActiveDirectoryGroupInfo(DirectoryEntry directoryEntry)
        {
            Id = Guid.NewGuid();
            Name = directoryEntry.Properties["cn"].Value != null ? directoryEntry.Properties["cn"].Value.ToString() : string.Empty;
            Description = directoryEntry.Properties["description"].Value != null ? directoryEntry.Properties["description"].Value.ToString() : string.Empty;
            DistinguishedName = directoryEntry.Properties["distinguishedName"].Value != null ? directoryEntry.Properties["distinguishedName"].Value.ToString() : string.Empty;
            Members = new List<string>();
        }
    }
}
