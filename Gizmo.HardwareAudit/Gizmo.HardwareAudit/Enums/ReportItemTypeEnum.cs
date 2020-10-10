namespace Gizmo.HardwareAudit.Enums
{
    /// <summary>
    /// Enumeration that specifies TreeItem Type
    /// </summary>
    public enum ReportItemTypeEnum
    {
        /// <summary>
        /// None - initialization value
        /// </summary>
        None,
        /// <summary>
        /// Root - indicates that is Report Root
        /// </summary>
        Root,
        /// <summary>
        /// ActiveDirectory - indicates that is Active Directory Reports Read Only Item
        /// </summary>
        ActiveDirectory,
        /// <summary>
        /// Workgroup - indicates that is Workgroup Reports Read Only Item
        /// </summary>
        Workgroup,
        /// <summary>
        /// Container - container for other containers or items
        /// </summary>
        Container,
        /// <summary>
        /// Report - indicates that is Report Item
        /// </summary>
        Report,
    }
}
