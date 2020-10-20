using Gizmo.HardwareAudit.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Gizmo.HardwareAudit.Models
{
    public class ReportSettings : BaseViewModel, IDisposable
    {
        private readonly PropertyChangedEventHandler propertyChangedHandler;
        private readonly NotifyCollectionChangedEventHandler collectionChangedhandler;

        private Guid treeItemId = new Guid();
        public Guid TreeItemId
        {
            get => treeItemId;
            set
            {
                if (treeItemId == value) return;
                treeItemId = value;
                OnPropertyChanged();
            }
        }

        public ReportSettings()
        {
            propertyChangedHandler = new PropertyChangedEventHandler(Item_PropertyChanged);
            collectionChangedhandler = new NotifyCollectionChangedEventHandler(Items_CollectionChanged);
        }

        public void Initialise()
        {
            SubscribePropertyChanged(this);
        }

        private void SubscribePropertyChanged(ReportSettings item)
        {
            item.PropertyChanged += propertyChangedHandler;
        }

        private void UnsubscribePropertyChanged(ReportSettings item)
        {
            item.PropertyChanged -= propertyChangedHandler;
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (ReportSettings item in e.OldItems)
                {
                    UnsubscribePropertyChanged(item);
                }
            }

            if (e.NewItems != null)
            {
                foreach (ReportSettings item in e.NewItems)
                {
                    SubscribePropertyChanged(item);
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public void Dispose()
        {
            UnsubscribePropertyChanged(this);
        }
    }
}
