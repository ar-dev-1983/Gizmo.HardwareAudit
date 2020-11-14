using Gizmo.HardwareAuditClasses;
using Gizmo.WPF;
using System;
using System.ComponentModel;
using System.IO;
using System.Management;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Drawing;

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
        public delegate void AppViewModelEventHandler(AppViewModel appViewModel);
        public event AppViewModelEventHandler OnScanIsFinished;
        public event AppViewModelEventHandler OnErrorCatch;

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
                    }
                    catch (Exception)
                    {
                    }
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
                        if (SaveFileDialog(HostName + ScanTime.ToShortDateString().Replace(".", "_") + "_" + ScanTime.ToLongTimeString().Replace(":", "_"), "Scan files|*.scan") == true)
                        {
                            ExportScan(filePath);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }, (obj) => Scan != null ? ScanFinished && !IsAnyErrors : false);
            }
        }

        private WorkCommand exportExcelCommand;
        public WorkCommand ExportExcelCommand
        {
            get
            {
                return exportExcelCommand ??= new WorkCommand(obj =>
                {
                    try
                    {
                        var ScanTime = DateTime.Now;
                        if (SaveFileDialog(HostName + ScanTime.ToShortDateString().Replace(".", "_") + "_" + ScanTime.ToLongTimeString().Replace(":", "_"), "Excel files|*.xlsx") == true)
                        {
                        }
                    }
                    catch (Exception)
                    {
                    }
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

        private void RunProbe()
        {
            Probe.DoWork += ProbeScan_DoWork;
            Probe.RunWorkerCompleted += ProbeScan_RunWorkerCompleted;
            Probe.RunWorkerAsync();
        }

        private void ReleaseProbe()
        {
            Probe.DoWork -= ProbeScan_DoWork;
            Probe.RunWorkerCompleted -= ProbeScan_RunWorkerCompleted;
            Probe.CancelAsync();
        }

        private void ProbeScan_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                e.Result = ComputerHardwareScan.ScanUsingWMI(HostName, new ConnectionOptions(), true);
            }
            catch (Exception ex)
            {
                e.Result = ex;
            }
        }

        private void ProbeScan_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null && e.Result is ComputerHardwareScan)
            {
                ReleaseProbe();
                Scan = e.Result as ComputerHardwareScan;
                IsAnyErrors = false;
                ScanFinished = true;
            }
            else if (e.Result != null && e.Result is Exception)
            {
                ReleaseProbe();
                Error = e.Result as Exception;
                IsAnyErrors = true;
                ScanFinished = false;
            }
        }

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

        public void SaveAsPngFile(RenderTargetBitmap rtb)
        {
            try
            {
                var ScanTime = DateTime.Now;
                if (SaveFileDialog(HostName + ScanTime.ToShortDateString().Replace(".", "_") + "_" + ScanTime.ToLongTimeString().Replace(":", "_"), "PNG files|*.png") == true)
                {
                    PngBitmapEncoder png = new PngBitmapEncoder();
                    png.Frames.Add(BitmapFrame.Create(rtb));
                    MemoryStream stream = new MemoryStream();
                    png.Save(stream);
                    Image image = Image.FromStream(stream);
                    image.Save(filePath);
                }
            }
            catch (Exception)
            {
            }
        }
        
        public void SaveAsHtmlFile(string fileContent)
        {
            try
            {
                var ScanTime = DateTime.Now;
                if (SaveFileDialog(HostName + ScanTime.ToShortDateString().Replace(".", "_") + "_" + ScanTime.ToLongTimeString().Replace(":", "_"), "html files|*.html") == true)
                {
                    File.WriteAllText(filePath, fileContent);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
