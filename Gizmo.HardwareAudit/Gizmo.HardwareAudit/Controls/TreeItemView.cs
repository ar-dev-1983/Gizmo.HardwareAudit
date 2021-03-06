﻿using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.Services;
using Gizmo.HardwareAuditClasses;
using Gizmo.HardwareAuditClasses.Helpers;
using Gizmo.HardwareAuditWPF;
using Gizmo.WPF;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Gizmo.HardwareAudit.Controls
{
    public class TreeItemView : Control
    {
        private UIComboBox partScans;
        private ComputerHardwareScanView partScanView;
        private UIEnumSwitch partSwitch;
        private UIPopupButton partExportGroup;

        #region Commands
        public static RoutedCommand ExportAsPngFile = new RoutedCommand();

        public static RoutedCommand ExportAsHtmlFile = new RoutedCommand();

        private void ExportAsPngFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (partScanView != null)
            {
                IDialog SaveFileService = new DefaultDialogService();
                var ScanTime = DateTime.Now;
                if (SaveFileService.SaveFileDialog(Item.Name + "_" + ScanTime.ToShortDateString().Replace(".", "_") + "_" + ScanTime.ToLongTimeString().Replace(":", "_"), "PNG files|*.png") == true)
                {
                    var partScanViewBackground = partScanView.Background;
                    partScanView.Background = ThemeManager.GetResource(Theme, "WindowBackgroundBrush") as SolidColorBrush;
                    MemoryStream stream = new MemoryStream();
                    VisualHelper.SnapShotPNG(partScanView).Save(stream);
                    var image = System.Drawing.Image.FromStream(stream);
                    image.Save(SaveFileService.FilePath);
                    partScanView.Background = partScanViewBackground;
                }
            }
        }
        
        private Dictionary<string, fakeColor> Pupulate()
        {
            var BrushList = new Dictionary<string, fakeColor>();
            var b = ThemeManager.GetResource(Theme, "WindowBackgroundBrush") as SolidColorBrush;
            var t = ThemeManager.GetResource(Theme, "WindowForegroundBrush") as SolidColorBrush;
            var h = ThemeManager.GetResource(Theme, "RowHighlightBrush") as SolidColorBrush;

            BrushList.Add("Background", new fakeColor(b.Color.A, b.Color.R, b.Color.G, b.Color.B));
            BrushList.Add("Text", new fakeColor(t.Color.A, t.Color.R, t.Color.G, t.Color.B));
            BrushList.Add("Header", new fakeColor(h.Color.A, h.Color.R, h.Color.G, h.Color.B));
            return BrushList;
        }

        private void ExportAsHtmlFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (partSwitch!=null)
            {
                if (partScans != null)
                {
                    if (partScans.SelectedValue != null)
                    {
                        IDialog SaveFileService = new DefaultDialogService();
                        var ScanTime = DateTime.Now;
                        if (SaveFileService.SaveFileDialog(Item.Name + "_" + ScanTime.ToShortDateString().Replace(".", "_") + "_" + ScanTime.ToLongTimeString().Replace(":", "_"), "html files|*.html") == true)
                        {
                            var result = htmlSerializer.Serialize(Item.Name, partScans.SelectedValue as ComputerHardwareScan, Pupulate(), partSwitch.SelectedItems, Item.Description, Item.Address, Item.FQDN, true);
                            File.WriteAllText(SaveFileService.FilePath, result);
                        }
                    }
                }
            }
        }
        #endregion

        public TreeItemView()
: base()
        {
            CommandBindings.Add(new CommandBinding(ExportAsPngFile, ExportAsPngFile_Executed));
            CommandBindings.Add(new CommandBinding(ExportAsHtmlFile, ExportAsHtmlFile_Executed));

            DefaultStyleKey = typeof(TreeItemView);
        }

        static TreeItemView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeItemView), new FrameworkPropertyMetadata(typeof(TreeItemView)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            partScans = GetTemplateChild("partScans") as UIComboBox;
            partScanView = GetTemplateChild("partScanView") as ComputerHardwareScanView;
            partSwitch = GetTemplateChild("partSwitch") as UIEnumSwitch;
            partExportGroup = GetTemplateChild("partExportGroup") as UIPopupButton;
            partExportGroup.PopupOpened -= PartExportGroup_PopupOpened;
            partExportGroup.PopupOpened += PartExportGroup_PopupOpened;
            var viewModesBinding = new Binding("SelectedItems") 
            {
                Mode=  BindingMode.OneWay,
                Source=partSwitch
            };
            partScanView.SetBinding(ComputerHardwareScanView.ViewModesProperty, viewModesBinding);
        }

        private void PartExportGroup_PopupOpened(object sender, RoutedEventArgs e)
        {
            if (partScanView != null && ScanAvailable)
            {
                CanBeExported = (int)partScanView.ActualHeight != 0 && (int)partScanView.ActualWidth != 0;
            }
            else
                CanBeExported = false;
        }

        public TreeItem Item
        {
            get => (TreeItem)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public bool ScanAvailable
        {
            get => (bool)GetValue(ScanAvailableProperty);
            set => SetValue(ScanAvailableProperty, value);
        }
        public Visibility IsChildComputer
        {
            get => (Visibility)GetValue(IsChildComputerProperty);
            set => SetValue(IsChildComputerProperty, value);
        }
        public Visibility ScanControlsVisibility
        {
            get => (Visibility)GetValue(ScanControlsVisibilityProperty);
            set => SetValue(ScanControlsVisibilityProperty, value);
        }
        public bool CanBeExported
        {
            get => (bool)GetValue(CanBeExportedProperty);
            set => SetValue(CanBeExportedProperty, value);
        }

        public UIThemeEnum Theme
        {
            get => (UIThemeEnum)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }
        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(TreeItem), typeof(TreeItemView), new UIPropertyMetadata(null, new PropertyChangedCallback(OnItemPropertyChanged)));
        public static readonly DependencyProperty ScanAvailableProperty = DependencyProperty.Register("ScanAvailable", typeof(bool), typeof(TreeItemView), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsChildComputerProperty = DependencyProperty.Register("IsChildComputer", typeof(Visibility), typeof(TreeItemView), new UIPropertyMetadata(Visibility.Hidden));
        public static readonly DependencyProperty ScanControlsVisibilityProperty = DependencyProperty.Register("ScanControlsVisibility", typeof(Visibility), typeof(TreeItemView), new UIPropertyMetadata(Visibility.Hidden));
        public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register("Theme", typeof(UIThemeEnum), typeof(TreeItemView), new UIPropertyMetadata(UIThemeEnum.BlueDark));
        public static readonly DependencyProperty CanBeExportedProperty = DependencyProperty.Register("CanBeExported", typeof(bool), typeof(TreeItemView), new UIPropertyMetadata(false));

        private static void OnItemPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TreeItemView tiv = (TreeItemView)o;
            tiv.Refresh();
        }

        private void Refresh()
        {
            if (Item != null)
            {
                IsChildComputer = Item.Type == ItemTypeEnum.ChildComputer ? Visibility.Visible : Visibility.Hidden;
                ScanControlsVisibility = Item.Type == ItemTypeEnum.ChildComputer ? Item.ScanAvailable ? Visibility.Visible : Visibility.Hidden : Visibility.Hidden;
                Item.HardwareScans.CollectionChanged -= HardwareScansChanged;
                Item.HardwareScans.CollectionChanged += HardwareScansChanged;
                if (Item.ScanAvailable)
                {
                    ScanAvailable = Item.ScanAvailable;
                    if (partScans != null)
                    {
                        if (Item.HardwareScans.Count != 0)
                        {
                            partScans.SelectedIndex = 0;
                        }
                        else
                        {
                            partScans.SelectedIndex = -1;
                        }
                    }
                }
                else
                {
                    ScanAvailable = false;
                    partScans.SelectedIndex = -1;
                }
            }
            //GC.Collect();
        }

        private void HardwareScansChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Item != null)
            {
                if (Item.HardwareScans != null)
                {
                    if (partScans != null)
                    {

                        if (Item.HardwareScans.Count != 0)
                        {
                            ScanAvailable = true;
                            partScans.SelectedIndex = -1;
                            partScans.UpdateLayout();
                            partScanView.UpdateLayout();
                            partScans.SelectedIndex = 0;
                        }
                        else
                        {
                            ScanAvailable = false;
                            partScans.SelectedIndex = -1;
                        }
                        IsChildComputer = Item.Type == ItemTypeEnum.ChildComputer ? Visibility.Visible : Visibility.Hidden;
                        ScanControlsVisibility = Item.Type == ItemTypeEnum.ChildComputer ? Item.ScanAvailable ? Visibility.Visible : Visibility.Hidden : Visibility.Hidden;
                    }
                }
            }
        }
    }
}
