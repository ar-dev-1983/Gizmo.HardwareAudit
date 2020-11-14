using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAuditWPF;

namespace Gizmo.HardwareAudit.ViewModels
{
    public class ReportContainerSettingsViewModel : BaseViewModel
    {

        #region Private Properties
        private string containerName;
        private string containerDescription;
        private bool useCustomIcon = false;
        private GizmiComputerHardwareIconsEnum customIcon = GizmiComputerHardwareIconsEnum.None;
        #endregion

        #region Public Properties
        public string ContainerName
        {
            get => containerName;
            set
            {
                if (containerName == value) return;
                containerName = value;
                OnPropertyChanged();
            }
        }
        public string ContainerDescription
        {
            get => containerDescription;
            set
            {
                if (containerDescription == value) return;
                containerDescription = value;
                OnPropertyChanged();
            }
        }

        public bool UseCustomIcon
        {
            get => useCustomIcon;
            set
            {
                if (useCustomIcon == value) return;
                useCustomIcon = value;
                OnPropertyChanged();
            }
        }

        public GizmiComputerHardwareIconsEnum CustomIcon
        {
            get => customIcon;
            set
            {
                if (customIcon == value) return;
                customIcon = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public ReportContainerSettingsViewModel()
        {
            ContainerDescription = string.Empty;
            ContainerName = string.Empty;
        }
        public ReportContainerSettingsViewModel(string name, string description)
        {
            ContainerDescription = description;
            ContainerName = name;
        }
        public ReportContainerSettingsViewModel(string name, string description, bool useCustomIcon, GizmiComputerHardwareIconsEnum customIcon)
        {
            ContainerDescription = description;
            ContainerName = name;
            UseCustomIcon = useCustomIcon;
            CustomIcon = customIcon;
        }
    }
}
