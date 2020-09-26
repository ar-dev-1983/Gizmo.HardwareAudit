using Gizmo.HardwareAudit.Interfaces;
using System;
using System.Collections.ObjectModel;

namespace Gizmo.HardwareAudit.ViewModels
{
    public class ContainerSettingsViewModel : BaseViewModel
    {

        #region Private Properties
        private ObservableCollection<UserProfile> userProfiles;
        private string containerName;
        private string containerDescription;
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

        public ContainerSettingsViewModel(ObservableCollection<UserProfile> userprofiles)
        {
            UserProfiles = userprofiles;
            UserProfileId = new Guid();
            UseParentUserProfile = true;
            ContainerDescription = string.Empty;
            ContainerName = string.Empty;
        }

        public ContainerSettingsViewModel(ObservableCollection<UserProfile> userprofiles, string name, string description, bool useParentId, Guid profileId)
        {
            UserProfiles = userprofiles;
            UserProfileId = profileId;
            UseParentUserProfile = useParentId;
            ContainerDescription = description;
            ContainerName = name;
        }
    }
}
