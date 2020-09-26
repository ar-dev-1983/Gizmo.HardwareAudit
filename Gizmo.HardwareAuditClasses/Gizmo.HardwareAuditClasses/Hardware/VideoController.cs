using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Management;

namespace Gizmo.HardwareAuditClasses
{
    public class VideoController
    {
        [Category("VideoController")]
        [Description("Видео адаптер: имя")]
        public string Name { set; get; }

        [Category("VideoController")]
        [Description("Видео адаптер: процессор")]
        public string VideoProcessor { set; get; }

        [Category("VideoController")]
        [Description("Видео адаптер: режим")]
        public string VideoModeDescription { set; get; }

        public VideoController()
        {
            Name = string.Empty;
            VideoProcessor = string.Empty;
            VideoModeDescription = string.Empty;
        }

        public static List<VideoController> Enumerate(ManagementScope Scope)
        {
            var result = new List<VideoController>();
            try
            {
                Scope.Options.Timeout = new TimeSpan(0, 1, 0);
                Scope.Connect();
                ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_VideoController");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);

                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject m in queryCollection)
                {
                    result.Add(new VideoController
                    {
                        Name = m["Name"] != null ? StringFormatting.CleanInvalidXmlChars(m["Name"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        VideoProcessor = m["VideoProcessor"] != null ? StringFormatting.CleanInvalidXmlChars(m["VideoProcessor"].ToString()).TrimStart().TrimEnd() : string.Empty,
                        VideoModeDescription = m["VideoModeDescription"] != null ? StringFormatting.CleanInvalidXmlChars(m["VideoModeDescription"].ToString()).TrimStart().TrimEnd() : string.Empty
                    });
                }
            }
            catch (Exception) { }
            if (result.Count > 0)
            {
                return result;
            }
            else
            {
                return new List<VideoController>();
            }
        }
    }
}
