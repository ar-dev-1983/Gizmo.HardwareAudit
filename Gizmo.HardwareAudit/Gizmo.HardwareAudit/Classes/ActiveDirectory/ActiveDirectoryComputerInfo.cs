using System;
using System.DirectoryServices;

namespace Gizmo.HardwareAudit
{
    public class ActiveDirectoryComputerInfo
    {
        public Guid Id { set; get; }
        public string CN { set; get; }
        public string Description { set; get; }
        public string DistinguishedName { set; get; }
        public string DNSHostName { set; get; }
        public string WhenCreated { set; get; }
        public string WhenChanged { set; get; }
        public string Name { set; get; }
        public string LastLogon { set; get; }
        public string OperatingSystem { set; get; }
        public string LastLogonTimestamp { set; get; }

        public string ExtensionAttribute1 { set; get; }
        public string ExtensionAttribute2 { set; get; }
        public string ExtensionAttribute4 { set; get; }
        public string ExtensionAttribute6 { set; get; }
        public string ExtensionAttribute7 { set; get; }

        public ActiveDirectoryComputerInfo()
        {
            Id = Guid.NewGuid();
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

        public ActiveDirectoryComputerInfo(DirectoryEntry directoryEntry)
        {
            Id = Guid.NewGuid();
            CN = string.Empty;
            Description = directoryEntry.Properties["description"].Value != null ? directoryEntry.Properties["description"].Value.ToString() : string.Empty;
            DistinguishedName = directoryEntry.Properties["distinguishedName"].Value != null ? directoryEntry.Properties["distinguishedName"].Value.ToString() : string.Empty;
            DNSHostName = directoryEntry.Properties["dNSHostName"].Value != null ? directoryEntry.Properties["dNSHostName"].Value.ToString().ToLower() : string.Empty;
            WhenCreated = string.Empty;
            WhenChanged = string.Empty;
            Name = directoryEntry.Properties["name"].Value != null ? directoryEntry.Properties["name"].Value.ToString().ToLower() : string.Empty;
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
