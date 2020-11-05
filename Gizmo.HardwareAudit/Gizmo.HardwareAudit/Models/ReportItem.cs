using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAuditClasses.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text.Json.Serialization;

namespace Gizmo.HardwareAudit.Models
{
    public class ReportItem : BaseViewModel, IDisposable
    {
        private readonly PropertyChangedEventHandler propertyChangedHandler;
        private readonly NotifyCollectionChangedEventHandler collectionChangedhandler;

        #region Private Properties
        private Guid id = Guid.NewGuid();
        private Guid parentId = Guid.NewGuid();
        private ReportItemTypeEnum type = ReportItemTypeEnum.None;
        private ReportItemTypeEnum parentType = ReportItemTypeEnum.None;
        private string name = string.Empty;
        private string description = string.Empty;
        private ObservableCollection<ReportItem> children = new ObservableCollection<ReportItem>();
        private bool isSelected = false;
        private bool isExpanded = false;
        private bool useCustomIcon = false;
        private GizmoIconEnum customIcon = GizmoIconEnum.None;
        private ReportSettings settings = null;
        public DataTable dataTable = null;
        private bool reportIsBusy = false;
        private bool reportBuildIsDone = false;

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

        public ReportItemTypeEnum Type
        {
            get => type;
            set
            {
                if (type == value) return;
                type = value;
                OnPropertyChanged();
            }
        }

        public ReportItemTypeEnum ParentType
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

        public string Description
        {
            get => description;
            set
            {
                if (description == value) return;
                description = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ReportItem> Children
        {
            get => children;
            set
            {
                if (children == value) return;
                children = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
        public ReportItem SelectedReport
        {
            get
            {
                return Traverse(this, node => node.Children).FirstOrDefault(m => m.IsSelected);
            }
        }

        public ReportSettings Settings
        {
            get => settings;
            set
            {
                if (settings == value) return;
                settings = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public DataTable DataTable
        {
            get => dataTable;
            set
            {
                if (dataTable == value) return;
                dataTable = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public bool ReportIsBusy
        {
            get => reportIsBusy;
            set
            {
                if (reportIsBusy == value) return;
                reportIsBusy = value;
                OnPropertyChanged();
            }
        }
        [JsonIgnore]
        public bool ReportBuildIsDone
        {
            get => reportBuildIsDone;
            set
            {
                if (reportBuildIsDone == value) return;
                reportBuildIsDone = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public ReportItem()
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

        private void SubscribePropertyChanged(ReportItem item)
        {
            item.PropertyChanged += propertyChangedHandler;
            item.Children.CollectionChanged += collectionChangedhandler;
            foreach (var subitem in item.Children)
            {
                SubscribePropertyChanged(subitem);
            }
        }

        private void UnsubscribePropertyChanged(ReportItem item)
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
                foreach (ReportItem item in e.OldItems)
                {
                    UnsubscribePropertyChanged(item);
                }
            }

            if (e.NewItems != null)
            {
                foreach (ReportItem item in e.NewItems)
                {
                    SubscribePropertyChanged(item);
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsSelected"))
            {
                OnNamedPropertyChanged("SelectedReport");
            }
        }

        public void Dispose()
        {
            UnsubscribePropertyChanged(this);
            Children.CollectionChanged -= collectionChangedhandler;
        }

        public static ReportItem CreateRoot()
        {
            var result = new ReportItem()
            {
                Type = ReportItemTypeEnum.Root,
                Name = "Reports",
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ParentId = new Guid()
            };
            return result;
        }

        internal void InitialiseDataGrid()
        {
            if (Type == ReportItemTypeEnum.Report)
            {
                DataTable = new DataTable();
                DataTable.TableName = Name.Replace("  ", " ").Replace("  ", " ").Replace("  ", " ").Replace(" ", "_");
                if (Settings != null)
                {
                    if (Settings.Columns != null)
                    {
                        if (Settings.Columns.Count != 0)
                        {
                            if (Settings.ReportType == ReportTypeEnum.ComputerComponentsReport)
                            {
                                DataTable.Columns.Add("#");
                                DataTable.Columns.Add("Computer Name");
                                DataTable.Columns.Add("Computer Description");
                                DataTable.Columns.Add("Computer FQDN");
                                DataTable.Columns.Add("Computer IP Address");
                            }
                            else if (Settings.ReportType == ReportTypeEnum.ComputerComponentsQuantityReport)
                            {
                                DataTable.Columns.Add("#");
                            }
                            foreach (var column in Settings.Columns)
                            {
                                if (column.IsSelected)
                                    DataTable.Columns.Add(column.PropertyDescription);
                            }
                            if (Settings.ReportType == ReportTypeEnum.ComputerComponentsQuantityReport)
                            {
                                DataTable.Columns.Add("Quantity");
                            }
                        }
                    }
                }
            }
        }
    }
}

