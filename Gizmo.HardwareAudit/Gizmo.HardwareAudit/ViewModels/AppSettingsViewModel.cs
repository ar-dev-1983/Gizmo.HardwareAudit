using Gizmo.HardwareAudit.Classes;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using System;
using System.Collections.ObjectModel;

namespace Gizmo.HardwareAudit.ViewModels
{
    public class AppSettingsViewModel : BaseViewModel
    {

        #region Private Properties
        private AppSettings settings;
        private int selectedCheckPortIndex;
        private int selectedUserProfileIndex;
        #endregion

        #region Public Properties
        public AppSettings Settings
        {
            get => settings;
            set
            {
                if (settings == value) return;
                settings = value;
                OnPropertyChanged();
            }
        }
        public bool LoadLastFile
        {
            get => settings.LoadLastFile;
            set
            {
                if (settings.LoadLastFile == value) return;
                settings.LoadLastFile = value;
                OnPropertyChanged();
            }
        }
        public bool CheckPortsThenPing
        {
            get => settings.CheckPortsThenPing;
            set
            {
                if (settings.CheckPortsThenPing == value) return;
                settings.CheckPortsThenPing = value;
                OnPropertyChanged();
            }
        }
        public bool CheckSharedFoldersThenPing
        {
            get => settings.CheckSharedFoldersThenPing;
            set
            {
                if (settings.CheckSharedFoldersThenPing == value) return;
                settings.CheckSharedFoldersThenPing = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<CheckTPCPortSetting> DefaultCheckPorts
        {
            get => settings.DefaultCheckPorts;
            set
            {
                if (settings.DefaultCheckPorts == value) return;
                settings.DefaultCheckPorts = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<UserProfile> UserProfiles
        {
            get => settings.UserProfiles;
            set
            {
                if (settings.UserProfiles == value) return;
                settings.UserProfiles = value;
                OnPropertyChanged();
            }
        }

        public int SelectedCheckPortIndex
        {
            get => selectedCheckPortIndex;
            set
            {
                if (selectedCheckPortIndex == value) return;
                selectedCheckPortIndex = value;
                OnPropertyChanged();
            }
        }

        public int SelectedUserProfileIndex
        {
            get => selectedUserProfileIndex;
            set
            {
                if (selectedUserProfileIndex == value) return;
                selectedUserProfileIndex = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands

        #region CheckPorts

        private WorkCommand addCheckPortCommand;
        public WorkCommand AddCheckPortCommand
        {
            get
            {
                return addCheckPortCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        DefaultCheckPorts.Add(new CheckTPCPortSetting());
                    }
                    catch (Exception) { }
                });
            }
        }

        private WorkCommand deleteCheckPortCommand;
        public WorkCommand DeleteCheckPortCommand
        {
            get
            {
                return deleteCheckPortCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (SelectedCheckPortIndex != -1)
                        {
                            var si = SelectedCheckPortIndex;
                            DefaultCheckPorts.RemoveAt(si);
                        }
                    }
                    catch (Exception) { }
                });
            }
        }

        private WorkCommand upCheckPortCommand;
        public WorkCommand UpCheckPortCommand
        {
            get
            {
                return upCheckPortCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (SelectedCheckPortIndex != -1)
                        {
                            var si = SelectedCheckPortIndex;
                            if (si - 1 != -1)
                            {
                                DefaultCheckPorts.Move(si, si - 1);
                            }
                        }
                    }
                    catch (Exception) { }
                });
            }
        }

        private WorkCommand downCheckPortCommand;
        public WorkCommand DownCheckPortCommand
        {
            get
            {
                return downCheckPortCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (SelectedCheckPortIndex != -1)
                        {
                            var si = SelectedCheckPortIndex;
                            if (si + 1 != DefaultCheckPorts.Count)
                            {
                                DefaultCheckPorts.Move(si, si + 1);
                            }
                        }
                    }
                    catch (Exception) { }
                });
            }
        }
        #endregion

        #region UserProfiles

        private WorkCommand addUserProfileCommand;
        public WorkCommand AddUserProfileCommand
        {
            get
            {
                return addUserProfileCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        UserProfiles.Add(new UserProfile(Settings.USl));
                    }
                    catch (Exception) { }
                });
            }
        }

        private WorkCommand deleteUserProfileCommand;
        public WorkCommand DeleteUserProfileCommand
        {
            get
            {
                return deleteUserProfileCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (selectedUserProfileIndex != -1)
                        {
                            var si = selectedUserProfileIndex;
                            UserProfiles.RemoveAt(si);
                        }
                    }
                    catch (Exception) { }
                });
            }
        }

        private WorkCommand upUserProfileCommand;
        public WorkCommand UpUserProfileCommand
        {
            get
            {
                return upUserProfileCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (selectedUserProfileIndex != -1)
                        {
                            var si = selectedUserProfileIndex;
                            if (si - 1 != -1)
                            {
                                UserProfiles.Move(si, si - 1);
                            }
                        }
                    }
                    catch (Exception) { }
                });
            }
        }

        private WorkCommand downUserProfileCommand;
        public WorkCommand DownUserProfileCommand
        {
            get
            {
                return downUserProfileCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (selectedUserProfileIndex != -1)
                        {
                            var si = selectedUserProfileIndex;
                            if (si + 1 != DefaultCheckPorts.Count)
                            {
                                UserProfiles.Move(si, si + 1);
                            }
                        }
                    }
                    catch (Exception) { }
                });
            }
        }
        #endregion
        #endregion

        public AppSettingsViewModel(AppSettings appSettings)
        {
            Settings = appSettings;
        }
    }
}
