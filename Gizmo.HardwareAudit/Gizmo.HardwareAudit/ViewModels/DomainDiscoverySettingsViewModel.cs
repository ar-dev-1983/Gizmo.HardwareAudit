using Gizmo.HardwareAudit.Interfaces;
using System.Collections.ObjectModel;

namespace Gizmo.HardwareAudit.ViewModels
{
    public class DomainDiscoverySettingsViewModel : BaseViewModel
    {
        #region Private Properties
        private ObservableCollection<UserProfile> userProfiles;
        private DomainDiscoverySettings settings;
        #endregion

        #region Public Properties
        public ObservableCollection<UserProfile> UserProfiles
        {
            get => userProfiles;
            set
            {
                if (userProfiles == value) return;
                userProfiles = value;
                OnPropertyChanged();
            }
        }
        public DomainDiscoverySettings Settings
        {
            get => settings;
            set
            {
                if (settings == value) return;
                settings = value;
                OnPropertyChanged();
            }
        }

        public DomainDiscoverySettingsViewModel()
        {
            userProfiles = new ObservableCollection<UserProfile>();
            settings = new DomainDiscoverySettings();
        }
        public DomainDiscoverySettingsViewModel(ObservableCollection<UserProfile> userprofiles)
        {
            UserProfiles = userprofiles;
            settings = new DomainDiscoverySettings();
        }
        public DomainDiscoverySettingsViewModel(ObservableCollection<UserProfile> userProfiles, DomainDiscoverySettings settings)
        {
            UserProfiles = userProfiles;
            Settings = settings;
        }
        #endregion
    }
}
