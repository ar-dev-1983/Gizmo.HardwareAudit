using Gizmo.HardwareAudit.Classes;
using Gizmo.HardwareAudit.Enums;
using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using System;

namespace Gizmo.HardwareAudit.ViewModels
{
    //NOTE: ReportViewModel is a part of AppViewModel Class
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


        private WorkCommand newReportCommand;
        public WorkCommand NewReportCommand
        {
            get
            {
                return newReportCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        dialogService.ShowMessage("test");
                    }
                    catch (Exception e)
                    {
                        AddLogItem(DateTime.Now, e.Message, "Exception", LogItemTypeEnum.Error, "NewReportCommand");
                    }
                }, (obj) => SelectedReportItem != null);
            }
        }
    }
}
