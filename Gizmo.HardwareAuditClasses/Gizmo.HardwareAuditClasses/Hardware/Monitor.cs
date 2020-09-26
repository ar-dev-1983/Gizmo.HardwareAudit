using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Management;

namespace Gizmo.HardwareAuditClasses
{
    public class Monitor
    {
        public Guid Id { set; get; }

        [Category("Monitor")]
        [Description("Монитор: производитель")]
        public string Manufacturer { set; get; }

        [Category("Monitor")]
        [Description("Монитор: модель")]
        public string MonitorModel { set; get; }

        [Category("Monitor")]
        [Description("Монитор: серийный номер")]
        public string MonitorSerialNumber { set; get; }

        public Monitor()
        {
            Id = Guid.NewGuid();
            MonitorModel = string.Empty;
            MonitorSerialNumber = string.Empty;
            Manufacturer = string.Empty;
        }

        public static List<Monitor> Enumerate(ManagementScope Scope)
        {
            var result = new List<Monitor>();
            try
            {
                Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                Scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM WmiMonitorID");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                ManagementObjectCollection queryCollection = searcher.Get();
                var queryList = queryCollection.Cast<ManagementObject>().ToList();
                foreach (ManagementObject m in queryList)
                {
                    var mm = string.Empty;
                    var sn = string.Empty;
                    var mn = string.Empty;
                    try { mm = StringFormatting.ConvertUInt16ArrayToString((UInt16[])m["UserFriendlyName"]).Replace("\0", "").Replace("SERIES", "").Replace("Series", "").Replace("ACER", "").Replace("HWP ", "").Replace("DELL ", "").TrimStart().TrimEnd(); } catch (Exception) { }
                    try { sn = StringFormatting.ConvertUInt16ArrayToString((UInt16[])m["SerialNumberID"]).Replace("\0", "").TrimStart().TrimEnd(); } catch (Exception) { }
                    try { mn = StringFormatting.ConvertUInt16ArrayToString((UInt16[])m["ManufacturerName"]).Replace("\0", "").Replace("VSC", "ViewSonic").Replace("ACR", "Acer").Replace("DEL", "Dell").Replace("PHL", "Philips").TrimStart().TrimEnd(); } catch (Exception) { }
                    result.Add(new Monitor() { MonitorModel = StringFormatting.CleanInvalidXmlChars(mm), MonitorSerialNumber = StringFormatting.CleanInvalidXmlChars(sn), Manufacturer = StringFormatting.CleanInvalidXmlChars(mn) });
                }

            }
            catch (Exception) { }
            return result;
        }
    }
}
