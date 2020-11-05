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
        [Description("Учетная запись AD")]
        public string UserName { set; get; }

        [ReportVisibility(true)]
        [Description("Имя")]
        public string FirstName { set; get; }

        [ReportVisibility(true)]
        [Description("Фамилия")]
        public string LastName { set; get; }

        [ReportVisibility(true)]
        [Description("Инициалы")]
        public string MN { set; get; }

        [ReportVisibility(true)]
        [Description("ФИО")]
        public string ScreenName { set; get; }

        [ReportVisibility(true)]
        [Description("Описание")]
        public string Description { set; get; }

        [ReportVisibility(true)]
        [Description("Комната")]
        public string Room { set; get; }

        [ReportVisibility(true)]
        [Description("Улица")]
        public string Street { set; get; }

        [ReportVisibility(true)]
        [Description("Город")]
        public string Town { set; get; }

        [ReportVisibility(true)]
        [Description("Область")]
        public string Area { set; get; }

        [ReportVisibility(true)]
        [Description("Страна")]
        public string Country { set; get; }

        [ReportVisibility(true)]
        [Description("Почтовый индекс")]
        public string PostCode { set; get; }

        [ReportVisibility(true)]
        [Description("Почтовый ящик")]
        public string PostBox { set; get; }

        [ReportVisibility(true)]
        [Description("Телефонный номер")]
        public string PhoneNumber { set; get; }

        [ReportVisibility(true)]
        [Description("Адрес электронной почты")]
        public string Email { set; get; }


        [ReportVisibility(false)]
        public string DistinguishedName { set; get; }

        [ReportVisibility(true)]
        [Description("IsActive")]
        public bool IsActive { set; get; }

        [ReportVisibility(true)]
        [Description("PasswordChangeable")]
        public bool PasswordChangeable { set; get; }

        [ReportVisibility(true)]
        [Description("PasswordExpires")]
        public bool PasswordExpires { set; get; }

        [ReportVisibility(true)]
        [Description("PasswordRequired")]
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
