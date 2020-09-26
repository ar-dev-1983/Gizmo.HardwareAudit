using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.ViewModels;

namespace Gizmo.HardwareAudit.Services
{
    public class AppSettingsDialogService : IAppSettingsDialog
    {
        public AppSettings Settings { set; get; }

        public bool EditSettingsDialog(AppSettings settings)
        {
            AppSettingsDialog dialog = new AppSettingsDialog(settings);
            switch (dialog.ShowDialog())
            {
                case true:
                    Settings = (dialog.DataContext as AppSettingsViewModel).Settings;
                    return true;
                default:
                    return false;
            }
        }
    }
}
