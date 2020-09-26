using Gizmo.HardwareAudit.Interfaces;
using Gizmo.HardwareAudit.Models;
using Gizmo.HardwareAuditClasses;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace Gizmo.HardwareAudit.Services
{
    public class SerializationService : ISerialization
    {
        public ObservableCollection<LogItem> OpenLog(string filename)
        {
            return JsonSerializer.Deserialize<ObservableCollection<LogItem>>(File.ReadAllText(filename));
        }

        public void SaveLog(string filename, ObservableCollection<LogItem> list)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize<ObservableCollection<LogItem>>(list, options);
            File.WriteAllText(filename, jsonString);
        }

        public TreeItem OpenModel(string filename)
        {
            return JsonSerializer.Deserialize<TreeItem>(File.ReadAllText(filename));
        }

        public void SaveModel(string filename, TreeItem item)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize<TreeItem>(item, options);
            File.WriteAllText(filename, jsonString);
        }

        public void SaveSettings(string filename, AppSettings settings)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize<AppSettings>(settings, options);
            File.WriteAllText(filename, jsonString);
        }

        public AppSettings OpenSettings(string filename)
        {
            return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(filename));
        }

        public void ExportScan(string filename, ComputerHardwareScan scan)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            var jsonString = JsonSerializer.Serialize<ComputerHardwareScan>(scan, options);
            File.WriteAllText(filename, jsonString);
        }

        public ComputerHardwareScan ImportScan(string filename)
        {
            return JsonSerializer.Deserialize<ComputerHardwareScan>(File.ReadAllText(filename));
        }
    }
}
