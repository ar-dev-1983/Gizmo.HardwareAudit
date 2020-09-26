namespace Gizmo.HardwareAudit.Enums
{
    /// <summary>
    /// Enumeration that specifies Probe action Type
    /// </summary>
    public enum ProbeTypeEnum
    {
        /// <summary>
        /// Perform network Ping action
        /// </summary>
        Ping,
        /// <summary>
        /// Perform check for TCP Ports action
        /// </summary>
        CheckTCPPort,
        /// <summary>
        /// Perform check for shared folders action
        /// </summary>
        CheckSharedFolders,
        /// <summary>
        /// Perform hardware scan action
        /// </summary>
        CheckWMI
    }
}
