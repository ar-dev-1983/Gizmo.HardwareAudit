using Gizmo.HardwareAudit.Enums;
using System;

namespace Gizmo.HardwareAudit
{
    public class LogItem
    {
        public DateTime TimeStamp { set; get; }
        public string Content { set; get; }
        public string Value { set; get; }
        public LogItemTypeEnum Type { set; get; }
        public string Source { set; get; }

        public LogItem()
        {
            TimeStamp = new DateTime();
            Content = string.Empty;
            Value = string.Empty;
            Source = string.Empty;
            Type = LogItemTypeEnum.Information;
        }

        public LogItem(DateTime timeStamp, string content, string value, LogItemTypeEnum type, string source)
        {
            TimeStamp = timeStamp;
            Content = content;
            Value = value;
            Type = type;
            Source = source;
        }
    }

}
