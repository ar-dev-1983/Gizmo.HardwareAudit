using System.Security;
using System.Windows;
namespace Gizmo.HardwareAudit.Behaviors
{
    public static class SecurePasswordAttachedProperties
    {
        public static SecureString GetEncryptedPassword(DependencyObject obj)
        {
            return (SecureString)obj.GetValue(EncryptedPasswordProperty);
        }

        public static void SetEncryptedPassword(DependencyObject obj, SecureString value)
        {
            obj.SetValue(EncryptedPasswordProperty, value);
        }
        public static readonly DependencyProperty EncryptedPasswordProperty = DependencyProperty.RegisterAttached("EncryptedPassword", typeof(SecureString), typeof(SecurePasswordAttachedProperties));
    }
}
