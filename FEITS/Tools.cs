using System;
using System.Drawing;
using System.Linq;
using System.Text;

namespace FEITS
{
    static class Tools
    {
        public static ushort GetValue(char c)
        {
            return BitConverter.ToUInt16(Encoding.Unicode.GetBytes(string.Empty + c), 0);
        }

        public static int GetLength(string s, FontCharacter[] chars)
        {
            return s.Select(GetValue).Select(val => Math.Max(chars[val].Width, chars[val].CropWidth)).Sum();
        }

        public static Image DrawString(FontCharacter[] characters, Image BaseImage, string Message, int StartX, int StartY, Color? TC = null)
        {
            Color TextColor = TC.HasValue ? TC.Value : Color.Black;
            int CurX = StartX;
            int CurY = StartY;
            Bitmap NewImage = BaseImage.Clone() as Bitmap;
            using (Graphics g = Graphics.FromImage(NewImage))
            {
                foreach (char c in Message)
                {
                    if (c == '\n')
                    {
                        CurY += 20;
                        CurX = StartX;
                    }
                    else
                    {
                        FontCharacter cur = characters[GetValue(c)];
                        g.DrawImage(cur.GetGlyph(TextColor), new Point(CurX, CurY - cur.CropHeight));
                        CurX += cur.CropWidth;
                    }
                }
            }
            return NewImage;
        }

        public static void SaveLineToRaw(Message msg)
        {
            if(msg.spokenText != string.Empty)
            {
                msg.spokenText = msg.spokenText.Replace('’', '\'').Replace('~', '～').Replace("  ", " ");

                int lineIndex;
                if (msg.rawLine.Contains("$Nu"))
                {
                    lineIndex = msg.rawLine.IndexOf("$", msg.rawLine.IndexOf("$Nu") + 2);
                }
                else
                {
                    lineIndex = msg.rawLine.IndexOf("$", msg.speechIndex);
                }

                string oldDialog = string.Empty;
                if (lineIndex > msg.speechIndex)
                    oldDialog = msg.rawLine.Substring(msg.speechIndex, lineIndex - msg.speechIndex);
                else
                    oldDialog = msg.rawLine.Substring(msg.speechIndex);

                msg.rawLine = msg.rawLine.Replace(oldDialog, msg.spokenText);
            }

            msg.rawLine = msg.rawLine.Replace(Environment.NewLine, "\\n");
        }
    }
}
