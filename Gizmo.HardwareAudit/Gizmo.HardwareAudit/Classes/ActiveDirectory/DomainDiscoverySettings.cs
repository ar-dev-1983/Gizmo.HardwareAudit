using Gizmo.HardwareAudit.Enums;
using System;

namespace Gizmo.HardwareAudit
{
    public class DomainDiscoverySettings
    {
        public Guid Id { set; get; }
        public DomainDiscoveryModeEnum Mode { set; get; }
        public string Name { set; get; }
        public Guid UserProfileId { set; get; }

        public DomainDiscoverySettings()
        {
            Id = Guid.NewGuid();
            Mode = DomainDiscoveryModeEnum.FromDomainName;
            Name = string.Empty;
            UserProfileId = new Guid();
        }

        public DomainDiscoverySettings(DomainDiscoveryModeEnum mode, string name, Guid userProfileId)
        {
            Mode = mode;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            UserProfileId = userProfileId;
        }
    }
}
