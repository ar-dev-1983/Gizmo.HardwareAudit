using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAuditClasses;
using System.Collections.ObjectModel;

namespace Gizmo.HardwareAudit.Interfaces
{
    public interface ISerialization
    {
        ObservableCollection<LogItem> OpenLog(string filename);
        void SaveLog(string filename, ObservableCollection<LogItem> list);

        TreeItem OpenModel(string filename);
        void SaveModel(string filename, TreeItem item);

        AppSettings OpenSettings(string filename);
        void SaveSettings(string filename, AppSettings settings);

        ComputerHardwareScan ImportScan(string filename);
        void ExportScan(string filename, ComputerHardwareScan scan);

        ObservableCollection<CheckTPCPortSetting> ImportCheckPortsList(string filename);
        void ExportCheckPortsList(string filename, ObservableCollection<CheckTPCPortSetting> list);

        ReportItem OpenReportModel(string filename);
        void SaveReportModel(string filename, ReportItem item);

    }
}
