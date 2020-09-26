using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAuditClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;

namespace Gizmo.HardwareAudit.Models
{
    public class TreeItem : BaseViewModel, IDisposable
    {
        private readonly PropertyChangedEventHandler propertyChangedHandler;
        private readonly NotifyCollectionChangedEventHandler collectionChangedhandler;

        #region Private Properties
        private Guid id = Guid.NewGuid();
        private Guid parentId = Guid.NewGuid();
        private ItemTypeEnum type = ItemTypeEnum.None;
        private ItemTypeEnum parentType = ItemTypeEnum.None;

        private DomainDiscoverySettings domainSettings;
        private ItemSortEnum sortByName = ItemSortEnum.Ascending;
        private ItemSortEnum sortByDescription = ItemSortEnum.Ascending;
        private ItemSortEnum sortByStatus = ItemSortEnum.Ascending;
        private ItemSortEnum sortByLastScanDateTime = ItemSortEnum.Ascending;
        private ItemSortEnum sortByPreviousScanDateTime = ItemSortEnum.Ascending;

        private string name = string.Empty;
        private string description = string.Empty;
        private string fqdn = string.Empty;
        private string address = string.Empty;
        private ItemStatusEnum status = ItemStatusEnum.Unknown;

        private ObservableCollection<ComputerHardwareScan> hardwareScans = new ObservableCollection<ComputerHardwareScan>();
        private ObservableCollection<TreeItem> children = new ObservableCollection<TreeItem>();

        private ObservableCollection<string> sharedFolders = new ObservableCollection<string>();
        private ObservableCollection<CheckTPCPortResult> checkPortResults = new ObservableCollection<CheckTPCPortResult>();

        private Guid userProfileId = new Guid();
        private bool useParentUserProfile = true;
        private bool probeInUse = false;
        private DateTime lastScanDateTime = new DateTime();
        private DateTime previousScanDateTime = new DateTime();
        private bool isSelected = false;
        private bool isExpanded = false;
        #endregion

        #region Public Properties
        public DomainDiscoverySettings DomainSettings
        {
            get => domainSettings;
            set
            {
                if (domainSettings == value) return;
                domainSettings = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public bool ProbeInUse
        {
            get => probeInUse;
            set
            {
                if (probeInUse == value) return;
                probeInUse = value;
                OnPropertyChanged();
            }
        }

        public bool UseParentUserProfile
        {
            get => useParentUserProfile;
            set
            {
                if (useParentUserProfile == value) return;
                useParentUserProfile = value;
                OnPropertyChanged();
            }
        }

        public Guid UserProfileId
        {
            get => userProfileId;
            set
            {
                if (userProfileId == value) return;
                userProfileId = value;
                OnPropertyChanged();
            }
        }

        public ItemSortEnum SortByName
        {
            get => sortByName;
            set
            {
                if (sortByName == value) return;
                sortByName = value;
                OnPropertyChanged();
            }
        }

        public ItemSortEnum SortByDescription
        {
            get => sortByDescription;
            set
            {
                if (sortByDescription == value) return;
                sortByDescription = value;
                OnPropertyChanged();
            }
        }

        public ItemSortEnum SortByStatus
        {
            get => sortByStatus;
            set
            {
                if (sortByStatus == value) return;
                sortByStatus = value;
                OnPropertyChanged();
            }
        }

        public ItemSortEnum SortByLastScanDateTime
        {
            get => sortByLastScanDateTime;
            set
            {
                if (sortByLastScanDateTime == value) return;
                sortByLastScanDateTime = value;
                OnPropertyChanged();
            }
        }

        public ItemSortEnum SortByPreviousScanDateTime
        {
            get => sortByPreviousScanDateTime;
            set
            {
                if (sortByPreviousScanDateTime == value) return;
                sortByPreviousScanDateTime = value;
                OnPropertyChanged();
            }
        }

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

        [Category("Item")]
        [Description("Оборудование: имя")]
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

        [Category("Item")]
        [Description("Оборудование: описание")]
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

        [Category("Item")]
        [Description("Оборудование: FQDN")]
        public string FQDN
        {
            get => fqdn;
            set
            {
                if (fqdn == value) return;
                fqdn = value;
                OnPropertyChanged();
            }
        }

        [Category("Item")]
        [Description("Оборудование: IP-адрес")]
        public string Address
        {
            get => address;
            set
            {
                if (address == value) return;
                address = value;
                OnPropertyChanged();
            }
        }

        [Category("Item")]
        [Description("Оборудование: статус")]
        public ItemStatusEnum Status
        {
            get => status;
            set
            {
                if (status == value) return;
                status = value;
                OnPropertyChanged();
            }
        }
        [Category("Item")]
        [Description("Оборудование: время последнего сканирования")]
        public DateTime LastScanDateTime
        {
            get => lastScanDateTime;
            set
            {
                if (lastScanDateTime == value) return;
                lastScanDateTime = value;
                OnPropertyChanged();
            }
        }
        [Category("Item")]
        [Description("Оборудование: время предыдущего сканирования")]
        public DateTime PreviousScanDateTime
        {
            get => previousScanDateTime;
            set
            {
                if (previousScanDateTime == value) return;
                previousScanDateTime = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ComputerHardwareScan> HardwareScans
        {
            get => hardwareScans;
            set
            {
                if (hardwareScans == value) return;
                hardwareScans = value;
                OnPropertyChanged();
            }
        }
        [JsonIgnore]
        public bool ScansAvailable
        {
            get => HardwareScans != null;
        }
        [JsonIgnore]
        public int LastScanIndex
        {
            get => ScansAvailable && HardwareScans.Count > 0 ? 0 : -1;
        }
        [JsonIgnore]
        public bool ScanAvailable
        {
            get => ScansAvailable && HardwareScans.Count > 0;
        }
        public ObservableCollection<TreeItem> Children
        {
            get => children;
            set
            {
                if (children == value) return;
                children = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SharedFolders
        {
            get => sharedFolders;
            set
            {
                if (sharedFolders == value) return;
                sharedFolders = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CheckTPCPortResult> CheckPortResults
        {
            get => checkPortResults;
            set
            {
                if (checkPortResults == value) return;
                checkPortResults = value;
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
        [JsonIgnore]
        public TreeItem SelectedItem
        {
            get
            {
                return Traverse(this, node => node.Children).FirstOrDefault(m => m.IsSelected);
            }
        }
        #endregion

        public TreeItem()
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

        private void SubscribePropertyChanged(TreeItem item)
        {
            item.PropertyChanged += propertyChangedHandler;
            item.Children.CollectionChanged += collectionChangedhandler;
            foreach (var subitem in item.Children)
            {
                SubscribePropertyChanged(subitem);
            }
        }

        private void UnsubscribePropertyChanged(TreeItem item)
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
                foreach (TreeItem item in e.OldItems)
                {
                    UnsubscribePropertyChanged(item);
                }
            }

            if (e.NewItems != null)
            {
                foreach (TreeItem item in e.NewItems)
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

        #region Public Methods

        internal void AddScan(ComputerHardwareScan scan)
        {
            HardwareScans.Add(scan);
            LastScanDateTime = hardwareScans != null ? hardwareScans.Count != 0 ? hardwareScans[0].ScanTime : new DateTime() : new DateTime();
            PreviousScanDateTime = hardwareScans != null ? hardwareScans.Count != 0 ? hardwareScans.Count > 1 ? hardwareScans[1].ScanTime : new DateTime() : new DateTime() : new DateTime();
        }

        internal void AddScan(int index, ComputerHardwareScan scan)
        {
            HardwareScans.Insert(index, scan);
            LastScanDateTime = hardwareScans != null ? hardwareScans.Count != 0 ? hardwareScans[0].ScanTime : new DateTime() : new DateTime();
            PreviousScanDateTime = hardwareScans != null ? hardwareScans.Count != 0 ? hardwareScans.Count > 1 ? hardwareScans[1].ScanTime : new DateTime() : new DateTime() : new DateTime();
        }

        internal void DeleteScan(int index)
        {
            HardwareScans.RemoveAt(index);
            LastScanDateTime = hardwareScans != null ? hardwareScans.Count != 0 ? hardwareScans[0].ScanTime : new DateTime() : new DateTime();
            PreviousScanDateTime = hardwareScans != null ? hardwareScans.Count != 0 ? hardwareScans.Count > 1 ? hardwareScans[1].ScanTime : new DateTime() : new DateTime() : new DateTime();
        }

        internal void ClearScans()
        {
            HardwareScans.Clear();
            LastScanDateTime = new DateTime();
            PreviousScanDateTime = new DateTime();
        }

        internal void ClearScansButKeepLastOne()
        {
            if (HardwareScans.Count != 0)
            {
                if (HardwareScans.Count > 1)
                {
                    do
                    {
                        hardwareScans.RemoveAt(HardwareScans.Count - 1);
                    }
                    while (HardwareScans.Count != 1);
                }
            }
            LastScanDateTime = hardwareScans != null ? hardwareScans.Count != 0 ? hardwareScans[0].ScanTime : new DateTime() : new DateTime();
            PreviousScanDateTime = new DateTime();
        }

        internal void ClearScansButKeepLastTwo()
        {
            if (HardwareScans.Count != 0)
            {
                if (HardwareScans.Count > 2)
                {
                    do
                    {
                        hardwareScans.RemoveAt(HardwareScans.Count - 1);
                    }
                    while (HardwareScans.Count != 2);
                }
            }
            LastScanDateTime = hardwareScans != null ? hardwareScans.Count != 0 ? hardwareScans[0].ScanTime : new DateTime() : new DateTime();
            PreviousScanDateTime = hardwareScans != null ? hardwareScans.Count != 0 ? hardwareScans.Count > 1 ? hardwareScans[1].ScanTime : new DateTime() : new DateTime() : new DateTime();
        }

        public static TreeItem CreateRoot()
        {
            var result = new TreeItem()
            {
                Type = ItemTypeEnum.Root,
                Description = "",
                Name = "Home",
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                ParentId = new Guid(),
                Status = ItemStatusEnum.Container
            };
            result.Children.Add(CreateActiveDirectory());
            result.Children.Add(CreateWorkgroup());
            return result;
        }

        private static TreeItem CreateActiveDirectory()
        {
            return new TreeItem()
            {
                Type = ItemTypeEnum.ActiveDirectory,
                Description = "",
                Name = "Active Directory",
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                ParentId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Status = ItemStatusEnum.Container
            };
        }

        private static TreeItem CreateWorkgroup()
        {
            return new TreeItem()
            {
                Type = ItemTypeEnum.Workgroup,
                Description = "",
                Name = "Workgroup",
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                ParentId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Status = ItemStatusEnum.Container
            };
        }

        public void SortChildrenByName()
        {
            if (Children != null)
            {
                if (Children.Count != 0)
                {
                    SortByName = SortByName == ItemSortEnum.Ascending ? ItemSortEnum.Descending : ItemSortEnum.Ascending;
                    var Result = SortByName == ItemSortEnum.Ascending ? Children.OrderBy(x => x.Name).ToList() : Children.OrderByDescending(x => x.Name).ToList();
                    Children.Clear();
                    foreach (var node in Result)
                    {
                        Children.Add(node);
                    }
                }
            }
        }

        public void SortChildrenByDescription()
        {
            if (Children != null)
            {
                if (Children.Count != 0)
                {
                    SortByDescription = SortByDescription == ItemSortEnum.Ascending ? ItemSortEnum.Descending : ItemSortEnum.Ascending;
                    var Result = SortByDescription == ItemSortEnum.Ascending ? Children.OrderBy(x => x.Description).ToList() : Children.OrderByDescending(x => x.Description).ToList();
                    Children.Clear();
                    foreach (var node in Result)
                    {
                        Children.Add(node);
                    }
                }
            }
        }

        public void SortChildrenByStatus()
        {
            if (Children != null)
            {
                if (Children.Count != 0)
                {
                    SortByStatus = SortByStatus == ItemSortEnum.Ascending ? ItemSortEnum.Descending : ItemSortEnum.Ascending;
                    var Result = SortByStatus == ItemSortEnum.Ascending ? Children.OrderBy(x => x.Status).ToList() : Children.OrderByDescending(x => x.Status).ToList();
                    Children.Clear();
                    foreach (var node in Result)
                    {
                        Children.Add(node);
                    }
                }
            }
        }

        public void SortChildrenByLastScanDateTime()
        {
            if (Children != null)
            {
                if (Children.Count != 0)
                {
                    SortByLastScanDateTime = SortByLastScanDateTime == ItemSortEnum.Ascending ? ItemSortEnum.Descending : ItemSortEnum.Ascending;
                    var Result = SortByLastScanDateTime == ItemSortEnum.Ascending ? Children.OrderBy(x => x.LastScanDateTime).ToList() : Children.OrderByDescending(x => x.LastScanDateTime).ToList();
                    Children.Clear();
                    foreach (var node in Result)
                    {
                        Children.Add(node);
                    }
                }
            }
        }

        public void SortChildrenByPreviousScanDateTime()
        {
            if (Children != null)
            {
                if (Children.Count != 0)
                {
                    SortByPreviousScanDateTime = SortByPreviousScanDateTime == ItemSortEnum.Ascending ? ItemSortEnum.Descending : ItemSortEnum.Ascending;
                    var Result = SortByPreviousScanDateTime == ItemSortEnum.Ascending ? Children.OrderBy(x => x.PreviousScanDateTime).ToList() : Children.OrderByDescending(x => x.PreviousScanDateTime).ToList();
                    Children.Clear();
                    foreach (var node in Result)
                    {
                        Children.Add(node);
                    }
                }
            }
        }

        #endregion
    }
}
