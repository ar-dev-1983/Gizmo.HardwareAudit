using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;

namespace Gizmo.HardwareAudit.ViewModels
{
    public partial class AppViewModel : BaseViewModel
    {
        private ReportItem reportRoot;
        public ReportItem ReportRoot
        {
            get => reportRoot;
            set
            {
                if (reportRoot == value) return;
                reportRoot = value;
                OnPropertyChanged();
            }
        }
        public ReportItem SelectedReportItem => ReportRoot.SelectedReport;

    }
}
