using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAudit.Services;
using Gizmo.HardwareAudit.ViewModels;
using Gizmo.WPF;
using System;
using System.ComponentModel;
using System.IO;
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
        #region Properties
        private bool baloonShow = true;
        private bool IsRealyClosingApp = true;
        private readonly AppViewModel appvm;
        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private System.Windows.Forms.ContextMenuStrip m_contextMenu;
        private WindowState m_storedWindowState = WindowState.Normal;
        #endregion

        #region NotifyIcon
        private void CreateTrayNotifyIcon()
        {
            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.BalloonTipText = "The app has been minimised. Click the tray icon to show.";
            m_notifyIcon.BalloonTipTitle = "Gizmo Hardware Audit";
            m_notifyIcon.Text = "Gizmo Hardware Audit";
            Stream iconStream = Application.GetResourceStream(new Uri("pack://application:,,,/Gizmo.HardwareAudit;component/Resources/Icons/AppIcon.ico")).Stream;
            m_notifyIcon.Icon = new System.Drawing.Icon(iconStream);
            m_notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            m_contextMenu = new System.Windows.Forms.ContextMenuStrip();
            var restoreMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            restoreMenuItem.Text = "Restore";
            restoreMenuItem.Click += NotifyIcon_DoubleClick;
            m_contextMenu.Items.Add(restoreMenuItem);
            var separatorMenuItem = new System.Windows.Forms.ToolStripSeparator();
            m_contextMenu.Items.Add(separatorMenuItem);

            var exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitMenuItem.Text = "Close";
            exitMenuItem.Click += ExitMenuItem_Click;
            m_contextMenu.Items.Add(exitMenuItem);

            m_notifyIcon.ContextMenuStrip = m_contextMenu;
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            IsRealyClosingApp = true;
            Close();
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = m_storedWindowState;
            if (m_notifyIcon != null)
            {
                m_notifyIcon.Visible = false;
            }
        }
        #endregion

        #region MainWindow Events
        public MainWindow()
        {
            InitializeComponent();
            appvm = new AppViewModel(new DefaultDialogService(), new TreeItemDialogService(), new SerializationService(), new AppSettingsDialogService());
            appvm.OnUnlocked += Appvm_OnUnlocked;
            appvm.OnAppSettingsChanged += Appvm_OnAppSettingsChanged;
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

        private void Appvm_OnUnlocked(AppViewModel appViewModel)
        {
            CreateTrayNotifyIcon();
            IsRealyClosingApp = !appvm.Settings.MinimizeToTray;
        }

        private void Appvm_OnAppSettingsChanged(AppViewModel appViewModel)
        {
            IsRealyClosingApp = !appvm.Settings.MinimizeToTray;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (IsRealyClosingApp)
            {
                appvm.Close();
            }
            else
            {
                if (appvm.IsUnlocked)
                {
                    e.Cancel = true;
                    m_storedWindowState = WindowState;
                    Hide();
                    if (m_notifyIcon != null)
                    {
                        m_notifyIcon.Visible = true;
                        if (baloonShow)
                        {
                            baloonShow = false;
                            m_notifyIcon.ShowBalloonTip(2000);
                        }
                    }
                }
            }
            base.OnClosing(e);
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            IsRealyClosingApp = true;
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
            m_notifyIcon.Dispose();
            m_notifyIcon = null;

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
                            }
                        }
                        else
                        {
                            treeItem.IsSelected = true;
                            //updates menu items for selected tree item in case if there changes occures. Not the best idea, but i has't find another solution
                            appvm.OnNamedPropertyChanged("SelectedTreeItemSharedFoldersMenuList");
                            appvm.OnNamedPropertyChanged("SelectedTreeItemTCPPortsMenuList");
                        }
                    }
                    else if ((sender as UITreeListItem).DataContext is ReportItem)
                    {
                        var reportItem = (sender as UITreeListItem).DataContext as ReportItem;
                        if (appvm.SelectedReportItem != null)
                        {
                            if (appvm.SelectedReportItem.Id != reportItem.Id)
                            {
                                reportItem.IsSelected = true;
                            }
                        }
                        else
                        {
                            reportItem.IsSelected = true;
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
                else
                {
                    appvm.InvokeUnlocked();
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
                else
                {
                    appvm.InvokeUnlocked();
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
                        else
                        {
                            appvm.InvokeUnlocked();
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
                    else
                    {
                        appvm.InvokeUnlocked();
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
                    else
                    {
                        appvm.InvokeUnlocked();
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
        private void Reports_Click(object sender, RoutedEventArgs e)
        {
            if (tpTabPanelLeft != null)
                tpTabPanelLeft.SelectedIndex = 1;
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

        #region Search Event
        private void sbTreeItemsSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                if ((sender as UISearchBox).SelectedItem != null)
                {
                    appvm.NavigateTo(((sender as UISearchBox).SelectedItem as TreeItem).Id);
                }
            }
        }
        #endregion
    }
}