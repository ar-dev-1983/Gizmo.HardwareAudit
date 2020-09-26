namespace Gizmo.HardwareAudit.Enums

{ /// <summary>
  /// Enumeration that specifies DomainDiscovery enumeration mode
  /// </summary>
    public enum DomainDiscoveryModeEnum
    {
        /// <summary>
        /// Enumerate information from Active Directory usign Domain Name or Forest Name
        /// </summary>
        FromDomainName,
        /// <summary>
        /// Enumerate information from Active Directory usign FQDN of domain controller server
        /// </summary>
        FromDomainController
    }
}
