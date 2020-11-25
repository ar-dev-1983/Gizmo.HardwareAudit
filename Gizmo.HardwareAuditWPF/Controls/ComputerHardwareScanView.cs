using Gizmo.HardwareAuditClasses;
using Gizmo.HardwareAuditClasses.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class ComputerHardwareScanView : Control
    {
        public ComputerHardwareScanView()
: base()
        {
            ViewModes = new ObservableCollection<object>();
            DefaultStyleKey = typeof(ComputerHardwareScanView);
        }
        static ComputerHardwareScanView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComputerHardwareScanView), new FrameworkPropertyMetadata(typeof(ComputerHardwareScanView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ViewModes.CollectionChanged -= ViewModes_CollectionChanged;
            ViewModes.CollectionChanged += ViewModes_CollectionChanged;
        }

        private void ViewModes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateViewMode();       
        }

        public ComputerHardwareScan Item
        {
            get => (ComputerHardwareScan)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public ObservableCollection<object> ViewModes
        {
            get => (ObservableCollection<object>)GetValue(ViewModesProperty);
            set => SetValue(ViewModesProperty, value);
        }
        public FontFamily Icons
        {
            get => (FontFamily)GetValue(IconsProperty);
            set => SetValue(IconsProperty, value);
        }
        public Visibility ShowSystemEnclosure
        {
            get => (Visibility)GetValue(ShowSystemEnclosureProperty);
            set => SetValue(ShowSystemEnclosureProperty, value);
        }
        public Visibility ShoCPUs
        {
            get => (Visibility)GetValue(ShoCPUsProperty);
            set => SetValue(ShoCPUsProperty, value);
        }
        public Visibility ShowMemoryDevices
        {
            get => (Visibility)GetValue(ShowMemoryDevicesProperty);
            set => SetValue(ShowMemoryDevicesProperty, value);
        }
        public Visibility ShowVideoControllers
        {
            get => (Visibility)GetValue(ShowVideoControllersProperty);
            set => SetValue(ShowVideoControllersProperty, value);
        }
        public Visibility ShowDisplays
        {
            get => (Visibility)GetValue(ShowDisplaysProperty);
            set => SetValue(ShowDisplaysProperty, value);
        }
        public Visibility ShowNetworkAdapters
        {
            get => (Visibility)GetValue(ShowNetworkAdaptersProperty);
            set => SetValue(ShowNetworkAdaptersProperty, value);
        }
        public Visibility ShowPhysicalDrives
        {
            get => (Visibility)GetValue(ShowPhysicalDrivesProperty);
            set => SetValue(ShowPhysicalDrivesProperty, value);
        }
        public Visibility ShowPartitions
        {
            get => (Visibility)GetValue(ShowPartitionsProperty);
            set => SetValue(ShowPartitionsProperty, value);
        }
        public Visibility ShowLicenses
        {
            get => (Visibility)GetValue(ShowLicensesProperty);
            set => SetValue(ShowLicensesProperty, value);
        }
        public Visibility ShowPrinters
        {
            get => (Visibility)GetValue(ShowPrintersProperty);
            set => SetValue(ShowPrintersProperty, value);
        }
        public Visibility ShowLocalUsers
        {
            get => (Visibility)GetValue(ShowLocalUsersProperty);
            set => SetValue(ShowLocalUsersProperty, value);
        }
        public Visibility ShowLocalGroups
        {
            get => (Visibility)GetValue(ShowLocalGroupsProperty);
            set => SetValue(ShowLocalGroupsProperty, value);
        }
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(ComputerHardwareScan), typeof(ComputerHardwareScanView), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ViewModesProperty = DependencyProperty.Register("ViewModes", typeof(ObservableCollection<object>), typeof(ComputerHardwareScanView), new UIPropertyMetadata(null, new PropertyChangedCallback(OnViewModesChanged)));
        public static readonly DependencyProperty IconsProperty = DependencyProperty.Register("Icons", typeof(FontFamily), typeof(ComputerHardwareScanView), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ShowSystemEnclosureProperty = DependencyProperty.Register("ShowSystemEnclosure", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShoCPUsProperty = DependencyProperty.Register("ShoCPUs", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowMemoryDevicesProperty = DependencyProperty.Register("ShowMemoryDevices", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowVideoControllersProperty = DependencyProperty.Register("ShowVideoControllers", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowDisplaysProperty = DependencyProperty.Register("ShowDisplays", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowNetworkAdaptersProperty = DependencyProperty.Register("ShowNetworkAdapters", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowPhysicalDrivesProperty = DependencyProperty.Register("ShowPhysicalDrives", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowPartitionsProperty = DependencyProperty.Register("ShowPartitions", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowLicensesProperty = DependencyProperty.Register("ShowLicenses", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowPrintersProperty = DependencyProperty.Register("ShowPrinters", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowLocalUsersProperty = DependencyProperty.Register("ShowLocalUsers", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));
        public static readonly DependencyProperty ShowLocalGroupsProperty = DependencyProperty.Register("ShowLocalGroups", typeof(Visibility), typeof(ComputerHardwareScanView), new UIPropertyMetadata(Visibility.Visible));

        private static void OnViewModesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o != null)
            {
                if (o is ComputerHardwareScanView control)
                {
                    control.UpdateViewMode();
                }
            }
        }

        internal void UpdateViewMode()
        {
            if (ViewModes != null)
            {
                if (ViewModes.Count != 0)
                {
                    bool showAll = false;
                    foreach (var node in ViewModes)
                    {
                        if (Equals(node, UIViewModeEnum.All))
                        {
                            SetAllToState(Visibility.Visible);
                            showAll = true;
                            return;
                        }
                    }
                    if (!showAll)
                    {
                        SetAllToState(Visibility.Collapsed);

                        foreach (var node in ViewModes)
                        {
                            if (Equals(node, UIViewModeEnum.SystemEnclosure))
                            {
                                ShowSystemEnclosure = Visibility.Visible;
                            }
                            else if (Equals(node, UIViewModeEnum.CPUs))
                            {
                                ShoCPUs = Visibility.Visible;
                            }
                            else if (Equals(node, UIViewModeEnum.MemoryDevices))
                            {
                                ShowMemoryDevices = Visibility.Visible;
                            }
                            else if (Equals(node, UIViewModeEnum.VideoControllers))
                            {
                                ShowVideoControllers = Visibility.Visible;
                            }
                            else if (Equals(node, UIViewModeEnum.Displays))
                            {
                                ShowDisplays = Visibility.Visible;
                            }
                            else if (Equals(node, UIViewModeEnum.NetworkAdapters))
                            {
                                ShowNetworkAdapters = Visibility.Visible;
                            }
                            else if (Equals(node, UIViewModeEnum.PhysicalDrives))
                            {
                                ShowPhysicalDrives = Visibility.Visible;
                            }
                            else if (Equals(node, UIViewModeEnum.Partitions))
                            {
                                ShowPartitions = Visibility.Visible;
                            }
                            else if (Equals(node, UIViewModeEnum.Licenses))
                            {
                                ShowLicenses = Visibility.Visible;
                            }
                            else if (Equals(node, UIViewModeEnum.Printers))
                            {
                                ShowPrinters = Visibility.Visible;
                            }
                            else if (Equals(node, UIViewModeEnum.LocalUsers))
                            {
                                ShowLocalUsers = Visibility.Visible;
                            }
                            else if (Equals(node, UIViewModeEnum.LocalGroups))
                            {
                                ShowLocalGroups = Visibility.Visible;
                            }
                        }
                    } 
                }
                else
                {
                    SetAllToState(Visibility.Visible);
                }
            }
            else
            {
                SetAllToState(Visibility.Visible);
            }
        }
        private void SetAllToState(Visibility state)
        {
            ShowSystemEnclosure = state;
            ShoCPUs = state;
            ShowMemoryDevices = state;
            ShowVideoControllers = state;
            ShowDisplays = state;
            ShowNetworkAdapters = state;
            ShowPhysicalDrives = state;
            ShowPartitions = state;
            ShowLicenses = state;
            ShowPrinters = state;
            ShowLocalUsers = state;
            ShowLocalGroups = state;
        }
    }
}
