namespace Gizmo.HardwareAuditClasses.Enums
{
    //This is for future use.
    //By default hardware scan is from Windows OS host
    //Plan is to use this to determine how to scan and present dataL: WMI for Windows or Gizmo Hardware Audit agent for others 
    public enum ScanTypeEnum
    {
        WindowsOS = 0,
        LinuxOS = 1,
        Unknown = 99
    }
}
