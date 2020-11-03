using Gizmo.HardwareAudit.Classes.Helpers;
using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAuditClasses.Enums;
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
        private GizmoIconEnum customIcon = GizmoIconEnum.None;
        private ContainerItem root;
        private ObservableCollection<ClassProperties> columns;
        private EnumProperies selectedComponentGroupingItem;
        private EnumProperies selectedComponentItem;
        private ObservableCollection<EnumProperies> componentTypeItems;
        private ObservableCollection<EnumProperies> componentGroupingItems;
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

        public GizmoIconEnum CustomIcon
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
        public EnumProperies SelectedComponentGroupingItem
        {
            get => selectedComponentGroupingItem;
            set
            {
                if (selectedComponentGroupingItem == value) return;
                selectedComponentGroupingItem = value;
                ComponentTypeItems = selectedComponentGroupingItem != null ? AttributeWorkForEnums.Enumerate(typeof(ComponentTypeEnum), ComponentTypeEnum.None, selectedComponentGroupingItem.Value) : new ObservableCollection<EnumProperies>();
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
        public ObservableCollection<EnumProperies> ComponentGroupingItems
        {
            get => componentGroupingItems;
            set
            {
                if (componentGroupingItems == value) return;
                componentGroupingItems = value;
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


        public ReportItemSettingsViewModel(string name, string description, bool useCustomIcon, GizmoIconEnum customIcon, TreeItem treeItem, ReportSettings reportSettings)
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
            ComponentGroupingItems = AttributeWorkForEnums.Enumerate(typeof(ComponentGroupingTypeEnum), ComponentGroupingTypeEnum.None);

            foreach (var node in ComponentGroupingItems)
            {
                if (Object.Equals(node.Value, reportSettings.ComponentGroupingItem))
                    SelectedComponentGroupingItem = node;
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
            ComponentGroupingItems = AttributeWorkForEnums.Enumerate(typeof(ComponentGroupingTypeEnum), ComponentGroupingTypeEnum.None);

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
