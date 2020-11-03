using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Models;
using System;
using System.Collections.ObjectModel;

namespace Gizmo.HardwareAudit.Classes.Helpers
{
    public class TreeItemHelper
    {
        public static void GetContainersFromTreeItem(TreeItem treeItem, ObservableCollection<ContainerItem> items, Guid selectedItemParentId)
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

        public static void GetAllContainersFromTreeItem(TreeItem treeItem, ObservableCollection<ContainerItem> items)
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
                        GetAllContainersFromTreeItem(node, newContainer.Children);
                    }
                    items.Add(newContainer);
                }
            }
        }

        public static void GetAllContainersFromTreeItemAndSelectById(TreeItem treeItem, ObservableCollection<ContainerItem> items, Guid selectedId)
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
                        IsExpanded = true,
                        Type = node.Type,
                        ParentType = node.ParentType,
                        IsTrueContainer = node.Type != ItemTypeEnum.None,
                        IsSelected = node.Id == selectedId
                    };
                    if (node.Children.Count != 0)
                    {
                        GetAllContainersFromTreeItemAndSelectById(node, newContainer.Children, selectedId);
                    }
                    items.Add(newContainer);
                }
            }
        }
    }
}
