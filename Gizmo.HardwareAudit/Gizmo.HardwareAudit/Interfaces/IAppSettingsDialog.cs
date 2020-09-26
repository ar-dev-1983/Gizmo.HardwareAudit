using Gizmo.HardwareAudit.Models;

namespace Gizmo.HardwareAudit.Interfaces
{
    public interface IAppSettingsDialog
    {
        AppSettings Settings { set; get; }
        bool EditSettingsDialog(AppSettings settings);
    }
}
