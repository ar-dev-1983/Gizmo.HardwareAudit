﻿using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Gizmo.HardwareAudit.Models
{
    public class ContainerItem : BaseViewModel, IDisposable
    {
        private readonly PropertyChangedEventHandler propertyChangedHandler;
        private readonly NotifyCollectionChangedEventHandler collectionChangedhandler;

        #region Private Properties
        private Guid id = Guid.NewGuid();
        private Guid parentId = Guid.NewGuid();
        private ItemTypeEnum type = ItemTypeEnum.None;
        private ItemTypeEnum parentType = ItemTypeEnum.None;
        private string name = string.Empty;
        private ObservableCollection<ContainerItem> children = new ObservableCollection<ContainerItem>();
        private bool isSelected = false;
        private bool isExpanded = false;
        private bool isTrueContainer = false;
        #endregion

        #region Public Properties
        public Guid Id
        {
            get => id;
            set
            {
                if (id == value) return;
                id = value;
                OnPropertyChanged();
            }
        }

        public Guid ParentId
        {
            get => parentId;
            set
            {
                if (parentId == value) return;
                parentId = value;
                OnPropertyChanged();
            }
        }

        public ItemTypeEnum Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                OnPropertyChanged();
            }
        }

        public ItemTypeEnum ParentType
        {
            get => parentType;
            set
            {
                if (parentType == value) return;
                parentType = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;
                name = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ContainerItem> Children
        {
            get => children;
            set
            {
                if (children == value) return;
                children = value;
                OnPropertyChanged();
            }
        }
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected == value) return;
                isSelected = value;
                OnPropertyChanged();
            }
        }
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (isExpanded == value) return;
                isExpanded = value;
                OnPropertyChanged();
            }
        }
        public bool IsTrueContainer
        {
            get => isTrueContainer;
            set
            {
                if (isTrueContainer == value) return;
                isTrueContainer = value;
                OnPropertyChanged();
            }
        }
        public ContainerItem SelectedItem
        {
            get
            {
                return Traverse(this, node => node.Children).FirstOrDefault(m => m.IsSelected);
            }
        }
        #endregion

        public ContainerItem()
        {
            propertyChangedHandler = new PropertyChangedEventHandler(Item_PropertyChanged);
            collectionChangedhandler = new NotifyCollectionChangedEventHandler(Items_CollectionChanged);
            Children.CollectionChanged += collectionChangedhandler;
        }

        public void Initialise()
        {
            SubscribePropertyChanged(this);
        }

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

        private void SubscribePropertyChanged(ContainerItem item)
        {
            item.PropertyChanged += propertyChangedHandler;
            item.Children.CollectionChanged += collectionChangedhandler;
            foreach (var subitem in item.Children)
            {
                SubscribePropertyChanged(subitem);
            }
        }

        private void UnsubscribePropertyChanged(ContainerItem item)
        {
            foreach (var subitem in item.Children)
            {
                UnsubscribePropertyChanged(subitem);
            }
            item.Children.CollectionChanged -= collectionChangedhandler;
            item.PropertyChanged -= propertyChangedHandler;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (ContainerItem item in e.OldItems)
                {
                    UnsubscribePropertyChanged(item);
                }
            }

            if (e.NewItems != null)
            {
                foreach (ContainerItem item in e.NewItems)
                {
                    SubscribePropertyChanged(item);
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSelected"))
            {
                OnNamedPropertyChanged("SelectedItem");
            }
        }

        public void Dispose()
        {
            UnsubscribePropertyChanged(this);
            Children.CollectionChanged -= collectionChangedhandler;
        }
    }
}
