using Gizmo.HardwareAudit.Classes;
using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using System;

namespace Gizmo.HardwareAudit.ViewModels
{
    //NOTE: ReportViewModel is a part of AppViewModel Class
    public partial class AppViewModel : BaseViewModel
    {
        private ReportItem reportRoot;
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


        private WorkCommand newReportCommand;
        public WorkCommand NewReportCommand
        {
            get
            {
                return newReportCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "NewReportCommand");
                    }
                }, (obj) => SelectedReportItem != null);
            }
        }

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
    }
}
