using Gizmo.HardwareAudit.Enums;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gizmo.HardwareAudit.Views
{
    ///
    /// this part of the code contains the code from the project FontAwesome5
    /// https://github.com/MartinTopfstedt/FontAwesome5
    ///

    public class GizmoIcon : TextBlock
    {
        private static readonly FontFamily GizmoIconFontFamily = new FontFamily(new Uri("pack://application:,,,/Resources/Fonts/Gizmo.IconFont.ttf"), "./#Gizmo.IconFont");

        public GizmoIconEnum Icon
        {
            get { return (GizmoIconEnum)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(GizmoIconEnum), typeof(GizmoIcon), new PropertyMetadata(GizmoIconEnum.None, OnIconPropertyChanged));

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetValue(FontFamilyProperty, GizmoIconFontFamily);
            d.SetValue(TextAlignmentProperty, TextAlignment.Center);
            d.SetValue(TextProperty, char.ConvertFromUtf32((int)e.NewValue));
        }
    }

    public class GizmoIconImage : Image
    {
        private static readonly FontFamily GizmoIconFontFamily = new FontFamily(new Uri("pack://application:,,,/Resources/Fonts/Gizmo.IconFont.ttf"), "./#Gizmo.IconFont");
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        public GizmoIconEnum Icon
        {
            get { return (GizmoIconEnum)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }
        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(GizmoIconImage), new PropertyMetadata(Brushes.Black, OnIconPropertyChanged));
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(GizmoIconEnum), typeof(GizmoIconImage), new PropertyMetadata(GizmoIconEnum.None, OnIconPropertyChanged));
        public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(GizmoIconImage), new PropertyMetadata(GizmoIconFontFamily, OnIconPropertyChanged));

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is GizmoIconImage gizmoIconImage)) return;
            gizmoIconImage.SetValue(SourceProperty, UpdateImageSource(gizmoIconImage.Icon, gizmoIconImage.Foreground, gizmoIconImage.FontFamily));
        }

        public static ImageSource UpdateImageSource(GizmoIconEnum icon, Brush foregroundBrush, FontFamily fontFamily, double emSize = 100)
        {
            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                var typeFace = new Typeface(fontFamily, FontStyles.Normal, FontWeights.Regular, FontStretches.Normal);
                drawingContext.DrawText(new FormattedText(char.ConvertFromUtf32((int)icon), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeFace, emSize, foregroundBrush, 1) { TextAlignment = TextAlignment.Center }, new Point(0, 0));
            }
            return new DrawingImage(visual.Drawing);
        }
    }
}
