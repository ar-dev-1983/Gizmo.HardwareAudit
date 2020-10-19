using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Models;
using System;

namespace Gizmo.HardwareAudit.Interfaces
{
    public interface ITreeItemDialog
    {
        string Name { get; set; }
        string Description { get; set; }
        string FQDN { get; set; }
        string IPAddress { get; set; }
        string DomainControllerName { get; set; }
        bool UseParentUserProfile { get; set; }
        Guid UserProfileId { get; set; }
        DomainDiscoverySettings DomainSettings { set; get; }
        Guid SelectedContainerId { set; get; }

        bool ContainerSettingsDialog(AppSettings settings);
        bool ContainerSettingsDialog(AppSettings settings, string name, string desc, bool useParentId, Guid profileId);
        bool ActiveDirectorySettingsDialog(AppSettings settings);
        bool ActiveDirectorySettingsDialog(AppSettings settings, DomainDiscoverySettings domainDiscoverySettings);
        bool ComputerSettingsDialog(AppSettings settings);
        bool ComputerSettingsDialog(AppSettings settings, string name, string desc, string fqdn, string address, bool useParentId, Guid profileId);
        bool ChooseContainerDialog(AppSettings settings, TreeItem treeItem, bool useSelectedParentId);

        bool UseCustomIcon { get; set; }
        GizmoIconEnum CustomIcon { set; get; }

        bool ReportContainerSettingsDialog(AppSettings settings);
        bool ReportContainerSettingsDialog(AppSettings settings, string name, string desc, bool useCustomIcon, GizmoIconEnum customIcon);

    }
}
