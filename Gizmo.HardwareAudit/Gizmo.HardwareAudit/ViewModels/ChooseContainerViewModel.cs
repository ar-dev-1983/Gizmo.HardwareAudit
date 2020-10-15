using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Gizmo.HardwareAudit.ViewModels
{
    public class ChooseContainerViewModel : BaseViewModel
    {
        private ContainerItem root;
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
        public ContainerItem SelectedContainer => Root.SelectedItem;

        public ChooseContainerViewModel(TreeItem treeItem, bool useSelectedParentId)
        {
            Root = new ContainerItem();

            if (useSelectedParentId)
                GetContainersFromTreeItem(treeItem, Root.Children, treeItem.SelectedItem.ParentId);
            else
                GetContainersFromTreeItem(treeItem, Root.Children);

            Root.Initialise();
            if (Root != null)
            {
                Root.PropertyChanged += SelectedItem_PropertyChanged;
            }
        }

        private void GetContainersFromTreeItem(TreeItem treeItem, ObservableCollection<ContainerItem> items, Guid selectedItemParentId)
        {
            foreach (var node in treeItem.Children)
            {
                if (node.Type != ItemTypeEnum.ChildComputer && node.Type != ItemTypeEnum.ChildDevice)
                {
                    var newContainer = new ContainerItem()
                    {
                        Id = node.Id,
                        Name = node.Name,
                        ParentId = node.ParentId,
                        IsExpanded = node.IsExpanded,
                        Type = node.Type,
                        ParentType = node.ParentType,
                        IsTrueContainer = node.Type == ItemTypeEnum.ChildContainer
                        || node.Type == ItemTypeEnum.Workgroup,
                        IsSelected = node.Id == selectedItemParentId
                    };
                    if (node.Children.Count != 0)
                    {
                        GetContainersFromTreeItem(node, newContainer.Children, selectedItemParentId);
                    }
                    items.Add(newContainer);
                }
            }
        }

        private void GetContainersFromTreeItem(TreeItem treeItem, ObservableCollection<ContainerItem> items)
        {
            foreach (var node in treeItem.Children)
            {
                if (node.Type != ItemTypeEnum.ChildComputer && node.Type != ItemTypeEnum.ChildDevice)
                {
                    var newContainer = new ContainerItem()
                    {
                        Id = node.Id,
                        Name = node.Name,
                        ParentId = node.ParentId,
                        IsExpanded = node.IsExpanded,
                        Type = node.Type,
                        ParentType = node.ParentType,
                        IsTrueContainer = node.Type != ItemTypeEnum.None
                    };
                    if (node.Children.Count != 0)
                    {
                        GetContainersFromTreeItem(node, newContainer.Children);
                    }
                    items.Add(newContainer);
                }
            }
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
