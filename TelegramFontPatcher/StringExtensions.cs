using System.Text;

namespace TelegramFontPatcher
{
    internal static class StringExtensions
    {
        public static byte[] RawBytes(this string self, Encoding encoding, int length)
        {
            var result = new byte[encoding.GetMaxByteCount(length)];
            encoding.GetBytes(self, 0, self.Length, result, 0);

            return result;
        }
    }
}
