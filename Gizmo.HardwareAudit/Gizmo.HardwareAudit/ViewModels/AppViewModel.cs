using Gizmo.HardwareAudit.Controls;
using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Controls;

namespace Gizmo.HardwareAudit.ViewModels
{
    public partial class AppViewModel : BaseViewModel
    {
        #region Private Properties
        readonly IDialog dialogService;
        readonly ITreeItemDialog treeItemDialogService;
        readonly ISerialization serializationService;
        readonly IAppSettingsDialog appSettingsService;
        private TreeItem root;
        private List<BackgroundWorkerWithId> WMIProbes;
        private List<BackgroundWorkerWithId> NetProbes;

        private bool isUnlocked;
        private bool isFirstRun;
        private string appPath;
        public ObservableCollection<CheckTPCPortSetting> defaultCheckPorts;
        private ObservableCollection<LogItem> log;
        private AppSettings settings = new AppSettings();
        #endregion

        #region Public Properties
        public class LogClass
        {
            public ObservableCollection<LogItem> ILogList { get; set; }
        }

        public ObservableCollection<LogItem> Log
        {
            get => log;
            set
            {
                if (log == value) return;
                log = value;
                OnPropertyChanged();
            }
        }

        public TreeItem Root
        {
            get => root;
            set
            {
                if (root == value) return;
                root = value;
                OnPropertyChanged();
            }
        }
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
        public bool IsUnlocked
        {
            get => isUnlocked;
            set
            {
                if (isUnlocked == value) return;
                isUnlocked = value;
                OnPropertyChanged();
            }
        }
        public bool IsFirstRun
        {
            get => isFirstRun;
            set
            {
                if (isFirstRun == value) return;
                isFirstRun = value;
                OnPropertyChanged();
            }
        }

        public TreeItem SelectedTreeItem => Root.SelectedItem;
        public ObservableCollection<MenuItem> SelectedTreeItemSharedFoldersMenuList => BuildSharedFoldersMenuItems();
        public ObservableCollection<MenuItem> SelectedTreeItemTCPPortsMenuList => BuildCheckPortResultsMenuItems();
        #endregion

        #region AppViewModel
        public AppViewModel(IDialog defaultDialogService, ITreeItemDialog treeItemService, ISerialization jsonService, IAppSettingsDialog appSettingsDialog)
        {
            appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Gizmo\\HardwareAudit");
            if (!Directory.Exists(appPath))
            {
                Directory.CreateDirectory(appPath);
            }
            var SettingsFile = Path.Combine(appPath, "Settings.dat");
            dialogService = defaultDialogService;
            treeItemDialogService = treeItemService;
            serializationService = jsonService;
            appSettingsService = appSettingsDialog;
            if (new FileInfo(SettingsFile).Exists)
            {
                Settings = serializationService.OpenSettings(SettingsFile);
                IsFirstRun = Settings.MSL == string.Empty || Settings.MSH == string.Empty;
                IsUnlocked = false;
            }
            else
            {
                IsFirstRun = true;
                IsUnlocked = false;
            }

            InitLog();

            if (Settings.LoadLastFile)
            {
                if (Settings.LastFile != "")
                {
                    Root = serializationService.OpenModel(Settings.LastFile);
                    AddLogItem(DateTime.Now, "Model File: \"" + Settings.LastFile + "\"", "Loaded", LogItemTypeEnum.Information, "AppViewModel");
                }
                else
                {
                    Root = new TreeItem() { Children = { TreeItem.CreateRoot() } };
                }
                if (Settings.LastReportFile != "")
                {
                    ReportRoot = serializationService.OpenReportModel(Settings.LastReportFile);
                    AddLogItem(DateTime.Now, "Report Model File: \"" + Settings.LastReportFile + "\"", "Loaded", LogItemTypeEnum.Information, "AppViewModel");
                }
                else
                {
                    ReportRoot = new ReportItem() { Children = { ReportItem.CreateRoot() } };
                }
            }
            else
            {
                Root = new TreeItem() { Children = { TreeItem.CreateRoot() } };
                ReportRoot = new ReportItem() { Children = { ReportItem.CreateRoot() } };
            }
            InitProbes();
            Root.Initialise();
            if (Root != null)
            {
                Root.PropertyChanged += SelectedItem_PropertyChanged;
            }
            InitReports();
            ReportRoot.Initialise();
            if (ReportRoot != null)
            {
                ReportRoot.PropertyChanged += SelectedItem_PropertyChanged;
            }
        }

        private void SelectedItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedItem"))
            {
                OnNamedPropertyChanged("SelectedTreeItem");
            }
            else if (e.PropertyName.Equals("SelectedReport"))
            {
                OnNamedPropertyChanged("SelectedReportItem");
            }
        }

        public void FirstRun(SecureString password, DataProtectionScope scope)
        {
            if (appSettingsService != null)
            {
                if (password.Length == 0)
                {
                    Settings.Scope = scope;
                    Settings.MSL = string.Empty;
                    Settings.MSH = string.Empty;

                    IsFirstRun = true;
                    IsUnlocked = false;
                    AddLogItem(DateTime.Now, "Master password is empty!", string.Empty, LogItemTypeEnum.Warning, string.Empty);
                }
                else
                {
                    Settings.Scope = scope;
                    Settings.MSL = UserProfile.GetSaltString();
                    Settings.MSH = UserProfile.GetHashString(password, Settings.MSL, 10000, 100);
                    Settings.USl = UserProfile.GetHashString(password, Settings.MSH, 50431, 142);
                    serializationService.SaveSettings(Path.Combine(appPath, "Settings.dat"), Settings);
                    IsFirstRun = false;
                    IsUnlocked = true;
                }
            }
        }

        public bool Unlock(SecureString password)
        {
            var WhatToDoNext = true;

            if (appSettingsService != null)
            {
                if (password.Length == 0)
                {
                    AddLogItem(DateTime.Now, "Master password is empty!", string.Empty, LogItemTypeEnum.Warning, string.Empty);
                }
                else
                {
                    if (UserProfile.GetHashString(password, Settings.MSL, 10000, 100) == Settings.MSH)
                    {
                        var Errors = Settings.SaltUserProfiles(UserProfile.GetHashString(password, Settings.MSH, 50431, 142));
                        foreach (var node in Errors)
                        {
                            if (Log != null)
                            {
                                Log.Add(node);
                            }
                        }
                        if (Errors.Count != 0)
                        {
                            IsUnlocked = true;
                            WhatToDoNext = dialogService.QueryYesNoAnswer("There is an errors occurred during process of decryption passwords.\n Preess \"Yes\" to continue or \"No\" to close app?");
                        }
                        else
                        {
                            IsUnlocked = true;
                        }
                    }
                }
            }
            return WhatToDoNext;
        }

        public bool ChangeUnlock(SecureString password, SecureString newpassword, DataProtectionScope scope)
        {
            var WhatToDoNext = true;
            if (appSettingsService != null)
            {
                if (password.Length == 0)
                {
                    AddLogItem(DateTime.Now, "Master password is empty!", string.Empty, LogItemTypeEnum.Warning, string.Empty);
                }
                else
                {
                    if (newpassword.Length == 0)
                    {
                        AddLogItem(DateTime.Now, "New master password is empty!", string.Empty, LogItemTypeEnum.Warning, string.Empty);
                    }
                    else
                    {
                        if (UserProfile.GetHashString(password, Settings.MSL, 10000, 100) == Settings.MSH)
                        {
                            Settings.USl = UserProfile.GetHashString(password, Settings.MSH, 50431, 142);
                            Settings.MSL = UserProfile.GetSaltString();
                            Settings.MSH = UserProfile.GetHashString(newpassword, Settings.MSL, 10000, 100);
                            var Errors = Settings.ChangeSaltInUserProfiles(UserProfile.GetHashString(newpassword, Settings.MSH, 50431, 142), scope);
                            foreach (var node in Errors)
                            {
                                if (Log != null)
                                {
                                    Log.Add(node);
                                }
                            }
                            if (Errors.Count != 0)
                            {
                                WhatToDoNext = dialogService.QueryYesNoAnswer("There is an errors occurred during process of changin passwords.\n Preess \"Yes\" to continue or \"No\" to close app?");
                            }
                            else
                            {
                                serializationService.SaveSettings(Path.Combine(appPath, "Settings.dat"), Settings);
                                IsUnlocked = true;
                            }
                        }
                    }
                }
            }
            return WhatToDoNext;
        }

        public void Close()
        {
            AddLogItem(DateTime.Now, "Application closing", string.Empty, LogItemTypeEnum.Information, "Close");
            AddLogItem(DateTime.Now, "Application log file \"" + Settings.LogFile + "\"", "Saved", LogItemTypeEnum.Information, "Close");
            serializationService.SaveLog(Path.Combine(appPath, Settings.LogFile), Log);
            serializationService.SaveSettings(Path.Combine(appPath, "Settings.dat"), Settings);
        }
        #endregion

        #region Adding Elements
        internal void AddLogItem(DateTime timeStamp, string content, string value, LogItemTypeEnum type, string source)
        {
            if (Log != null)
            {
                Log.Add(new LogItem(timeStamp, content, value, type, source));
            }
        }

        internal void AddContainer(string name, string description, TreeItem Parent, bool useParentUserProfileId, Guid userProfileId)
        {
            if (Parent != null)
            {
                Parent.Children.Add(new TreeItem() { ParentId = Parent.Id, Name = name, Description = description, ParentType = Parent.Type == ItemTypeEnum.ChildContainer ? Parent.ParentType : Parent.Type, Type = ItemTypeEnum.ChildContainer, UseParentUserProfile = useParentUserProfileId, UserProfileId = useParentUserProfileId ? new Guid() : userProfileId });
                AddLogItem(DateTime.Now, "Container \"" + name + "\" added", string.Empty, LogItemTypeEnum.Information, "AddContainer");
            }
        }

        internal TreeItem NewContainer(string name, string description, TreeItem Parent, bool useParentUserProfileId, Guid userProfileId)
        {
            AddLogItem(DateTime.Now, "Container \"" + name + "\" added", string.Empty, LogItemTypeEnum.Information, "AddContainer");
            return new TreeItem() { ParentId = Parent.Id, Name = name, Description = description, ParentType = Parent.Type == ItemTypeEnum.ChildContainer ? Parent.ParentType : Parent.Type, Type = ItemTypeEnum.ChildContainer, UseParentUserProfile = useParentUserProfileId, UserProfileId = useParentUserProfileId ? new Guid() : userProfileId };
        }

        internal void AddComputer(string name, string description, string fqdn, string address, TreeItem Parent, bool useParentUserProfileId, Guid userProfileId)
        {
            if (Parent != null)
            {
                var Child = new TreeItem() { ParentId = Parent.Id, Name = name, Description = description, FQDN = fqdn, Address = address, ParentType = Parent.Type == ItemTypeEnum.ChildContainer ? Parent.ParentType : Parent.Type, Type = ItemTypeEnum.ChildComputer, UseParentUserProfile = useParentUserProfileId, UserProfileId = useParentUserProfileId ? new Guid() : userProfileId };
                Parent.Children.Add(Child);
                InitProbe(Child);
                AddLogItem(DateTime.Now, "Computer \"" + name + "\" added", string.Empty, LogItemTypeEnum.Information, "AddComputer");
            }
        }

        internal TreeItem NewComputer(string name, string description, string fqdn, string address, TreeItem Parent, bool useParentUserProfileId, Guid userProfileId)
        {
            var Child = new TreeItem() { ParentId = Parent.Id, Name = name, Description = description, FQDN = fqdn, Address = address, ParentType = Parent.Type == ItemTypeEnum.ChildContainer ? Parent.ParentType : Parent.Type, Type = ItemTypeEnum.ChildComputer, UseParentUserProfile = useParentUserProfileId, UserProfileId = useParentUserProfileId ? new Guid() : userProfileId };
            InitProbe(Child);
            AddLogItem(DateTime.Now, "Computer \"" + name + "\" added", string.Empty, LogItemTypeEnum.Information, "AddComputer");
            return Child;
        }

        internal void AddDevice(string name, string description, string fqdn, string address, TreeItem Parent, bool useParentUserProfileId, Guid userProfileId)
        {
            if (Parent != null)
            {
                var Child = new TreeItem() { ParentId = Parent.Id, Name = name, Description = description, FQDN = fqdn, Address = address, ParentType = Parent.Type == ItemTypeEnum.ChildContainer ? Parent.ParentType : Parent.Type, Type = ItemTypeEnum.ChildDevice, UseParentUserProfile = useParentUserProfileId, UserProfileId = useParentUserProfileId ? new Guid() : userProfileId };
                Parent.Children.Add(Child);
                InitProbe(Child);
                AddLogItem(DateTime.Now, "Device \"" + name + "\" added", string.Empty, LogItemTypeEnum.Information, "AddDevice");
            }
        }
        #endregion

        #region Management
        public static IEnumerable<T> Traverse<T>(T item, Func<T, IEnumerable<T>> childSelector)
        {
            var stack = new Stack<T>();
            stack.Push(item);
            while (stack.Any())
            {
                var next = stack.Pop();
                yield return next;
                foreach (var child in childSelector(next))
                    stack.Push(child);
            }
        }

        internal void Dispose()
        {
            foreach (var node in WMIProbes)
            {
                node.Dispose();
            }
            foreach (var node in NetProbes)
            {
                node.Dispose();
            }
        }

        internal void InitProbes()
        {
            WMIProbes = new List<BackgroundWorkerWithId>();
            NetProbes = new List<BackgroundWorkerWithId>();
            foreach (var node in Root.Children)
            {
                InitProbe(node);
            }
        }

        internal void InitProbe(TreeItem Item)
        {
            if (WMIProbes.Find(x => x.Id == Item.Id) == null)
            {
                WMIProbes.Add(new BackgroundWorkerWithId(Item.Id) { WorkerSupportsCancellation = true });
            }
            if (NetProbes.Find(x => x.Id == Item.Id) == null)
            {
                NetProbes.Add(new BackgroundWorkerWithId(Item.Id) { WorkerSupportsCancellation = true });
            }
            if (Item.Children.Count != 0)
            {
                foreach (var node in Item.Children)
                {
                    InitProbe(node);
                }
            }
        }

        internal void InitLog()
        {
            Log = new ObservableCollection<LogItem>();
            if (Settings.LogFile != string.Empty)
            {
                if (new FileInfo(Path.Combine(appPath, Settings.LogFile)).Exists)
                {
                    Log = serializationService.OpenLog(Path.Combine(appPath, Settings.LogFile));
                    AddLogItem(DateTime.Now, "Application log file \"" + Settings.LogFile + "\"", "Loaded", LogItemTypeEnum.Information, "InitLog");
                }
                else
                {
                    serializationService.SaveLog(Path.Combine(appPath, Settings.LogFile), Log);
                    AddLogItem(DateTime.Now, "Application log file", "Not found", LogItemTypeEnum.Warning, "InitLog");
                    AddLogItem(DateTime.Now, "Application log file \"" + Settings.LogFile + "\"", "Created", LogItemTypeEnum.Information, "InitLog");
                }
            }
            else
            {
                Settings.LogFile = "GizmoAppLog.log";
                serializationService.SaveLog(Path.Combine(appPath, Settings.LogFile), Log);
                AddLogItem(DateTime.Now, "Application log file", "Not found", LogItemTypeEnum.Warning, "InitLog");
                AddLogItem(DateTime.Now, "Application log file \"" + Settings.LogFile + "\"", "Created", LogItemTypeEnum.Information, "InitLog");
            }
        }

        private bool RunProbe(Guid id, object arg, ProbeTypeEnum probeType)
        {
            var result = false;
            if (probeType != ProbeTypeEnum.CheckWMI)
            {
                using (var probe = NetProbes.Find(x => x.Id == id))
                {
                    if (probe.Done && !probe.IsBusy)
                    {
                        switch (probeType)
                        {
                            case ProbeTypeEnum.Ping:
                                {
                                    probe.DoWork += CheckPing_DoWork;
                                    probe.RunWorkerCompleted += CheckPing_RunWorkerCompleted;

                                    if (arg != null)
                                        probe.RunWorkerAsync(arg);
                                    else
                                        probe.RunWorkerAsync();

                                    result = true;
                                    break;
                                }
                            case ProbeTypeEnum.CheckTCPPort:
                                {
                                    probe.DoWork += CheckTCPPorts_DoWork;
                                    probe.RunWorkerCompleted += CheckTCPPorts_RunWorkerComplited;

                                    if (arg != null)
                                        probe.RunWorkerAsync(arg);
                                    else
                                        probe.RunWorkerAsync();


                                    result = true;
                                    break;
                                }
                            case ProbeTypeEnum.CheckSharedFolders:
                                {
                                    probe.DoWork += CheckSharedFolders_DoWork;
                                    probe.RunWorkerCompleted += CheckSharedFolders_RunWorkerComplited;

                                    if (arg != null)
                                        probe.RunWorkerAsync(arg);
                                    else
                                        probe.RunWorkerAsync();


                                    result = true;
                                    break;
                                }
                        }
                    }
                }
            }
            else
            {
                using (var probe = WMIProbes.Find(x => x.Id == id))
                {
                    if (probe.Done && !probe.IsBusy)
                    {
                        probe.DoWork += CheckWMI_DoWork;
                        probe.RunWorkerCompleted += CheckWMI_RunWorkerCompleted;

                        if (arg != null)
                            probe.RunWorkerAsync(arg);
                        else
                            probe.RunWorkerAsync();
                    }

                    result = true;
                }
            }
            return result;
        }

        private void ReleaseProbe(Guid id, ProbeTypeEnum probeType)
        {
            if (probeType != ProbeTypeEnum.CheckWMI)
            {
                using (var probe = NetProbes.Find(x => x.Id == id))
                {
                    switch (probeType)
                    {
                        case ProbeTypeEnum.Ping:
                            {
                                probe.DoWork -= CheckPing_DoWork;
                                probe.RunWorkerCompleted -= CheckPing_RunWorkerCompleted;
                                probe.CancelAsync();
                                probe.Done = true;
                                break;
                            }
                        case ProbeTypeEnum.CheckTCPPort:
                            {
                                probe.DoWork -= CheckTCPPorts_DoWork;
                                probe.RunWorkerCompleted -= CheckTCPPorts_RunWorkerComplited;
                                probe.CancelAsync();
                                probe.Done = true;
                                break;
                            }
                        case ProbeTypeEnum.CheckSharedFolders:
                            {
                                probe.DoWork -= CheckSharedFolders_DoWork;
                                probe.RunWorkerCompleted -= CheckSharedFolders_RunWorkerComplited;
                                probe.CancelAsync();
                                probe.Done = true;
                                break;
                            }
                    }
                }
            }
            else
            {
                using (var probe = WMIProbes.Find(x => x.Id == id))
                {
                    probe.DoWork -= CheckWMI_DoWork;
                    probe.RunWorkerCompleted -= CheckWMI_RunWorkerCompleted;
                    probe.CancelAsync();
                    probe.Done = true;
                }
            }
            GC.Collect();
        }

        #region Items Search
        internal bool FindTreeItemAndDelete(Guid id)
        {
            if (Root.Children.Count != 0)
            {
                foreach (var node in Root.Children)
                {
                    if (node.Id == id)
                    {
                        Root.Children.Remove(node);
                        return true;
                    }
                }
                foreach (var node in Root.Children)
                {
                    var result = FindChildrenIdAndDelete(node, id);
                    if (result)
                    { return true; }
                }
            }
            return false;
        }

        internal bool FindChildrenIdAndDelete(TreeItem Item, Guid id)
        {
            if (Item.Children.Count != 0)
            {
                foreach (var node in Item.Children)
                {
                    if (node.Id == id)
                    {
                        Item.Children.Remove(node);
                        return true;
                    }
                }
                foreach (var node in Item.Children)
                {
                    if (FindChildrenIdAndDelete(node, id))
                        return true;
                }
            }
            return false;
        }

        internal bool FindReportItemAndDelete(Guid id)
        {
            if (ReportRoot.Children.Count != 0)
            {
                foreach (var node in ReportRoot.Children)
                {
                    if (node.Id == id)
                    {
                        ReportRoot.Children.Remove(node);
                        return true;
                    }
                }
                foreach (var node in ReportRoot.Children)
                {
                    var result = FindReportChildrenIdAndDelete(node, id);
                    if (result)
                    { return true; }
                }
            }
            return false;
        }

        internal bool FindReportChildrenIdAndDelete(ReportItem Item, Guid id)
        {
            if (Item.Children.Count != 0)
            {
                foreach (var node in Item.Children)
                {
                    if (node.Id == id)
                    {
                        Item.Children.Remove(node);
                        return true;
                    }
                }
                foreach (var node in Item.Children)
                {
                    if (FindReportChildrenIdAndDelete(node, id))
                        return true;
                }
            }
            return false;
        }

        internal ConnectionOptions BuildConnectionOptions(Guid id)
        {
            try
            {
                var userProfile = Settings.UserProfiles.Where(x => x.Id == id).First();
                return new ConnectionOptions() { Username = userProfile.UserName, SecurePassword = userProfile.UserPassword };
            }
            catch (Exception e)
            {
                AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "BuildConnectionOptions");
                return new ConnectionOptions();
            }
        }

        internal ConnectionOptions GetConnectionOptions(Guid id)
        {
            try
            {
                var node = FindTreeItemByGuid(id);
                if (!node.UseParentUserProfile && node.UserProfileId != new Guid())
                {
                    return BuildConnectionOptions(node.UserProfileId);
                }
                else
                {
                    var result = GetConnectionOptions(node.ParentId);
                    if (result != null)
                        return result;
                    else
                        return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal TreeItem FindTreeItemByGuid(Guid id)
        {
            if (Root.Children.Count != 0)
            {
                return Traverse(Root, node => node.Children).FirstOrDefault(m => m.Id == id);
            }
            return null;
        }

        internal ReportItem FindReportItemByGuid(Guid id)
        {
            if (ReportRoot.Children.Count != 0)
            {
                return Traverse(ReportRoot, node => node.Children).FirstOrDefault(m => m.Id == id);
            }
            return null;
        }

        internal void ClearHardwareScansInChildrends(TreeItem Item, ItemTypeEnum Type)
        {
            if (Item.Children.Count != 0)
            {
                foreach (var node in Item.Children)
                {
                    if (node.Type == Type)
                    {
                        node.ClearScans();
                        if (node.Status == ItemStatusEnum.OnlineAndHasData)
                        {
                            node.Status = ItemStatusEnum.Online;
                        }
                    }
                    else if (node.Children.Count != 0)
                    {
                        ClearHardwareScansInChildrends(node, Type);
                    }
                }
            }
        }

        internal void ClearHardwareScansButKeepLastOneInChildrends(TreeItem Item, ItemTypeEnum Type)
        {
            if (Item.Children.Count != 0)
            {
                foreach (var node in Item.Children)
                {
                    if (node.Type == Type)
                    {
                        node.ClearScansButKeepLastOne();
                    }
                    else if (node.Children.Count != 0)
                    {
                        ClearHardwareScansButKeepLastOneInChildrends(node, Type);
                    }
                }
            }
        }

        internal void ClearHardwareScansButKeepLastTwoInChildrends(TreeItem Item, ItemTypeEnum Type)
        {
            if (Item.Children.Count != 0)
            {
                foreach (var node in Item.Children)
                {
                    if (node.Type == Type)
                    {
                        node.ClearScansButKeepLastTwo();
                    }
                    else if (node.Children.Count != 0)
                    {
                        ClearHardwareScansButKeepLastTwoInChildrends(node, Type);
                    }
                }
            }
        }

        internal void FindAllChilderenByType(TreeItem Item, ItemTypeEnum Type, List<TreeItem> List)
        {
            if (Item.Children.Count != 0)
            {
                List.AddRange(Traverse(Item, node => node.Children).Where(m => m.Type == Type));
            }
        }

        internal void FindAllChilderenByType(ReportItem Item, ReportItemTypeEnum Type, List<ReportItem> List)
        {
            if (Item.Children.Count != 0)
            {
                List.AddRange(Traverse(Item, node => node.Children).Where(m => m.Type == Type));
            }
        }

        internal TreeItem FindTreeItemByName(string Name)
        {
            if (Root.Children.Count != 0)
            {
                return Traverse(Root, node => node.Children).FirstOrDefault(m => m.Name == Name);
            }
            return null;
        }

        internal bool IsTreeItemPresent(string Name)
        {
            if (Root.Children.Count != 0)
            {
                return Traverse(Root, node => node.Children).FirstOrDefault(m => m.Name == Name) != null;
            }
            return false;
        }
        #endregion

        #region Menu Building
        internal ObservableCollection<MenuItem> BuildSharedFoldersMenuItems()
        {
            ObservableCollection<MenuItem> result = new ObservableCollection<MenuItem>();
            if (SelectedTreeItem != null)
            {
                if (SelectedTreeItem.SharedFolders.Count > 0)
                {
                    foreach (var node in SelectedTreeItem.SharedFolders)
                        result.Add(new MenuItem() { Height = 22, Header = node, Command = OpenSharedFolderCommand, CommandParameter = node, Icon = new GizmoIcon() { Icon = GizmoIconEnum.OpenSharedFolder, FontSize = 16 } });
                }
            }
            return result;
        }

        internal ObservableCollection<MenuItem> BuildCheckPortResultsMenuItems()
        {
            ObservableCollection<MenuItem> result = new ObservableCollection<MenuItem>();
            if (SelectedTreeItem != null)
            {
                if (SelectedTreeItem.CheckPortResults.Count > 0)
                {
                    foreach (var node in SelectedTreeItem.CheckPortResults)
                        if (node.IsOpened)
                            foreach (var port in Settings.DefaultCheckPorts)
                                if (node.Id == port.Id)
                                    result.Add(new MenuItem() { Height = 22, Header = port.Description, Command = OpenTCPPortCommand, CommandParameter = port.Id, Icon = new GizmoIcon() { Icon = GizmoIconEnum.OpenConnection, FontSize = 16 } });
                }
            }
            return result;
        }
        #endregion

        #endregion
    }
}
