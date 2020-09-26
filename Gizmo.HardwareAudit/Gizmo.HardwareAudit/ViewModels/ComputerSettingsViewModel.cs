using Gizmo.HardwareAudit.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace Gizmo.HardwareAudit.ViewModels
{
    public class ComputerSettingsViewModel : BaseViewModel
    {

        #region Private Properties
        private ObservableCollection<UserProfile> userProfiles;
        private string computerName;
        private string computerDescription;
        private string computerFQDN;
        private string computerAddress;
        private bool useParentUserProfile;
        private Guid userProfileId;
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
        public string ComputerName
        {
            get => computerName;
            set
            {
                if (computerName == value) return;
                computerName = value;
                OnPropertyChanged();
            }
        }
        public string ComputerDescription
        {
            get => computerDescription;
            set
            {
                if (computerDescription == value) return;
                computerDescription = value;
                OnPropertyChanged();
            }
        }
        public string ComputerFQDN
        {
            get => computerFQDN;
            set
            {
                if (computerFQDN == value) return;
                computerFQDN = value;
                OnPropertyChanged();
            }
        }
        public string ComputerAddress
        {
            get => computerAddress;
            set
            {
                if (computerAddress == value) return;
                computerAddress = value;
                OnPropertyChanged();
            }
        }
        public bool UseParentUserProfile
        {
            get => useParentUserProfile;
            set
            {
                if (useParentUserProfile == value) return;
                useParentUserProfile = value;
                OnPropertyChanged();
            }
        }
        public Guid UserProfileId
        {
            get => userProfileId;
            set
            {
                if (userProfileId == value) return;
                userProfileId = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public ComputerSettingsViewModel(ObservableCollection<UserProfile> userprofiles)
        {
            UserProfiles = userprofiles;
            UserProfileId = new Guid();
            UseParentUserProfile = true;
            ComputerName = string.Empty;
            ComputerDescription = string.Empty;
            ComputerFQDN = string.Empty;
            ComputerAddress = string.Empty;
        }

        public ComputerSettingsViewModel(ObservableCollection<UserProfile> userprofiles, string name, string desc, string fqdn, string address, bool useParentId, Guid profileId)
        {
            UserProfiles = userprofiles;
            UserProfileId = profileId;
            UseParentUserProfile = useParentId;
            ComputerName = name;
            ComputerDescription = desc;
            ComputerFQDN = fqdn;
            ComputerAddress = address;
        }
    }
}
