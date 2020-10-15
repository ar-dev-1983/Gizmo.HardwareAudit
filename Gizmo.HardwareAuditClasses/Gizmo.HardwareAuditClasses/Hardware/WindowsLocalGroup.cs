using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;

namespace Gizmo.HardwareAuditClasses.Hardware
{
    public class WindowsLocalGroup
    {
        [Category("Local Group")]
        [Description("Домен")]
        public string Domain { set; get; }
        [Category("Local Group")]
        [Description("Имя")]
        public string Name { set; get; }
        [Category("Local Group")]
        [Description("Описание")] 
        public string Description { set; get; }
        [Category("Local Group")]
        [Description("Полное имя профиля")]
        public string Caption { set; get; }
        [Category("Local Group")]
        [Description("IsLocal")]
        public bool IsLocal { set; get; }
        [Category("Local Group")]
        [Description("Статус")]
        public string Status { set; get; }
        [Category("Local Group")]
        [Description("SID")]
        public string SID { set; get; }
        [Category("Local Group")]
        [Description("SIDType")]
        public string SIDType { set; get; }

        [Category("Local Group")]
        [Description("Члены группы")]
        public List<string> Childrens { set; get; }

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
                            node.Childrens.Add(child.Split(',')[1].Replace("Name=\"","").Replace("\"",""));
                    }
                }
                catch (Exception) { }
            }
            return result;
        }
    }
}
