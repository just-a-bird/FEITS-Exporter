﻿using FEITS.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace FEITS.Model
{
    public class ConversationModel
    {
        //Message reference
        private MessageBlock currentMessage;
        public MessageBlock CurrentMessage
        {
            get { return currentMessage; }
            set { currentMessage = value; lineIndex = 0; ResetParameters(); }
        }
        private int lineIndex;
        public int LineIndex { get { return lineIndex; } set { lineIndex = value; } }

        //Settings
        public string PlayerName = "Kamui";
        public bool EnableBackgrounds;
        public Image BackgroundImage;
        public Image[] TextBoxes = { Resources.TextBox, Resources.TextBox_Nohr, Resources.TextBox_Hoshido };
        public int TextboxIndex;

        //Character names with their translations
        private Dictionary<string, string> names;

        //
        private Color colorA = Color.FromArgb(0x5B, 0x58, 0x55);
        private Color colorB = Color.FromArgb(0x5B, 0x58, 0x55);

        //Line commands
        private bool hasPerms;
        public bool HasPerms { get { return hasPerms; } }
        private bool setType;
        public bool SetType { get { return setType; } }
        private string charActive = string.Empty;
        public string CharActive { get { return charActive; } }
        private string charA = string.Empty;
        public string CharA { get { return charA; } }
        private string charB = string.Empty;
        public string CharB { get { return charB; } }
        private int charSide;
        public int CharSide { get { return charSide; } }
        private const string defaultEmotion = "通常,";
        private string emotionA = defaultEmotion;
        public string EmotionA { get { return emotionA; } }
        private string emotionB = defaultEmotion;
        public string EmotionB { get { return emotionB; } }
        private ConversationTypes ConversationType = ConversationTypes.Type1;

        public ConversationModel()
        {
            //Grab names from PID and sort them into a dictionary
            names = new Dictionary<string, string>();
            string[] pids = Resources.PID.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in pids)
            {
                var p = str.Split(new[] { '\t' });
                names[p[0]] = p[1];
            }
        }

        public void ResetParameters()
        {
            hasPerms = setType = false;
            charActive = charA = charB = string.Empty;
            emotionA = emotionB = defaultEmotion;
            ConversationType = ConversationTypes.Type1;
        }

        public void GetCommandsBeforeIndex()
        {
            ResetParameters();

            for(int i = 0; i < lineIndex; i++)
            {
                GetParsedCommands(currentMessage.MessageLines[i]);
            }
        }

        public string GetParsedCommands(MessageLine line)
        {
            if (!line.CommandsParsed)
                line.ParseLineCommands();

            //$E
            if(line.Emotion != string.Empty)
            {
                if(charActive != string.Empty && charActive == charB)
                    emotionB = line.Emotion;
                else
                    emotionA = line.Emotion;
            }

            //$Ws
            if(line.CharacterActive != string.Empty)
            {
                charActive = line.CharacterActive;
            }

            //$Wm
            if(line.CharacterSide != -1)
            {
                charSide = line.CharacterSide;

                if (ConversationType == ConversationTypes.Type1)
                {
                    if (charSide == 3)
                    {
                        charA = line.CharacterA;
                        emotionA = defaultEmotion;
                    }
                    else if (charSide == 7)
                    {
                        charB = line.CharacterB;
                        emotionB = defaultEmotion;
                    }
                }
                else
                {
                    if (charSide == 0 || charSide == 2)
                    {
                        charA = line.CharacterA;
                        emotionA = defaultEmotion;
                    }
                    else if (charSide == 6)
                    {
                        charB = line.CharacterB;
                        emotionB = defaultEmotion;
                    }
                }
            }

            //$Wd
            if(line.Wd)
            {
                if(charActive == charB)
                {
                    charActive = charA;
                    charB = string.Empty;
                }
                else
                {
                    charA = string.Empty;
                }
            }

            //$a
            if(!hasPerms)
            {
                if(line.HasPerms)
                {
                    hasPerms = true;
                }
            }

            //$t0 & $t1
            if(line.SetType && !setType)
            {
                ConversationType = line.ConversationType;
                setType = true;
            }

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
            Bitmap box = new Bitmap(400, 240);
            Bitmap tb = TextBoxes[TextboxIndex].Clone() as Bitmap;

            //Generate text image from string
            if (line.Contains("$Nu") && hasPerms)
            {
                line = line.Replace("$Nu", PlayerName);
            }

            line = line.Replace(Environment.NewLine, "\n");

            //Draw the line's text
            Bitmap text = TextGeneration.DrawString(new Bitmap(312, 50), line, 0, 22, Color.FromArgb(68, 8, 0)) as Bitmap;

            using (Graphics g = Graphics.FromImage(tb))
            {
                g.DrawImage(text, new Point(29, 0));
            }

            //Name box
            string name = names.ContainsKey(charActive) ? names[charActive] : (charActive == "username" ? PlayerName : charActive);
            int nameLength = TextGeneration.GetLength(name);
            Bitmap nb = TextGeneration.DrawString(Resources.NameBox, name, Resources.NameBox.Width / 2 - nameLength / 2, 16, Color.FromArgb(253, 234, 177)) as Bitmap;  //Center name in NameBox

            using (Graphics g = Graphics.FromImage(box))
            {
                if (EnableBackgrounds)
                {
                    g.DrawImage(BackgroundImage, new Point(0, 0));
                }

                if (charA != string.Empty)
                {
                    Image ca = TextGeneration.GetCharacterStageImage(charA, emotionA, colorA, true);
                    g.DrawImage((charActive == charA) ? ca : TextGeneration.Fade(ca), new Point(-28, box.Height - ca.Height + 14));
                }

                if (charB != string.Empty)
                {
                    Image cb = TextGeneration.GetCharacterStageImage(charB, emotionB, colorB, false);
                    g.DrawImage((charActive == charB) ? cb : TextGeneration.Fade(cb), new Point(box.Width - cb.Width + 28, box.Height - cb.Height + 14));
                }

                g.DrawImage(tb, new Point(10, box.Height - tb.Height + 2));

                if (charActive != string.Empty)
                {
                    g.DrawImage(nb, charActive == charB ? new Point(box.Width - nb.Width - 6, box.Height - tb.Height - 14) : new Point(7, box.Height - tb.Height - 14));
                }

                if (lineIndex > currentMessage.MessageLines.Count - 1)
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

            for (int i = 0; i <= lineIndex; i++)
            {
                string line = GetParsedCommands(currentMessage.MessageLines[i]);

                if (line.Contains("$Nu") && hasPerms)
                {
                    line = line.Replace("$Nu", PlayerName);
                }

                line = line.Replace(Environment.NewLine, "\n");

                if (charActive == charA)
                    topLine = line;
                else
                    bottomLine = line;
            }

            //Hard coded dimensions
            Bitmap box = new Bitmap(400, 240);
            Bitmap topBox = new Bitmap(1, 1), bottomBox = new Bitmap(1, 1);
            if (topLine != string.Empty && charA != string.Empty)
            {
                topBox = (TextBoxes[TextboxIndex].Clone()) as Bitmap;
                using (Graphics g = Graphics.FromImage(topBox))
                {
                    g.DrawImage(TextGeneration.GetCharacterBUImage(charA, emotionA, colorA, true), new Point(2, 3));
                    g.DrawImage(TextGeneration.DrawString(new Bitmap(282, 50), topLine, 0, 22, Color.FromArgb(68, 8, 0)), new Point(76, 0));
                }
            }

            if (bottomLine != string.Empty && charB != string.Empty)
            {
                bottomBox = (TextBoxes[TextboxIndex].Clone()) as Bitmap;
                using (Graphics g = Graphics.FromImage(bottomBox))
                {
                    g.DrawImage(TextGeneration.GetCharacterBUImage(charB, emotionB, colorB, true), new Point(2, 3));
                    g.DrawImage(TextGeneration.DrawString(new Bitmap(282, 50), bottomLine, 0, 22, Color.FromArgb(68, 8, 0)), new Point(76, 0));
                }
            }

            using (Graphics g = Graphics.FromImage(box))
            {
                if (lineIndex < currentMessage.MessageLines.Count - 1)
                {
                    using (Graphics g2 = Graphics.FromImage(charActive == charA ? topBox : bottomBox))
                    {
                        g2.DrawImage(Resources.KeyPress, new Point(TextBoxes[TextboxIndex].Width - 30, 32));
                    }
                }

                if (EnableBackgrounds)
                    g.DrawImage(BackgroundImage, new Point(0, 0));
                g.DrawImage(topBox, new Point(10, 3));
                g.DrawImage(bottomBox, new Point(10, box.Height - bottomBox.Height + 2));

                if (topLine != string.Empty && charA != string.Empty)
                {
                    string topName = names.ContainsKey(charA) ? names[charA] : (charA == "username" ? PlayerName : charA);
                    int nameLen = TextGeneration.GetLength(topName);
                    Bitmap topNameBox = TextGeneration.DrawString(Resources.NameBox, topName, Resources.NameBox.Width / 2 - nameLen / 2, 16, Color.FromArgb(253, 234, 177)) as Bitmap; //Center name in NameBox
                    g.DrawImage(topNameBox, new Point(7, topBox.Height - (topNameBox.Height - 20)));
                }

                if (bottomLine != string.Empty && charB != string.Empty)
                {
                    string bottomName = names.ContainsKey(charB) ? names[charB] : (charB == "username" ? PlayerName : charB);
                    int nameLen = TextGeneration.GetLength(bottomName);
                    Bitmap bottomNameBox = TextGeneration.DrawString(Resources.NameBox, bottomName, Resources.NameBox.Width / 2 - nameLen / 2, 16, Color.FromArgb(253, 234, 177)) as Bitmap;
                    g.DrawImage(bottomNameBox, new Point(7, box.Height - bottomBox.Height - 14));
                }
            }
            return box;
        }
        #endregion
    }
}