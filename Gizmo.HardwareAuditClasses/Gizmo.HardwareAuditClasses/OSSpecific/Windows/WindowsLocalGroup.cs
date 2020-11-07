using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.WindowsLocalGroup)]
    public class WindowsLocalGroup
    {
        [Description("Local Group Domain")]
        [ReportVisibility(true)]
        public string Domain { set; get; }

        [Description("Local Group Name")]
        [ReportVisibility(true)]
        public string Name { set; get; }

        [Description("Local Group Description")]
        [ReportVisibility(true)]
        public string Description { set; get; }

        [Description("Local Group Caption")]
        [ReportVisibility(true)]
        public string Caption { set; get; }

        [Description("Local Group is Local")]
        [ReportVisibility(true)]
        public bool IsLocal { set; get; }

        [Description("Local Group Status")]
        [ReportVisibility(true)]
        public string Status { set; get; }

        [Description("Local Group SID")]
        [ReportVisibility(true)]
        public string SID { set; get; }

        [Description("Local Group SID Type")]
        [ReportVisibility(true)]
        public string SIDType { set; get; }

        [ReportVisibility(false)]
        public List<string> Childrens { set; get; }

        [ReportVisibility(true)]
        [Description("Local Group Members")]
        public string MembersInOneLine => Childrens != null ? Childrens.Count != 0 ? string.Join("\n", Childrens) : string.Empty : string.Empty;

        public WindowsLocalGroup()
        {
            Domain = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Caption = string.Empty;
            IsLocal = false;
            Status = string.Empty;
            SID = string.Empty;
            SIDType = string.Empty;
            Childrens = new List<string>();
        }

        private static string GetSIDType(string type) => type switch
        {
            "1" => "SidTypeUser",
            "2" => "SidTypeGroup",
            "3" => "SidTypeDomain",
            "4" => "SidTypeAlias",
            "5" => "SidTypeWellKnownGroup",
            "6" => "SidTypeDeletedAccount",
            "7" => "SidTypeInvalid",
            "8" => "SidTypeUnknown",
            "9" => "SidTypeComputer",
            _ => string.Empty
        };

        public static List<WindowsLocalGroup> Enumerate(ManagementScope Scope)
        {
            var result = new List<WindowsLocalGroup>();
            try
            {
                Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                Scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Group");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject m in queryCollection)
                {
                    result.Add(new WindowsLocalGroup
                    {
                        Domain = m["Domain"] != null ? StringFormatting.CleanInvalidXmlChars(m["Domain"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        Caption = m["Caption"] != null ? StringFormatting.CleanInvalidXmlChars(m["Caption"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        Name = m["Name"] != null ? StringFormatting.CleanInvalidXmlChars(m["Name"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        Description = m["Description"] != null ? StringFormatting.CleanInvalidXmlChars(m["Description"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        IsLocal = m["LocalAccount"] != null && (StringFormatting.CleanInvalidXmlChars(m["LocalAccount"].ToString()).TrimStart().TrimEnd().ToLower() == "true"),
                        Status = m["Status"] != null ? StringFormatting.CleanInvalidXmlChars(m["Status"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        SID = m["SID"] != null ? StringFormatting.CleanInvalidXmlChars(m["SID"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        SIDType = m["SIDType"] != null ? GetSIDType(StringFormatting.CleanInvalidXmlChars(m["SIDType"].ToString()).TrimStart().TrimEnd()) : string.Empty,
                    });
                }
            }
            catch (Exception) { }
            foreach (var node in result)
            {
                try
                {
                    Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                    Scope.Connect();
                    ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_GroupUser WHERE GroupComponent = \"Win32_Group.Domain='" + node.Domain + "',Name='" + node.Name + "'\"");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                    ManagementObjectCollection queryCollection = searcher.Get();
                    foreach (ManagementObject m in queryCollection)
                    {
                        var child = m["PartComponent"] != null ? StringFormatting.CleanInvalidXmlChars(m["PartComponent"].ToString()).TrimStart().TrimEnd() : string.Empty;
                        if (child != string.Empty)
                            node.Childrens.Add(child.Split(',')[1].Replace("Name=\"", "").Replace("\"", ""));
                    }
                }
                catch (Exception) { }
            }
            return result;
        }
    }
}
