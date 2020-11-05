using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System;
using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.ActiveDirectoryComputerInfo)]
    public class ActiveDirectoryComputerInfo
    {
        [ReportVisibility(false)]
        public Guid Id { set; get; }

        [ReportVisibility(false)]
        public string CN { set; get; }

        [ReportVisibility(true)]
        [Description("AD Source")]
        public string SourceName { set; get; }

        [ReportVisibility(true)]
        [Description("Computer Name")]
        public string Name { set; get; }

        [ReportVisibility(true)]
        [Description("Computer Description")]
        public string Description { set; get; }

        [ReportVisibility(true)]
        [Description("Computer FQDN")]
        public string DNSHostName { set; get; }

        [ReportVisibility(false)]
        public string DistinguishedName { set; get; }

        [ReportVisibility(true)]
        [Description("When Created")]
        public string WhenCreated { set; get; }

        [ReportVisibility(true)]
        [Description("When Changed")]
        public string WhenChanged { set; get; }

        [ReportVisibility(true)]
        [Description("Last Logon")]
        public string LastLogon { set; get; }

        [ReportVisibility(true)]
        [Description("Operating System")]
        public string OperatingSystem { set; get; }

        [ReportVisibility(true)]
        [Description("LastLogon Timestamp")]
        public string LastLogonTimestamp { set; get; }

        [ReportVisibility(false)]
        public string ExtensionAttribute1 { set; get; }

        [ReportVisibility(false)]
        public string ExtensionAttribute2 { set; get; }

        [ReportVisibility(false)]
        public string ExtensionAttribute4 { set; get; }

        [ReportVisibility(false)]
        public string ExtensionAttribute6 { set; get; }

        [ReportVisibility(false)]
        public string ExtensionAttribute7 { set; get; }

        public ActiveDirectoryComputerInfo()
        {
            Id = Guid.NewGuid();
            SourceName = string.Empty;
            CN = string.Empty;
            Description = string.Empty;
            DistinguishedName = string.Empty;
            DNSHostName = string.Empty;
            WhenCreated = string.Empty;
            WhenChanged = string.Empty;
            Name = string.Empty;
            LastLogon = string.Empty;
            OperatingSystem = string.Empty;
            LastLogonTimestamp = string.Empty;

            ExtensionAttribute1 = string.Empty;
            ExtensionAttribute2 = string.Empty;
            ExtensionAttribute4 = string.Empty;
            ExtensionAttribute6 = string.Empty;
            ExtensionAttribute7 = string.Empty;
        }
    }
}
