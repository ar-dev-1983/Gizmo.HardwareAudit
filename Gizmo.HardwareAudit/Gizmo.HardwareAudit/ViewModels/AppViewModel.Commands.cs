using Gizmo.HardwareAudit.Classes;
using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAuditClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
namespace Gizmo.HardwareAudit.ViewModels
{
    public partial class AppViewModel : BaseViewModel
    {
        #region Commands

        #region Settings

        private WorkCommand editSettingsCommand;
        public WorkCommand EditSettingsCommand
        {
            get
            {
                return editSettingsCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (appSettingsService.EditSettingsDialog(Settings) == true)
                        {
                            Settings.LoadLastFile = appSettingsService.Settings.LoadLastFile;
                            Settings.CheckPortsThenPing = appSettingsService.Settings.CheckPortsThenPing;
                            Settings.CheckSharedFoldersThenPing = appSettingsService.Settings.CheckSharedFoldersThenPing;
                            Settings.DefaultCheckPorts = appSettingsService.Settings.DefaultCheckPorts;
                            Settings.UserProfiles = appSettingsService.Settings.UserProfiles;
                            Settings.Scope = appSettingsService.Settings.Scope;
                            foreach (var node in Settings.UserProfiles)
                            {
                                node.Password = UserProfile.EncryptString(node.UserPassword, Settings.USl, Settings.Scope);
                            }
                            serializationService.SaveSettings(Path.Combine(appPath, "Settings.dat"), Settings);

                            AddLogItem(DateTime.Now, "Settings File", "Saved", LogItemTypeEnum.Information, "EditSettingsCommand");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "EditSettingsCommand");
                    }
                });
            }
        }

        #endregion

        #region Model
        private WorkCommand newModelCommand;
        public WorkCommand NewModelCommand
        {
            get
            {
                return newModelCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (dialogService.QueryYesNoAnswer("Save existing Model before creating new one?"))
                        {
                            if (dialogService.SaveFileDialog("", "Model files|*.mdat") == true)
                            {
                                serializationService.SaveModel(dialogService.FilePath, Root);
                                Settings.LastFile = dialogService.FilePath;
                                AddLogItem(DateTime.Now, "Model File: \"" + dialogService.FilePath + "\"", "Saved", LogItemTypeEnum.Information, "NewModelCommand");
                            }
                        }

                        Root = new TreeItem() { Children = { TreeItem.CreateRoot() } };
                        Settings.LastFile = string.Empty;
                        InitLog();
                        InitProbes();
                        AddLogItem(DateTime.Now, "New Model created", "Successful", LogItemTypeEnum.Information, "NewModelCommand");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "NewModelCommand");
                    }
                });
            }
        }

        private WorkCommand saveModelCommand;
        public WorkCommand SaveModelCommand
        {
            get
            {
                return saveModelCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (Settings.LastFile != string.Empty)
                        {
                            serializationService.SaveModel(Settings.LastFile, Root);
                        }
                        else
                        {
                            if (dialogService.SaveFileDialog("", "Model files|*.mdat") == true)
                            {
                                serializationService.SaveModel(dialogService.FilePath, Root);
                                Settings.LastFile = dialogService.FilePath;
                            }
                        }
                        AddLogItem(DateTime.Now, "Model File: \"" + Settings.LastFile + "\"", "Saved", LogItemTypeEnum.Information, "SaveModelCommand");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "SaveModelCommand");
                    }
                });
            }
        }

        private WorkCommand saveAsModelCommand;
        public WorkCommand SaveAsModelCommand
        {
            get
            {
                return saveAsModelCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (dialogService.SaveFileDialog("", "Model files|*.mdat") == true)
                        {
                            serializationService.SaveModel(dialogService.FilePath, Root);
                            Settings.LastFile = dialogService.FilePath;
                            AddLogItem(DateTime.Now, "Model File: \"" + Settings.LastFile + "\"", "Saved", LogItemTypeEnum.Information, "SaveAsModelCommand");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "SaveAsModelCommand");
                    }
                });
            }
        }

        private WorkCommand openModelCommand;
        public WorkCommand OpenModelCommand
        {
            get
            {
                return openModelCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (dialogService.OpenFileDialog("Model files|*.mdat") == true)
                        {
                            Root = serializationService.OpenModel(dialogService.FilePath);
                            Settings.LastFile = dialogService.FilePath;
                            InitLog();
                            InitProbes();
                            Root.Initialise();
                            if (Root != null)
                            {
                                Root.PropertyChanged -= SelectedItem_PropertyChanged;
                                Root.PropertyChanged += SelectedItem_PropertyChanged;
                            }
                            AddLogItem(DateTime.Now, "Model File: \"" + Settings.LastFile + "\"", "Opened", LogItemTypeEnum.Information, "OpenModelCommand");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "OpenModelCommand");
                    }
                });
            }
        }
        #endregion

        #region Log

        private WorkCommand saveLogCommand;
        public WorkCommand SaveLogCommand
        {
            get
            {
                return saveLogCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (dialogService.SaveFileDialog("GizmoAppLog", "Log files|*.Log") == true)
                        {
                            serializationService.SaveLog(dialogService.FilePath, Log);
                            AddLogItem(DateTime.Now, "Log file: \"" + dialogService.FilePath + "\"", "Saved", LogItemTypeEnum.Information, "SaveLogCommand");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "SaveLogCommand");
                    }
                });
            }
        }

        private WorkCommand openLogCommand;
        public WorkCommand OpenLogCommand
        {
            get
            {
                return openLogCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (dialogService.OpenFileDialog("Log files|*.Log") == true)
                        {
                            Log.Clear();
                            Log = serializationService.OpenLog(dialogService.FilePath);
                            AddLogItem(DateTime.Now, "Log file: \"" + dialogService.FilePath + "\"", "Opened", LogItemTypeEnum.Information, "OpenLogCommand");
                            OnPropertyChanged("Log");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "OpenLogCommand");
                    }
                });
            }
        }

        private WorkCommand clearLogCommand;
        public WorkCommand ClearLogCommand
        {
            get
            {
                return clearLogCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        Log.Clear();
                        AddLogItem(DateTime.Now, "Application Log", "Cleared", LogItemTypeEnum.Information, "ClearLogCommand");
                        OnPropertyChanged("Log");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ClearLogCommand");
                    }
                });
            }
        }

        #endregion

        #region Container

        private WorkCommand newContainerCommand;
        public WorkCommand NewContainerCommand
        {
            get
            {
                return newContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (treeItemDialogService.ContainerSettingsDialog(settings) == true)
                        {
                            AddContainer(treeItemDialogService.Name, treeItemDialogService.Description, SelectedTreeItem, treeItemDialogService.UseParentUserProfile, treeItemDialogService.UserProfileId);
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "NewContainerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand editContainerCommand;
        public WorkCommand EditContainerCommand
        {
            get
            {
                return editContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (treeItemDialogService.ContainerSettingsDialog(settings, SelectedTreeItem.Name, SelectedTreeItem.Description, SelectedTreeItem.UseParentUserProfile, SelectedTreeItem.UserProfileId) == true)
                        {
                            SelectedTreeItem.Name = treeItemDialogService.Name;
                            SelectedTreeItem.Description = treeItemDialogService.Description;
                            SelectedTreeItem.UseParentUserProfile = treeItemDialogService.UseParentUserProfile;
                            SelectedTreeItem.UserProfileId = treeItemDialogService.UseParentUserProfile ? new Guid() : treeItemDialogService.UserProfileId;
                            AddLogItem(DateTime.Now, "Container \"" + SelectedTreeItem.Name + "\" settings", "Saved", LogItemTypeEnum.Information, "EditContainerCommand");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "EditContainerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand deleteContainerCommand;
        public WorkCommand DeleteContainerCommand
        {
            get
            {
                return deleteContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        FindTreeItemAndDelete(SelectedTreeItem.Id);
                        AddLogItem(DateTime.Now, "Container \"" + SelectedTreeItem.Name + "\"", "Deleted", LogItemTypeEnum.Information, "DeleteContainerCommand");
                        //SelectedTreeItem = null;
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "DeleteContainerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand clearContainerCommand;
        public WorkCommand ClearContainerCommand
        {
            get
            {
                return clearContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        SelectedTreeItem.Children.Clear();
                        AddLogItem(DateTime.Now, "Container \"" + SelectedTreeItem.Name + "\" childrens", "Cleared", LogItemTypeEnum.Information, "ClearContainerCommand");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ClearContainerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand browseActiveDirectoryToContainerCommand;
        public WorkCommand BrowseActiveDirectoryToContainerCommand
        {
            get
            {
                return browseActiveDirectoryToContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (treeItemDialogService.ActiveDirectorySettingsDialog(Settings) == true)
                        {
                            NetProbes.Find(x => x.Id == SelectedTreeItem.Id).DoWork += AddFromAD_DoWork;
                            NetProbes.Find(x => x.Id == SelectedTreeItem.Id).RunWorkerCompleted += AddFromAD_RunWorkerCompleted;
                            NetProbes.Find(x => x.Id == SelectedTreeItem.Id).RunWorkerAsync(argument: treeItemDialogService.DomainSettings);
                            SelectedTreeItem.ProbeInUse = true;
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "BrowseActiveDirectoryToContainerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand updateFromActiveDirectoryCommand;
        public WorkCommand UpdateFromActiveDirectoryCommand
        {
            get
            {
                return updateFromActiveDirectoryCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        NetProbes.Find(x => x.Id == SelectedTreeItem.Id).DoWork += AddFromAD_DoWork;
                        NetProbes.Find(x => x.Id == SelectedTreeItem.Id).RunWorkerCompleted += AddFromAD2_RunWorkerCompleted;
                        NetProbes.Find(x => x.Id == SelectedTreeItem.Id).RunWorkerAsync(argument: SelectedTreeItem.DomainSettings);
                        SelectedTreeItem.ProbeInUse = true;
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "UpdateFromActiveDirectoryCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand editDomainDiscoverySettingsCommand;
        public WorkCommand EditDomainDiscoverySettingsCommand
        {
            get
            {
                return editDomainDiscoverySettingsCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (SelectedTreeItem.DomainSettings != null)
                        {
                            if (treeItemDialogService.ActiveDirectorySettingsDialog(Settings, SelectedTreeItem.DomainSettings) == true)
                            {
                                SelectedTreeItem.DomainSettings.UserProfileId = treeItemDialogService.DomainSettings.UserProfileId;
                            }
                        }
                        else
                        {
                            AddLogItem(DateTime.Now, "Domain Discovery Settings for \"" + SelectedTreeItem.Name + "\" not set.", "Exception", LogItemTypeEnum.Error, "EditDomainDiscoverySettingsCommand");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "EditDomainDiscoverySettingsCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }


        private WorkCommand pingContainerCommand;
        public WorkCommand PingContainerCommand
        {
            get
            {
                return pingContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        foreach (var node in SelectedTreeItem.Children.Where(x => x.Type == ItemTypeEnum.ChildComputer))
                        {
                            if (RunProbe(node.Id, false, ProbeTypeEnum.Ping))
                            {
                                node.ProbeInUse = true;
                                node.Status = ItemStatusEnum.Unknown;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "PingContainerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand checkContainerCommand;
        public WorkCommand CheckContainerCommand
        {
            get
            {
                return checkContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        foreach (var node in SelectedTreeItem.Children.Where(x => x.Type == ItemTypeEnum.ChildComputer))
                        {
                            if (RunProbe(node.Id, true, ProbeTypeEnum.Ping))
                            {
                                node.ProbeInUse = true;
                                node.Status = ItemStatusEnum.Unknown;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "CheckContainerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand clearScansInContainerCommand;
        public WorkCommand ClearScansInContainerCommand
        {
            get
            {
                return clearScansInContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        ClearHardwareScansInChildrends(SelectedTreeItem, ItemTypeEnum.ChildComputer);
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ClearScansInContainerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand clearScansButKeepLastOneInContainerCommand;
        public WorkCommand ClearScansButKeepLastOneInContainerCommand
        {
            get
            {
                return clearScansButKeepLastOneInContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        ClearHardwareScansButKeepLastOneInChildrends(SelectedTreeItem, ItemTypeEnum.ChildComputer);
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ClearScansButKeepLastOneInContainerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand clearScansButKeepLastTwoInContainerCommand;
        public WorkCommand ClearScansButKeepLastTwoInContainerCommand
        {
            get
            {
                return clearScansButKeepLastTwoInContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        ClearHardwareScansButKeepLastTwoInChildrends(SelectedTreeItem, ItemTypeEnum.ChildComputer);
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ClearScansButKeepLastTwoInContainerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }


        #endregion

        #region Computer
        private WorkCommand newComputerCommand;
        public WorkCommand NewComputerCommand
        {
            get
            {
                return newComputerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (treeItemDialogService.ComputerSettingsDialog(Settings) == true)
                        {
                            AddComputer(treeItemDialogService.Name, treeItemDialogService.Description, treeItemDialogService.FQDN, treeItemDialogService.IPAddress, SelectedTreeItem, treeItemDialogService.UseParentUserProfile, treeItemDialogService.UserProfileId);
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "NewComputerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand editComputerCommand;
        public WorkCommand EditComputerCommand
        {
            get
            {
                return editComputerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (treeItemDialogService.ComputerSettingsDialog(settings, SelectedTreeItem.Name, SelectedTreeItem.Description, SelectedTreeItem.FQDN, SelectedTreeItem.Address, SelectedTreeItem.UseParentUserProfile, SelectedTreeItem.UserProfileId) == true)
                        {
                            SelectedTreeItem.Name = treeItemDialogService.Name;
                            SelectedTreeItem.Description = treeItemDialogService.Description;
                            SelectedTreeItem.FQDN = treeItemDialogService.FQDN;
                            SelectedTreeItem.Address = treeItemDialogService.IPAddress;
                            SelectedTreeItem.UseParentUserProfile = treeItemDialogService.UseParentUserProfile;
                            SelectedTreeItem.UserProfileId = treeItemDialogService.UseParentUserProfile ? new Guid() : treeItemDialogService.UserProfileId;
                            AddLogItem(DateTime.Now, "Computer \"" + SelectedTreeItem.Name + "\" settings", "Saved", LogItemTypeEnum.Information, "EditComputerCommand");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "EditComputerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand deleteComputerCommand;
        public WorkCommand DeleteComputerCommand
        {
            get
            {
                return deleteComputerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        FindTreeItemAndDelete(SelectedTreeItem.Id);
                        AddLogItem(DateTime.Now, "Computer \"" + SelectedTreeItem.Name + "\"", "Deleted", LogItemTypeEnum.Information, "DeleteComputerCommand");
                        //SelectedTreeItem = null;
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "DeleteComputerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand pingComputerCommand;
        public WorkCommand PingComputerCommand
        {
            get
            {
                return pingComputerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (RunProbe(SelectedTreeItem.Id, false, ProbeTypeEnum.Ping))
                        {
                            SelectedTreeItem.ProbeInUse = true;
                            SelectedTreeItem.Status = ItemStatusEnum.Unknown;
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "PingComputerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand checkComputerCommand;
        public WorkCommand CheckComputerCommand
        {
            get
            {
                return checkComputerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (RunProbe(SelectedTreeItem.Id, true, ProbeTypeEnum.Ping))
                        {
                            SelectedTreeItem.ProbeInUse = true;
                            SelectedTreeItem.Status = ItemStatusEnum.Unknown;
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "CheckComputerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand openSharedFolderCommand;
        public WorkCommand OpenSharedFolderCommand
        {
            get
            {
                return openSharedFolderCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        Process.Start(new ProcessStartInfo()
                        {
                            FileName = (obj as string),
                            UseShellExecute = true,
                            Verb = "open"
                        });
                        AddLogItem(DateTime.Now, "Opening shared folder: \"" + (obj as string) + "\"", string.Empty, LogItemTypeEnum.Warning, "OpenSharedFolderCommand");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "OpenSharedFolderCommand");
                    }
                }, (obj) => SelectedTreeItem != null);
            }
        }

        private WorkCommand openTCPPortCommand;
        public WorkCommand OpenTCPPortCommand
        {
            get
            {
                return openTCPPortCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        OpenTCPPort((Guid)obj);
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "OpenTCPPortCommand");
                    }
                }, (obj) => SelectedTreeItem != null);
            }
        }

        private WorkCommand importScanCommand;
        public WorkCommand ImportScanCommand
        {
            get
            {
                return importScanCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (dialogService.OpenFileDialog("Scan files|*.scan") == true)
                        {
                            SelectedTreeItem.HardwareScans.Add(serializationService.ImportScan(dialogService.FilePath));
                            SelectedTreeItem.HardwareScans = new ObservableCollection<ComputerHardwareScan>(SelectedTreeItem.HardwareScans.OrderBy(x => x.ScanTime).ToList());
                            AddLogItem(DateTime.Now, "Scan file \"" + dialogService.FilePath + "\"", "Imported", LogItemTypeEnum.Information, "ImportScanCommand");
                            OnPropertyChanged("SelectedTreeItem");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ImportScanCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand exportScanCommand;
        public WorkCommand ExportScanCommand
        {
            get
            {
                return exportScanCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (obj != null)
                        {
                            var index = (int)obj;
                            if (index != -1)
                            {
                                if (dialogService.SaveFileDialog(SelectedTreeItem.Name + "_" + SelectedTreeItem.HardwareScans[index].ScanTime.ToShortDateString().Replace(".", "_") + "_" + SelectedTreeItem.HardwareScans[index].ScanTime.ToLongTimeString().Replace(":", "_"), "Scan files|*.scan") == true)
                                {
                                    serializationService.ExportScan(dialogService.FilePath, SelectedTreeItem.HardwareScans[index]);
                                    AddLogItem(DateTime.Now, "Scan file \"" + dialogService.FilePath + "\"", "Exported", LogItemTypeEnum.Information, "ExportScanCommand");
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ExportScanCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand deleteScanCommand;
        public WorkCommand DeleteScanCommand
        {
            get
            {
                return deleteScanCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (obj != null)
                        {
                            var index = (int)obj;
                            if (index != -1)
                            {
                                SelectedTreeItem.DeleteScan(index);
                                OnPropertyChanged("SelectedTreeItem");
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "DeleteScanCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand clearScansCommand;
        public WorkCommand ClearScansCommand
        {
            get
            {
                return clearScansCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        SelectedTreeItem.ClearScans();
                        OnPropertyChanged("SelectedTreeItem");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ClearScansCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand clearScansButKeepLastOneCommand;
        public WorkCommand ClearScansButKeepLastOneCommand
        {
            get
            {
                return clearScansButKeepLastOneCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        SelectedTreeItem.ClearScansButKeepLastOne();
                        OnPropertyChanged("SelectedTreeItem");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ClearScansButKeepLastOneCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand clearScansButKeepLastTwoCommand;
        public WorkCommand ClearScansButKeepLastTwoCommand
        {
            get
            {
                return clearScansButKeepLastTwoCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        SelectedTreeItem.ClearScansButKeepLastTwo();
                        OnPropertyChanged("SelectedTreeItem");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ClearScansButKeepLastTwoCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        private WorkCommand moveToContainerCommand;
        public WorkCommand MoveToContainerCommand
        {
            get
            {
                return moveToContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        var DestinationContainer = FindTreeItemByGuid((Guid)obj);
                        var SourceContainer = FindTreeItemByGuid(SelectedTreeItem.ParentId);
                        var Item = SelectedTreeItem;
                        SelectedTreeItem.IsSelected = false;
                        FindChildrenIdAndDelete(Root, Item.Id);
                        DestinationContainer.Children.Add(Item);
                        DestinationContainer.IsExpanded = true;
                        Item.IsSelected = true;
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "MoveToContainerCommand");
                    }
                }, (obj) => SelectedTreeItem != null ? !SelectedTreeItem.ProbeInUse : false);
            }
        }

        #endregion

        #endregion

        #region Browse from Active Directory
        private void AddFromAD_DoWork(object sender, DoWorkEventArgs e)
        {
            DomainDiscoverySettings discoverySettings = e.Argument as DomainDiscoverySettings;
            List<object> result = new List<object>() { discoverySettings, null };
            try
            {
                result[1] = DomainDiscovery.EnumerateDomainInformation(discoverySettings.Name, Settings.UserProfiles.Where(x => x.Id == discoverySettings.UserProfileId).First(), discoverySettings.Mode);
                e.Result = result;
            }
            catch (Exception ex)
            {
                result[1] = ex;
                e.Result = result;
            }
        }

        private void AddFromAD_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var RootNode = FindTreeItemByGuid((sender as BackgroundWorkerWithId).Id);
            if (e.Result != null)
            {
                if (e.Result is List<object>)
                {
                    if ((e.Result as List<object>)[1] != null)
                    {
                        if ((e.Result as List<object>)[1] is List<DomainInformation> domainInformation)
                        {
                            if (domainInformation.Count != 0)
                            {
                                foreach (var node in domainInformation)
                                {
                                    var newRootContainer = new TreeItem();
                                    if (!IsTreeItemPresent(node.Name))
                                    {
                                        newRootContainer = NewContainer(node.Name, node.Description, RootNode, true, new Guid());
                                        newRootContainer.Type = ItemTypeEnum.DomainRoot;

                                        newRootContainer.DomainSettings = (e.Result as List<object>)[0] as DomainDiscoverySettings;
                                        newRootContainer.UseParentUserProfile = false;
                                        newRootContainer.UserProfileId = newRootContainer.DomainSettings.UserProfileId;

                                        InitProbe(newRootContainer);
                                        RootNode.Children.Add(newRootContainer);
                                    }
                                    else
                                    {
                                        newRootContainer = FindTreeItemByName(node.Name);

                                        newRootContainer.DomainSettings = (e.Result as List<object>)[0] as DomainDiscoverySettings;
                                        newRootContainer.UseParentUserProfile = false;
                                        newRootContainer.UserProfileId = newRootContainer.DomainSettings.UserProfileId;
                                    }
                                    if (node.HasChildren)
                                    {
                                        EnumerateChildrens(node, newRootContainer);
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        else if ((e.Result as List<object>)[1] is Exception)
                        {
                            AddLogItem(DateTime.Now, ((e.Result as List<object>)[1] as Exception).Message, "Exception", LogItemTypeEnum.Error, "AddFromAD_RunWorkerCompleted");
                        }
                    }
                    else if ((e.Result as List<object>)[1] is Exception)
                    {
                        AddLogItem(DateTime.Now, ((e.Result as List<object>)[1] as Exception).Message, "Exception", LogItemTypeEnum.Error, "AddFromAD_RunWorkerCompleted");
                    }
                }
            }
            RootNode.ProbeInUse = false;
            NetProbes.Find(x => x.Id == (sender as BackgroundWorkerWithId).Id).DoWork -= AddFromAD_DoWork;
            NetProbes.Find(x => x.Id == (sender as BackgroundWorkerWithId).Id).RunWorkerCompleted -= AddFromAD_RunWorkerCompleted;
            GC.Collect();
        }

        private void AddFromAD2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var RootNode = FindTreeItemByGuid((sender as BackgroundWorkerWithId).Id);
            RootNode.DomainSettings = (e.Result as List<object>)[0] as DomainDiscoverySettings;
            RootNode.UseParentUserProfile = false;
            RootNode.UserProfileId = RootNode.DomainSettings.UserProfileId;

            if (e.Result != null)
            {
                if (e.Result is List<object>)
                {
                    if ((e.Result as List<object>)[1] != null)
                    {
                        if ((e.Result as List<object>)[1] is List<DomainInformation> domainInformation)
                        {
                            if (domainInformation.Count != 0)
                            {
                                foreach (var node in domainInformation)
                                {
                                    if (node.Name == RootNode.Name)
                                    {
                                        if (node.HasChildren)
                                        {
                                            EnumerateChildrens(node, RootNode);
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                }
                            }
                        }
                        else if ((e.Result as List<object>)[1] is Exception)
                        {
                            AddLogItem(DateTime.Now, ((e.Result as List<object>)[1] as Exception).Message, "Exception", LogItemTypeEnum.Error, "AddFromAD2_RunWorkerCompleted");
                        }
                    }
                    else if ((e.Result as List<object>)[1] is Exception)
                    {
                        AddLogItem(DateTime.Now, ((e.Result as List<object>)[1] as Exception).Message, "Exception", LogItemTypeEnum.Error, "AddFromAD2_RunWorkerCompleted");
                    }
                }
            }
            RootNode.ProbeInUse = false;
            NetProbes.Find(x => x.Id == (sender as BackgroundWorkerWithId).Id).DoWork -= AddFromAD_DoWork;
            NetProbes.Find(x => x.Id == (sender as BackgroundWorkerWithId).Id).RunWorkerCompleted -= AddFromAD2_RunWorkerCompleted;
            GC.Collect();
        }

        private void EnumerateChildrens(DomainInformation node, TreeItem newRootContainer)
        {
            foreach (var child in node.Childrens)
            {
                switch (child.Type)
                {
                    case DomainInformationTypeEnum.OrganizationUnit:
                        {

                            if (newRootContainer.Children.Where(x => x.Name == child.Name).Count() == 0)
                            {
                                var newADContainer = NewContainer(child.Name, child.Description, newRootContainer, true, new Guid());
                                InitProbe(newADContainer);
                                if (child.HasChildren)
                                {
                                    EnumerateChildrens(child, newADContainer);
                                }
                                newRootContainer.Children.Add(newADContainer);
                            }
                            else
                            {
                                newRootContainer.Children.Where(x => x.Name == child.Name).First().Description = child.Description;
                                if (child.HasChildren)
                                {
                                    EnumerateChildrens(child, newRootContainer.Children.Where(x => x.Name == child.Name).First());
                                }
                            }
                            break;
                        }
                    case DomainInformationTypeEnum.Computer:
                        {
                            if (newRootContainer.Children.Where(x => x.Name == (child.Info as ActiveDirectoryComputerInfo).Name).Count() == 0)
                            {
                                var newADComputer = NewComputer((child.Info as ActiveDirectoryComputerInfo).Name, (child.Info as ActiveDirectoryComputerInfo).Description, (child.Info as ActiveDirectoryComputerInfo).DNSHostName, "", newRootContainer, true, new Guid());
                                InitProbe(newADComputer);
                                newRootContainer.Children.Add(newADComputer);
                            }
                            else
                            {
                                newRootContainer.Children.Where(x => x.Name == (child.Info as ActiveDirectoryComputerInfo).Name).First().Description = (child.Info as ActiveDirectoryComputerInfo).Description;
                            }
                            break;
                        }
                }
            }
        }
        #endregion

        #region Ping Computer
        private void CheckPing_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = new List<object>() { (bool)e.Argument, null };
            var treeItem = FindTreeItemByGuid((sender as BackgroundWorkerWithId).Id);
            try
            {
                string Name = treeItem.FQDN != string.Empty ? treeItem.FQDN : treeItem.Address != string.Empty ? treeItem.Address : treeItem.Name;
                result[1] = new PingWithId() { Id = treeItem.Id, NetworkIp = Name, Timeout = 1000 }.SendPing();
            }
            catch (Exception ex)
            {
                result[1] = ex;
            }
            e.Result = result;
        }

        private void CheckPing_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var node = FindTreeItemByGuid((sender as BackgroundWorkerWithId).Id);

            if (e.Result != null)
            {
                if (e.Result is List<object>)
                {
                    if ((e.Result as List<object>)[1] != null)
                    {
                        if ((e.Result as List<object>)[1] is IPStatus status)
                        {
                            if (status == IPStatus.Success)
                            {
                                ReleaseProbe((sender as BackgroundWorkerWithId).Id, ProbeTypeEnum.Ping);
                                if (node != null)
                                {
                                    node.Status = ItemStatusEnum.Online;
                                    node.ProbeInUse = false;

                                    if (Settings.CheckPortsThenPing && RunProbe(node.Id, (bool)(e.Result as List<object>)[0], ProbeTypeEnum.CheckTCPPort))
                                    {
                                        node.Status = ItemStatusEnum.OnlineAndCheckingTCPPorts;
                                        node.ProbeInUse = true;
                                    }
                                    else if ((bool)(e.Result as List<object>)[0] && RunProbe(node.Id, null, ProbeTypeEnum.CheckWMI))
                                    {
                                        node.Status = ItemStatusEnum.OnlineAndFetchingData;
                                        node.ProbeInUse = true;
                                    }
                                }
                            }
                            else
                            {
                                ReleaseProbe((sender as BackgroundWorkerWithId).Id, ProbeTypeEnum.Ping);
                                if (node != null)
                                {
                                    node.Status = ItemStatusEnum.Offline;
                                    node.ProbeInUse = false;
                                }
                            }
                        }
                        else if ((e.Result as List<object>)[1] is Exception)
                        {
                            ReleaseProbe((sender as BackgroundWorkerWithId).Id, ProbeTypeEnum.Ping);
                            if (node != null)
                            {
                                node.Status = ItemStatusEnum.Error;
                                node.ProbeInUse = false;
                            }
                            AddLogItem(DateTime.Now, ((e.Result as List<object>)[1] as Exception).Message, "Exception", LogItemTypeEnum.Error, "CheckPing_RunWorkerCompleted");
                        }
                    }
                }
            }
        }

        private void CheckTCPPorts_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = new List<object>() { (bool)e.Argument, null };
            var treeItem = FindTreeItemByGuid((sender as BackgroundWorkerWithId).Id);
            try
            {
                if (Settings.CheckPortsThenPing == true)
                {
                    string Name = treeItem.FQDN != string.Empty ? treeItem.FQDN : treeItem.Address != string.Empty ? treeItem.Address : treeItem.Name;
                    treeItem.CheckPortResults.Clear();
                    foreach (var CheckNode in Settings.DefaultCheckPorts.Where(x => x.IsEnabled))
                    {
                        treeItem.CheckPortResults.Add(CheckNode.CheckAsync(Name, 5000));
                    }
                }
                result[1] = true;
            }
            catch (Exception ex)
            {
                result[1] = ex;
            }
            e.Result = result;
        }

        private void CheckTCPPorts_RunWorkerComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            var node = FindTreeItemByGuid((sender as BackgroundWorkerWithId).Id);
            node.OnNamedPropertyChanged("CheckPortResults");
            if (e.Result != null)
            {
                if (e.Result is List<object>)
                {
                    if ((e.Result as List<object>)[1] != null)
                    {
                        if ((e.Result as List<object>)[1] is bool boolean)
                        {
                            if (boolean)
                            {
                                ReleaseProbe((sender as BackgroundWorkerWithId).Id, ProbeTypeEnum.CheckTCPPort);
                                if (node != null)
                                {
                                    node.Status = ItemStatusEnum.Online;
                                    node.ProbeInUse = false;

                                    if (Settings.CheckSharedFoldersThenPing && RunProbe(node.Id, (bool)(e.Result as List<object>)[0], ProbeTypeEnum.CheckSharedFolders))
                                    {
                                        node.Status = ItemStatusEnum.OnlineAndCheckingSharedFolders;
                                        node.ProbeInUse = true;
                                    }
                                    else if ((bool)(e.Result as List<object>)[0] && RunProbe(node.Id, null, ProbeTypeEnum.CheckWMI))
                                    {
                                        node.Status = ItemStatusEnum.OnlineAndFetchingData;
                                        node.ProbeInUse = true;
                                    }
                                }
                            }
                            else
                            {
                                ReleaseProbe((sender as BackgroundWorkerWithId).Id, ProbeTypeEnum.CheckTCPPort);
                                if (node != null)
                                {
                                    node.Status = ItemStatusEnum.Offline;
                                    node.ProbeInUse = false;
                                }
                            }
                        }
                        else if ((e.Result as List<object>)[1] is Exception)
                        {
                            ReleaseProbe((sender as BackgroundWorkerWithId).Id, ProbeTypeEnum.CheckTCPPort);
                            if (node != null)
                            {
                                node.Status = ItemStatusEnum.Error;
                                node.ProbeInUse = false;
                            }
                            AddLogItem(DateTime.Now, ((e.Result as List<object>)[1] as Exception).Message, "Exception", LogItemTypeEnum.Error, "CheckTCPPorts_RunWorkerComplited");
                        }
                    }
                }
            }
        }

        private void CheckSharedFolders_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = new List<object>() { (bool)e.Argument, null };

            var treeItem = FindTreeItemByGuid((sender as BackgroundWorkerWithId).Id);
            try
            {
                if (Settings.CheckSharedFoldersThenPing == true)
                {
                    string Name = treeItem.FQDN != string.Empty ? treeItem.FQDN : treeItem.Address != string.Empty ? treeItem.Address : treeItem.Name;
                    treeItem.SharedFolders.Clear();
                    treeItem.SharedFolders = GetDirectories(Name);
                }
                result[1] = true;
            }
            catch (Exception ex)
            {
                result[1] = ex;
            }
            e.Result = result;
        }

        private void CheckSharedFolders_RunWorkerComplited(object sender, RunWorkerCompletedEventArgs e)
        {
            var node = FindTreeItemByGuid((sender as BackgroundWorkerWithId).Id);
            node.OnNamedPropertyChanged("SharedFolders");
            if (e.Result != null)
            {
                if (e.Result is List<object>)
                {
                    if ((e.Result as List<object>)[1] != null)
                    {
                        if ((e.Result as List<object>)[1] is bool boolean)
                        {
                            if (boolean)
                            {
                                ReleaseProbe((sender as BackgroundWorkerWithId).Id, ProbeTypeEnum.CheckSharedFolders);
                                if (node != null)
                                {
                                    node.Status = ItemStatusEnum.Online;
                                    node.ProbeInUse = false;

                                    if ((bool)(e.Result as List<object>)[0] && RunProbe(node.Id, null, ProbeTypeEnum.CheckWMI))
                                    {
                                        node.Status = ItemStatusEnum.OnlineAndFetchingData;
                                        node.ProbeInUse = true;
                                    }
                                }
                            }
                            else
                            {
                                ReleaseProbe((sender as BackgroundWorkerWithId).Id, ProbeTypeEnum.CheckSharedFolders);
                                if (node != null)
                                {
                                    node.Status = ItemStatusEnum.Offline;
                                    node.ProbeInUse = false;
                                }
                            }
                        }
                        else if ((e.Result as List<object>)[1] is Exception)
                        {
                            ReleaseProbe((sender as BackgroundWorkerWithId).Id, ProbeTypeEnum.CheckSharedFolders);
                            if (node != null)
                            {
                                node.Status = ItemStatusEnum.Error;
                                node.ProbeInUse = false;
                            }
                            AddLogItem(DateTime.Now, ((e.Result as List<object>)[1] as Exception).Message, "Exception", LogItemTypeEnum.Error, "CheckSharedFolders_RunWorkerComplited");
                        }
                    }
                }
            }
        }
        #endregion

        #region Scan Computer WMI
        private void CheckWMI_DoWork(object sender, DoWorkEventArgs e)
        {
            var treeItem = FindTreeItemByGuid((sender as BackgroundWorkerWithId).Id);
            if (treeItem != null)
            {
                var treeItemConnectionOptions = treeItem.UseParentUserProfile ? GetConnectionOptions(treeItem.ParentId) : BuildConnectionOptions(treeItem.UserProfileId);

                string Name = treeItem.FQDN != string.Empty ? treeItem.FQDN : treeItem.Address != string.Empty ? treeItem.Address : treeItem.Name;
                try
                {
                    e.Result = ComputerHardwareScan.Scan(Name, treeItemConnectionOptions ?? new ConnectionOptions(), treeItem.Name.ToLower() == Environment.MachineName.ToLower());
                }
                catch (Exception ex)
                {
                    e.Result = ex;
                }
            }
        }

        private void CheckWMI_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var node = FindTreeItemByGuid((sender as BackgroundWorkerWithId).Id);

            if (e.Result != null && e.Result is ComputerHardwareScan)
            {
                ReleaseProbe((sender as BackgroundWorkerWithId).Id, ProbeTypeEnum.CheckWMI);
                if (node != null)
                {
                    node.Status = ItemStatusEnum.OnlineAndHasData;
                    node.ProbeInUse = false;
                    node.AddScan(0, e.Result as ComputerHardwareScan);
                    if (SelectedTreeItem != null && SelectedTreeItem.Id == node.Id)
                    {
                        OnPropertyChanged("SelectedTreeItem");
                    }
                }
            }
            else if (e.Result != null && e.Result is Exception)
            {
                ReleaseProbe((sender as BackgroundWorkerWithId).Id, ProbeTypeEnum.CheckWMI);
                if (node != null)
                {
                    node.Status = node.Status == ItemStatusEnum.OnlineAndFetchingData ? ItemStatusEnum.OnlineButHasNoData : ItemStatusEnum.Error;
                    node.ProbeInUse = false;
                }
                AddLogItem(DateTime.Now, (e.Result as Exception).Message, "Exception", LogItemTypeEnum.Error, "CheckWMI_RunWorkerCompleted");
            }
        }
        #endregion

        #region Private Methods
        private ObservableCollection<string> GetDirectories(string servername)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var enc866 = Encoding.GetEncoding(866);
            cmd.StartInfo.StandardOutputEncoding = enc866;
            cmd.Start();
            cmd.StandardInput.WriteLine($"net view {servername}");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string output = cmd.StandardOutput.ReadToEnd();

            cmd.WaitForExit();
            cmd.Close();
            var List = new ObservableCollection<string>();
            if (output.Contains("Disk"))
            {
                string CurrentExecutePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                output = output.Replace(CurrentExecutePath, "");
                output = output.Substring(output.LastIndexOf('-') + 2);
                output = output.Substring(0, output.IndexOf("The command completed successfully."));
                var outList = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => System.IO.Path.Combine(servername, x)).ToArray().ToList();
                foreach (var node in outList)
                {
                    List.Add("\\\\" + node.Split(new string[] { "Disk" }, StringSplitOptions.None)[0].TrimEnd());
                }
            }
            else if (output.Contains("Диск"))
            {
                string CurrentExecutePath = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                output = output.Replace(CurrentExecutePath, "");
                output = output.Substring(output.LastIndexOf('-') + 2);
                output = output.Substring(0, output.IndexOf("Команда выполнена успешно."));
                var outList = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => System.IO.Path.Combine(servername, x)).ToArray().ToList();
                foreach (var node in outList)
                {
                    List.Add("\\\\" + node.Split(new string[] { "Диск" }, StringSplitOptions.None)[0].TrimEnd());
                }
            }
            return List;
        }

        private void OpenTCPPort(Guid Id)
        {
            foreach (var node in Settings.DefaultCheckPorts)
            {
                if (node.Id == Id)
                {
                    string Name = SelectedTreeItem.FQDN != string.Empty ? SelectedTreeItem.FQDN : SelectedTreeItem.Address != string.Empty ? SelectedTreeItem.Address : SelectedTreeItem.Name;
                    Process.Start(new ProcessStartInfo() { FileName = node.LaunchPath.Replace("HostName", Name), Arguments = node.LaunchArg.Replace("HostName", Name), UseShellExecute = true });
                    AddLogItem(DateTime.Now, "Opening connection to: \"" + Name + "\"", node.Name, LogItemTypeEnum.Warning, "OpenTCPPort");
                }
            }
        }
        #endregion
    }
}
