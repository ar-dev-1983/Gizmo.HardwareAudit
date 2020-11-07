using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.WPF;
using System.Windows;
using System.Windows.Controls;

namespace Gizmo.HardwareAudit.Controls
{
    [TemplatePart(Name = partDataTable)]
    [TemplatePart(Name = partSwitch)]
    public class ReportItemView : Control
    {
        const string partDataTable = "PART_DataTable";
        const string partSwitch = "PART_EachValueIsASepareteRow";
        protected DataGrid reportDataGrid;
        protected UISwitch reportEachValueIsASepareteRow;

        internal DataGrid ReportDataGrid
        {
            get { return reportDataGrid; }
        }
        internal UISwitch ReportEachValueIsASepareteRow
        {
            get { return reportEachValueIsASepareteRow; }
        }

        public ReportItemView()
: base()
        {
            DefaultStyleKey = typeof(ReportItemView);
        }
        static ReportItemView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ReportItemView), new FrameworkPropertyMetadata(typeof(ReportItemView)));
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            reportDataGrid = GetTemplateChild(partDataTable) as DataGrid;
            reportEachValueIsASepareteRow = GetTemplateChild(partSwitch) as UISwitch;
        }

        public ReportItem Item
        {
            get => (ReportItem)GetValue(ItemProperty);
            set => SetValue(ItemProperty, value);
        }
        public Visibility IsReport
        {
            get => (Visibility)GetValue(IsReportProperty);
            set => SetValue(IsReportProperty, value);
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register("Item", typeof(ReportItem), typeof(ReportItemView), new UIPropertyMetadata(null, new PropertyChangedCallback(OnItemPropertyChanged)));
        public static readonly DependencyProperty IsReportProperty = DependencyProperty.Register("IsReport", typeof(Visibility), typeof(ReportItemView), new UIPropertyMetadata(Visibility.Collapsed));

        private static void OnItemPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ReportItemView tiv = (ReportItemView)o;
            tiv.Refresh();
        }

        private void Refresh()
        {
            if (Item != null)
            {
                IsReport = Item.Type == ReportItemTypeEnum.Report ? Visibility.Visible : Visibility.Collapsed;
                if (Item.Type == ReportItemTypeEnum.Report)
                {
                    if (ReportDataGrid != null)
                    {
                        if (Item.DataTable != null)
                            ReportDataGrid.DataContext = Item.DataTable.DefaultView;
                    }
                    if (Item.Settings.ReportType == ReportTypeEnum.ComputerComponentsReport)
                    {
                        if (ReportEachValueIsASepareteRow != null)
                        {
                            ReportEachValueIsASepareteRow.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    {
                        if (ReportEachValueIsASepareteRow != null)
                        {
                            ReportEachValueIsASepareteRow.Visibility = Visibility.Collapsed;
                        }
                    }
                }
                else
                {
                    if (ReportEachValueIsASepareteRow != null)
                    {
                        ReportEachValueIsASepareteRow.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }
}
