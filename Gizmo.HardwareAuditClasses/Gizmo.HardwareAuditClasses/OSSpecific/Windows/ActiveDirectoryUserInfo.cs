using Gizmo.HardwareAuditClasses.Enums;
using Gizmo.HardwareAuditClasses.Helpers;
using System;
using System.ComponentModel;

namespace Gizmo.HardwareAuditClasses
{
    [ComponentType(ComponentTypeEnum.ActiveDirectoryUserInfo)]
    public class ActiveDirectoryUserInfo
    {
        [ReportVisibility(false)]
        public Guid Id { set; get; }

        [ReportVisibility(true)]
        [Description("AD Source")]
        public string SourceName { set; get; }

        [ReportVisibility(true)]
        [Description("User Name")]
        public string UserName { set; get; }

        [ReportVisibility(true)]
        [Description("First name")]
        public string FirstName { set; get; }

        [ReportVisibility(true)]
        [Description("Last name")]
        public string LastName { set; get; }

        [ReportVisibility(true)]
        [Description("Initials")]
        public string MN { set; get; }

        [ReportVisibility(true)]
        [Description("Screen name")]
        public string ScreenName { set; get; }

        [ReportVisibility(true)]
        [Description("Description")]
        public string Description { set; get; }

        [ReportVisibility(true)]
        [Description("Room")]
        public string Room { set; get; }

        [ReportVisibility(true)]
        [Description("Street")]
        public string Street { set; get; }

        [ReportVisibility(true)]
        [Description("Town")]
        public string Town { set; get; }

        [ReportVisibility(true)]
        [Description("Area")]
        public string Area { set; get; }

        [ReportVisibility(true)]
        [Description("Country")]
        public string Country { set; get; }

        [ReportVisibility(true)]
        [Description("Post Code")]
        public string PostCode { set; get; }

        [ReportVisibility(true)]
        [Description("Post Box")]
        public string PostBox { set; get; }

        [ReportVisibility(true)]
        [Description("Phone Number")]
        public string PhoneNumber { set; get; }

        [ReportVisibility(true)]
        [Description("Email")]
        public string Email { set; get; }


        [ReportVisibility(false)]
        public string DistinguishedName { set; get; }

        [ReportVisibility(true)]
        [Description("Is Active")]
        public bool IsActive { set; get; }

        [ReportVisibility(true)]
        [Description("Password Changeable")]
        public bool PasswordChangeable { set; get; }

        [ReportVisibility(true)]
        [Description("Password Expires")]
        public bool PasswordExpires { set; get; }

        [ReportVisibility(true)]
        [Description("Password Required")]
        public bool PasswordRequired { set; get; }

        public ActiveDirectoryUserInfo()
        {
            Id = Guid.NewGuid();
            SourceName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            MN = string.Empty;
            ScreenName = string.Empty;
            Description = string.Empty;
            Room = string.Empty;
            Street = string.Empty;
            PostCode = string.Empty;
            PostBox = string.Empty;
            Town = string.Empty;

            Area = string.Empty;
            Country = string.Empty;
            PhoneNumber = string.Empty;
            Email = string.Empty;
            DistinguishedName = string.Empty;
            UserName = string.Empty;
            IsActive = false;
            PasswordChangeable = false;
            PasswordExpires = false;
            PasswordRequired = false;
        }
    }
}
