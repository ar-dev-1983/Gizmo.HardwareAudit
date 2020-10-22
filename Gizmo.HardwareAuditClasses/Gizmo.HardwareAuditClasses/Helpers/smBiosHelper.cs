namespace Gizmo.HardwareAuditClasses.Helpers
{
    public class SmBiosHelper
    {
        private readonly byte type;
        private readonly ushort handle;

        private readonly byte[] data;
        private readonly string[] strings;

        protected int ReturnByte(int offset)
        {
            if (offset < data.Length && offset >= 0)
                return data[offset];
            else
                return 0;
        }

        protected int ReturnWord(int offset)
        {
            if (offset + 1 < data.Length && offset >= 0)
                return (data[offset + 1] << 8) | data[offset];
            else
                return 0;
        }

        protected string ReturnString(int offset)
        {
            if (offset < data.Length && data[offset] > 0 &&
             data[offset] <= strings.Length)
                return strings[data[offset] - 1];
            else
                return "";
        }

        public SmBiosHelper(byte type, ushort handle, byte[] data, string[] strings)
        {
            this.type = type;
            this.handle = handle;
            this.data = data;
            this.strings = strings;
        }

        public string GetString(int offset)
        {
            return ReturnString(offset);
        }

        public int GetWord(int offset)
        {
            return ReturnWord(offset);
        }

        public int GetByte(int offset)
        {
            return ReturnByte(offset);
        }

        public byte Type { get { return type; } }
        public ushort Handle { get { return handle; } }


    }
}
