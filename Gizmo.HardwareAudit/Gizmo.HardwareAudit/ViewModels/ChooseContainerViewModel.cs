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
        private Container root;
        public Container Root
        {
            get => root;
            set
            {
                if (root == value) return;
                root = value;
                OnPropertyChanged();
            }
        }
        public Container SelectedContainer => Root.SelectedItem;

        public ChooseContainerViewModel(TreeItem treeItem)
        {
            Root = new Container();
            GetContainersFromTreeItem(treeItem, Root.Children, treeItem.SelectedItem.ParentId);
            Root.Initialise();
            if (Root != null)
            {
                Root.PropertyChanged += SelectedItem_PropertyChanged;
            }
        }
        private void GetContainersFromTreeItem(TreeItem treeItem, ObservableCollection<Container> items, Guid selectedItemParentId)
        {
            foreach (var node in treeItem.Children)
            {
                if (node.Type != ItemTypeEnum.ChildComputer && node.Type != ItemTypeEnum.ChildDevice)
                {
                    var newContainer = new Container()
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

        private void SelectedItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("SelectedItem"))
            {
                OnNamedPropertyChanged("SelectedContainer");
            }
        }

    }
}
