using Gizmo.HardwareAuditClasses;
using Gizmo.WPF;
using System;
using System.ComponentModel;
using System.IO;
using System.Management;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Collections.Generic;

#if NET45
using Newtonsoft.Json;
#else
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
#endif

namespace Gizmo.HardwareScan
{
    public class AppViewModel : UIBaseViewModel
    {
        #region Event Handlers
        public delegate void AppViewModelEventHandler(AppViewModel appViewModel);
        public event AppViewModelEventHandler OnScanIsFinished;
        public event AppViewModelEventHandler OnErrorCatch;
        #endregion

        #region Private Properties
        private bool scanFinished = false;
        private bool isAnyErrors = false;
        private Exception error;

        private ComputerHardwareScan scan = new ComputerHardwareScan();
        private string hostName = string.Empty;
        private BackgroundWorker probe;
        private string filePath = string.Empty;
        #endregion

        #region Public Properties
        public bool ScanFinished
        {
            get => scanFinished;
            set
            {
                if (scanFinished == value) return;
                scanFinished = value;
                if (value)
                    OnScanIsFinished?.Invoke(this);
                OnPropertyChanged();
            }
        }

        public bool IsAnyErrors
        {
            get => isAnyErrors;
            set
            {
                if (isAnyErrors == value) return;
                isAnyErrors = value;
                if (value)
                    OnErrorCatch?.Invoke(this);
                OnPropertyChanged();
            }
        }

        public Exception Error
        {
            get => error;
            set
            {
                if (error == value) return;
                error = value;
                OnPropertyChanged();
            }
        }

        public ComputerHardwareScan Scan
        {
            get => scan;
            set
            {
                if (scan == value) return;
                scan = value;
                OnPropertyChanged();
            }
        }

        public string HostName
        {
            get => hostName;
            set
            {
                if (hostName == value) return;
                hostName = value;
                OnPropertyChanged();
            }
        }
        public BackgroundWorker Probe
        {
            get => probe;
            set
            {
                if (probe == value) return;
                probe = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        private WorkCommand executeScanCommand;
        public WorkCommand ExecuteScanCommand
        {
            get
            {
                return executeScanCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        RunProbe(true);
                    }
                    catch (Exception) { }
                }, (obj) => Scan != null ? ScanFinished && !IsAnyErrors : false);
            }
        }

        private WorkCommand exportScanCommand;
        public WorkCommand ExportScanCommand
        {
            get
            {
                return exportScanCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        var ScanTime = DateTime.Now;
                        if (SaveFileDialog(HostName.ToLower() + "_" + ScanTime.ToShortDateString().Replace(".", "_") + "_" + ScanTime.ToLongTimeString().Replace(":", "_"), "Scan files|*.scan") == true)
                        {
                            ExportScan(filePath);
                        }
                    }
                    catch (Exception) { }
                }, (obj) => Scan != null ? ScanFinished && !IsAnyErrors : false);
            }
        }
        #endregion

        public AppViewModel(string hostname)
        {
            HostName = hostname;
            Probe = new BackgroundWorker() { WorkerSupportsCancellation = true };
            RunProbe();
        }

        #region Probe Methods
        private void RunProbe(bool refresh = false)
        {
            Probe.DoWork += ProbeScan_DoWork;
            Probe.RunWorkerCompleted += ProbeScan_RunWorkerCompleted;
            if (refresh)
                Probe.RunWorkerAsync(true);
            else
                Probe.RunWorkerAsync(false);
        }

        private void ReleaseProbe()
        {
            Probe.DoWork -= ProbeScan_DoWork;
            Probe.RunWorkerCompleted -= ProbeScan_RunWorkerCompleted;
            Probe.CancelAsync();
        }

        private void ProbeScan_DoWork(object sender, DoWorkEventArgs e)
        {
            var result = new List<object>() { (bool)e.Argument, null };
            try
            {
                result[1] = ComputerHardwareScan.ScanUsingWMI(HostName, new ConnectionOptions(), true);
            }
            catch (Exception ex)
            {
                result[1] = ex;
            }
            e.Result = result;
        }

        private void ProbeScan_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                if (e.Result is List<object>)
                {
                    if ((e.Result as List<object>)[1] != null)
                    {
                        if ((e.Result as List<object>)[1] is ComputerHardwareScan)
                        {
                            ReleaseProbe();
                            Scan = (e.Result as List<object>)[1] as ComputerHardwareScan;
                            if (!(bool)(e.Result as List<object>)[0])
                            {
                                IsAnyErrors = false;
                                ScanFinished = true;
                            }
                        }
                        else if ((e.Result as List<object>)[1] is Exception)
                        {
                            ReleaseProbe();
                            if (!(bool)(e.Result as List<object>)[0])
                            {
                                Error = (e.Result as List<object>)[1] as Exception;
                                IsAnyErrors = true;
                                ScanFinished = false;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Working with files Methods
        public void ExportScan(string filename)
        {
#if NET45
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                using (StreamWriter sw = new StreamWriter(filename))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, Scan);
                }
#else
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
            var jsonString = JsonSerializer.Serialize<ComputerHardwareScan>(Scan, options);
            File.WriteAllText(filename, jsonString);
#endif
        }

        public bool SaveFileDialog(string file, string type)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { FileName = file, Filter = type };
            switch (saveFileDialog.ShowDialog())
            {
                case true:
                    filePath = saveFileDialog.FileName;
                    return true;
                default:
                    return false;
            }
        }

        public void SaveAsPngFile(PngBitmapEncoder rtb)
        {
            try
            {
                var ScanTime = DateTime.Now;
                if (SaveFileDialog(HostName.ToLower() + "_" + ScanTime.ToShortDateString().Replace(".", "_") + "_" + ScanTime.ToLongTimeString().Replace(":", "_"), "PNG files|*.png") == true)
                {
                    MemoryStream stream = new MemoryStream();
                    rtb.Save(stream);
                    Image image = Image.FromStream(stream);
                    image.Save(filePath);
                }
            }
            catch (Exception) { }
        }

        public void SaveAsHtmlFile(string fileContent)
        {
            try
            {
                var ScanTime = DateTime.Now;
                if (SaveFileDialog(HostName.ToLower() + "_" + ScanTime.ToShortDateString().Replace(".", "_") + "_" + ScanTime.ToLongTimeString().Replace(":", "_"), "html files|*.html") == true)
                {
                    File.WriteAllText(filePath, fileContent);
                }
            }
            catch (Exception) { }
        }
        #endregion    
    }
}
