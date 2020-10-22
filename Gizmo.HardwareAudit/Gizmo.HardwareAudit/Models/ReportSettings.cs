using Gizmo.HardwareAudit.Interfaces;
using System;
using System.ComponentModel;

namespace Gizmo.HardwareAudit.Models
{
    public class ReportSettings : BaseViewModel, IDisposable
    {
        private readonly PropertyChangedEventHandler propertyChangedHandler;

        private Guid reportSourceContainerId = new Guid();
        private DateTime reportTimeStamp = new DateTime();

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
