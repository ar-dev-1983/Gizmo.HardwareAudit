using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.LinuxLocalUser)]
    public class LinuxLocalUser
    {
        [Description("Local User Domain")]
        [ReportVisibility(true)]
        public string Domain { set; get; }

        [Description("Local User Name")]
        [ReportVisibility(true)]
        public string Name { set; get; }

        [Description("Local User Account Type")]
        [ReportVisibility(true)]
        public string AccountType { set; get; }

        [Description("Local User Caption")]
        [ReportVisibility(true)]
        public string Caption { set; get; }

        [Description("Local User Full Name")]
        [ReportVisibility(true)]
        public string FullName { set; get; }

        [Description("Local User Description")]
        [ReportVisibility(true)]
        public string Description { set; get; }

        [Description("Local User is Active")]
        [ReportVisibility(true)]
        public bool IsActive { set; get; }

        [Description("Local User is Local")]
        [ReportVisibility(true)]
        public bool IsLocal { set; get; }

        [Description("Local User Lockout")]
        [ReportVisibility(true)]
        public bool Lockout { set; get; }

        [Description("Local User Password Changeable")]
        [ReportVisibility(true)]
        public bool PasswordChangeable { set; get; }

        [Description("Local User Password Expires")]
        [ReportVisibility(true)]
        public bool PasswordExpires { set; get; }

        [Description("Local User Password is Required")]
        [ReportVisibility(true)]
        public bool PasswordRequired { set; get; }

        [Description("Local User Status")]
        [ReportVisibility(true)]
        public string Status { set; get; }

        [Description("Local User SID")]
        [ReportVisibility(true)]
        public string SID { set; get; }

        [Description("Local User SID Type")]
        [ReportVisibility(true)]
        public string SIDType { set; get; }

        public LinuxLocalUser()
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
    }
}
