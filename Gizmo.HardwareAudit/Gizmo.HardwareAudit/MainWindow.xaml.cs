using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.Services;
using Gizmo.HardwareAudit.ViewModels;
using Gizmo.WPF;
using System;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;

namespace Gizmo.HardwareAudit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IDisposable
    {
        #region Variables

        private readonly AppViewModel appvm;

        #endregion

        #region MainWindow Events

        public MainWindow()
        {
            InitializeComponent();
            appvm = new AppViewModel(new DefaultDialogService(), new TreeItemDialogService(), new SerializationService(), new AppSettingsDialogService());
            DataContext = appvm;
            ThemeManager.ApplyThemeToWindow(this, appvm.Settings.Theme);
            if (appvm.IsFirstRun)
            {
                tbSetUnlockPassword.Focus();
            }
            else if (!appvm.IsUnlocked)
            {
                tbUnlockPassword.Focus();
            } 
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            appvm.Close();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (appvm != null)
                    appvm.Dispose();
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region TreeList Sort Events
        private void SortSelectedContainerByName_Click(object sender, RoutedEventArgs e)
        {
            if (appvm.SelectedTreeItem != null)
                if (appvm.SelectedTreeItem.Children.Count != 0)
                    appvm.SelectedTreeItem.SortChildrenByName();
        }

        private void SortSelectedContainerByDescription_Click(object sender, RoutedEventArgs e)
        {
            if (appvm.SelectedTreeItem != null)
                if (appvm.SelectedTreeItem.Children.Count != 0)
                    appvm.SelectedTreeItem.SortChildrenByDescription();
        }


        private void SortSelectedContainerByStatus_Click(object sender, RoutedEventArgs e)
        {
            if (appvm.SelectedTreeItem != null)
                if (appvm.SelectedTreeItem.Children.Count != 0)
                    appvm.SelectedTreeItem.SortChildrenByStatus();
        }


        private void SortSelectedContainerByLastScanDateTime_Click(object sender, RoutedEventArgs e)
        {
            if (appvm.SelectedTreeItem != null)
                if (appvm.SelectedTreeItem.Children.Count != 0)
                    appvm.SelectedTreeItem.SortChildrenByLastScanDateTime();
        }


        private void SortSelectedContainerByPreviousScanDateTime_Click(object sender, RoutedEventArgs e)
        {
            if (appvm.SelectedTreeItem != null)
                if (appvm.SelectedTreeItem.Children.Count != 0)
                    appvm.SelectedTreeItem.SortChildrenByPreviousScanDateTime();
        }
        #endregion

        #region TreeList Events
        void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                if ((sender as UITreeListItem).DataContext != null)
                {
                    if ((sender as UITreeListItem).DataContext is TreeItem)
                    {
                        var treeItem = (sender as UITreeListItem).DataContext as TreeItem;
                        if (appvm.SelectedTreeItem != null)
                        {
                            if (appvm.SelectedTreeItem.Id != treeItem.Id)
                            {
                                treeItem.IsSelected = true;
                                //updates menu items for selected tree item in case if there changes occures. Not the best idea, but i has't find another solution
                                appvm.OnNamedPropertyChanged("SelectedTreeItemSharedFoldersMenuList");
                                appvm.OnNamedPropertyChanged("SelectedTreeItemTCPPortsMenuList");
                                appvm.OnNamedPropertyChanged("SelectedTreeItemMoveToContainerMenuList");
                            }
                        }
                        else
                        {
                            treeItem.IsSelected = true;
                            //updates menu items for selected tree item in case if there changes occures. Not the best idea, but i has't find another solution
                            appvm.OnNamedPropertyChanged("SelectedTreeItemSharedFoldersMenuList");
                            appvm.OnNamedPropertyChanged("SelectedTreeItemTCPPortsMenuList");
                            appvm.OnNamedPropertyChanged("SelectedTreeItemMoveToContainerMenuList");
                        }
                    }
                }
            }
        }
        #endregion

        #region Unlock Events
        private void BtnUnlock_Click(object sender, RoutedEventArgs e)
        {
            if (appvm != null)
            {
                appvm.Unlock(tbUnlockPassword.SecurePassword);
                if (!appvm.IsUnlocked)
                {
                    tbUnlockTip.Foreground = tbUnlock.Foreground;
                    tbUnlockTip.Text = "* incorrect master password, try again";
                }
            }
        }

        private void BtnSetUnlock_Click(object sender, RoutedEventArgs e)
        {
            if (appvm != null)
            {
                appvm.FirstRun(tbSetUnlockPassword.SecurePassword, swScope.IsChecked ? DataProtectionScope.CurrentUser : DataProtectionScope.LocalMachine);
                if (!appvm.IsUnlocked)
                {
                    tbSetUnlockTip.Foreground = tbSetUnlock.Foreground;
                    tbSetUnlockTip.Text = "* master password is empty";
                }
            }
        }

        private void TbUnlockPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (appvm != null)
                {
                    var result = appvm.Unlock(tbUnlockPassword.SecurePassword);
                    if (result)
                    {
                        if (!appvm.IsUnlocked)
                        {
                            tbUnlockTip.Foreground = tbUnlock.Foreground;
                            tbUnlockTip.Text = "* incorrect master password, try again";
                        }
                    }
                    else
                    {
                        Close();
                    }
                }
            }
        }

        private void TbSetUnlockPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (appvm != null)
                {
                    appvm.FirstRun(tbSetUnlockPassword.SecurePassword, swScope.IsChecked ? DataProtectionScope.CurrentUser : DataProtectionScope.LocalMachine);
                    if (!appvm.IsUnlocked)
                    {
                        tbSetUnlockTip.Foreground = tbSetUnlock.Foreground;
                        tbSetUnlockTip.Text = "* master password is empty";
                    }
                }
            }
        }

        private void BtnChangeUnlock_Click(object sender, RoutedEventArgs e)
        {
            if (appvm != null)
            {
                tbNewUnlockPassword.Clear();
                tbNewUnlockPassword.Visibility = Visibility.Visible;
                spUnlock.Visibility = Visibility.Collapsed;
                spChangeUnlock.Visibility = Visibility.Visible;
                swNewScope.Visibility = Visibility.Visible;
            }
        }

        private void BtnSaveNewUnlock_Click(object sender, RoutedEventArgs e)
        {
            if (appvm != null)
            {
                if (tbNewUnlockPassword.SecurePassword.Length == 0)
                {
                    tbUnlockTip.Foreground = tbSetUnlock.Foreground;
                    tbUnlockTip.Text = "* new master password is empty";
                }
                var result = appvm.ChangeUnlock(tbUnlockPassword.SecurePassword, tbNewUnlockPassword.SecurePassword, swNewScope.IsChecked ? DataProtectionScope.CurrentUser : DataProtectionScope.LocalMachine);
                if (result)
                {
                    if (!appvm.IsUnlocked)
                    {
                        tbUnlockTip.Foreground = tbUnlock.Foreground;
                        tbUnlockTip.Text = "* incorrect old master password or new master password, try again";
                    }
                }
                else
                {
                    Close();
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (appvm != null)
            {
                tbNewUnlockPassword.Clear();
                tbNewUnlockPassword.Visibility = Visibility.Collapsed;
                spUnlock.Visibility = Visibility.Visible;
                spChangeUnlock.Visibility = Visibility.Collapsed;
                swNewScope.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region Theme Changing Events
       
        private void MiBlueDark_Click(object sender, RoutedEventArgs e)
        {
            appvm.Settings.Theme = UIThemeEnum.BlueDark;
            ThemeManager.ApplyThemeToWindow(this, appvm.Settings.Theme);
        }
        private void MiBlueLight_Click(object sender, RoutedEventArgs e)
        {
            appvm.Settings.Theme = UIThemeEnum.BlueLight;
            ThemeManager.ApplyThemeToWindow(this, appvm.Settings.Theme);
        }
        private void MiPurpleDark_Click(object sender, RoutedEventArgs e)
        {
            appvm.Settings.Theme = UIThemeEnum.PurpleDark;
            ThemeManager.ApplyThemeToWindow(this, appvm.Settings.Theme);
        }
        private void MiPurpleLight_Click(object sender, RoutedEventArgs e)
        {
            appvm.Settings.Theme = UIThemeEnum.PurpleLight;
            ThemeManager.ApplyThemeToWindow(this, appvm.Settings.Theme);
        }

        #endregion

        #region View changing Events
        private void Project_Click(object sender, RoutedEventArgs e)
        {
            if (tpTabPanelLeft != null)
                tpTabPanelLeft.SelectedIndex = 0;
        }

        private void Log_Click(object sender, RoutedEventArgs e)
        {
            if (tpTabPanelLeft != null)
                tpTabPanelLeft.SelectedIndex = 2;
        }
        #endregion

        #region About
        private void About_Click(object sender, RoutedEventArgs e)
        {
            new AboutWindow(appvm.Settings).ShowDialog();
        } 
        #endregion
    }
}