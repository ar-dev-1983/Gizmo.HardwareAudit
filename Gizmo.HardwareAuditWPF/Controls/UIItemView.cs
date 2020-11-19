using System.Windows;
using System.Windows.Controls;

namespace Gizmo.HardwareAuditWPF
{
    public class UIItemView : ContentControl
    {
        public UIItemView()
: base()
        {
            DefaultStyleKey = typeof(UIItemView);
        }
        static UIItemView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(UIItemView), new FrameworkPropertyMetadata(typeof(UIItemView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        public string Header01
        {
            get => (string)GetValue(Header01Property);
            set => SetValue(Header01Property, value);
        }
        public string Header02
        {
            get => (string)GetValue(Header02Property);
            set => SetValue(Header02Property, value);
        }
        public object Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        public string Value01
        {
            get => (string)GetValue(Value01Property);
            set => SetValue(Value01Property, value);
        }
        public string Value02
        {
            get => (string)GetValue(Value02Property);
            set => SetValue(Value02Property, value);
        }
        
        public UIItemViewTypeEnum ViewType
        {
            get => (UIItemViewTypeEnum)GetValue(ViewTypeProperty);
            set => SetValue(ViewTypeProperty, value);
        }
        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public bool Flat
        {
            get => (bool)GetValue(FlatProperty);
            set => SetValue(FlatProperty, value);
        }
        public static readonly DependencyProperty Header01Property = DependencyProperty.Register("Header01", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Header02Property = DependencyProperty.Register("Header02", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(UIItemView), new UIPropertyMetadata(null));
        public static readonly DependencyProperty Value01Property = DependencyProperty.Register("Value01", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Value02Property = DependencyProperty.Register("Value02", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty ViewTypeProperty = DependencyProperty.Register("ViewType", typeof(UIItemViewTypeEnum), typeof(UIItemView), new UIPropertyMetadata(UIItemViewTypeEnum.OneValue));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIItemView), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register("Flat", typeof(bool), typeof(UIItemView), new FrameworkPropertyMetadata(true));
    }
}
