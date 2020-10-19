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
        /// Container - container for other containers or items
        /// </summary>
        Container,
        /// <summary>
        /// Report - indicates that is Report Item
        /// </summary>
        Report,
    }
}
