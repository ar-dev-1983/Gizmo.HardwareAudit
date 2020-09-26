namespace Gizmo.HardwareAudit.Enums
{
    /// <summary>
    /// Enumeration that specifies TreeItem Type
    /// </summary>
    public enum ItemTypeEnum
    {
        /// <summary>
        /// None - initialisation value
        /// </summary>
        None,
        /// <summary>
        /// Root - indicates that is Model Root TreeItem
        /// </summary>
        Root,
        /// <summary>
        /// ActiveDirectory - indicates that is Active Directory Read Only Item
        /// </summary>
        ActiveDirectory,
        /// <summary>
        /// Workgroup - indicates that is Workgroup Read Only Item
        /// </summary>
        Workgroup,
        /// <summary>
        /// ChildComputer - computer
        /// </summary>
        ChildComputer,
        /// <summary>
        /// ChildContainer - container for other containers or computers
        /// </summary>
        ChildContainer,
        /// <summary>
        /// ChildDevice - unknown device (for fututre purpuses)
        /// </summary>
        ChildDevice,
        /// <summary>
        /// DomainRoot - domain root item, represents active directory domain
        /// </summary>
        DomainRoot,
        /// <summary>
        /// DomainSite - domain site item (for fututre purpuses)
        /// </summary>
        DomainSite,
    }
}
