using Gizmo.HardwareAudit.Classes.Helpers;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditWPF;
using System;
using System.Collections.ObjectModel;

namespace Gizmo.HardwareAudit.ViewModels
{
    public class ReportItemSettingsViewModel : BaseViewModel
    {

        #region Private Properties
        private string reportName;
        private string reportDescription;
        private bool useCustomIcon = false;
        private GizmiComputerHardwareIconsEnum customIcon = GizmiComputerHardwareIconsEnum.None;
        private ContainerItem root;
        private ObservableCollection<ClassProperties> columns;
        private EnumProperies selectedReportTypeItem;
        private EnumProperies selectedComponentItem;
        private ObservableCollection<EnumProperies> componentTypeItems;
        private ObservableCollection<EnumProperies> reportTypeItems;
        private bool eachValueIsASepareteRow = false;
        #endregion

        #region Public Properties
        public string ReportName
        {
            get => reportName;
            set
            {
                if (reportName == value) return;
                reportName = value;
                OnPropertyChanged();
            }
        }
        public string ReportDescription
        {
            get => reportDescription;
            set
            {
                if (reportDescription == value) return;
                reportDescription = value;
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
        public ContainerItem Root
        {
            get => root;
            set
            {
                if (root == value) return;
                root = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ClassProperties> Columns
        {
            get => columns;
            set
            {
                if (columns == value) return;
                columns = value;
                OnPropertyChanged();
            }
        }
        public EnumProperies SelectedReportTypeItem
        {
            get => selectedReportTypeItem;
            set
            {
                if (selectedReportTypeItem == value) return;
                selectedReportTypeItem = value;
                ComponentTypeItems = selectedReportTypeItem != null ? AttributeWorkForEnums.Enumerate(typeof(ComponentTypeEnum), ComponentTypeEnum.None, selectedReportTypeItem.Value) : new ObservableCollection<EnumProperies>();
                OnPropertyChanged();
            }
        }
        public EnumProperies SelectedComponentItem
        {
            get => selectedComponentItem;
            set
            {
                if (selectedComponentItem == value) return;
                selectedComponentItem = value;
                Columns = selectedComponentItem != null ? AttributeWorkForClass.Enumerate((ComponentTypeEnum)selectedComponentItem.Value) : new ObservableCollection<ClassProperties>();
                OnPropertyChanged();
            }
        }
        public ObservableCollection<EnumProperies> ComponentTypeItems
        {
            get => componentTypeItems;
            set
            {
                if (componentTypeItems == value) return;
                componentTypeItems = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<EnumProperies> ReportTypeItems
        {
            get => reportTypeItems;
            set
            {
                if (reportTypeItems == value) return;
                reportTypeItems = value;
                OnPropertyChanged();
            }
        }
        public bool EachValueIsASepareteRow
        {
            get => eachValueIsASepareteRow;
            set
            {
                if (eachValueIsASepareteRow == value) return;
                eachValueIsASepareteRow = value;
                OnPropertyChanged();
            }
        }

        public ContainerItem SelectedContainer => Root.SelectedItem;

        #endregion


        public ReportItemSettingsViewModel(string name, string description, bool useCustomIcon, GizmiComputerHardwareIconsEnum customIcon, TreeItem treeItem, ReportSettings reportSettings)
        {
            ReportDescription = description;
            ReportName = name;
            UseCustomIcon = useCustomIcon;
            CustomIcon = customIcon;
            EachValueIsASepareteRow = reportSettings.EachValueIsASepareteRow;
            Root = new ContainerItem();

            if (reportSettings.ReportSourceContainerId != new Guid())
                TreeItemHelper.GetAllContainersFromTreeItemAndSelectById(treeItem, Root.Children, reportSettings.ReportSourceContainerId);
            else
                TreeItemHelper.GetAllContainersFromTreeItem(treeItem, Root.Children);

            Root.Initialise();
            if (Root != null)
            {
                Root.PropertyChanged += SelectedItem_PropertyChanged;
            }
            ReportTypeItems = AttributeWorkForEnums.Enumerate(typeof(ReportTypeEnum), ReportTypeEnum.None);

            foreach (var node in ReportTypeItems)
            {
                if (Object.Equals(node.Value, reportSettings.ReportType))
                    SelectedReportTypeItem = node;
            }

            foreach (var node in ComponentTypeItems)
            {
                if (Object.Equals(node.Value, reportSettings.ComponentItem))
                    SelectedComponentItem = node;
            }


            if (Columns != null)
            {
                if (reportSettings.Columns != null)
                {
                    foreach (var node in Columns)
                    {
                        foreach (var previous_node in reportSettings.Columns)
                        {
                            if (node.SourceClassTypeName == previous_node.SourceClassTypeName && node.PropertyKey == previous_node.PropertyKey && node.PropertyBindingKey == previous_node.PropertyBindingKey)
                            {
                                node.IsSelected = previous_node.IsSelected;
                            }
                        }
                    }
                }
            }


        }

        public ReportItemSettingsViewModel(TreeItem treeItem)
        {
            ReportDescription = string.Empty;
            ReportName = string.Empty;
            Root = new ContainerItem();
            TreeItemHelper.GetAllContainersFromTreeItem(treeItem, Root.Children);

            Root.Initialise();
            if (Root != null)
            {
                Root.PropertyChanged += SelectedItem_PropertyChanged;
            }
            ReportTypeItems = AttributeWorkForEnums.Enumerate(typeof(ReportTypeEnum), ReportTypeEnum.None);

        }

        private void SelectedItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedItem"))
            {
                OnNamedPropertyChanged("SelectedContainer");
            }
        }
    }
}
