using Gizmo.HardwareAudit.Enums;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.HardwareAudit.Controls
{
    public class TreeItemIcon : ContentControl
    {
        public TreeItemIcon()
: base()
        {
            DefaultStyleKey = typeof(TreeItemIcon);
        }
        static TreeItemIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TreeItemIcon), new FrameworkPropertyMetadata(typeof(TreeItemIcon)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public ItemStatusEnum ItemStatus
        {
            get => (ItemStatusEnum)GetValue(ItemStatusProperty);
            set => SetValue(ItemStatusProperty, value);
        }
        public ItemTypeEnum ItemType
        {
            get => (ItemTypeEnum)GetValue(ItemTypeProperty);
            set => SetValue(ItemTypeProperty, value);
        }
        public bool UserProfileSpecified
        {
            get => (bool)GetValue(UserProfileSpecifiedProperty);
            set => SetValue(UserProfileSpecifiedProperty, value);
        }
        public Guid UserProfileIdSpecified
        {
            get => (Guid)GetValue(UserProfileIdSpecifieddProperty);
            set => SetValue(UserProfileIdSpecifieddProperty, value);
        }

        public bool UserProfileIsSpecified
        {
            get { return (bool)GetValue(UserProfileIsSpecifiedProperty); }
            private set { SetValue(UserProfileIsSpecifiedPropertyKey, value); }
        }
        public static readonly DependencyProperty ItemStatusProperty = DependencyProperty.Register("ItemStatus", typeof(ItemStatusEnum), typeof(TreeItemIcon), new UIPropertyMetadata(ItemStatusEnum.Unknown));
        public static readonly DependencyProperty ItemTypeProperty = DependencyProperty.Register("ItemType", typeof(ItemTypeEnum), typeof(TreeItemIcon), new UIPropertyMetadata(ItemTypeEnum.None));
        public static readonly DependencyProperty UserProfileSpecifiedProperty = DependencyProperty.Register("UserProfileSpecified", typeof(bool), typeof(TreeItemIcon), new UIPropertyMetadata(true));
        public static readonly DependencyProperty UserProfileIdSpecifieddProperty = DependencyProperty.Register("UserProfileIdSpecified", typeof(Guid), typeof(TreeItemIcon), new UIPropertyMetadata(new Guid(), OnUserProfileIdSpecifiedPropertyChanged));
        private static void OnUserProfileIdSpecifiedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeItemIcon tii = (TreeItemIcon)d;
            tii.CheckUserProfileId();
        }
        internal void CheckUserProfileId()
        {
            UserProfileIsSpecified = UserProfileIdSpecified != new Guid();
        }
        private static readonly DependencyPropertyKey UserProfileIsSpecifiedPropertyKey = DependencyProperty.RegisterReadOnly("UserProfileIsSpecified", typeof(bool), typeof(TreeItemIcon), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty UserProfileIsSpecifiedProperty = UserProfileIsSpecifiedPropertyKey.DependencyProperty;

    }
}
