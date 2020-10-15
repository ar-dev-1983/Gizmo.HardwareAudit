using Gizmo.HardwareAudit.Enums;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.HardwareAudit.Views
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
        public string Header03
        {
            get => (string)GetValue(Header03Property);
            set => SetValue(Header03Property, value);
        }
        public string Header04
        {
            get => (string)GetValue(Header04Property);
            set => SetValue(Header04Property, value);
        }
        public string Header05
        {
            get => (string)GetValue(Header05Property);
            set => SetValue(Header05Property, value);
        }
        public string Header06
        {
            get => (string)GetValue(Header06Property);
            set => SetValue(Header06Property, value);
        }
        public string Header07
        {
            get => (string)GetValue(Header07Property);
            set => SetValue(Header07Property, value);
        }
        public string Header08
        {
            get => (string)GetValue(Header08Property);
            set => SetValue(Header08Property, value);
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
        public string Value03
        {
            get => (string)GetValue(Value03Property);
            set => SetValue(Value03Property, value);
        }
        public string Value04
        {
            get => (string)GetValue(Value04Property);
            set => SetValue(Value04Property, value);
        }
        public string Value05
        {
            get => (string)GetValue(Value05Property);
            set => SetValue(Value05Property, value);
        }
        public string Value06
        {
            get => (string)GetValue(Value06Property);
            set => SetValue(Value06Property, value);
        }
        public string Value07
        {
            get => (string)GetValue(Value07Property);
            set => SetValue(Value07Property, value);
        }
        public string Value08
        {
            get => (string)GetValue(Value08Property);
            set => SetValue(Value08Property, value);
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
        public bool ShowSeparator
        {
            get => (bool)GetValue(ShowSeparatorProperty);
            set => SetValue(ShowSeparatorProperty, value);
        }
        public static readonly DependencyProperty Header01Property = DependencyProperty.Register("Header01", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Header02Property = DependencyProperty.Register("Header02", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Header03Property = DependencyProperty.Register("Header03", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Header04Property = DependencyProperty.Register("Header04", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Header05Property = DependencyProperty.Register("Header05", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Header06Property = DependencyProperty.Register("Header06", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Header07Property = DependencyProperty.Register("Header07", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Header08Property = DependencyProperty.Register("Header08", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(UIItemView), new UIPropertyMetadata(null));
        public static readonly DependencyProperty Value01Property = DependencyProperty.Register("Value01", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Value02Property = DependencyProperty.Register("Value02", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Value03Property = DependencyProperty.Register("Value03", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Value04Property = DependencyProperty.Register("Value04", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Value05Property = DependencyProperty.Register("Value05", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Value06Property = DependencyProperty.Register("Value06", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Value07Property = DependencyProperty.Register("Value07", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty Value08Property = DependencyProperty.Register("Value08", typeof(string), typeof(UIItemView), new UIPropertyMetadata(string.Empty));
        public static readonly DependencyProperty ViewTypeProperty = DependencyProperty.Register("ViewType", typeof(UIItemViewTypeEnum), typeof(UIItemView), new UIPropertyMetadata(UIItemViewTypeEnum.OneValue));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(UIItemView), new UIPropertyMetadata(new CornerRadius(0)));
        public static readonly DependencyProperty FlatProperty = DependencyProperty.Register("Flat", typeof(bool), typeof(UIItemView), new FrameworkPropertyMetadata(true));
        public static readonly DependencyProperty ShowSeparatorProperty = DependencyProperty.Register("ShowSeparator", typeof(bool), typeof(UIItemView), new FrameworkPropertyMetadata(false));

    }
}
