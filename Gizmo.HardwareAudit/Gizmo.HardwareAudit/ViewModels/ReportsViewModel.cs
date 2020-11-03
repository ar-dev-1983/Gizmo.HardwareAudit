using Gizmo.HardwareAudit.Classes;
using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAuditClasses;
using Gizmo.HardwareAuditClasses.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;

namespace Gizmo.HardwareAudit.ViewModels
{
    //NOTE: ReportViewModel is a part of AppViewModel Class
    public partial class AppViewModel : BaseViewModel
    {
        #region ReportItem Private Properties
        private ReportItem reportRoot;
        #endregion

        #region ReportItem Public Properties
        public ReportItem ReportRoot
        {
            get => reportRoot;
            set
            {
                if (reportRoot == value) return;
                reportRoot = value;
                OnPropertyChanged();
            }
        }

        public ReportItem SelectedReportItem => ReportRoot.SelectedReport;
        #endregion

        #region Adding Elements
        internal void AddReportContainer(string name, string description, ReportItem Parent, bool useCustomIcom, GizmoIconEnum customIcon)
        {
            if (Parent != null)
            {
                Parent.Children.Add(new ReportItem() { ParentId = Parent.Id, Name = name, Description = description, UseCustomIcon = useCustomIcom, CustomIcon = customIcon, ParentType = Parent.Type == ReportItemTypeEnum.Container ? Parent.ParentType : Parent.Type, Type = ReportItemTypeEnum.Container });
                AddLogItem(DateTime.Now, "Container \"" + name + "\" added", string.Empty, LogItemTypeEnum.Information, "AddReportContainer");
            }
        }

        internal ReportItem NewReportContainer(string name, string description, ReportItem Parent, bool useCustomIcom, GizmoIconEnum customIcon)
        {
            AddLogItem(DateTime.Now, "Container \"" + name + "\" added", string.Empty, LogItemTypeEnum.Information, "NewReportContainer");
            return new ReportItem() { ParentId = Parent.Id, Name = name, Description = description, UseCustomIcon = useCustomIcom, CustomIcon = customIcon, ParentType = Parent.Type == ReportItemTypeEnum.Container ? Parent.ParentType : Parent.Type, Type = ReportItemTypeEnum.Container };
        }

        internal void AddReportItem(string name, string description, ReportItem Parent, bool useCustomIcom, GizmoIconEnum customIcon, ReportSettings reportSettings)
        {
            if (Parent != null)
            {
                var Child = new ReportItem() { ParentId = Parent.Id, Name = name, Description = description, UseCustomIcon = useCustomIcom, CustomIcon = customIcon, ParentType = Parent.Type == ReportItemTypeEnum.Container ? Parent.ParentType : Parent.Type, Type = ReportItemTypeEnum.Report, Settings = reportSettings };
                Child.InitialiseDataGrid();
                Parent.Children.Add(Child);
                AddLogItem(DateTime.Now, "Report \"" + name + "\" added", string.Empty, LogItemTypeEnum.Information, "AddReportContainer");
            }
        }

        internal ReportItem NewReportItem(string name, string description, ReportItem Parent, bool useCustomIcom, GizmoIconEnum customIcon, ReportSettings reportSettings)
        {
            var Child = new ReportItem() { ParentId = Parent.Id, Name = name, Description = description, UseCustomIcon = useCustomIcom, CustomIcon = customIcon, ParentType = Parent.Type == ReportItemTypeEnum.Container ? Parent.ParentType : Parent.Type, Type = ReportItemTypeEnum.Report, Settings = reportSettings };
            Child.InitialiseDataGrid();
            AddLogItem(DateTime.Now, "Report \"" + name + "\" added", string.Empty, LogItemTypeEnum.Information, "NewReportContainer");
            return Child;
        }
        #endregion

        #region Report Model
        private WorkCommand newReportModelCommand;
        public WorkCommand NewReportModelCommand
        {
            get
            {
                return newReportModelCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (dialogService.QueryYesNoAnswer("Save existing Report Model before creating new one?"))
                        {
                            if (dialogService.SaveFileDialog("", "Report Model files|*.rdat") == true)
                            {
                                serializationService.SaveReportModel(dialogService.FilePath, ReportRoot);
                                Settings.LastReportFile = dialogService.FilePath;
                                AddLogItem(DateTime.Now, "Report Model File: \"" + dialogService.FilePath + "\"", "Saved", LogItemTypeEnum.Information, "NewReportModelCommand");
                            }
                        }

                        ReportRoot = new ReportItem() { Children = { ReportItem.CreateRoot() } };
                        AddLogItem(DateTime.Now, "New Report Model created", "Successful", LogItemTypeEnum.Information, "NewReportModelCommand");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "NewReportModelCommand");
                    }
                });
            }
        }

        private WorkCommand saveReportModelCommand;
        public WorkCommand SaveReportModelCommand
        {
            get
            {
                return saveReportModelCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (Settings.LastReportFile != string.Empty)
                        {
                            serializationService.SaveReportModel(Settings.LastReportFile, ReportRoot);
                        }
                        else
                        {
                            if (dialogService.SaveFileDialog("", "Report Model files|*.rdat") == true)
                            {
                                Settings.LastReportFile = dialogService.FilePath;
                                serializationService.SaveReportModel(dialogService.FilePath, ReportRoot);
                            }
                        }
                        AddLogItem(DateTime.Now, "Report Model File: \"" + Settings.LastReportFile + "\"", "Saved", LogItemTypeEnum.Information, "SaveReportModelCommand");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "SaveReportModelCommand");
                    }
                });
            }
        }

        private WorkCommand saveAsReportModelCommand;
        public WorkCommand SaveAsReportModelCommand
        {
            get
            {
                return saveAsReportModelCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (dialogService.SaveFileDialog("", "Report Model files|*.rdat") == true)
                        {
                            serializationService.SaveReportModel(dialogService.FilePath, ReportRoot);
                            Settings.LastReportFile = dialogService.FilePath;
                            AddLogItem(DateTime.Now, "Model File: \"" + Settings.LastReportFile + "\"", "Saved", LogItemTypeEnum.Information, "SaveAsReportModelCommand");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "SaveAsReportModelCommand");
                    }
                });
            }
        }

        private WorkCommand openReportModelCommand;
        public WorkCommand OpenReportModelCommand
        {
            get
            {
                return openReportModelCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (dialogService.OpenFileDialog("Report Model files|*.rdat") == true)
                        {
                            ReportRoot = serializationService.OpenReportModel(dialogService.FilePath);
                            InitReports();
                            ReportRoot.Initialise();
                            if (ReportRoot != null)
                            {
                                ReportRoot.PropertyChanged -= SelectedItem_PropertyChanged;
                                ReportRoot.PropertyChanged += SelectedItem_PropertyChanged;
                            }
                            Settings.LastReportFile = dialogService.FilePath;

                            AddLogItem(DateTime.Now, "Report Model File: \"" + Settings.LastReportFile + "\"", "Opened", LogItemTypeEnum.Information, "OpenReportModelCommand");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "OpenReportModelCommand");
                    }
                });
            }
        }
        #endregion

        #region Report Commands
        private WorkCommand newReportCommand;
        public WorkCommand NewReportCommand
        {
            get
            {
                return newReportCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (treeItemDialogService.ReportItemSettingsDialog(settings, Root) == true)
                        {
                            AddReportItem(treeItemDialogService.Name, treeItemDialogService.Description, SelectedReportItem, treeItemDialogService.UseCustomIcon, treeItemDialogService.CustomIcon, treeItemDialogService.ReportSettings);
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "NewReportCommand");
                    }
                }, (obj) => SelectedReportItem != null);
            }
        }

        private WorkCommand editReportCommand;
        public WorkCommand EditReportCommand
        {
            get
            {
                return editReportCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (treeItemDialogService.ReportItemSettingsDialog(settings, SelectedReportItem.Name, SelectedReportItem.Description, SelectedReportItem.UseCustomIcon, SelectedReportItem.CustomIcon, Root, SelectedReportItem.Settings) == true)
                        {
                            SelectedReportItem.Name = treeItemDialogService.Name;
                            SelectedReportItem.Description = treeItemDialogService.Description;
                            SelectedReportItem.UseCustomIcon = treeItemDialogService.UseCustomIcon;
                            SelectedReportItem.CustomIcon = treeItemDialogService.CustomIcon;
                            SelectedReportItem.Settings = treeItemDialogService.ReportSettings;
                            SelectedReportItem.InitialiseDataGrid();
                            AddLogItem(DateTime.Now, "Report \"" + SelectedReportItem.Name + "\" settings", "Saved", LogItemTypeEnum.Information, "EditReportCommand");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "EditReportCommand");
                    }
                }, (obj) => SelectedReportItem != null ? !SelectedReportItem.ReportIsBusy : false);
            }
        }

        private WorkCommand deleteReportCommand;
        public WorkCommand DeleteReportCommand
        {
            get
            {
                return deleteReportCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        FindReportItemAndDelete(SelectedReportItem.Id);
                        AddLogItem(DateTime.Now, "Report \"" + SelectedReportItem.Name + "\"", "Deleted", LogItemTypeEnum.Information, "DeleteReportItemCommand");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "DeleteReportItemCommand");
                    }
                }, (obj) => SelectedReportItem != null ? !SelectedReportItem.ReportIsBusy : false);
            }
        }

        private WorkCommand buildReportCommand;
        public WorkCommand BuildReportCommand
        {
            get
            {
                return buildReportCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        var item = FindReportItemByGuid(SelectedReportItem.Id);
                        item.ReportIsBusy = true;
                        item.ReportBuildIsDone = false;
                        SelectedReportItem.IsSelected = false;
                        BuildDataGrid(item);
                        item.IsSelected = true;
                        item.ReportIsBusy = false;
                        item.ReportBuildIsDone = true;
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "BuildReportCommand");
                    }
                }, (obj) => SelectedReportItem != null ? !SelectedReportItem.ReportIsBusy : false);
            }
        }

        private WorkCommand exportAsCSVReportCommand;
        public WorkCommand ExportAsCSVReportCommand
        {
            get
            {
                return exportAsCSVReportCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (SelectedReportItem.ReportBuildIsDone)
                        {
                            if (dialogService.SaveFileDialog(SelectedReportItem.Name.Replace(" ", "_") + ".csv", "CSV files|*.csv") == true)
                            {
                                ExportAsCSVFile(SelectedReportItem, dialogService.FilePath);
                            }
                        }
                        else
                        {
                            var item = FindReportItemByGuid(SelectedReportItem.Id);
                            SelectedReportItem.IsSelected = false;
                            if (dialogService.SaveFileDialog(item.Name.Replace(" ", "_") + ".csv", "CSV files|*.csv") == true)
                            {
                                ExportAsCSVFile(item, dialogService.FilePath);
                            }
                            item.IsSelected = true;
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ExportAsCSVReportCommand");
                    }
                }, (obj) => SelectedReportItem != null ? !SelectedReportItem.ReportIsBusy : false);
            }
        }

        private WorkCommand exportAsXMLReportCommand;
        public WorkCommand ExportAsXMLReportCommand
        {
            get
            {
                return exportAsXMLReportCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (SelectedReportItem.ReportBuildIsDone)
                        {
                            if (dialogService.SaveFileDialog(SelectedReportItem.Name.Replace(" ", "_") + ".xml", "XML files|*.xml") == true)
                            {
                                ExportAsXMLFile(SelectedReportItem, dialogService.FilePath);
                            }
                        }
                        else
                        {
                            var item = FindReportItemByGuid(SelectedReportItem.Id);
                            SelectedReportItem.IsSelected = false;
                            if (dialogService.SaveFileDialog(item.Name.Replace(" ", "_") + ".xml", "XML files|*.xml") == true)
                            {
                                ExportAsXMLFile(item, dialogService.FilePath);
                            }
                            item.IsSelected = true;
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ExportAsXMLReportCommand");
                    }
                }, (obj) => SelectedReportItem != null ? !SelectedReportItem.ReportIsBusy : false);
            }
        }

        private WorkCommand exportAsJSONReportCommand;
        public WorkCommand ExportAsJSONReportCommand
        {
            get
            {
                return exportAsJSONReportCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (SelectedReportItem.ReportBuildIsDone)
                        {
                            if (dialogService.SaveFileDialog(SelectedReportItem.Name.Replace(" ", "_") + ".json", "JSON files|*.json") == true)
                            {
                                ExportAsJSONFile(SelectedReportItem, dialogService.FilePath);
                            }
                        }
                        else
                        {
                            var item = FindReportItemByGuid(SelectedReportItem.Id);
                            SelectedReportItem.IsSelected = false;
                            if (dialogService.SaveFileDialog(item.Name.Replace(" ", "_") + ".json", "JSON files|*.json") == true)
                            {
                                ExportAsJSONFile(item, dialogService.FilePath);
                            }
                            item.IsSelected = true;
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ExportAsJSONReportCommand");
                    }
                }, (obj) => SelectedReportItem != null);
            }
        }

        private WorkCommand exportAsExcelReportCommand;
        public WorkCommand ExportAsExcelReportCommand
        {
            get
            {
                return exportAsExcelReportCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (SelectedReportItem.ReportBuildIsDone)
                        {
                            if (dialogService.SaveFileDialog(SelectedReportItem.Name.Replace(" ", "_") + ".xlsx", "Excel files|*.xlsx") == true)
                            {
                                ExportAsExcelFile(SelectedReportItem, dialogService.FilePath);
                            }
                        }
                        else
                        {
                            var item = FindReportItemByGuid(SelectedReportItem.Id);
                            SelectedReportItem.IsSelected = false;
                            if (dialogService.SaveFileDialog(item.Name.Replace(" ", "_") + ".xlsx", "Excel files|*.xlsx") == true)
                            {
                                ExportAsExcelFile(item, dialogService.FilePath);
                            }
                            item.IsSelected = true;
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ExportAsExcelReportCommand");
                    }
                }, (obj) => SelectedReportItem != null ? !SelectedReportItem.ReportIsBusy : false);
            }
        }
        #endregion

        #region Report Container Commands
        private WorkCommand newReportContainerCommand;
        public WorkCommand NewReportContainerCommand
        {
            get
            {
                return newReportContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (treeItemDialogService.ReportContainerSettingsDialog(settings) == true)
                        {
                            AddReportContainer(treeItemDialogService.Name, treeItemDialogService.Description, SelectedReportItem, treeItemDialogService.UseCustomIcon, treeItemDialogService.CustomIcon);
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "NewReportContainerCommand");
                    }
                }, (obj) => SelectedReportItem != null);
            }
        }

        private WorkCommand editReportContainerCommand;
        public WorkCommand EditReportContainerCommand
        {
            get
            {
                return editReportContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        if (treeItemDialogService.ReportContainerSettingsDialog(settings, SelectedReportItem.Name, SelectedReportItem.Description, SelectedReportItem.UseCustomIcon, SelectedReportItem.CustomIcon) == true)
                        {
                            SelectedReportItem.Name = treeItemDialogService.Name;
                            SelectedReportItem.Description = treeItemDialogService.Description;
                            SelectedReportItem.UseCustomIcon = treeItemDialogService.UseCustomIcon;
                            SelectedReportItem.CustomIcon = treeItemDialogService.CustomIcon;
                            AddLogItem(DateTime.Now, "Container \"" + SelectedReportItem.Name + "\" settings", "Saved", LogItemTypeEnum.Information, "EditReportContainerCommand");
                        }
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "EditReportContainerCommand");
                    }
                }, (obj) => SelectedReportItem != null);
            }
        }

        private WorkCommand deleteReportContainerCommand;
        public WorkCommand DeleteReportContainerCommand
        {
            get
            {
                return deleteReportContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        FindReportItemAndDelete(SelectedReportItem.Id);
                        AddLogItem(DateTime.Now, "Container \"" + SelectedReportItem.Name + "\"", "Deleted", LogItemTypeEnum.Information, "DeleteReportContainerCommand");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "DeleteReportContainerCommand");
                    }
                }, (obj) => SelectedReportItem != null);
            }
        }

        private WorkCommand clearReportContainerCommand;
        public WorkCommand ClearReportContainerCommand
        {
            get
            {
                return clearReportContainerCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        SelectedReportItem.Children.Clear();
                        AddLogItem(DateTime.Now, "Container \"" + SelectedReportItem.Name + "\" childrens", "Cleared", LogItemTypeEnum.Information, "ClearReportContainerCommand");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "ClearReportContainerCommand");
                    }
                }, (obj) => SelectedReportItem != null);
            }
        }
        #endregion

        internal void InitReports()
        {
            foreach (var node in ReportRoot.Children)
            {
                InitReport(node);
            }
        }

        internal void InitReport(ReportItem Item)
        {
            if (Item.Type == ReportItemTypeEnum.Report)
            {
                Item.InitialiseDataGrid();
            }
            if (Item.Children.Count != 0)
            {
                foreach (var node in Item.Children)
                {
                    InitReport(node);
                }
            }
        }

        internal void BuildDataGrid(ReportItem Item)
        {
            if (Item.Type == ReportItemTypeEnum.Report)
            {
                Item.dataTable.Rows.Clear();
                if (Item.Settings != null)
                {
                    if (Item.Settings.Columns != null)
                    {
                        if (Item.Settings.Columns.Count != 0)
                        {
                            if (Item.Settings.ComponentGroupingItem == ComponentGroupingTypeEnum.ByContainer)
                            {
                                FetchDataByContainer(Item);
                            }
                            else if (Item.Settings.ComponentGroupingItem == ComponentGroupingTypeEnum.ByInstance)
                            {
                                FetchDataByInstance(Item);
                            }
                        }
                    }
                }
            }
        }

        internal void FetchDataByContainer(ReportItem reportItem)
        {
            if (reportItem.Settings.ReportSourceContainerId != new Guid())
            {
                var SourceTreeItem = FindTreeItemByGuid(reportItem.Settings.ReportSourceContainerId);
                var Items = new List<TreeItem>();
                FindAllChilderenByType(SourceTreeItem, ItemTypeEnum.ChildComputer, Items);
                switch (reportItem.Settings.ComponentItem)
                {
                    case ComponentTypeEnum.None:
                        break;
                    case ComponentTypeEnum.BIOSInformation:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                FillDataFromSingleValue(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].BIOSInformation);
                            }
                        }
                        break;
                    case ComponentTypeEnum.CPUInformation:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                FillDataFromListOfValues(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].CPUs);
                            }
                        }
                        break;
                    case ComponentTypeEnum.MemoryDevice:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                FillDataFromListOfValues(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].MemoryDevices);
                            }
                        }
                        break;
                    case ComponentTypeEnum.MotherBoardInformation:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                FillDataFromSingleValue(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].MotherBoardInformation);
                            }
                        }
                        break;
                    case ComponentTypeEnum.NetworkAdapter:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                FillDataFromListOfValues(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].NetworkAdapters);
                            }
                        }
                        break;
                    case ComponentTypeEnum.VideoController:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                FillDataFromListOfValues(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].VideoControllers);
                            }
                        }
                        break;
                    case ComponentTypeEnum.PhysicalDrive:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                FillDataFromListOfValues(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].PhysicalDrives);
                            }
                        }
                        break;
                    case ComponentTypeEnum.LogicalDrive:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                FillDataFromListOfValues(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].LogicalDrives);
                            }
                        }
                        break;
                    case ComponentTypeEnum.Monitor:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                FillDataFromListOfValues(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].Monitors);
                            }
                        }
                        break;
                    case ComponentTypeEnum.Printer:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                FillDataFromListOfValues(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].Printers);
                            }
                        }
                        break;
                    case ComponentTypeEnum.SystemInformation:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                FillDataFromSingleValue(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].SystemInformation);
                            }
                        }
                        break;
                    case ComponentTypeEnum.WindowsInformation:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                if (node.HardwareScans[0].ScanType == ScanTypeEnum.WindowsOS)
                                    FillDataFromSingleValue(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].WindowsInformation);
                            }
                        }
                        break;
                    case ComponentTypeEnum.WindowsLocalUser:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                if (node.HardwareScans[0].ScanType == ScanTypeEnum.WindowsOS)
                                    FillDataFromListOfValues(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].WindowsLocalUsers);
                            }
                        }
                        break;
                    case ComponentTypeEnum.WindowsLocalGroup:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                if (node.HardwareScans[0].ScanType == ScanTypeEnum.WindowsOS)
                                    FillDataFromListOfValues(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].WindowsLocalGroups);
                            }
                        }
                        break;
                    case ComponentTypeEnum.SoftwareLicensingProduct:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                if (node.HardwareScans[0].ScanType == ScanTypeEnum.WindowsOS)
                                    FillDataFromListOfValues(reportItem, node, (reportItem.DataTable.Rows.Count + 1).ToString(), node.HardwareScans[0].SoftwareLicensingProducts);
                            }
                        }
                        break;
                    case ComponentTypeEnum.ActiveDirectoryComputerInfo:
                        break;
                    case ComponentTypeEnum.ActiveDirectoryGroupInfo:
                        break;
                    case ComponentTypeEnum.ActiveDirectoryUserInfo:
                        break;
                        //case ComponentTypeEnum.LinuxInformation:
                        //    break;
                        //case ComponentTypeEnum.LinuxLocalUser:
                        //    break;
                        //case ComponentTypeEnum.LinuxLocalGroup:
                        //    break;
                }
            }
        }

        internal void FetchDataByInstance(ReportItem reportItem)
        {
            if (reportItem.Settings.ReportSourceContainerId != new Guid())
            {
                var SourceTreeItem = FindTreeItemByGuid(reportItem.Settings.ReportSourceContainerId);
                var Items = new List<TreeItem>();
                FindAllChilderenByType(SourceTreeItem, ItemTypeEnum.ChildComputer, Items);


                switch (reportItem.Settings.ComponentItem)
                {
                    case ComponentTypeEnum.BIOSInformation:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                CountAndFillDataFromSingleValue(reportItem, node.HardwareScans[0].BIOSInformation);
                            }
                            break;
                        }

                    case ComponentTypeEnum.SystemInformation:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                CountAndFillDataFromSingleValue(reportItem, node.HardwareScans[0].SystemInformation);
                            }
                            break;
                        }

                    case ComponentTypeEnum.MotherBoardInformation:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                CountAndFillDataFromSingleValue(reportItem, node.HardwareScans[0].MotherBoardInformation);
                            }

                            break;
                        }

                    case ComponentTypeEnum.WindowsInformation:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                if (node.HardwareScans[0].ScanType == ScanTypeEnum.WindowsOS)
                                    CountAndFillDataFromSingleValue(reportItem, node.HardwareScans[0].WindowsInformation);
                            }

                            break;
                        }

                    case ComponentTypeEnum.CPUInformation:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                CountAndFillDataFromListOfValues(reportItem, node.HardwareScans[0].CPUs);
                            }

                            break;
                        }

                    case ComponentTypeEnum.MemoryDevice:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                CountAndFillDataFromListOfValues(reportItem, node.HardwareScans[0].MemoryDevices);
                            }

                            break;
                        }

                    case ComponentTypeEnum.Monitor:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                CountAndFillDataFromListOfValues(reportItem, node.HardwareScans[0].Monitors);
                            }

                            break;
                        }

                    case ComponentTypeEnum.NetworkAdapter:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                CountAndFillDataFromListOfValues(reportItem, node.HardwareScans[0].NetworkAdapters);
                            }

                            break;
                        }

                    case ComponentTypeEnum.PhysicalDrive:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                CountAndFillDataFromListOfValues(reportItem, node.HardwareScans[0].PhysicalDrives);
                            }

                            break;
                        }

                    case ComponentTypeEnum.SoftwareLicensingProduct:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                if (node.HardwareScans[0].ScanType == ScanTypeEnum.WindowsOS)
                                    CountAndFillDataFromListOfValues(reportItem, node.HardwareScans[0].SoftwareLicensingProducts);
                            }

                            break;
                        }

                    case ComponentTypeEnum.VideoController:
                        {
                            foreach (var node in Items.Where(x => x.ScanAvailable))
                            {
                                CountAndFillDataFromListOfValues(reportItem, node.HardwareScans[0].VideoControllers);
                            }

                            break;
                        }
                }
            }
        }

        private static void FillDataFromListOfValues(ReportItem reportItem, TreeItem node, string index, IList list)
        {
            if (reportItem.Settings.EachValueIsASepareteRow)
            {
                foreach (var inode in list)
                {
                    DataRow newRow = reportItem.DataTable.NewRow();
                    newRow["#"] = index;
                    newRow["Computer Name"] = node.Name;
                    newRow["Computer Description"] = node.Description;
                    newRow["Computer FQDN"] = node.FQDN;
                    newRow["Computer IP Address"] = node.Address;
                    foreach (var column in reportItem.Settings.Columns.Where(x => x.IsSelected))
                    {
                        var value = string.Empty;
                        try
                        {
                            value = inode.GetType().GetProperty(column.PropertyKey).GetValue(inode, null) + string.Empty;
                        }
                        catch (Exception) { }
                        newRow[column.PropertyDescription] = value;
                    }
                    reportItem.DataTable.Rows.Add(newRow);
                }
            }
            else
            {
                DataRow newRow = reportItem.DataTable.NewRow();
                newRow["#"] = index;
                newRow["Computer Name"] = node.Name;
                newRow["Computer Description"] = node.Description;
                newRow["Computer FQDN"] = node.FQDN;
                newRow["Computer IP Address"] = node.Address;
                foreach (var column in reportItem.Settings.Columns.Where(x => x.IsSelected))
                {
                    try
                    {
                        var value = string.Empty;
                        foreach (var inode in list)
                        {
                            value += inode.GetType().GetProperty(column.PropertyKey).GetValue(inode, null) + "\n";
                        }
                        newRow[column.PropertyDescription] = value.TrimEnd('\n').TrimEnd('\r');
                    }
                    catch (Exception) { }
                }
                reportItem.DataTable.Rows.Add(newRow);
            }
        }

        private static void FillDataFromSingleValue(ReportItem reportItem, TreeItem node, string index, object value)
        {
            DataRow newRow = reportItem.DataTable.NewRow();
            newRow["#"] = index;
            newRow["Computer Name"] = node.Name;
            newRow["Computer Description"] = node.Description;
            newRow["Computer FQDN"] = node.FQDN;
            newRow["Computer IP Address"] = node.Address;

            foreach (var column in reportItem.Settings.Columns.Where(x => x.IsSelected))
            {
                newRow[column.PropertyDescription] = value.GetType().GetProperty(column.PropertyKey).GetValue(value, null).ToString();
            }
            reportItem.DataTable.Rows.Add(newRow);

        }

        private static void CountAndFillDataFromSingleValue(ReportItem reportItem, object value)
        {
            DataRow newRow = reportItem.DataTable.NewRow();
            foreach (var column in reportItem.Settings.Columns.Where(x => x.IsSelected))
            {
                newRow[column.PropertyDescription] = value.GetType().GetProperty(column.PropertyKey).GetValue(value, null).ToString();
            }
            SearchRowAndInsertNew(reportItem, newRow);
        }

        private static void CountAndFillDataFromListOfValues(ReportItem reportItem, IList list)
        {
            foreach (var node in list)
            {
                DataRow newRow = reportItem.DataTable.NewRow();
                foreach (var column in reportItem.Settings.Columns.Where(x => x.IsSelected))
                {
                    newRow[column.PropertyDescription] = node.GetType().GetProperty(column.PropertyKey).GetValue(node, null).ToString();
                }
                SearchRowAndInsertNew(reportItem, newRow);
            }
        }

        private static void SearchRowAndInsertNew(ReportItem reportItem, DataRow newRow)
        {
            var isInTable = false;
            var isInTableRowIndex = -1;

            foreach (DataRow row in reportItem.DataTable.Rows)
            {
                var isRowEqualToExist = 0;
                for (int i = 1; i <= reportItem.DataTable.Columns.Count - 2; i++)
                {
                    if (Object.Equals(row[i], newRow[i]))
                    {
                        isRowEqualToExist += 1;
                    }
                }
                if (isRowEqualToExist == reportItem.DataTable.Columns.Count - 2)
                {
                    isInTable = true;
                    isInTableRowIndex = reportItem.DataTable.Rows.IndexOf(row);
                }
            }
            if (isInTable)
            {
                var Quantity = Convert.ToInt32(reportItem.DataTable.Rows[isInTableRowIndex]["Quantity"]);
                reportItem.DataTable.Rows[isInTableRowIndex]["Quantity"] = Quantity + 1;
            }
            else
            {
                newRow["#"] = reportItem.DataTable.Rows.Count + 1;
                newRow["Quantity"] = 1;
                reportItem.DataTable.Rows.Add(newRow);
            }
        }

        private void ExportAsCSVFile(ReportItem reportItem, string fileName)
        {
            if (!reportItem.ReportBuildIsDone)
            {
                reportItem.ReportIsBusy = true;
                reportItem.ReportBuildIsDone = false;
                BuildDataGrid(reportItem);
                reportItem.ReportIsBusy = false;
                reportItem.ReportBuildIsDone = true;
            }

            var reportData = new StringBuilder();
            for (int column = 0; column < reportItem.DataTable.Columns.Count; column++)
            {
                if (column == reportItem.DataTable.Columns.Count - 1)
                    reportData.Append(reportItem.DataTable.Columns[column].ColumnName.ToString().Replace(",", ";"));
                else
                    reportData.Append(reportItem.DataTable.Columns[column].ColumnName.ToString().Replace(",", ";") + ',');
            }
            reportData.Append(Environment.NewLine);
            for (int row = 0; row < reportItem.DataTable.Rows.Count; row++)
            {
                for (int column = 0; column < reportItem.DataTable.Columns.Count; column++)
                {
                    if (column == reportItem.DataTable.Columns.Count - 1)
                        reportData.Append(reportItem.DataTable.Rows[row][column].ToString().Replace(",", ";"));
                    else
                        reportData.Append(reportItem.DataTable.Rows[row][column].ToString().Replace(",", ";") + ',');
                }

                if (row != reportItem.DataTable.Rows.Count - 1)
                    reportData.Append(Environment.NewLine);
            }
            using (StreamWriter objWriter = new StreamWriter(fileName))
            {
                objWriter.WriteLine(reportData);
            }
        }

        private void ExportAsXMLFile(ReportItem reportItem, string fileName)
        {
            if (!reportItem.ReportBuildIsDone)
            {
                reportItem.ReportIsBusy = true;
                reportItem.ReportBuildIsDone = false;
                BuildDataGrid(reportItem);
                reportItem.ReportIsBusy = false;
                reportItem.ReportBuildIsDone = true;
            }

            using (DataTable exportTable = reportItem.DataTable.Copy())
            {
                foreach (DataColumn node in exportTable.Columns)
                {
                    node.ColumnName = node.ColumnName.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace(" ", "_").Replace("#", "Index");
                }
                exportTable.WriteXml(fileName);
            }
        }

        private void ExportAsJSONFile(ReportItem reportItem, string fileName)
        {
            if (!reportItem.ReportBuildIsDone)
            {
                reportItem.ReportIsBusy = true;
                reportItem.ReportBuildIsDone = false;
                BuildDataGrid(reportItem);
                reportItem.ReportIsBusy = false;
                reportItem.ReportBuildIsDone = true;
            }
            serializationService.ExportAsJson(fileName,
                reportItem.dataTable.AsEnumerable().
                Select(r => r.Table.Columns.Cast<DataColumn>().
                Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal]))
                .ToDictionary(z => z.Key, z => z.Value)).ToList());
        }

        private void ExportAsExcelFile(ReportItem reportItem, string fileName)
        {
            if (!reportItem.ReportBuildIsDone)
            {
                reportItem.ReportIsBusy = true;
                reportItem.ReportBuildIsDone = false;
                BuildDataGrid(reportItem);
                reportItem.ReportIsBusy = false;
                reportItem.ReportBuildIsDone = true;
            }
            using (var workbook = SpreadsheetDocument.Create(fileName, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new Sheets();
                var sheetPart = workbook.WorkbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                sheetPart.Worksheet = new Worksheet(sheetData);

                Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                uint sheetId = 1;
                if (sheets.Elements<Sheet>().Count() > 0)
                {
                    sheetId = sheets.Elements<Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                }

                Sheet sheet = new Sheet() { Id = relationshipId, SheetId = sheetId, Name = reportItem.DataTable.TableName };
                sheets.Append(sheet);

                Row headerRow = new Row();

                var columns = new List<string>();
                foreach (DataColumn column in reportItem.DataTable.Columns)
                {
                    columns.Add(column.ColumnName);
                    Cell cell = new Cell();
                    cell.DataType = CellValues.String;
                    cell.CellValue = new CellValue(column.ColumnName);
                    headerRow.AppendChild(cell);
                }

                sheetData.AppendChild(headerRow);

                foreach (DataRow dsrow in reportItem.DataTable.Rows)
                {
                    Row newRow = new Row();
                    foreach (var col in columns)
                    {
                        Cell cell = new Cell();
                        cell.DataType = CellValues.String;
                        cell.CellValue = new CellValue(dsrow[col].ToString()); 
                        newRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(newRow);
                }
            }
        }

    }
}

