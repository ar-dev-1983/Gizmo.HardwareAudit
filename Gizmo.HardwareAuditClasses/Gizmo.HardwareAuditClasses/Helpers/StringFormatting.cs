using System;
using System.Linq;
using System.Text;
using System.Xml;

namespace Gizmo.HardwareAuditClasses
{
    public class StringFormatting
    {
        public static string UTF8ByteArrayToString(byte[] input) => new UTF8Encoding().GetString(input);

        public static byte[] StringToUTF8ByteArray(string input) => new UTF8Encoding().GetBytes(input);

        public static string ConvertUInt16ArrayToString(ushort[] input)
        {
            char[] charArray = new char[input.Length];
            Array.Copy(input, charArray, input.Length);
            return new string(charArray);
        }

        public static string CleanInvalidXmlChars(string input)
        {
            return string.Join(" ", new string(input.Where(ch => XmlConvert.IsXmlChar(ch)).ToArray()).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).TrimStart().TrimEnd();
        }
    }

}
