namespace Gizmo.HardwareAudit.Enums
{
    /// <summary>
    /// Enumeration that specifies TreeItem status
    /// </summary>
    public enum ItemStatusEnum
    {
        /// <summary>
        /// Unknown status
        /// </summary>
        Unknown,
        /// <summary>
        /// Used for Containers
        /// </summary>
        Container,
        /// <summary>
        /// Indicates there is error
        /// </summary>
        Error,
        /// <summary>
        /// Network status is Online
        /// </summary>
        Online,
        /// <summary>
        /// Network status is Offline
        /// </summary>
        Offline,
        /// <summary>
        /// Network status is Online, but no hardware scan data was recived
        /// </summary>
        OnlineButHasNoData,
        /// <summary>
        /// Network status is Online, and hardware scan data now fetching from remote computer
        /// </summary>
        OnlineAndFetchingData,
        /// <summary>
        /// Network status is Online, and hardware scan data was recived
        /// </summary>
        OnlineAndHasData,
        /// <summary>
        /// Network status is Online, and hardware scan data was changed since last scanning
        /// </summary>
        OnlineAndDataHasChanged,
        /// <summary>
        /// Network status is Online, and now checking for avialable TCP ports from configured list
        /// </summary>
        OnlineAndCheckingTCPPorts,
        /// <summary>
        /// Network status is Online, and now checking for avialable shared folders or disks
        /// </summary>
        OnlineAndCheckingSharedFolders
    }
}
