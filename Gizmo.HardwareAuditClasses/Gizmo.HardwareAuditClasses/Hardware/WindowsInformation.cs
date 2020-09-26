using System;
using System.ComponentModel;
using System.Management;

namespace Gizmo.HardwareAuditClasses
{
    public class WindowsInformation
    {
        [Category("OSInformation")]
        [Description("ОС: Производитель")]
        public string Manufacturer { set; get; }

        [Category("OSInformation")]
        [Description("ОС: Имя")]
        public string Name { set; get; }

        [Category("OSInformation")]
        [Description("ОС: Версия")]
        public string Version { set; get; }

        [Category("OSInformation")]
        [Description("ОС: Архитектура")]
        public string OSArchitecture { set; get; }

        [Category("OSInformation")]
        [Description("ОС: Путь к папке Windows")]
        public string WindowsDirectory { set; get; }

        public string TotalMemory { set; get; }

        public string AviailableMemory { set; get; }

        [Description("ОС: дата установки")]
        public string InstallDate { set; get; }

        public WindowsInformation()
        {
            WindowsDirectory = string.Empty;
            Name = string.Empty;
            Version = string.Empty;
            Manufacturer = string.Empty;
            OSArchitecture = string.Empty;
            TotalMemory = string.Empty;
            AviailableMemory = string.Empty;
            InstallDate = string.Empty;
        }
        public static WindowsInformation Enumerate(ManagementScope Scope)
        {
            var result = new WindowsInformation();
            try
            {
                Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                Scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject m in queryCollection)
                {
                    result.WindowsDirectory = m["WindowsDirectory"] != null ? StringFormatting.CleanInvalidXmlChars(m["WindowsDirectory"].ToString()).TrimStart().TrimEnd() : string.Empty;
                    result.Name = m["Caption"] != null ? StringFormatting.CleanInvalidXmlChars(m["Caption"].ToString().Replace(" (Registered Trademark)", "")).TrimStart().TrimEnd() : string.Empty;
                    result.Version = m["Version"] != null ? StringFormatting.CleanInvalidXmlChars(m["Version"].ToString()).TrimStart().TrimEnd() : string.Empty;
                    result.OSArchitecture = m["OSArchitecture"] != null ? StringFormatting.CleanInvalidXmlChars(m["OSArchitecture"].ToString()).TrimStart().TrimEnd() : string.Empty;
                    result.Manufacturer = m["Manufacturer"] != null ? StringFormatting.CleanInvalidXmlChars(m["Manufacturer"].ToString()).TrimStart().TrimEnd() : string.Empty;
                    result.InstallDate = m["InstallDate"] != null ? ManagementDateTimeConverter.ToDateTime(StringFormatting.CleanInvalidXmlChars(m["InstallDate"].ToString()).TrimStart().TrimEnd()).ToString() : string.Empty;
                    result.AviailableMemory = m["FreePhysicalMemory"] != null ? ((double)((UInt64)m["FreePhysicalMemory"]) / 1024 / 1024).ToString("0.00 Gb").TrimStart().TrimEnd() : string.Empty;
                }
            }
            catch (Exception) { }
            return result;
        }

    }
}
