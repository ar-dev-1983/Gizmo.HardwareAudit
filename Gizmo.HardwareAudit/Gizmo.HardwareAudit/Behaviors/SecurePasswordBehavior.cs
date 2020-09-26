using Microsoft.Xaml.Behaviors;
using System.Security;
using System.Windows;
using System.Windows.Controls;
namespace Gizmo.HardwareAudit.Behaviors
{

    public class SecurePasswordBehavior : Behavior<PasswordBox>
    {
        public static readonly DependencyProperty SecurePasswordProperty = DependencyProperty.Register("SecurePassword", typeof(SecureString), typeof(SecurePasswordBehavior), new PropertyMetadata(default(SecureString)));

        private bool _skipUpdate;

        public SecureString SecurePassword
        {
            get { return (SecureString)GetValue(SecurePasswordProperty); }
            set { SetValue(SecurePasswordProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.PasswordChanged += PasswordBox_PasswordChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PasswordChanged -= PasswordBox_PasswordChanged;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.Property == SecurePasswordProperty)
            {
                if (!_skipUpdate && e.NewValue != null)
                {
                    _skipUpdate = true;
                    AssociatedObject.Password = UserProfile.ToInsecureString(e.NewValue as SecureString);
                    _skipUpdate = false;
                }
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _skipUpdate = true;
            SecurePassword = AssociatedObject.SecurePassword;
            _skipUpdate = false;
        }
    }
}
