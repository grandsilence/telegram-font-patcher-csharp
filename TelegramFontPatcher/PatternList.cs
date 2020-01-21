using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramFontPatcher
{
    internal class PatternList : List<Tuple<byte[], byte[]>>
    {
        public PatternList(string newFont, IEnumerable<string> oldFonts)
        {
            foreach (string oldFont in oldFonts)
            {
                Add(oldFont, newFont, Encoding.ASCII);
                Add(oldFont, newFont, Encoding.Unicode);
            }
        }

        private void Add(string oldFont, string newFont, Encoding encoding)
        {
            Add(new Tuple<byte[], byte[]>(oldFont.RawBytes(encoding, oldFont.Length), newFont.RawBytes(encoding, oldFont.Length)));
        }
    }
}
