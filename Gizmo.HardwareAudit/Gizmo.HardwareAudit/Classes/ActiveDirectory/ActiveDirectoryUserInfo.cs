using System;
using System.ComponentModel;
using System.DirectoryServices;

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

        [Description("IsActive")]
        public bool IsActive { set; get; }
        [Description("PasswordChangeable")]
        public bool PasswordChangeable { set; get; }
        [Description("PasswordExpires")]
        public bool PasswordExpires { set; get; }
        [Description("PasswordRequired")]
        public bool PasswordRequired { set; get; }

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
            UserName = string.Empty;
            IsActive = false;
            PasswordChangeable = false;
            PasswordExpires = false;
            PasswordRequired = false;
        }

        public ActiveDirectoryUserInfo(DirectoryEntry directoryEntry)
        {
            Id = Guid.NewGuid();
            FirstName = directoryEntry.Properties["givenName"].Value != null ? directoryEntry.Properties["givenName"].Value.ToString() : string.Empty;
            LastName = directoryEntry.Properties["sn"].Value != null ? directoryEntry.Properties["sn"].Value.ToString() : string.Empty;
            MN = directoryEntry.Properties["initials"].Value != null ? directoryEntry.Properties["initials"].Value.ToString() : string.Empty;
            ScreenName = directoryEntry.Properties["displayName"].Value != null ? directoryEntry.Properties["displayName"].Value.ToString() : string.Empty;
            Description = directoryEntry.Properties["description"].Value != null ? directoryEntry.Properties["description"].Value.ToString() : string.Empty;
            Room = directoryEntry.Properties["physicalDeliveryOfficeName"].Value != null ? directoryEntry.Properties["physicalDeliveryOfficeName"].Value.ToString() : string.Empty;
            Street = directoryEntry.Properties["streetAddress"].Value != null ? directoryEntry.Properties["streetAddress"].Value.ToString() : string.Empty;
            PostCode = directoryEntry.Properties["postalCode"].Value != null ? directoryEntry.Properties["postalCode"].Value.ToString() : string.Empty;
            PostBox = directoryEntry.Properties["postOfficeBox"].Value != null ? directoryEntry.Properties["postOfficeBox"].Value.ToString() : string.Empty;
            Town = directoryEntry.Properties["l"].Value != null ? directoryEntry.Properties["l"].Value.ToString() : string.Empty;

            Area = directoryEntry.Properties["st"].Value != null ? directoryEntry.Properties["st"].Value.ToString() : string.Empty;
            Country = directoryEntry.Properties["co"].Value != null ? directoryEntry.Properties["co"].Value.ToString() : string.Empty;
            PhoneNumber = directoryEntry.Properties["telephoneNumber"].Value != null ? directoryEntry.Properties["telephoneNumber"].Value.ToString() : string.Empty;
            Email = directoryEntry.Properties["mail"].Value != null ? directoryEntry.Properties["mail"].Value.ToString() : string.Empty;
            DistinguishedName = directoryEntry.Properties["distinguishedName"].Value != null ? directoryEntry.Properties["distinguishedName"].Value.ToString() : string.Empty;
            UserName = directoryEntry.Properties["sAMAccountName"].Value != null ? directoryEntry.Properties["sAMAccountName"].Value.ToString() : string.Empty;
            var userAccountControl = directoryEntry.Properties["userAccountControl"].Value != null ? directoryEntry.Properties["userAccountControl"].Value.ToString() : string.Empty;
            /*
             * This is a hex values for userAccountControl property
            00000001	SCRIPT
            00000002	ACCOUNTDISABLE
            00000008	HOMEDIR_REQUIRED
            00000010	LOCKOUT
            00000020	PASSWD_NOTREQD
            00000040	PASSWD_CANT_CHANGE
            00000080	ENCRYPTED_TEXT_PWD_ALLOWED
            00000100	TEMP_DUPLICATE_ACCOUNT
            00000200	NORMAL_ACCOUNT
            00000800	INTERDOMAIN_TRUST_ACCOUNT
            00001000	WORKSTATION_TRUST_ACCOUNT
            00002000	SERVER_TRUST_ACCOUNT
            00010000	DONT_EXPIRE_PASSWORD
            00020000	MNS_LOGON_ACCOUNT
            00040000	SMARTCARD_REQUIRED
            00080000	TRUSTED_FOR_DELEGATION
            00100000	NOT_DELEGATED
            00200000	USE_DES_KEY_ONLY
            00400000	DONT_REQ_PREAUTH
            00800000	PASSWORD_EXPIRED
            01000000	TRUSTED_TO_AUTH_FOR_DELEGATION
            04000000	PARTIAL_SECRETS_ACCOUNT
             */
            if (userAccountControl != string.Empty)
            {
                var heXvalue = int.Parse(userAccountControl).ToString("X8");
                IsActive = heXvalue[7] == '2' ? false : true;
                PasswordRequired = heXvalue[6] == '2' ? false : true;
                PasswordChangeable = heXvalue[6] == '4' ? false : true;
                PasswordExpires = heXvalue[3] == '1' ? false : true;
            }
            else
            {
                IsActive = false;
                PasswordChangeable = false;
                PasswordExpires = false;
                PasswordRequired = false;
            }
        }
    }
}
