using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.ViewModels;
using System;

namespace Gizmo.HardwareAudit.Services
{
    public class TreeItemDialogService : ITreeItemDialog
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string FQDN { get; set; }
        public string IPAddress { get; set; }
        public string DomainControllerName { get; set; }
        public bool UseParentUserProfile { get; set; }
        public Guid UserProfileId { get; set; }
        public DomainDiscoverySettings DomainSettings { set; get; }
        public Guid SelectedContainerId { set; get; }

        public bool ContainerSettingsDialog(AppSettings settings)
        {
            ContainerDialog dialog = new ContainerDialog(settings);
            switch (dialog.ShowDialog())
            {
                case true:
                    Name = (dialog.DataContext as ContainerSettingsViewModel).ContainerName;
                    Description = (dialog.DataContext as ContainerSettingsViewModel).ContainerDescription;
                    UseParentUserProfile = (dialog.DataContext as ContainerSettingsViewModel).UseParentUserProfile;
                    UserProfileId = (dialog.DataContext as ContainerSettingsViewModel).UserProfileId;
                    return true;
                default:
                    return false;
            }
        }

        public bool ContainerSettingsDialog(AppSettings settings, string name, string desc, bool useParentId, Guid profileId)
        {
            ContainerDialog dialog = new ContainerDialog(settings, name, desc, useParentId, profileId);
            switch (dialog.ShowDialog())
            {
                case true:
                    Name = (dialog.DataContext as ContainerSettingsViewModel).ContainerName;
                    Description = (dialog.DataContext as ContainerSettingsViewModel).ContainerDescription;
                    UseParentUserProfile = (dialog.DataContext as ContainerSettingsViewModel).UseParentUserProfile;
                    UserProfileId = (dialog.DataContext as ContainerSettingsViewModel).UserProfileId;
                    return true;
                default:
                    return false;
            }
        }

        public bool ActiveDirectorySettingsDialog(AppSettings settings)
        {
            ActiveDirectoryDialog dialog = new ActiveDirectoryDialog(settings);
            switch (dialog.ShowDialog())
            {
                case true:
                    DomainSettings = (dialog.DataContext as DomainDiscoverySettingsViewModel).Settings;
                    return true;
                default:
                    return false;
            }
        }

        public bool ActiveDirectorySettingsDialog(AppSettings settings, DomainDiscoverySettings domainDiscoverySettings)
        {
            ActiveDirectoryDialog dialog = new ActiveDirectoryDialog(settings, domainDiscoverySettings);
            switch (dialog.ShowDialog())
            {
                case true:
                    DomainSettings = (dialog.DataContext as DomainDiscoverySettingsViewModel).Settings;
                    return true;
                default:
                    return false;
            }
        }

        public bool ComputerSettingsDialog(AppSettings settings)
        {
            ComputerDialog dialog = new ComputerDialog(settings);
            switch (dialog.ShowDialog())
            {
                case true:
                    Name = (dialog.DataContext as ComputerSettingsViewModel).ComputerName;
                    Description = (dialog.DataContext as ComputerSettingsViewModel).ComputerDescription;
                    FQDN = (dialog.DataContext as ComputerSettingsViewModel).ComputerFQDN;
                    IPAddress = (dialog.DataContext as ComputerSettingsViewModel).ComputerAddress;
                    UseParentUserProfile = (dialog.DataContext as ComputerSettingsViewModel).UseParentUserProfile;
                    UserProfileId = (dialog.DataContext as ComputerSettingsViewModel).UserProfileId;
                    return true;
                default:
                    return false;
            }
        }

        public bool ComputerSettingsDialog(AppSettings settings, string name, string desc, string fqdn, string address, bool useParentId, Guid profileId)
        {
            ComputerDialog dialog = new ComputerDialog(settings, name, desc, fqdn, address, useParentId, profileId);
            switch (dialog.ShowDialog())
            {
                case true:
                    Name = (dialog.DataContext as ComputerSettingsViewModel).ComputerName;
                    Description = (dialog.DataContext as ComputerSettingsViewModel).ComputerDescription;
                    FQDN = (dialog.DataContext as ComputerSettingsViewModel).ComputerFQDN;
                    IPAddress = (dialog.DataContext as ComputerSettingsViewModel).ComputerAddress;
                    UseParentUserProfile = (dialog.DataContext as ComputerSettingsViewModel).UseParentUserProfile;
                    UserProfileId = (dialog.DataContext as ComputerSettingsViewModel).UserProfileId;
                    return true;
                default:
                    return false;
            }
        }

        public bool ChooseContainerDialog(AppSettings settings, TreeItem treeItem, bool useSelectedParentId)
        {
            ChooseContainerWindow dialog = new ChooseContainerWindow(settings, treeItem, useSelectedParentId);
            switch (dialog.ShowDialog())
            {
                case true:
                    SelectedContainerId = (dialog.DataContext as ChooseContainerViewModel).SelectedContainer.Id;
                    return true;
                default:
                    return false;
            }
        }

        public bool UseCustomIcon { get; set; }
        public GizmoIconEnum CustomIcon { set; get; }

        public bool ReportContainerSettingsDialog(AppSettings settings)
        {
            ReportContainerDialog dialog = new ReportContainerDialog(settings);
            switch (dialog.ShowDialog())
            {
                case true:
                    Name = (dialog.DataContext as ReportContainerSettingsViewModel).ContainerName;
                    Description = (dialog.DataContext as ReportContainerSettingsViewModel).ContainerDescription;
                    UseCustomIcon = (dialog.DataContext as ReportContainerSettingsViewModel).UseCustomIcon;
                    CustomIcon = (dialog.DataContext as ReportContainerSettingsViewModel).CustomIcon;
                    return true;
                default:
                    return false;
            }
        }

        public bool ReportContainerSettingsDialog(AppSettings settings, string name, string desc, bool useCustomIcon, GizmoIconEnum customIcon)
        {
            ReportContainerDialog dialog = new ReportContainerDialog(settings, name, desc, useCustomIcon, customIcon);
            switch (dialog.ShowDialog())
            {
                case true:
                    Name = (dialog.DataContext as ReportContainerSettingsViewModel).ContainerName;
                    Description = (dialog.DataContext as ReportContainerSettingsViewModel).ContainerDescription;
                    UseCustomIcon = (dialog.DataContext as ReportContainerSettingsViewModel).UseCustomIcon;
                    CustomIcon = (dialog.DataContext as ReportContainerSettingsViewModel).CustomIcon; 
                    return true;
                default:
                    return false;
            }
        }
    }
}
