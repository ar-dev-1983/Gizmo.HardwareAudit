using Gizmo.HardwareAuditClasses;
using Gizmo.WPF;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAuditWPF
{
    public class HardwareScanView : Control
    {
        public HardwareScanView()
: base()
        {
            DefaultStyleKey = typeof(HardwareScanView);
        }
        static HardwareScanView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HardwareScanView), new FrameworkPropertyMetadata(typeof(HardwareScanView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Refresh();
        }

        public ComputerHardwareScan Item
        {
            get => (ComputerHardwareScan)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public UIViewModeEnum ViewMode
        {
            get => (UIViewModeEnum)GetValue(ViewModeProperty);
            set => SetValue(ViewModeProperty, value);
        }
        public FontFamily IconFontFamily
        {
            get => (FontFamily)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(ComputerHardwareScan), typeof(HardwareScanView), new UIPropertyMetadata(null, new PropertyChangedCallback(OnItemPropertyChanged)));
        public static readonly DependencyProperty ViewModeProperty = DependencyProperty.Register("ViewMode", typeof(UIViewModeEnum), typeof(HardwareScanView), new UIPropertyMetadata(UIViewModeEnum.All, new PropertyChangedCallback(OnViewModePropertyChanged)));
        public static readonly DependencyProperty IconFontFamilyProperty = DependencyProperty.Register("IconFontFamily", typeof(FontFamily), typeof(HardwareScanView), new UIPropertyMetadata(null));

        private static void OnItemPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            HardwareScanView tiv = (HardwareScanView)o;
            tiv.Refresh();
        }
        private static void OnViewModePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            HardwareScanView tiv = (HardwareScanView)o;
            tiv.Refresh();
        }

        private void Refresh()
        {
            if (GetTemplateChild("PART_HardwareScan") is StackPanel partHardwareScan)
            {
                partHardwareScan.Children.Clear();
                UpdateLayout();
                GC.Collect();
                if (Item != null)
                {
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.SystemEnclosure:
                            _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "SYSTEM" });
                            _ = partHardwareScan.Children.Add(new UIItemView()
                            {
                                ViewType = UIItemViewTypeEnum.FourValues,
                                Header01 = "System Enclosure",
                                Header02 = "Product",
                                Value01 = Item.SystemInformation.ManufacturerName,
                                Value02 = Item.SystemInformation.ProductName,
                                Header03 = "Serial Number",
                                Header04 = "Version",
                                Value03 = Item.SystemInformation.SerialNumber,
                                Value04 = Item.SystemInformation.Version,
                            });
                            _ = partHardwareScan.Children.Add(new UIItemView()
                            {
                                ViewType = UIItemViewTypeEnum.FourValues,
                                Header01 = "Motherboard",
                                Header02 = "Product",
                                Value01 = Item.MotherBoardInformation.ManufacturerName,
                                Value02 = Item.MotherBoardInformation.ProductName,
                                Header03 = "Serial Number",
                                Header04 = "Version",
                                Value03 = Item.MotherBoardInformation.SerialNumber,
                                Value04 = Item.MotherBoardInformation.Version
                            });
                            _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "OS" });
                            _ = partHardwareScan.Children.Add(new UIItemView()
                            {
                                ViewType = UIItemViewTypeEnum.FourValues,
                                Header01 = "Manufacturer",
                                Header02 = "Product",
                                Value01 = Item.WindowsInformation.Manufacturer,
                                Value02 = Item.WindowsInformation.Name,
                                Header03 = "Version",
                                Header04 = "Architecture",
                                Value03 = Item.WindowsInformation.Version,
                                Value04 = Item.WindowsInformation.OSArchitecture,
                            });
                            _ = partHardwareScan.Children.Add(new UIItemView()
                            {
                                ViewType = UIItemViewTypeEnum.FourValues,
                                Header01 = "Install Date",
                                Header02 = "Directory",
                                Value01 = Item.WindowsInformation.InstallDate,
                                Value02 = Item.WindowsInformation.WindowsDirectory,
                                Header03 = "Total Memory",
                                Header04 = "Aviailable Memory",
                                Value03 = Item.WindowsInformation.TotalMemory,
                                Value04 = Item.WindowsInformation.AviailableMemory
                            });
                            _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.OneSmallValue, Header01 = "LoggedInUser", Value01 = Item.LoggedInUser });
                            break;
                    }
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.CPUs:
                            {
                                if (Item.IsCPUsPresent)
                                {
                                    _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "CPU" });
                                    foreach (var node in Item.CPUs)
                                    {
                                        _ = partHardwareScan.Children.Add(new UIItemView()
                                        {
                                            Icon = IconFontFamily != null ? new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.CPU, FontSize = 16, IconFontFamily = IconFontFamily } : new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.CPU, FontSize = 16 },
                                            ViewType = UIItemViewTypeEnum.CPU,
                                            Header01 = node.SlotLocator,
                                            Header02 = "Model",
                                            Value01 = node.ManufacturerName,
                                            Value02 = node.Version,
                                            Header03 = "Cores",
                                            Header04 = "Threads",
                                            Value03 = node.CoreCount.ToString(),
                                            Value04 = node.ThreadCount.ToString(),
                                        });
                                    }
                                }

                                break;
                            }
                    }
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.MemoryDevices:
                            {
                                if (Item.IsMemoryDevicesPresent)
                                {
                                    _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "MEMORY DEVICES" });
                                    foreach (var node in Item.MemoryDevices)
                                    {
                                        _ = partHardwareScan.Children.Add(new UIItemView()
                                        {
                                            Icon = IconFontFamily != null ? new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.MemoryDevice, FontSize = 16, IconFontFamily = IconFontFamily } : new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.MemoryDevice, FontSize = 16 },
                                            ViewType = UIItemViewTypeEnum.MemoryDevice,
                                            Header02 = "Vendor",
                                            Header03 = "Part Number",
                                            Header04 = "Size",
                                            Value01 = node.MemoryTypeString,
                                            Value02 = node.ManufacturerName,
                                            Value03 = node.PartNumber,
                                            Value04 = node.SizeString,
                                            ShowSeparator = Item.MemoryDevices.IndexOf(node) != Item.MemoryDevices.Count - 1
                                        });
                                    }
                                }

                                break;
                            }
                    }
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.VideoControllers:
                            {
                                if (Item.IsVideoControllersPresent)
                                {
                                    _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "VIDEO CARDS" });
                                    foreach (var node in Item.VideoControllers)
                                    {
                                        _ = partHardwareScan.Children.Add(new UIItemView()
                                        {
                                            Icon = IconFontFamily != null ? new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.VideoAdapter, FontSize = 16, IconFontFamily = IconFontFamily } : new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.VideoAdapter, FontSize = 16 },
                                            ViewType = UIItemViewTypeEnum.VideoController,
                                            Header01 = "Product",
                                            Header02 = "CPU",
                                            Value01 = node.Name,
                                            Value02 = node.VideoProcessor,
                                            Header03 = "Mode",
                                            Value03 = node.VideoModeDescription,
                                        });
                                    }
                                }

                                break;
                            }
                    }
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.Displays:
                            {
                                if (Item.IsMonitorsPresent)
                                {
                                    _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "DISPLAYS" });
                                    foreach (var node in Item.Monitors)
                                    {
                                        _ = partHardwareScan.Children.Add(new UIItemView()
                                        {
                                            Icon = IconFontFamily != null ? new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.Monitor, FontSize = 16, IconFontFamily = IconFontFamily } : new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.Monitor, FontSize = 16 },
                                            ViewType = UIItemViewTypeEnum.Monitor,
                                            Header01 = "Vendor",
                                            Header02 = "Product",
                                            Header03 = "Serial Number",
                                            Value01 = node.Manufacturer,
                                            Value02 = node.MonitorModel,
                                            Value03 = node.MonitorSerialNumber
                                        });
                                    }
                                }

                                break;
                            }
                    }
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.NetworkAdapters:
                            {
                                if (Item.IsNetworkAdaptersPresent)
                                {
                                    _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "NETWORK CARDS" });
                                    foreach (var node in Item.NetworkAdapters)
                                    {
                                        _ = partHardwareScan.Children.Add(new UIItemView()
                                        {
                                            Icon = IconFontFamily != null ? new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.NetworkAdapter, FontSize = 16, IconFontFamily = IconFontFamily } : new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.NetworkAdapter, FontSize = 16 },
                                            ViewType = UIItemViewTypeEnum.NetworkAdapter,
                                            Header01 = "Product",
                                            Header02 = "IP",
                                            Value01 = node.Adapter,
                                            Value02 = node.IPAddress,
                                            Header03 = "Mask",
                                            Value03 = node.SubnetMasks,
                                            Header04 = "Geteway",
                                            Value04 = node.DefaultGeteway,
                                            Header05 = "MAC",
                                            Value05 = node.MAC,
                                            Header07 = "DHCP",
                                            Value07 = node.DHCP_Enabled,
                                            Header08 = "DHCP Server",
                                            Value08 = node.DHCP_ServerIP,
                                            ShowSeparator = Item.NetworkAdapters.IndexOf(node) != Item.NetworkAdapters.Count - 1
                                        });
                                    }
                                }

                                break;
                            }
                    }
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.PhysicalDrives:
                            {
                                if (Item.IsPhysicalDrivesPresent)
                                {
                                    _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "PHYSICAL DRIVES" });
                                    foreach (var node in Item.PhysicalDrives)
                                    {
                                        _ = partHardwareScan.Children.Add(new UIItemView()
                                        {
                                            Icon = IconFontFamily != null ? new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.PhysicalDisk, FontSize = 16, IconFontFamily = IconFontFamily } : new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.PhysicalDisk, FontSize = 16 },
                                            ViewType = UIItemViewTypeEnum.PhysicalDrive,
                                            Header01 = "Product",
                                            Header02 = "Serial Number",
                                            Value01 = node.Model,
                                            Value02 = node.SerialNumber,
                                            Header03 = "Size",
                                            Value03 = node.Size
                                        });
                                    }
                                }

                                break;
                            }
                    }
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.Partitions:
                            {
                                if (Item.IsLogicalDrivesPresent)
                                {
                                    _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "PARTITIONS" });
                                    foreach (var node in Item.LogicalDrives)
                                    {
                                        _ = partHardwareScan.Children.Add(new UIItemView()
                                        {
                                            Icon = IconFontFamily != null ? new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.Partition, FontSize = 16, IconFontFamily = IconFontFamily } : new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.Partition, FontSize = 16 },
                                            ViewType = UIItemViewTypeEnum.Partition,
                                            Header01 = node.Letter,
                                            Header02 = "Aviailable Size",
                                            Value01 = node.TotalSize,
                                            Value02 = node.AviailableSize
                                        });
                                    }
                                }

                                break;
                            }
                    }
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.Licenses:
                            {
                                if (Item.IsSoftwareLicensingProductsPresent)
                                {
                                    _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "LICENSES" });
                                    foreach (var node in Item.SoftwareLicensingProducts)
                                    {
                                        _ = partHardwareScan.Children.Add(new UIItemView()
                                        {
                                            Icon = IconFontFamily != null ? new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.Windows, FontSize = 16, IconFontFamily = IconFontFamily } : new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.Windows, FontSize = 16 },
                                            ViewType = UIItemViewTypeEnum.MicrosoftLicenseInformation,
                                            Header01 = "Name",
                                            Header02 = "Channel",
                                            Header03 = "Status",
                                            Header04 = "Partial Product Key",
                                            Header05 = "Type",
                                            Header06 = "Product Key ID",
                                            Value01 = node.Name,
                                            Value02 = node.Description,
                                            Value03 = node.LicenseStatus,
                                            Value04 = node.PatrialProductKey,
                                            Value05 = node.LicenseFamily,
                                            Value06 = node.ProductKeyID,
                                            ShowSeparator = Item.SoftwareLicensingProducts.IndexOf(node) != Item.SoftwareLicensingProducts.Count - 1
                                        });
                                    }
                                }

                                break;
                            }
                    }
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.Printers:
                            {
                                if (Item.IsPrintersPresent)
                                {
                                    _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "PRINTERS" });
                                    foreach (var node in Item.Printers)
                                    {
                                        _ = partHardwareScan.Children.Add(new UIItemView()
                                        {
                                            Icon = IconFontFamily != null ? new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.Printer, FontSize = 16, IconFontFamily = IconFontFamily } : new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.Printer, FontSize = 16 },
                                            ViewType = UIItemViewTypeEnum.Printer,
                                            Header01 = "Name",
                                            Header02 = "Port",
                                            Header03 = "Local",
                                            Header04 = "Network",
                                            Header05 = "Shared",
                                            Value01 = node.DefaultPrinter,
                                            Value02 = node.DefaultPrinterPortName,
                                            Value03 = node.IsLocal.ToString(),
                                            Value04 = node.IsNetwork.ToString(),
                                            Value05 = node.IsShared.ToString(),
                                            ShowSeparator = Item.Printers.IndexOf(node) != Item.Printers.Count - 1
                                        });
                                    }
                                }

                                break;
                            }
                    }
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.LocalUsers:
                            {
                                if (Item.IsWindowsLocalUsersPresent)
                                {
                                    _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "LOCAL USERS" });
                                    foreach (var node in Item.WindowsLocalUsers)
                                    {
                                        _ = partHardwareScan.Children.Add(new UIItemView()
                                        {
                                            Icon = IconFontFamily != null ? new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.Users, FontSize = 16, IconFontFamily = IconFontFamily } : new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.Users, FontSize = 16 },
                                            ViewType = UIItemViewTypeEnum.WindowsLocalUser,
                                            Header01 = "Name",
                                            Header02 = "Caption",
                                            Header03 = "Description",
                                            Header04 = "Active",
                                            Header05 = "Status",
                                            Header06 = "Changeable",
                                            Header07 = "Expires",
                                            Header08 = "Required",
                                            Value01 = node.Name,
                                            Value02 = node.Caption,
                                            Value03 = node.Description,
                                            Value04 = node.IsActive.ToString(),
                                            Value05 = node.Status,
                                            Value06 = node.PasswordChangeable.ToString(),
                                            Value07 = node.PasswordExpires.ToString(),
                                            Value08 = node.PasswordRequired.ToString(),
                                            ShowSeparator = Item.WindowsLocalUsers.IndexOf(node) != Item.WindowsLocalUsers.Count - 1
                                        });
                                    }
                                }

                                break;
                            }
                    }
                    switch (ViewMode)
                    {
                        case UIViewModeEnum.All:
                        case UIViewModeEnum.LocalGroups:
                            {
                                if (Item.IsWindowsLocalGroupsPresent)
                                {
                                    _ = partHardwareScan.Children.Add(new UIItemView() { ViewType = UIItemViewTypeEnum.Header, Header01 = "LOCAL GROUPS" });
                                    foreach (var node in Item.WindowsLocalGroups.Where(x => x.Childrens.Count != 0).ToList())
                                    {
                                        _ = partHardwareScan.Children.Add(new UIItemView()
                                        {
                                            Icon = IconFontFamily != null ? new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.UserGroup, FontSize = 16, IconFontFamily = IconFontFamily } : new GizmoIcon() { Icon = GizmiComputerHardwareIconsEnum.UserGroup, FontSize = 16 },
                                            ViewType = UIItemViewTypeEnum.WindowsLocalGroup,
                                            Header01 = "Name",
                                            Header02 = "Caption",
                                            Header03 = "Description",
                                            Header04 = "Local",
                                            Header05 = "Status",
                                            Header06 = "Users",
                                            Value01 = node.Name,
                                            Value02 = node.Caption,
                                            Value03 = node.Description,
                                            Value04 = node.IsLocal.ToString(),
                                            Value05 = node.Status,
                                            Value06 = node.MembersInOneLine,
                                            ShowSeparator = Item.WindowsLocalGroups.Where(x => x.Childrens.Count != 0).ToList().IndexOf(node) != Item.WindowsLocalGroups.Where(x => x.Childrens.Count != 0).ToList().Count() - 1
                                        });
                                    }
                                }

                                break;
                            }
                    }
                }
            }
        }
    }
}
