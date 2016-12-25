using FEITS.Properties;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace FEITS.Model
{
    public class ConversationModel
    {
        //File container
        public ParsedFileContainer File { get; } = new ParsedFileContainer();
        private int messageIndex;

        public int MessageIndex
        {
            get { return messageIndex; }
            set
            {
                messageIndex = value;
                lineIndex = 0;
                ResetParameters();
            }
        }

        private int lineIndex;

        public int LineIndex
        {
            get { return lineIndex; }
            set { lineIndex = value; }
        }

        //Settings
        public string PlayerName = "Kamui";

        //TODO(Robin): Why does this exist?
        public PlayerGender PlayerGender = PlayerGender.Male;

        private bool enableBackgrounds;

        public bool EnableBackgrounds
        {
            get { return enableBackgrounds; }
            set
            {
                //Changing the value should set the background image to default
                enableBackgrounds = value;
                BackgroundImage = Resources.SupportBG;
            }
        }

        public Image BackgroundImage;
        public Image[] TextBoxes = {Resources.TextBox, Resources.TextBox_Nohr, Resources.TextBox_Hoshido};
        public int TextboxIndex;

        //Character names with their translations
        private readonly Dictionary<string, string> names;

        //
        private Color colorA = Color.FromArgb(0x5B, 0x58, 0x55);
        private Color colorB = Color.FromArgb(0x5B, 0x58, 0x55);

        //Line commands
        public bool HasPerms { get; private set; }
        public bool SetType { get; private set; }
        public string CharActive { get; private set; } = string.Empty;
        public string CharA { get; private set; } = string.Empty;

        public string CharB { get; private set; } = string.Empty;
        public int CharSide { get; private set; } = -1;
        private const string DefaultEmotion = "通常,";
        public string EmotionA { get; private set; } = DefaultEmotion;
        public string EmotionB { get; private set; } = DefaultEmotion;
        private ConversationTypes ConversationType = ConversationTypes.Type1;

        public ConversationModel()
        {
            //Grab names from PID and sort them into a dictionary
            names = new Dictionary<string, string>();
            var pids = Resources.PID.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in pids)
            {
                var p = str.Split('\t');
                names[p[0]] = p[1];
            }
        }

        public void ResetParameters()
        {
            HasPerms = SetType = false;
            CharActive = CharA = CharB = string.Empty;
            EmotionA = EmotionB = DefaultEmotion;
            CharSide = -1;
            ConversationType = ConversationTypes.Type1;
        }

        public void GetCommandsUpUntilIndex()
        {
            ResetParameters();

            for (var i = 0; i < lineIndex; i++)
            {
                GetParsedCommands(File.MessageList[messageIndex].MessageLines[i]);
            }
        }

        public string GetParsedCommands(MessageLine line)
        {
            if (line.SpokenText != string.Empty)
            {
                line.UpdateRawWithNewDialogue();
                line.RawLine = line.RawLine.Replace("\\n", "\n").Replace("$k\n", "$k\\n");
            }

            line.SpokenText = line.RawLine;

            for (var i = 0; i < line.SpokenText.Length; i++)
            {
                if (line.SpokenText[i] == '$')
                {
                    var res = MessageBlock.ParseCommand(line.SpokenText, i);
                    line.SpokenText = res.Item1;

                    if (res.Item2.numParams > 0)
                        if (res.Item2.Params[0] == "ベロア")
                            res.Item2.Params[0] = "べロア"; // Velour Fix

                    switch (res.Item2.cmd)
                    {
                        case "$E":
                            if (CharActive != string.Empty && CharActive == CharB)
                            {
                                if (res.Item2.Params[0] != ",")
                                    EmotionB = res.Item2.Params[0];
                                else
                                    EmotionB = DefaultEmotion;
                            }
                            else
                            {
                                if (res.Item2.Params[0] != ",")
                                    EmotionA = res.Item2.Params[0];
                                else
                                    EmotionA = DefaultEmotion;
                            }
                            break;
                        case "$Ws":
                            CharActive = res.Item2.Params[0];
                            break;
                        case "$Wm":
                            CharSide = Convert.ToInt32(res.Item2.Params[1]);

                            if (ConversationType == ConversationTypes.Type1)
                            {
                                if (CharSide == 3)
                                {
                                    CharA = res.Item2.Params[0];
                                    EmotionA = DefaultEmotion;
                                }
                                else if (CharSide == 7)
                                {
                                    CharB = res.Item2.Params[0];
                                    EmotionB = DefaultEmotion;
                                }
                            }
                            else
                            {
                                if (CharSide == 0 | CharSide == 2)
                                {
                                    CharA = res.Item2.Params[0];
                                    EmotionA = DefaultEmotion;
                                }
                                else if (CharSide == 6)
                                {
                                    CharB = res.Item2.Params[0];
                                    EmotionB = DefaultEmotion;
                                }
                            }
                            break;
                        case "$Wd":
                            if (CharActive == CharB)
                            {
                                CharActive = CharA;
                                CharB = string.Empty;
                            }
                            else
                            {
                                CharA = string.Empty;
                            }
                            break;
                        case "$a":
                            HasPerms = true;
                            break;
                        case "$t0":
                            if (!SetType)
                                ConversationType = ConversationTypes.Type0;
                            SetType = true;
                            break;
                        case "$t1":
                            if (!SetType)
                                ConversationType = ConversationTypes.Type1;
                            SetType = true;
                            break;
                        case "$Nu":
                            line.SpokenText = line.SpokenText.Substring(0, i) + "$Nu" + line.SpokenText.Substring(i);
                            i += 2;
                            break;
                        case "$Wa":
                            break;
                        case "$Wc":
                            break;
                        default:
                            break;
                    }
                    i--;
                }
            }

            if (string.IsNullOrWhiteSpace(line.SpokenText))
            {
                line.SpokenText = string.Empty;
            }

            line.SpeechIndex = line.RawLine.LastIndexOf(line.SpokenText);
            line.RawLine = line.RawLine.Replace("\\n", "\n").Replace("$k\n", "$k\\n");
            line.SpokenText.Replace("\\n", "\n").Replace("$k\n", "$k\\n");
            line.SpokenText = line.SpokenText.Replace("\n", Environment.NewLine);

            return line.SpokenText;
        }

        #region BoxRender

        public Image RenderPreviewBox(string line)
        {
            return ConversationType == ConversationTypes.Type1 ? RenderTypeOne(line) : RenderTypeZero();
        }

        private Image RenderTypeOne(string line)
        {
            //Probably shouldn't be hard-coded
            var box = new Bitmap(400, 240);
            var tb = TextBoxes[TextboxIndex].Clone() as Bitmap;

            //Generate text image from string
            if (line.Contains("$Nu") && HasPerms)
            {
                line = line.Replace("$Nu", PlayerName);
            }

            line = line.Replace(Environment.NewLine, "\n");

            //Draw the line's text
            var text = AssetGeneration.DrawString(new Bitmap(310, 50), line, 0, 22, Color.FromArgb(68, 8, 0)) as Bitmap;

            using (var g = Graphics.FromImage(tb))
            {
                g.DrawImage(text, new Point(29, 0));
            }

            //Name box
            var name = names.ContainsKey(CharActive)
                ? names[CharActive]
                : (CharActive == "username" ? PlayerName : CharActive);
            var nameLength = AssetGeneration.GetLength(name);
            var nb =
                AssetGeneration.DrawString(Resources.NameBox, name, Resources.NameBox.Width/2 - nameLength/2, 16,
                    Color.FromArgb(253, 234, 177)) as Bitmap; //Center name in NameBox

            using (var g = Graphics.FromImage(box))
            {
                if (enableBackgrounds)
                {
                    g.DrawImage(BackgroundImage, new Point(0, 0));
                }

                if (CharA != string.Empty)
                {
                    var ca = AssetGeneration.GetCharacterStageImage(CharA, EmotionA, colorA, true, PlayerGender);
                    g.DrawImage((CharActive == CharA) ? ca : AssetGeneration.Fade(ca),
                        new Point(-28, box.Height - ca.Height + 14));
                }

                if (CharB != string.Empty)
                {
                    var cb = AssetGeneration.GetCharacterStageImage(CharB, EmotionB, colorB, false, PlayerGender);
                    g.DrawImage((CharActive == CharB) ? cb : AssetGeneration.Fade(cb),
                        new Point(box.Width - cb.Width + 28, box.Height - cb.Height + 14));
                }

                g.DrawImage(tb, new Point(10, box.Height - tb.Height + 2));

                if (CharActive != string.Empty)
                {
                    g.DrawImage(nb,
                        CharActive == CharB
                            ? new Point(box.Width - nb.Width - 6, box.Height - tb.Height - 14)
                            : new Point(7, box.Height - tb.Height - 14));
                }

                if (lineIndex > File.MessageList[messageIndex].MessageLines.Count - 1)
                {
                    g.DrawImage(Resources.KeyPress, new Point(box.Width - 33, box.Height - tb.Height + 32));
                }
            }

            return box;
        }

        private Image RenderTypeZero()
        {
            string topLine = string.Empty, bottomLine = string.Empty;
            ResetParameters();

            for (var i = 0; i <= lineIndex; i++)
            {
                var line = GetParsedCommands(File.MessageList[messageIndex].MessageLines[i]);

                if (line.Contains("$Nu") && HasPerms)
                {
                    line = line.Replace("$Nu", PlayerName);
                }

                line = line.Replace(Environment.NewLine, "\n");

                if (CharActive == CharA)
                    topLine = line;
                else
                    bottomLine = line;
            }

            //Hard coded dimensions
            var box = new Bitmap(400, 240);
            Bitmap topBox = new Bitmap(1, 1), bottomBox = new Bitmap(1, 1);
            if (topLine != string.Empty && CharA != string.Empty)
            {
                topBox = (TextBoxes[TextboxIndex].Clone()) as Bitmap;
                using (var g = Graphics.FromImage(topBox))
                {
                    g.DrawImage(AssetGeneration.GetCharacterBuImage(CharA, EmotionA, colorA, true, PlayerGender),
                        new Point(2, 3));
                    g.DrawImage(
                        AssetGeneration.DrawString(new Bitmap(260, 50), topLine, 0, 22, Color.FromArgb(68, 8, 0)),
                        new Point(76, 0));
                }
            }

            if (bottomLine != string.Empty && CharB != string.Empty)
            {
                bottomBox = (TextBoxes[TextboxIndex].Clone()) as Bitmap;
                using (var g = Graphics.FromImage(bottomBox))
                {
                    g.DrawImage(AssetGeneration.GetCharacterBuImage(CharB, EmotionB, colorB, true, PlayerGender),
                        new Point(2, 3));
                    g.DrawImage(
                        AssetGeneration.DrawString(new Bitmap(282, 50), bottomLine, 0, 22, Color.FromArgb(68, 8, 0)),
                        new Point(76, 0));
                }
            }

            using (var g = Graphics.FromImage(box))
            {
                if (lineIndex < File.MessageList[messageIndex].MessageLines.Count - 1)
                {
                    using (var g2 = Graphics.FromImage(CharActive == CharA ? topBox : bottomBox))
                    {
                        g2.DrawImage(Resources.KeyPress, new Point(TextBoxes[TextboxIndex].Width - 30, 32));
                    }
                }

                if (enableBackgrounds)
                    g.DrawImage(BackgroundImage, new Point(0, 0));
                g.DrawImage(topBox, new Point(10, 3));
                g.DrawImage(bottomBox, new Point(10, box.Height - bottomBox.Height + 2));

                if (topLine != string.Empty && CharA != string.Empty)
                {
                    var topName = names.ContainsKey(CharA) ? names[CharA] : (CharA == "username" ? PlayerName : CharA);
                    var nameLen = AssetGeneration.GetLength(topName);
                    var topNameBox =
                        AssetGeneration.DrawString(Resources.NameBox, topName, Resources.NameBox.Width/2 - nameLen/2, 16,
                            Color.FromArgb(253, 234, 177)) as Bitmap; //Center name in NameBox
                    g.DrawImage(topNameBox, new Point(7, topBox.Height - (topNameBox.Height - 20)));
                }

                if (bottomLine != string.Empty && CharB != string.Empty)
                {
                    var bottomName = names.ContainsKey(CharB)
                        ? names[CharB]
                        : (CharB == "username" ? PlayerName : CharB);
                    var nameLen = AssetGeneration.GetLength(bottomName);
                    var bottomNameBox =
                        AssetGeneration.DrawString(Resources.NameBox, bottomName, Resources.NameBox.Width/2 - nameLen/2,
                            16, Color.FromArgb(253, 234, 177)) as Bitmap;
                    g.DrawImage(bottomNameBox, new Point(7, box.Height - bottomBox.Height - 14));
                }
            }
            return box;
        }

        #endregion

        public Image RenderConversation()
        {
            foreach (var line in File.MessageList[messageIndex].MessageLines)
                line.UpdateRawWithNewDialogue();

            var images = new List<Image>();
            var index = lineIndex;
            ResetParameters();
            for (var i = 0; i < File.MessageList[messageIndex].MessageLines.Count; i++)
            {
                GetParsedCommands(File.MessageList[messageIndex].MessageLines[i]);
                var parsed = File.MessageList[messageIndex].MessageLines[i].SpokenText;
                if (!string.IsNullOrWhiteSpace(parsed) && !string.IsNullOrEmpty(parsed))
                {
                    images.Add(RenderPreviewBox(parsed));
                }
            }
            lineIndex = index;
            var bmp = new Bitmap(images.Max(i => i.Width), images.Sum(i => i.Height));
            using (var g = Graphics.FromImage(bmp))
            {
                var h = 0;
                foreach (var img in images)
                {
                    g.DrawImage(img, new Point(0, h));
                    h += img.Height;
                }
            }

            return bmp;
        }

        public PlayerGender GetPlayerGenderFromMessageList()
        {
            return File.MessageList[MessageIndex].Prefix.Contains("PCF") ? PlayerGender.Female : PlayerGender.Male;
        }
    }
}