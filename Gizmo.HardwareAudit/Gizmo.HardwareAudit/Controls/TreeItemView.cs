using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Models;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.HardwareAudit.Controls
{
    public class TreeItemView : Control
    {
        public TreeItemView()
: base()
        {
            DefaultStyleKey = typeof(TreeItemView);
        }
        static TreeItemView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeItemView), new FrameworkPropertyMetadata(typeof(TreeItemView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
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

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(TreeItem), typeof(TreeItemView), new UIPropertyMetadata(null, new PropertyChangedCallback(OnItemPropertyChanged)));
        public static readonly DependencyProperty ScanAvailableProperty = DependencyProperty.Register("ScanAvailable", typeof(bool), typeof(TreeItemView), new UIPropertyMetadata(false));
        public static readonly DependencyProperty IsChildComputerProperty = DependencyProperty.Register("IsChildComputer", typeof(Visibility), typeof(TreeItemView), new UIPropertyMetadata(Visibility.Collapsed));

        private static void OnItemPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            TreeItemView tiv = (TreeItemView)o;
            tiv.Refresh();
        }

        private void Refresh()
        {
            if (Item != null)
            {
                ScanAvailable = Item.ScanAvailable;
                IsChildComputer = Item.Type == ItemTypeEnum.ChildComputer ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}
