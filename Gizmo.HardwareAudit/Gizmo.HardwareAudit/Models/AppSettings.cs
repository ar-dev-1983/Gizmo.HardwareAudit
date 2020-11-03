using Gizmo.HardwareAudit.Interfaces;
using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace Gizmo.HardwareAudit.Models
{
    public class AppSettings : BaseViewModel
    {
        private string mSL;
        private string uSl;
        private string mSH;
        private UIThemeEnum theme;
        private bool loadLastFile;
        private string lastFile;
        private string lastReportFile;
        private string logFile;
        private bool checkPortsThenPing;
        private bool checkSharedFoldersThenPing;
        private ObservableCollection<CheckTPCPortSetting> defaultCheckPorts;
        private ObservableCollection<UserProfile> userProfiles;
        private DataProtectionScope scope;
        public string MSL
        {
            get => mSL;
            set
            {
                if (mSL == value) return;
                mSL = value;
                OnPropertyChanged();
            }
        }
        [JsonIgnore]
        public string USl
        {
            get => uSl;
            set
            {
                if (uSl == value) return;
                uSl = value;
                OnPropertyChanged();
            }
        }
        public string MSH
        {
            get => mSH;
            set
            {
                if (mSH == value) return;
                mSH = value;
                OnPropertyChanged();
            }
        }
        public UIThemeEnum Theme
        {
            get => theme;
            set
            {
                if (theme == value) return;
                theme = value;
                OnPropertyChanged();
            }
        }
        public bool LoadLastFile
        {
            get => loadLastFile;
            set
            {
                if (loadLastFile == value) return;
                loadLastFile = value;
                OnPropertyChanged();
            }
        }
        public string LastFile
        {
            get => lastFile;
            set
            {
                if (lastFile == value) return;
                lastFile = value;
                OnPropertyChanged();
            }
        }
        public string LastReportFile
        {
            get => lastReportFile;
            set
            {
                if (lastReportFile == value) return;
                lastReportFile = value;
                OnPropertyChanged();
            }
        }
        public string LogFile
        {
            get => logFile;
            set
            {
                if (logFile == value) return;
                logFile = value;
                OnPropertyChanged();
            }
        }
        public bool CheckPortsThenPing
        {
            get => checkPortsThenPing;
            set
            {
                if (checkPortsThenPing == value) return;
                checkPortsThenPing = value;
                OnPropertyChanged();
            }
        }
        public bool CheckSharedFoldersThenPing
        {
            get => checkSharedFoldersThenPing;
            set
            {
                if (checkSharedFoldersThenPing == value) return;
                checkSharedFoldersThenPing = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<CheckTPCPortSetting> DefaultCheckPorts
        {
            get => defaultCheckPorts;
            set
            {
                if (defaultCheckPorts == value) return;
                defaultCheckPorts = value;
                OnPropertyChanged();
            }
        }
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
        public DataProtectionScope Scope
        {
            get => scope;
            set
            {
                if (scope == value) return;
                scope = value;
                OnPropertyChanged();
            }
        }
        public AppSettings()
        {
            mSL = string.Empty;
            mSH = string.Empty;
            theme = UIThemeEnum.BlueDark;
            loadLastFile = false;
            lastFile = string.Empty;
            lastReportFile = string.Empty;
            logFile = string.Empty;
            checkPortsThenPing = false;
            checkSharedFoldersThenPing = false;
            defaultCheckPorts = new ObservableCollection<CheckTPCPortSetting>();
            userProfiles = new ObservableCollection<UserProfile>();
            scope = DataProtectionScope.CurrentUser;
        }

        internal List<LogItem> SaltUserProfiles(string salt)
        {
            var result = new List<LogItem>();
            USl = salt;
            if (userProfiles != null)
            {
                foreach (var node in userProfiles)
                {
                    node.Salt = salt;
                    try
                    {
                        node.UserPassword = UserProfile.DecryptString(node.Password, node.Salt, scope);
                    }
                    catch (Exception ex)
                    {
                        result.Add(new LogItem() { Type = Enums.LogItemTypeEnum.Error, Content = ex.Message, TimeStamp = DateTime.Now, Source = "SaltUserProfiles", Value = "Profile \"" + node.ProfileName + "\"" });
                    }
                }
            }
            return result;
        }

        internal List<LogItem> ChangeSaltInUserProfiles(string newSalt, DataProtectionScope newScope)
        {
            var result = new List<LogItem>();

            if (userProfiles != null)
            {
                foreach (var node in userProfiles)
                {
                    try
                    {
                        node.UserPassword = UserProfile.DecryptString(node.Password, USl, scope);
                    }
                    catch (Exception ex)
                    {
                        result.Add(new LogItem() { Type = Enums.LogItemTypeEnum.Error, Content = ex.Message, TimeStamp = DateTime.Now, Source = "ChangeSaltInUserProfiles", Value = "Profile \"" + node.ProfileName + "\"" });
                    }
                    node.Salt = newSalt;
                    try
                    {
                        node.Password = UserProfile.EncryptString(node.UserPassword, node.Salt, newScope);
                    }
                    catch (Exception ex)
                    {
                        result.Add(new LogItem() { Type = Enums.LogItemTypeEnum.Error, Content = ex.Message, TimeStamp = DateTime.Now, Source = "ChangeSaltInUserProfiles", Value = "Profile \"" + node.ProfileName + "\"" });
                    }
                    try
                    {
                        node.UserPassword = UserProfile.DecryptString(node.Password, node.Salt, newScope);
                    }
                    catch (Exception ex)
                    {
                        result.Add(new LogItem() { Type = Enums.LogItemTypeEnum.Error, Content = ex.Message, TimeStamp = DateTime.Now, Source = "ChangeSaltInUserProfiles", Value = "Profile \"" + node.ProfileName + "\"" });
                    }
                }
                USl = newSalt;
                Scope = newScope;
            }
            return result;
        }
    }
}
