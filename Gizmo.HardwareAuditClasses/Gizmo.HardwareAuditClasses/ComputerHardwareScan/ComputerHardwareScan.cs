using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.ComputerInformation)]
    public class ComputerHardwareScan
    {
        [ReportVisibility(false)]
        public Guid Id { set; get; }

        [ReportVisibility(false)]
        public ScanTypeEnum ScanType { set; get; }

        [Description("Logged In User")]
        [ReportVisibility(true)]
        public string LoggedInUser { set; get; }

        [ReportVisibility(false)]
        public Version Version { set; get; }

        [Description("ScanTime")]
        [ReportVisibility(true)]
        public DateTime ScanTime { set; get; }

        [Description("Windows Information")]
        [ReportVisibility(true)]
        public WindowsInformation WindowsInformation { set; get; }

        [Description("System Information")]
        [ReportVisibility(true)]
        public SystemInformation SystemInformation { set; get; }
       
        [Description("BIOS Information")]
        [ReportVisibility(true)]
        public BIOSInformation BIOSInformation { set; get; }

        [Description("MotherBoard Information")]
        [ReportVisibility(true)]
        public MotherBoardInformation MotherBoardInformation { set; get; }

        [Description("CPUs")]
        [ReportVisibility(true)]
        public List<CPUInformation> CPUs { set; get; }
        
        [ReportVisibility(false)]
        [JsonIgnore]
        public bool IsCPUsPresent => CPUs != null && (CPUs.Count > 0);

        [Description("Memory Devices")]
        [ReportVisibility(true)]
        public List<MemoryDevice> MemoryDevices { set; get; }

        [ReportVisibility(false)]
        [JsonIgnore]
        public bool IsMemoryDevicesPresent => MemoryDevices != null && (MemoryDevices.Count > 0);

        [Description("Monitors")]
        [ReportVisibility(true)]
        public List<Monitor> Monitors { set; get; }

        [ReportVisibility(false)]
        [JsonIgnore]
        public bool IsMonitorsPresent => Monitors != null && (Monitors.Count > 0);

        [Description("Video Controllers")]
        [ReportVisibility(true)]
        public List<VideoController> VideoControllers { set; get; }

        [ReportVisibility(false)]
        [JsonIgnore]
        public bool IsVideoControllersPresent => VideoControllers != null && (VideoControllers.Count > 0);

        [Description("Network Adapters")]
        [ReportVisibility(true)]
        public List<NetworkAdapter> NetworkAdapters { set; get; }

        [ReportVisibility(false)]
        [JsonIgnore]
        public bool IsNetworkAdaptersPresent => NetworkAdapters != null && (NetworkAdapters.Count > 0);

        [Description("Physical Drives")]
        [ReportVisibility(true)]
        public List<PhysicalDrive> PhysicalDrives { set; get; }

        [ReportVisibility(false)]
        [JsonIgnore]
        public bool IsPhysicalDrivesPresent => PhysicalDrives != null && (PhysicalDrives.Count > 0);

        [Description("Logical Drives")]
        [ReportVisibility(true)]
        public List<LogicalDrive> LogicalDrives { set; get; }

        [ReportVisibility(false)]
        [JsonIgnore]
        public bool IsLogicalDrivesPresent => LogicalDrives != null && (LogicalDrives.Count > 0);

        [Description("Printers")]
        [ReportVisibility(true)]
        public List<Printer> Printers { set; get; }

        [ReportVisibility(false)]
        [JsonIgnore]
        public bool IsPrintersPresent => Printers != null && (Printers.Count > 0);

        [Description("Software Licensing Products")]
        [ReportVisibility(true)]
        public List<SoftwareLicensingProduct> SoftwareLicensingProducts { set; get; }

        [ReportVisibility(false)]
        [JsonIgnore]
        public bool IsSoftwareLicensingProductsPresent => SoftwareLicensingProducts != null && (SoftwareLicensingProducts.Count > 0);

        [Description("Windows Local Users")]
        [ReportVisibility(true)]
        public List<WindowsLocalUser> WindowsLocalUsers { set; get; }

        [ReportVisibility(false)]
        [JsonIgnore]
        public bool IsWindowsLocalUsersPresent => WindowsLocalUsers != null && (WindowsLocalUsers.Count > 0);

        [Description("Windows Local Groups")]
        [ReportVisibility(true)]
        public List<WindowsLocalGroup> WindowsLocalGroups { set; get; }
        
        [ReportVisibility(false)]
        [JsonIgnore]
        public bool IsWindowsLocalGroupsPresent => WindowsLocalGroups != null && (WindowsLocalGroups.Count > 0);

        public ComputerHardwareScan()
        {
            Id = Guid.NewGuid();
            ScanType = ScanTypeEnum.WindowsOS;
            LoggedInUser = string.Empty;

            BIOSInformation = new BIOSInformation();
            SystemInformation = new SystemInformation();
            MotherBoardInformation = new MotherBoardInformation();
            CPUs = new List<CPUInformation>();
            MemoryDevices = new List<MemoryDevice>();
            WindowsInformation = new WindowsInformation();
            Monitors = new List<Monitor>();
            PhysicalDrives = new List<PhysicalDrive>();
            LogicalDrives = new List<LogicalDrive>();
            VideoControllers = new List<VideoController>();
            NetworkAdapters = new List<NetworkAdapter>();
            Printers = new List<Printer>();
            WindowsLocalUsers = new List<WindowsLocalUser>();
            WindowsLocalGroups = new List<WindowsLocalGroup>();
        }

        public static ComputerHardwareScan Scan(string Name, ConnectionOptions User, bool IsLocal)
        {
            ComputerHardwareScan result = new ComputerHardwareScan();
            var IsThereWMI = false;
            byte[] raw = null;
            byte majorVersion = 0;
            byte minorVersion = 0;
            string ScopeName;
            try
            {
                ScopeName = System.Net.Dns.GetHostEntry(Name).HostName;
            }
            catch (Exception)
            {
                ScopeName = Name;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                result.ScanType = ScanTypeEnum.WindowsOS;
                try
                {
                    var Scope = IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\WMI") : new ManagementScope("\\\\" + ScopeName + "\\root\\WMI", User);
                    Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                    Scope.Connect();
                    ObjectQuery query = new ObjectQuery("SELECT * FROM MSSMBios_RawSMBiosTables");
                    ManagementObjectCollection collection;
                    using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query))
                    {
                        collection = searcher.Get();
                    }

                    foreach (ManagementObject mo in collection)
                    {
                        raw = (byte[])mo["SMBiosData"];
                        majorVersion = (byte)mo["SmbiosMajorVersion"];
                        minorVersion = (byte)mo["SmbiosMinorVersion"];
                        break;
                    }
                    IsThereWMI = true;

                }
                catch (Exception) { }

                if (IsThereWMI)
                {
                    if (majorVersion > 0 || minorVersion > 0)
                        result.Version = new Version(majorVersion, minorVersion);

                    if (raw != null && raw.Length > 0)
                    {
                        int offset = 0;
                        byte type = raw[offset];
                        while (offset + 4 < raw.Length && type != 127)
                        {

                            type = raw[offset];
                            int length = raw[offset + 1];
                            ushort handle = (ushort)((raw[offset + 2] << 8) | raw[offset + 3]);

                            if (offset + length > raw.Length)
                                break;
                            byte[] data = new byte[length];
                            Array.Copy(raw, offset, data, 0, length);
                            offset += length;

                            List<string> stringsList = new List<string>();
                            if (offset < raw.Length && raw[offset] == 0)
                                offset++;

                            while (offset < raw.Length && raw[offset] != 0)
                            {
                                StringBuilder sb = new StringBuilder();
                                while (offset < raw.Length && raw[offset] != 0)
                                {
                                    sb.Append((char)raw[offset]); offset++;
                                }
                                offset++;
                                stringsList.Add(sb.ToString());
                            }
                            offset++;
                            switch (type)
                            {
                                case 0x00:
                                    {
                                        var Helper = new SmBiosHelper(type, handle, data, stringsList.ToArray());
                                        result.BIOSInformation.Vendor = Helper.GetString(0x04);
                                        result.BIOSInformation.Version = Helper.GetString(0x05);
                                    }
                                    break;
                                case 0x01:
                                    {
                                        var Helper = new SmBiosHelper(type, handle, data, stringsList.ToArray());
                                        result.SystemInformation.ManufacturerName = Helper.GetString(0x04);
                                        result.SystemInformation.ProductName = Helper.GetString(0x05);
                                        result.SystemInformation.Version = Helper.GetString(0x06);
                                        result.SystemInformation.SerialNumber = Helper.GetString(0x07);
                                        result.SystemInformation.Family = Helper.GetString(0x1A);
                                    }
                                    break;
                                case 0x02:
                                    {
                                        var Helper = new SmBiosHelper(type, handle, data, stringsList.ToArray());
                                        result.MotherBoardInformation.ManufacturerName = Helper.GetString(0x04).Trim();
                                        result.MotherBoardInformation.ProductName = Helper.GetString(0x05).Trim();
                                        result.MotherBoardInformation.Version = Helper.GetString(0x06).Trim();
                                        result.MotherBoardInformation.SerialNumber = Helper.GetString(0x07).Trim();
                                    }
                                    break;
                                case 0x04:
                                    {
                                        var Helper = new SmBiosHelper(type, handle, data, stringsList.ToArray());
                                        var cpu = new CPUInformation
                                        {
                                            SlotLocator = "Slot #" + (result.CPUs.Count + 1),
                                            ManufacturerName = Helper.GetString(0x07).Trim(),
                                            Version = Helper.GetString(0x10).Trim(),
                                            CoreCount = Helper.GetByte(0x23),
                                            CoreEnabled = Helper.GetByte(0x24),
                                            ThreadCount = Helper.GetByte(0x25),
                                            ExternalClock = Helper.GetWord(0x12)
                                        };
                                        result.CPUs.Add(cpu);
                                    }
                                    break;
                                case 0x11:
                                    {
                                        var Helper = new SmBiosHelper(type, handle, data, stringsList.ToArray());
                                        if (Helper.GetString(0x10).Trim().Contains("SYSTEM ROM") != true)
                                        {
                                            if (Helper.GetWord(0x0C) != 0)
                                            {
                                                var m = new MemoryDevice
                                                {
                                                    DeviceLocator = "Memory Slot #" + (result.MemoryDevices.Count + 1),
                                                    BankLocator = Helper.GetString(0x11).Trim(),
                                                    MemoryType = Helper.GetByte(0x12),
                                                    FormFactor = Helper.GetByte(0x0E),
                                                    ManufacturerName = Helper.GetString(0x17).Trim(),
                                                    SerialNumber = Helper.GetString(0x18).Trim(),
                                                    PartNumber = Helper.GetString(0x1A).Trim(),
                                                    Speed = Helper.GetWord(0x15),
                                                    Size = Helper.GetWord(0x0C),
                                                    AssetTag = Helper.GetString(0x19).Trim()
                                                };
                                                result.MemoryDevices.Add(m);
                                            }
                                            else
                                            {
                                                var m = new MemoryDevice
                                                {
                                                    DeviceLocator = "Memory Slot #" + (result.MemoryDevices.Count + 1),
                                                    BankLocator = Helper.GetString(0x11).Trim(),
                                                    MemoryType = Helper.GetByte(0x12),
                                                    FormFactor = Helper.GetByte(0x0E),
                                                    ManufacturerName = string.Empty,
                                                    SerialNumber = string.Empty,
                                                    PartNumber = string.Empty,
                                                    Speed = Helper.GetWord(0x15),
                                                    Size = Helper.GetWord(0x0C),
                                                    AssetTag = string.Empty
                                                };
                                                result.MemoryDevices.Add(m);
                                            }
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    try
                    {
                        var Scope = IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2") : new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2", User);
                        Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                        Scope.Connect();
                        ObjectQuery query = new ObjectQuery("Select username FROM Win32_ComputerSystem");
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                        ManagementObjectCollection queryCollection = searcher.Get();
                        foreach (ManagementObject m in queryCollection)
                        {
                            if (m["username"] != null)
                            {
                                if (m["username"].ToString().TrimStart().TrimEnd() != string.Empty)
                                    result.LoggedInUser = m["username"].ToString().TrimStart().TrimEnd();
                            }
                        }
                    }
                    catch (Exception) { }

                    result.WindowsInformation = WindowsInformation.Enumerate(IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2") : new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2", User));
                    result.LogicalDrives = LogicalDrive.Enumerate(IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2") : new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2", User));
                    result.PhysicalDrives = PhysicalDrive.Enumerate(IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2") : new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2", User));
                    result.NetworkAdapters = NetworkAdapter.Enumerate(IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2") : new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2", User));
                    result.Monitors = Monitor.Enumerate(IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\WMI") : new ManagementScope("\\\\" + ScopeName + "\\root\\WMI", User));
                    result.VideoControllers = VideoController.Enumerate(IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2") : new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2", User));
                    result.Printers = Printer.Enumerate(IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2") : new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2", User));
                    result.SoftwareLicensingProducts = SoftwareLicensingProduct.Enumerate(IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2") : new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2", User));
                    result.WindowsLocalUsers = WindowsLocalUser.Enumerate(IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2") : new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2", User));
                    result.WindowsLocalGroups = WindowsLocalGroup.Enumerate(IsLocal ? new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2") : new ManagementScope("\\\\" + ScopeName + "\\root\\cimv2", User));

                    if (result.IsMemoryDevicesPresent)
                    {
                        var AllMem = 0.0f;
                        foreach (var node in result.MemoryDevices)
                        {
                            AllMem += node.Size;
                        }
                        result.WindowsInformation.TotalMemory = (AllMem / 1024).ToString("0.00 Gb");
                    }
                    result.ScanTime = DateTime.Now;
                }
                else
                {
                    throw new Exception("An error occurred while querying for WMI data: access denied");
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                result.ScanType = ScanTypeEnum.LinuxOS;

            }
            else
            {
                result.ScanType = ScanTypeEnum.Unknown;
                throw new Exception("An error occurred while querying for data");
            }

            return result;
        }
    }
}
