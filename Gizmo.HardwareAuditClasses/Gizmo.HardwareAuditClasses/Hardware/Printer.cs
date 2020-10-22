using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Management;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.Printer)]
    public class Printer
    {
        [Description("Printer Name")]
        [ReportVisibility(true)]
        public string DefaultPrinter { set; get; }

        [Description("Printer Port")]
        [ReportVisibility(true)]
        public string DefaultPrinterPortName { set; get; }

        [Description("Printer is Local")]
        [ReportVisibility(true)]
        public bool IsLocal { set; get; }

        [Description("Printer is Network")]
        [ReportVisibility(true)]
        public bool IsNetwork { set; get; }

        [Description("Printer is Shared")]
        [ReportVisibility(true)]
        public bool IsShared { set; get; }

        public Printer()
        {
            DefaultPrinter = string.Empty;
            DefaultPrinterPortName = string.Empty;
            IsLocal = false;
            IsNetwork = false;
            IsShared = false;
        }

        public static List<Printer> Enumerate(ManagementScope Scope)
        {
            var result = new List<Printer>();

            try
            {
                Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                Scope.Connect();

                string softwareRegLoc = @"SYSTEM\CurrentControlSet\Control\Print\Printers";

                ManagementClass registry = new ManagementClass(Scope, new ManagementPath("StdRegProv"), null);
                ManagementBaseObject inParams = registry.GetMethodParameters("EnumKey");
                inParams["hDefKey"] = 0x80000002;   //HKEY_LOCAL_MACHINE
                inParams["sSubKeyName"] = softwareRegLoc;

                // Read Registry Key Names 
                ManagementBaseObject outParams = registry.InvokeMethod("EnumKey", inParams, null);
                string[] programGuids = outParams["sNames"] as string[];

                foreach (string subKeyName in programGuids)
                {
                    var NA = new Printer() { IsLocal = true, IsNetwork = false };

                    try
                    {
                        inParams = registry.GetMethodParameters("GetStringValue");
                        inParams["hDefKey"] = 0x80000002;   //HKEY_LOCAL_MACHINE
                        inParams["sSubKeyName"] = softwareRegLoc + @"\" + subKeyName;

                        inParams["sValueName"] = "Name";
                        outParams = registry.InvokeMethod("GetStringValue", inParams, null);
                        if (outParams.Properties["sValue"].Value != null) { NA.DefaultPrinter = outParams.Properties["sValue"].Value.ToString().TrimStart().TrimEnd(); }
                    }
                    catch (Exception) { }

                    try
                    {
                        inParams["sValueName"] = "Port";
                        outParams = registry.InvokeMethod("GetStringValue", inParams, null);
                        if (outParams.Properties["sValue"].Value != null) { NA.DefaultPrinterPortName = outParams.Properties["sValue"].Value.ToString().TrimStart().TrimEnd(); }
                    }
                    catch (Exception) { }

                    try
                    {
                        inParams["sValueName"] = "ObjectGUID";
                        outParams = registry.InvokeMethod("GetStringValue", inParams, null);
                        if (outParams.Properties["sValue"].Value != null) { if (outParams.Properties["sValue"].Value.ToString() != "" || outParams.Properties["sValue"].Value.ToString() != string.Empty) { NA.IsShared = true; } }
                    }
                    catch (Exception) { }



                    if (NA.DefaultPrinter != "" || !NA.DefaultPrinter.Contains("Fax") || !NA.DefaultPrinter.Contains("PDF") || !NA.DefaultPrinter.Contains("Send") || !NA.DefaultPrinter.Contains("XPS") || !NA.DefaultPrinter.Contains("OneNote"))
                    {
                        if (result.Where(x => x.DefaultPrinter == NA.DefaultPrinter).Count() == 0)
                        {
                            NA.DefaultPrinter = NA.DefaultPrinter
                                .Replace(" (MS)", "")
                                .Replace(" PCL 6", "")
                                .Replace(" PS", "")
                                .Replace(" (копия 1)", "")
                                .Replace(" PCL 5", "")
                                .Replace(" PCL6", "")
                                .Replace("(0)", "")
                                .Replace(" UPD", "")
                                .Replace(" series", "")
                                .Replace(" Series", "")
                                .Replace(" OOP", "")
                                .Replace("PORTPROMPT:", "PDF")
                                .Replace("BULLZIP", "PDF")
                                .Replace("FOXIT_Reader:", "PDF")
                                .Replace("pdfcmon", "PDF")
                                .Replace("DOT4_001", "DOT4")
                                .Replace("DOT4_002", "DOT4")
                                .Replace("USB001", "USB")
                                .Replace("USB002", "USB")
                                .Replace("USB002", "USB")
                                .Replace("hp ", "HP ")
                                .Replace("LPT1:", "LPT").TrimStart().TrimEnd();
                            result.Add(NA);
                        }
                    }
                }
            }
            catch (Exception) { }
            return result;
        }
    }
}
