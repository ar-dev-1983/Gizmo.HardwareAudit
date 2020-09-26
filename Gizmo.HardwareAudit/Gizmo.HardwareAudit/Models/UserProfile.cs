using Gizmo.HardwareAudit.Interfaces;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace Gizmo.HardwareAudit
{
    public class UserProfile : BaseViewModel
    {
        #region Private Properties
        private Guid id;
        private string profileName;
        private string salt;
        private string userName;
        private string password;
        private SecureString userPassword;
        #endregion

        #region Public Properties
        public Guid Id
        {
            get => id;
            set
            {
                if (id == value) return;
                id = value;
                OnPropertyChanged();
            }
        }
        public string ProfileName
        {
            get => profileName;
            set
            {
                if (profileName == value) return;
                profileName = value;
                OnPropertyChanged();
            }
        }
        [JsonIgnore]
        public string Salt
        {
            get => salt;
            set
            {
                if (salt == value) return;
                salt = value;
                OnPropertyChanged();
            }
        }
        public string UserName
        {
            get => userName;
            set
            {
                if (userName == value) return;
                userName = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => password;
            set
            {
                if (password == value) return;
                password = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public SecureString UserPassword
        {
            get => userPassword;
            set
            {
                if (userPassword == value) return;
                userPassword = value;
                OnPropertyChanged();
            }
        }

        #endregion


        public UserProfile()
        {
            id = Guid.NewGuid();
            profileName = string.Empty;
            userName = string.Empty;
            userPassword = new SecureString();
            salt = GetSaltString();
            password = string.Empty;
        }

        public UserProfile(string salt_value)
        {
            id = Guid.NewGuid();
            profileName = string.Empty;
            userName = string.Empty;
            userPassword = new SecureString();
            salt = salt_value;
            password = string.Empty;
        }

        public static string EncryptString(SecureString input, string salt, DataProtectionScope scope)
        {
            return Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(ToInsecureString(input)), Encoding.Unicode.GetBytes(salt), scope));
        }

        public static SecureString DecryptString(string encryptedData, string salt, DataProtectionScope scope)
        {
            return ToSecureString(Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), Encoding.Unicode.GetBytes(salt), scope)));
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }

        public static string GetSaltString()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string GetSaltString(int length)
        {
            byte[] salt = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string GetHashString(SecureString password, string salt, int iterations, int length)
        {
            //--    Leave it until .net 5 release   --//
            //#if NET471
            //using var deriveBytes = new Rfc2898DeriveBytes(Encoding.Unicode.GetBytes(ToInsecureString(password)), Encoding.Unicode.GetBytes(salt), iterations);
            //#elif NETCOREAPP3_1 
            //using var deriveBytes = new Rfc2898DeriveBytes(Encoding.Unicode.GetBytes(ToInsecureString(password)), Encoding.Unicode.GetBytes(salt), iterations, HashAlgorithmName.SHA512);
            //#endif
            using var deriveBytes = new Rfc2898DeriveBytes(Encoding.Unicode.GetBytes(ToInsecureString(password)), Encoding.Unicode.GetBytes(salt), iterations);
            return Convert.ToBase64String(deriveBytes.GetBytes(length));
        }
    }
}
