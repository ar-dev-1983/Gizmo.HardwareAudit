using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;

namespace Gizmo.HardwareAuditClasses.Hardware
{
    public class WindowsLocalUser
    {
        [Category("Local User")]
        [Description("Домен")]
        public string Domain { set; get; }
        [Category("Local User")]
        [Description("Имя")]
        public string Name { set; get; }
        [Category("Local User")]
        [Description("Тип")]
        public string AccountType { set; get; }
        [Category("Local User")]
        [Description("Полное имя профиля")]
        public string Caption { set; get; }
        [Category("Local User")]
        [Description("Полное имя")]
        public string FullName { set; get; }
        [Category("Local User")]
        [Description("Описание")]
        public string Description { set; get; }
        [Category("Local User")]
        [Description("IsActive")]
        public bool IsActive { set; get; }
        [Category("Local User")]
        [Description("IsLocal")]
        public bool IsLocal { set; get; }
        [Category("Local User")]
        [Description("Lockout")]
        public bool Lockout { set; get; }
        [Category("Local User")]
        [Description("PasswordChangeable")]
        public bool PasswordChangeable { set; get; }
        [Category("Local User")]
        [Description("PasswordExpires")]
        public bool PasswordExpires { set; get; }
        [Category("Local User")]
        [Description("PasswordRequired")]
        public bool PasswordRequired { set; get; }
        [Category("Local User")]
        [Description("Статус")]
        public string Status { set; get; }
        [Category("Local User")]
        [Description("SID")]
        public string SID { set; get; }
        [Category("Local User")]
        [Description("SIDType")]
        public string SIDType { set; get; }

        public WindowsLocalUser()
        {
            Domain = string.Empty;
            Name = string.Empty;
            Description = string.Empty;
            AccountType = string.Empty;
            Caption = string.Empty;
            FullName = string.Empty;
            IsActive = false;
            IsLocal = false;
            Lockout = false;
            PasswordChangeable = false;
            PasswordExpires = false;
            PasswordRequired = false;
            Status = string.Empty;
            SID = string.Empty;
            SIDType = string.Empty;
        }

        private static string GetAccountType(string type) => type switch
        {
            "256" => "Temporary duplicate account",
            "512" => "Normal account",
            "2048" => "Interdomain trust account",
            "4096" => "Workstation trust account",
            "8192" => "Server trust account",
            _ => string.Empty
        };

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

        public static List<WindowsLocalUser> Enumerate(ManagementScope Scope)
        {
            var result = new List<WindowsLocalUser>();
            try
            {
                Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                Scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_UserAccount");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject m in queryCollection)
                {
                    result.Add(new WindowsLocalUser
                    {
                        Domain = m["Domain"] != null ? StringFormatting.CleanInvalidXmlChars(m["Domain"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        Caption = m["Caption"] != null ? StringFormatting.CleanInvalidXmlChars(m["Caption"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        FullName = m["FullName"] != null ? StringFormatting.CleanInvalidXmlChars(m["FullName"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        AccountType = m["AccountType"] != null ? GetAccountType(StringFormatting.CleanInvalidXmlChars(m["AccountType"].ToString()).TrimStart().TrimEnd()) : string.Empty,
                        Name = m["Name"] != null ? StringFormatting.CleanInvalidXmlChars(m["Name"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        Description = m["Description"] != null ? StringFormatting.CleanInvalidXmlChars(m["Description"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        IsActive = m["Disabled"] != null && (StringFormatting.CleanInvalidXmlChars(m["Disabled"].ToString()).TrimStart().TrimEnd().ToLower() == "false"),
                        IsLocal = m["LocalAccount"] != null && (StringFormatting.CleanInvalidXmlChars(m["LocalAccount"].ToString()).TrimStart().TrimEnd().ToLower() == "true"),
                        Lockout = m["Lockout"] != null && (StringFormatting.CleanInvalidXmlChars(m["Lockout"].ToString()).TrimStart().TrimEnd().ToLower() == "true"),
                        PasswordChangeable = m["PasswordChangeable"] != null && (StringFormatting.CleanInvalidXmlChars(m["PasswordChangeable"].ToString()).TrimStart().TrimEnd().ToLower() == "true"),
                        PasswordExpires = m["PasswordExpires"] != null && (StringFormatting.CleanInvalidXmlChars(m["PasswordExpires"].ToString()).TrimStart().TrimEnd().ToLower() == "true"),
                        PasswordRequired = m["PasswordRequired"] != null && (StringFormatting.CleanInvalidXmlChars(m["PasswordRequired"].ToString()).TrimStart().TrimEnd().ToLower() == "true"),
                        Status = m["Status"] != null ? StringFormatting.CleanInvalidXmlChars(m["Status"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        SID = m["SID"] != null ? StringFormatting.CleanInvalidXmlChars(m["SID"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        SIDType = m["SIDType"] != null ? GetSIDType(StringFormatting.CleanInvalidXmlChars(m["SIDType"].ToString()).TrimStart().TrimEnd()) : string.Empty,
                    });
                }
            }
            catch (Exception) { }
            return result;
        }
    }
}
