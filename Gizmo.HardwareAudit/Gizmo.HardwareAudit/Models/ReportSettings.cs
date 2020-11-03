using Gizmo.HardwareAudit.Classes.Helpers;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAuditClasses.Enums;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Gizmo.HardwareAudit.Models
{
    public class ReportSettings : BaseViewModel, IDisposable
    {
        private readonly PropertyChangedEventHandler propertyChangedHandler;

        private Guid reportSourceContainerId = new Guid();
        private DateTime reportTimeStamp = new DateTime();
        private ObservableCollection<ClassProperties> columns;
        private ComponentGroupingTypeEnum componentGroupingItem;
        private ComponentTypeEnum componentItem;
        private bool eachValueIsASepareteRow = false;
        public Guid ReportSourceContainerId
        {
            get => reportSourceContainerId;
            set
            {
                if (reportSourceContainerId == value) return;
                reportSourceContainerId = value;
                OnPropertyChanged();
            }
        }

        public DateTime ReportTimeStamp
        {
            get => reportTimeStamp;
            set
            {
                if (reportTimeStamp == value) return;
                reportTimeStamp = value;
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
        public ComponentGroupingTypeEnum ComponentGroupingItem
        {
            get => componentGroupingItem;
            set
            {
                if (componentGroupingItem == value) return;
                componentGroupingItem = value;
                OnPropertyChanged();
            }
        }
        public ComponentTypeEnum ComponentItem
        {
            get => componentItem;
            set
            {
                if (componentItem == value) return;
                componentItem = value;
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

        public ReportSettings()
        {
            propertyChangedHandler = new PropertyChangedEventHandler(Item_PropertyChanged);
            Initialise();
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

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

        }

        public void Dispose()
        {
            UnsubscribePropertyChanged(this);
        }
    }
}
