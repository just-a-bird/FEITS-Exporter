using FEITS.Properties;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
                LineIndex = 0;
                ResetParameters();
            }
        }

        public int LineIndex { get; set; }

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
        private Image[] TextBoxes { get; } = {Resources.TextBox, Resources.TextBox_Nohr, Resources.TextBox_Hoshido};
        public int TextboxIndex { get; set; }

        //Character names with their translations
        private readonly IReadOnlyDictionary<string, string> Names;

        //
        private Color ColorA { get; } = Color.FromArgb(0x5B, 0x58, 0x55);
        private Color ColorB { get; } = Color.FromArgb(0x5B, 0x58, 0x55);

        //Line commands
        private bool HasPerms { get; set; }
        private bool SetType { get; set; }
        private string CharActive { get; set; } = string.Empty;
        private string CharA { get; set; } = string.Empty;

        private string CharB { get; set; } = string.Empty;
        private int CharSide { get; set; } = -1;
        private const string DefaultEmotion = "通常,";
        private string EmotionA { get; set; } = DefaultEmotion;
        private string EmotionB { get; set; } = DefaultEmotion;
        private ConversationTypes ConversationType = ConversationTypes.Type1;

        public ConversationModel()
        {
            //Grab names from PID and sort them into a dictionary
            var names = new Dictionary<string, string>();
            var pids = Resources.PID.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in pids)
            {
                var p = str.Split('\t');
                names[p[0]] = p[1];
            }
            Names = new ReadOnlyDictionary<string, string>(names);
        }

        private void ResetParameters()
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

            for (var i = 0; i < LineIndex; i++)
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
                if (line.SpokenText[i] != '$') continue;

                var res = MessageBlock.ParseCommand(line.SpokenText, i);
                line.SpokenText = res.Item1;

                if (res.Item2.numParams > 0)
                    if (res.Item2.Params[0] == "ベロア")
                        res.Item2.Params[0] = "べロア"; // Velour Fix

                switch (res.Item2.cmd)
                {
                    case "$E":
                        if (string.IsNullOrEmpty(CharActive) || CharActive != CharB)
                            EmotionA = res.Item2.Params[0] != "," ? res.Item2.Params[0] : DefaultEmotion;
                        else
                            EmotionB = res.Item2.Params[0] != "," ? res.Item2.Params[0] : DefaultEmotion;
                        break;
                    case "$Ws":
                        CharActive = res.Item2.Params[0];
                        break;
                    case "$Wm":
                        CharSide = Convert.ToInt32(res.Item2.Params[1]);

                        //NOTE(Robin): Prepare an exception for multiple possible fail states below
                        var unexpectedCharSideException = new ArgumentOutOfRangeException(nameof(CharSide), CharSide,
                            "Unexpected character side parameter.");

                        switch (ConversationType)
                        {
                            case ConversationTypes.Type0:
                            {
                                switch (CharSide)
                                {
                                    case 0:
                                    case 2:
                                        CharA = res.Item2.Params[0];
                                        EmotionA = DefaultEmotion;
                                        break;
                                    case 6:
                                        CharB = res.Item2.Params[0];
                                        EmotionB = DefaultEmotion;
                                        break;
                                    default:
                                        throw unexpectedCharSideException;
                                }
                                break;
                            }
                            case ConversationTypes.Type1:
                            {
                                switch (CharSide)
                                {
                                    case 3:
                                        CharA = res.Item2.Params[0];
                                        EmotionA = DefaultEmotion;
                                        break;
                                    case 7:
                                        CharB = res.Item2.Params[0];
                                        EmotionB = DefaultEmotion;
                                        break;
                                    default:
                                        throw unexpectedCharSideException;
                                }
                                break;
                            }
                            default:
                                throw new ArgumentOutOfRangeException(nameof(ConversationType), ConversationType,
                                    "Unexpected conversation type.");
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
                            Debug.Assert(CharActive == CharA, $"{nameof(CharActive)} == {nameof(CharA)}");
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
                        line.SpokenText = $"{line.SpokenText.Substring(0, i)}$Nu{line.SpokenText.Substring(i)}";
                        i += 2;
                        break;
                    case "$Wa":
                    case "$Wc":
                    case "$w":
                    case "$k":
                    case "$p":
                    case "$k\\n":
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(res.Item2.cmd), res.Item2.cmd,
                            "Unexpected command.");
                }
                i--;
            }

            if (string.IsNullOrWhiteSpace(line.SpokenText))
            {
                line.SpokenText = string.Empty;
            }

            line.SpeechIndex = line.RawLine.LastIndexOf(line.SpokenText, StringComparison.Ordinal);

            Func<string, string> purify = s => s.Replace("\\n", "\n")
                                                .Replace("$k\n", "$k\\n")
                                                .Replace("\n", Environment.NewLine);

            line.RawLine = purify(line.RawLine);
            line.SpokenText = purify(line.SpokenText);

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
            var tb = (Bitmap) TextBoxes[TextboxIndex].Clone();

            //Generate text image from string
            if (line.Contains("$Nu") && HasPerms)
            {
                line = line.Replace("$Nu", PlayerName);
            }

            line = line.Replace(Environment.NewLine, "\n");

            //Draw the line's text
            var text = (Bitmap) AssetGeneration.DrawString(new Bitmap(310, 50), line, 0, 22, Color.FromArgb(68, 8, 0));

            using (var g = Graphics.FromImage(tb))
            {
                g.DrawImage(text, new Point(29, 0));
            }

            //Name box
            var name = Names.ContainsKey(CharActive)
                ? Names[CharActive]
                : (CharActive == "username" ? PlayerName : CharActive);
            var nameLength = AssetGeneration.GetLength(name);
            var nb =
                (Bitmap)
                    AssetGeneration.DrawString(Resources.NameBox, name, Resources.NameBox.Width/2 - nameLength/2, 16,
                        Color.FromArgb(253, 234, 177)); //Center name in NameBox

            using (var g = Graphics.FromImage(box))
            {
                if (enableBackgrounds)
                {
                    g.DrawImage(BackgroundImage, new Point(0, 0));
                }

                if (CharA != string.Empty)
                {
                    var ca = AssetGeneration.GetCharacterStageImage(CharA, EmotionA, ColorA, true, PlayerGender);
                    g.DrawImage((CharActive == CharA) ? ca : AssetGeneration.Fade(ca),
                        new Point(-28, box.Height - ca.Height + 14));
                }

                if (CharB != string.Empty)
                {
                    var cb = AssetGeneration.GetCharacterStageImage(CharB, EmotionB, ColorB, false, PlayerGender);
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

                if (LineIndex > File.MessageList[messageIndex].MessageLines.Count - 1)
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

            for (var i = 0; i <= LineIndex; i++)
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
            Bitmap topBox = new Bitmap(1, 1),
                   bottomBox = new Bitmap(1, 1);
            if (topLine != string.Empty && CharA != string.Empty)
            {
                topBox = (Bitmap) (TextBoxes[TextboxIndex].Clone());
                using (var g = Graphics.FromImage(topBox))
                {
                    g.DrawImage(AssetGeneration.GetCharacterBuImage(CharA, EmotionA, ColorA, true, PlayerGender),
                        new Point(2, 3));
                    g.DrawImage(
                        AssetGeneration.DrawString(new Bitmap(260, 50), topLine, 0, 22, Color.FromArgb(68, 8, 0)),
                        new Point(76, 0));
                }
            }

            if (bottomLine != string.Empty && CharB != string.Empty)
            {
                bottomBox = (Bitmap) TextBoxes[TextboxIndex].Clone();
                using (var g = Graphics.FromImage(bottomBox))
                {
                    g.DrawImage(AssetGeneration.GetCharacterBuImage(CharB, EmotionB, ColorB, true, PlayerGender),
                        new Point(2, 3));
                    g.DrawImage(
                        AssetGeneration.DrawString(new Bitmap(282, 50), bottomLine, 0, 22, Color.FromArgb(68, 8, 0)),
                        new Point(76, 0));
                }
            }

            using (var g = Graphics.FromImage(box))
            {
                if (LineIndex < File.MessageList[messageIndex].MessageLines.Count - 1)
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
                    var topName = Names.ContainsKey(CharA) ? Names[CharA] : (CharA == "username" ? PlayerName : CharA);
                    var nameLen = AssetGeneration.GetLength(topName);
                    var topNameBox =
                        (Bitmap)
                            AssetGeneration.DrawString(Resources.NameBox, topName, Resources.NameBox.Width/2 - nameLen/2,
                                16, Color.FromArgb(253, 234, 177)); //Center name in NameBox
                    g.DrawImage(topNameBox, new Point(7, topBox.Height - (topNameBox.Height - 20)));
                }

                if (string.IsNullOrEmpty(bottomLine) || string.IsNullOrEmpty(CharB))
                {
                    return box;
                }
                {
                    var bottomName = Names.ContainsKey(CharB)
                        ? Names[CharB]
                        : (CharB == "username" ? PlayerName : CharB);
                    var nameLen = AssetGeneration.GetLength(bottomName);
                    var bottomNameBox =
                        (Bitmap)
                            AssetGeneration.DrawString(Resources.NameBox, bottomName,
                                Resources.NameBox.Width/2 - nameLen/2, 16, Color.FromArgb(253, 234, 177));
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
            var index = LineIndex;
            ResetParameters();
            foreach (var t in File.MessageList[messageIndex].MessageLines)
            {
                GetParsedCommands(t);
                var parsed = t.SpokenText;
                if (!string.IsNullOrWhiteSpace(parsed))
                {
                    images.Add(RenderPreviewBox(parsed));
                }
            }
            LineIndex = index;
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
            const string femalePrefix = "PCF";
            return File.MessageList[MessageIndex].Prefix.Contains(femalePrefix)
                ? PlayerGender.Female
                : PlayerGender.Male;
        }
    }
}