using System;
using System.ComponentModel;

namespace Gizmo.HardwareAudit
{
    public class ActiveDirectoryUserInfo
    {
        public Guid Id { set; get; }
        [Description("Учетная запись AD")]
        public string UserName { set; get; }
        [Description("Имя")]
        public string FirstName { set; get; }
        [Description("Фамилия")]
        public string LastName { set; get; }
        [Description("Инициалы")]
        public string MN { set; get; }
        [Description("ФИО")]
        public string ScreenName { set; get; }
        [Description("Описание")]
        public string Description { set; get; }
        [Description("Комната")]
        public string Room { set; get; }
        [Description("Улица")]
        public string Street { set; get; }
        [Description("Город")]
        public string Town { set; get; }
        [Description("Область")]
        public string Area { set; get; }
        [Description("Страна")]
        public string Country { set; get; }
        [Description("Почтовый индекс")]
        public string PostCode { set; get; }
        [Description("Почтовый ящик")]
        public string PostBox { set; get; }
        [Description("Телефонный номер")]
        public string PhoneNumber { set; get; }
        [Description("Адрес электронной почты")]
        public string Email { set; get; }

        public string DistinguishedName { set; get; }
        public string DPName { set; get; }
        public string SGNames { set; get; }

        [Description("Состояние УЗ")]
        public string UserAccountControl { set; get; }

        public ActiveDirectoryUserInfo()
        {
            Id = Guid.NewGuid();
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
            DPName = string.Empty;
            UserName = string.Empty;
            SGNames = string.Empty;
            UserAccountControl = string.Empty;
        }
    }
}
