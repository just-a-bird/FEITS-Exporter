using FEITS.Forms;
using FEITS.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FEITS
{
    public partial class Form1 : Form
    {
        private Image[] Images = { Resources.Awakening_0, Resources.Awakening_1 };

        private bool[] ValidCharacters;
        private FontCharacter[] Characters;
        private Dictionary<string, string> Names;
        private Dictionary<string, byte[]> FaceData;

        private string PLAYER_NAME = "Kamui";
        private string RAW_MESSAGE = "$t1$Wmエリーゼ|3$w0|$Wsエリーゼ|$Wa$Eびっくり,汗|This is an example conversation.$k$p$Wmサクラ|7$w0|$Wsサクラ|$Wa$E怒,汗|It takes place between\nSakura and Elise.$k";
        private string MODIFIED_MESSAGE;
        private bool HAS_PERMS;                     //Permission to use username?
        private bool SET_TYPE;                      //Set conversation type

        private int CUR_INDEX;
        private List<Message> Messages = new List<Message>();
        private string CHAR_A = string.Empty;
        private string CHAR_B = string.Empty;
        private string CHAR_ACTIVE = string.Empty;
        private const string DEFAULT_EMOTION = "通常,"; // 通常
        private string EMOTION_A = DEFAULT_EMOTION;
        private string EMOTION_B = DEFAULT_EMOTION;
        private Color COLOR_A = Color.FromArgb(0x5B, 0x58, 0x55);
        private Color COLOR_B = Color.FromArgb(0x5B, 0x58, 0x55);
        private ConversationTypes CONVERSATION_TYPE = ConversationTypes.TYPE_1;
        private bool USE_BACKGROUNDS;
        private Image BACKGROUND_IMAGE;

        private string[] EyeStyles = { "a", "b", "c", "d", "e", "f", "g" };
        private string[] Kamuis = { "マイユニ男1", "マイユニ男2", "マイユニ女1", "マイユニ女2" };

        private Image[] TextBoxes = { Resources.TextBox, Resources.TextBox_Nohr, Resources.TextBox_Hoshido };
        private int TextboxIndex = 0;

        private List<string> ResourceList = new List<string>();

        public enum ConversationTypes
        {
            TYPE_0,
            TYPE_1
        }

        public Form1()
        {
            InitializeComponent();

            //I believe this is setting up the font characters
            ValidCharacters = new bool[0x10000];
            Characters = new FontCharacter[0x10000];
            for(int i = 0; i < Resources.chars.Length / 0x10; i++)
            {
                FontCharacter fc = new FontCharacter(Resources.chars, i * 0x10);
                ValidCharacters[fc.Value] = true;
                fc.SetGlyph(Images[fc.IMG]);
                Characters[fc.Value] = fc;
            }

            //Grab names from PID and sort them into a dictionary
            Names = new Dictionary<string, string>();
            string[] pids = Resources.PID.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach(string str in pids)
            {
                var p = str.Split(new[] { '\t' });
                Names[p[0]] = p[1];
            }

            //Grab face data and assign to dictionary
            FaceData = new Dictionary<string, byte[]>();
            string[] fids = Resources.FID.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for(int i = 0; i < fids.Length; i++)
            {
                byte[] dat = new byte[0x48];
                Array.Copy(Resources.faces, i * 0x48, dat, 0, 0x48);
                FaceData[fids[i]] = dat;
            }

            ResourceSet set = Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
            foreach(DictionaryEntry o in set)
            {
                ResourceList.Add(o.Key as string);
            }
            Resources.ResourceManager.ReleaseAllResources();

            MODIFIED_MESSAGE = RAW_MESSAGE;
            ReloadScript(true);

            TB_ProtagName.Text = PLAYER_NAME;
            CB_Textbox.SelectedIndex = TextboxIndex;
            CHK_Backgrounds.Checked = USE_BACKGROUNDS;
        }

        private void ReloadScript(bool resetIndex)
        {
            //Split the script and parse it for data
            Messages = ParseMessages(MODIFIED_MESSAGE);

            if (resetIndex)
                CUR_INDEX = 0;
            else if (CUR_INDEX >= Messages.Count)
                CUR_INDEX = Messages.Count - 1;

            B_Next.Enabled = (CUR_INDEX < Messages.Count - 1);
            B_Prev.Enabled = (CUR_INDEX > 0);

            if (resetIndex)
                ResetParameters();

            for(int i = 0; i <= CUR_INDEX; i++)
            {
                UpdateParse(Messages[i]);
            }

            //Push the spoken text to the text box
            TB_CurrentLine.Text = Messages[CUR_INDEX].spokenText;
        }

        /// <summary>
        /// Parses message data
        /// and stores them for later use.
        /// </summary>
        /// <param name="msg">The message to parse.</param>
        /// <returns>Fully-parsed message.</returns>
        private Message UpdateParse(Message msg)
        {
            if(msg.spokenText != string.Empty)
            {
                Tools.SaveLineToRaw(msg);
                msg.rawLine = msg.rawLine.Replace("\\n", "\n").Replace("$k\n", "$k\\n");
            }

            Console.WriteLine(msg.spokenText);
            msg.spokenText = msg.rawLine;

            for(int i = 0; i < msg.spokenText.Length; i++)
            {
                if(msg.spokenText[i] == '$')
                {
                    Tuple<string, Command> res = ParseCommand(msg.spokenText, i);
                    Console.WriteLine(msg.spokenText);
                    msg.spokenText = res.Item1;
                    Console.WriteLine(msg.spokenText);
                    
                    if(res.Item2.numParams > 0)
                        if (res.Item2.Params[0] == "ベロア")
                            res.Item2.Params[0] = "べロア"; // Velour Fix

                    switch(res.Item2.cmd)
                    {
                        case "$E":
                            if(CHAR_ACTIVE != string.Empty && CHAR_ACTIVE == CHAR_B)
                            {
                                EMOTION_B = res.Item2.Params[0];
                                TB_Emotion.Text = EMOTION_B;
                            }
                            else
                            {
                                EMOTION_A = res.Item2.Params[0];
                                TB_Emotion.Text = EMOTION_A;
                            }
                            break;
                        case "$Ws":
                            CHAR_ACTIVE = res.Item2.Params[0];
                            TB_ActiveChar.Text = CHAR_ACTIVE;
                            break;
                        case "$Wm":
                            if (CONVERSATION_TYPE == ConversationTypes.TYPE_1)
                            {
                                if (res.Item2.Params[1] == "3")
                                {
                                    CHAR_A = res.Item2.Params[0];
                                    EMOTION_A = DEFAULT_EMOTION;
                                    TB_Portrait.Text = CHAR_A;
                                }
                                else if (res.Item2.Params[1] == "7")
                                {
                                    CHAR_B = res.Item2.Params[0];
                                    EMOTION_B = DEFAULT_EMOTION;
                                    TB_Portrait.Text = CHAR_B;
                                }
                            }
                            else
                            {
                                if (res.Item2.Params[1] == "0" || res.Item2.Params[1] == "2")
                                {
                                    CHAR_A = res.Item2.Params[0];
                                    EMOTION_A = DEFAULT_EMOTION;
                                    TB_Portrait.Text = CHAR_A;
                                }
                                else if (res.Item2.Params[1] == "6")
                                {
                                    CHAR_B = res.Item2.Params[0];
                                    EMOTION_B = DEFAULT_EMOTION;
                                    TB_Portrait.Text = CHAR_B;
                                }
                            }
                            break;
                        case "$Wd":
                            if (CHAR_ACTIVE == CHAR_B)
                            {
                                CHAR_ACTIVE = CHAR_A;
                                CHAR_B = string.Empty;
                            }
                            else
                                CHAR_A = string.Empty;
                            break;
                        case "$a":
                            HAS_PERMS = true;
                            break;
                        case "$t0":
                            if (!SET_TYPE)
                                CONVERSATION_TYPE = ConversationTypes.TYPE_0;
                            SET_TYPE = true;
                            break;
                        case "$t1":
                            if (!SET_TYPE)
                                CONVERSATION_TYPE = ConversationTypes.TYPE_1;
                            SET_TYPE = true;
                            break;
                        case "$Nu":
                            msg.spokenText = msg.spokenText.Substring(0, i) + "$Nu" + msg.spokenText.Substring(i);
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

            msg.speechIndex = msg.rawLine.IndexOf(msg.spokenText);
            Console.WriteLine(msg.speechIndex);

            msg.rawLine = msg.rawLine.Replace("\\n", "\n").Replace("$k\n", "$k\\n");

            msg.spokenText.Replace("\\n", "\n").Replace("$k\n", "$k\\n");
            msg.spokenText = msg.spokenText.Replace("\n", Environment.NewLine);
            return msg;
        }

        private Tuple<string, Command> ParseCommand(string line, int offset)
        {
            string trunc = line.Substring(offset);
            string[] NoParams = { "$Wa", "$Wc", "$a", "$Nu", "$N0", "$N1", "$k\\n", "$k", "$t0", "$t1", "$p" };
            string[] SingleParams = { "$E", "$Sbs", "$Svp", "$Sre", "$Fw", "$Ws", "$VF", "$Ssp", "$Fo", "$VNMPID", "$Fi", "$b", "$Wd", "$w", "$l" };
            string[] DoubleParams = { "$Wm", "$Sbv", "$Sbp", "$Sls", "$Slp" };
            Command newCMD = new Command();

            foreach(string delim in NoParams)
            {
                if(trunc.StartsWith(delim))
                {
                    newCMD.cmd = delim;
                    newCMD.numParams = 0;
                    newCMD.Params = new string[newCMD.numParams];
                    line = line.Substring(0, offset) + line.Substring(offset + delim.Length);
                    break;
                }
            }
            
            foreach(string delim in SingleParams)
            {
                if(trunc.StartsWith(delim))
                {
                    newCMD.cmd = delim;
                    newCMD.numParams = 1;
                    newCMD.Params = new string[newCMD.numParams];
                    int ind = line.IndexOf("|", offset);
                    newCMD.Params[0] = line.Substring(offset + delim.Length, ind - (offset + delim.Length));
                    line = line.Substring(0, offset) + line.Substring(ind + 1);
                }
            }

            foreach(string delim in DoubleParams)
            {
                if(trunc.StartsWith(delim))
                {
                    newCMD.cmd = delim;
                    newCMD.numParams = 2;
                    newCMD.Params = new string[newCMD.numParams];
                    int ind = line.IndexOf("|", offset);
                    int ind2 = line.IndexOf("|", ind + 1);

                    if(delim == "$Wm")
                    {
                        newCMD.Params[0] = line.Substring(offset + delim.Length, ind - (offset + delim.Length));
                        newCMD.Params[1] = line.Substring(ind + 1, 1);
                        line = line.Substring(0, offset) + line.Substring(ind + 2);
                    }
                    else
                    {
                        newCMD.Params[0] = line.Substring(offset + delim.Length, ind - (offset + delim.Length));
                        newCMD.Params[1] = line.Substring(ind + 1, ind2 - (ind + 1));
                        line = line.Substring(0, offset) + line.Substring(ind2 + 1);
                    }
                }
            }

            Tuple<string, Command> ret = new Tuple<string, Command>(line, newCMD);
            return ret;
        }

        /// <summary>
        /// Returns a list of parsed messages.
        /// </summary>
        /// <param name="rawScript">The script to parse.</param>
        /// <returns>Message list split by line ends.</returns>
        private List<Message> ParseMessages(string rawScript)
        {
            var newMsgs = new List<Message>();

            //Split the lines based on the line-end patterns
            var delimiters = new List<string> { "$k$p", "$k\\n" };
            string pattern = "(?<=" + string.Join("|", delimiters.Select(d => Regex.Escape(d)).ToArray()) + ")";
            string[] splits = Regex.Split(rawScript, pattern);

            List<string> msgs = new List<string>();
            msgs.AddRange(splits.Where(s => (!string.IsNullOrWhiteSpace(Parse(s)) && !string.IsNullOrEmpty(Parse(s)))));

            foreach(string str in msgs)
            {
                Console.WriteLine(str);

                //Add lines to new Message class
                Message newMessage = new Message();
                newMessage.rawLine = str;

                //Send them for parsing and then add to the list
                //newMessage = UpdateParse(newMessage);
                newMsgs.Add(newMessage);
            }
            return newMsgs;
        }

        private string Parse(string MSG)
        {
            for (int i = 0; i < MSG.Length; i++)
            {
                if (MSG[i] == '$')
                {
                    Tuple<string, Command> res = ParseCommand(MSG, i);
                    MSG = res.Item1;
                    switch (res.Item2.cmd)
                    {
                        case "$E":
                        case "$Ws":
                        case "$Wm":
                        case "$Wd":
                        case "$a":
                        case "$t0":
                        case "$t1":
                        case "$Nu":
                            i += 2;
                            break;
                        case "$Wa":
                        case "$Wc":
                        default:
                            break;
                    }
                    i--;
                }
            }

            MSG = MSG.Replace("\\n", "\n").Replace("$k\n","$k\\n");
            return MSG;
        }

        private void ResetParameters()
        {
            CHAR_A = CHAR_B = CHAR_ACTIVE = string.Empty;
            HAS_PERMS = SET_TYPE = false;
            EMOTION_A = EMOTION_B = DEFAULT_EMOTION;
            CONVERSATION_TYPE = ConversationTypes.TYPE_1;
        }

        private Image RenderBox(string line)
        {
            return CONVERSATION_TYPE == ConversationTypes.TYPE_1 ? RenderTypeOne(line) : RenderTypeZero();
        }

        private Image RenderTypeOne(string line)
        {
            Bitmap box = new Bitmap(PB_TextBox.Width, PB_TextBox.Height);
            Bitmap tb = TextBoxes[TextboxIndex].Clone() as Bitmap;

            //Generate text image from string
            if(line.Contains("$Nu") && HAS_PERMS)
            {
                line = line.Replace("$Nu", PLAYER_NAME);
            }

            line = line.Replace(Environment.NewLine, "\n");

            Bitmap text = Tools.DrawString(Characters, new Bitmap(312, 50), line, 0, 22, Color.FromArgb(68, 8, 0)) as Bitmap;

            using (Graphics g = Graphics.FromImage(tb))
            {
                g.DrawImage(text, new Point(29, 0));
            }

            string name = Names.ContainsKey(CHAR_ACTIVE) ? Names[CHAR_ACTIVE] : (CHAR_ACTIVE == "username" ? PLAYER_NAME : CHAR_ACTIVE);
            int nameLength = Tools.GetLength(name, Characters);
            Bitmap nb = Tools.DrawString(Characters, Resources.NameBox, name, Resources.NameBox.Width / 2 - nameLength / 2, 16, Color.FromArgb(253, 234, 177)) as Bitmap; //Center name in NameBox

            using (Graphics g = Graphics.FromImage(box))
            {
                if(USE_BACKGROUNDS)
                {
                    g.DrawImage(BACKGROUND_IMAGE, new Point(0, 0));
                }

                if(CHAR_A != string.Empty)
                {
                    Image ca = GetCharacterStageImage(CHAR_A, EMOTION_A, COLOR_A, true);
                    g.DrawImage((CHAR_ACTIVE == CHAR_A) ? ca : Fade(ca), new Point(-28, box.Height - ca.Height + 14));
                }

                if(CHAR_B != string.Empty)
                {
                    Image cb = GetCharacterStageImage(CHAR_B, EMOTION_B, COLOR_B, false);
                    g.DrawImage((CHAR_ACTIVE == CHAR_B) ? cb : Fade(cb), new Point(box.Width - cb.Width + 28, box.Height - cb.Height + 14));
                }

                g.DrawImage(tb, new Point(10, box.Height - tb.Height + 2));

                if(CHAR_ACTIVE != string.Empty)
                {
                    g.DrawImage(nb, CHAR_ACTIVE == CHAR_B ? new Point(box.Width - nb.Width - 6, box.Height - tb.Height - 14) : new Point(7, box.Height - tb.Height - 14));
                }

                if(CUR_INDEX < Messages.Count - 1)
                {
                    g.DrawImage(Resources.KeyPress, new Point(box.Width - 33, box.Height - tb.Height + 32));
                }
            }

            return box;
        }

        private Image RenderTypeZero()
        {
            string TopMessage = string.Empty, BottomMessage = string.Empty;
            ResetParameters();
            
            for(int i = 0; i <= CUR_INDEX; i++)
            {
                string msg = UpdateParse(Messages[i]).spokenText;

                if(msg.Contains("$Nu") && HAS_PERMS)
                {
                    msg = msg.Replace("$Nu", PLAYER_NAME);
                }

                msg = msg.Replace(Environment.NewLine, "\n");

                if (CHAR_ACTIVE == CHAR_A)
                    TopMessage = msg;
                else
                    BottomMessage = msg;
            }

            Bitmap Box = new Bitmap(PB_TextBox.Width, PB_TextBox.Height);
            Bitmap TopBox = new Bitmap(1, 1), BottomBox = new Bitmap(1, 1);
            if (TopMessage != string.Empty && CHAR_A != string.Empty)
            {
                TopBox = (TextBoxes[TextboxIndex].Clone()) as Bitmap;
                using (Graphics g = Graphics.FromImage(TopBox))
                {
                    g.DrawImage(GetCharacterBUImage(CHAR_A, EMOTION_A, COLOR_A, true), new Point(2, 3));
                    g.DrawImage(Tools.DrawString(Characters, new Bitmap(282, 50), TopMessage, 0, 22, Color.FromArgb(68, 8, 0)), new Point(76, 0));
                }
            }
            if (BottomMessage != string.Empty && CHAR_B != string.Empty)
            {
                BottomBox = (TextBoxes[TextboxIndex].Clone()) as Bitmap;
                using (Graphics g = Graphics.FromImage(BottomBox))
                {
                    g.DrawImage(GetCharacterBUImage(CHAR_B, EMOTION_B, COLOR_B, true), new Point(2, 3));
                    g.DrawImage(Tools.DrawString(Characters, new Bitmap(282, 50), BottomMessage, 0, 22, Color.FromArgb(68, 8, 0)), new Point(76, 0));
                }
            }
            using (Graphics g = Graphics.FromImage(Box))
            {
                if (CUR_INDEX < Messages.Count - 1)
                {
                    using (Graphics g2 = Graphics.FromImage(CHAR_ACTIVE == CHAR_A ? TopBox : BottomBox))
                    {
                        g2.DrawImage(Resources.KeyPress, new Point(TextBoxes[TextboxIndex].Width - 30, 32));
                    }
                }
                if (USE_BACKGROUNDS)
                    g.DrawImage(BACKGROUND_IMAGE, new Point(0, 0));
                g.DrawImage(TopBox, new Point(10, 3));
                g.DrawImage(BottomBox, new Point(10, Box.Height - BottomBox.Height + 2));
                if (TopMessage != string.Empty && CHAR_A != string.Empty)
                {
                    string TopName = Names.ContainsKey(CHAR_A) ? Names[CHAR_A] : (CHAR_A == "username" ? PLAYER_NAME : CHAR_A);
                    int NameLen = Tools.GetLength(TopName, Characters);
                    Bitmap TopNameBox = Tools.DrawString(Characters, Resources.NameBox, TopName, Resources.NameBox.Width / 2 - NameLen / 2, 16, Color.FromArgb(253, 234, 177)) as Bitmap; // Center Name in NameBox
                    g.DrawImage(TopNameBox, new Point(7, TopBox.Height - (TopNameBox.Height - 20)));
                }
                if (BottomMessage != string.Empty && CHAR_B != string.Empty)
                {
                    string BottomName = Names.ContainsKey(CHAR_B) ? Names[CHAR_B] : (CHAR_B == "username" ? PLAYER_NAME : CHAR_B);
                    int NameLen = Tools.GetLength(BottomName, Characters);
                    Bitmap BottomNameBox = Tools.DrawString(Characters, Resources.NameBox, BottomName, Resources.NameBox.Width / 2 - NameLen / 2, 16, Color.FromArgb(253, 234, 177)) as Bitmap; // Center Name in NameBox
                    g.DrawImage(BottomNameBox, new Point(7, Box.Height - BottomBox.Height - 14));
                }
            }
            return Box;
        }

        private Image GetCharacterStageImage(string CName, string CEmo, Color HairColor, bool Slot1)
        {
            bool USER = CName == "username";
            string hairname = "_st_髪";
            string dat_id = "FSID_ST_" + CName;
            if (USER)
            {
                dat_id = "FSID_ST_" + (new[] { "マイユニ_男1", "マイユニ_男2", "マイユニ_女1", "マイユニ_女2" })[0] + "_顔" + EyeStyles[0].ToUpper();
                CName = EyeStyles[0] + Kamuis[0];
                hairname = CName.Substring(1) + hairname + 0;
            }
            else
                hairname = CName + hairname + "0";
            var Emos = CEmo.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string resname = CName + "_st_" + Emos[0];
            Image C;
            if (ResourceList.Contains(resname))
                C = Resources.ResourceManager.GetObject(resname) as Image;
            else
                C = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(C))
            {
                if (USER && 0 > 0)
                {
                    g.DrawImage(Resources.ResourceManager.GetObject((new[] { "マイユニ男1", "マイユニ男2", "マイユニ女1", "マイユニ女2" })[0] + "_st_アクセサリ1_" + 0) as Image, new Point(0, 0));
                }
                for (int i = 1; i < Emos.Length; i++)
                {
                    string exresname = CName + "_st_" + Emos[i];
                    if (Emos[i] == "汗" && ResourceList.Contains(exresname))
                    {
                        g.DrawImage(Resources.ResourceManager.GetObject(exresname) as Image, new Point(BitConverter.ToUInt16(FaceData[dat_id], 0x40), BitConverter.ToUInt16(FaceData[dat_id], 0x42)));
                    }
                    else if (Emos[i] == "照" && ResourceList.Contains(exresname))
                    {
                        g.DrawImage(Resources.ResourceManager.GetObject(exresname) as Image, new Point(BitConverter.ToUInt16(FaceData[dat_id], 0x38), BitConverter.ToUInt16(FaceData[dat_id], 0x3A)));
                    }
                }
                if (ResourceList.Contains(hairname))
                {
                    Bitmap hair = Resources.ResourceManager.GetObject(hairname) as Bitmap;
                    g.DrawImage(ColorHair(hair, HairColor), new Point(0, 0));
                }
                if (USER && 0 > 0)
                {
                    g.DrawImage(Resources.ResourceManager.GetObject((new[] { "マイユニ男1", "マイユニ男2", "マイユニ女1", "マイユニ女2" })[0] + "_st_アクセサリ2_" + 0) as Image, new Point(133, 28));
                }
            }
            if (Slot1)
                C.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return C;
        }

        private Image GetCharacterBUImage(string CName, string CEmo, Color HairColor, bool Crop)
        {
            string hairname = "_bu_髪";
            string dat_id = "FSID_BU_" + CName;
            bool USER = CName == "username";
            if (USER)
            {
                dat_id = "FSID_BU_" + (new[] { "マイユニ_男1", "マイユニ_男2", "マイユニ_女1", "マイユニ_女2" })[0] + "_顔" + EyeStyles[0].ToUpper();
                CName = EyeStyles[0] + Kamuis[0];
                hairname = CName.Substring(1) + hairname + 0;
            }
            else
                hairname = CName + hairname + "0";
            var Emos = CEmo.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string resname = CName + "_bu_" + Emos[0];
            Image C;
            if (ResourceList.Contains(resname))
                C = Resources.ResourceManager.GetObject(resname) as Image;
            else
                C = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(C))
            {
                if (USER && 0 > 0)
                {
                    g.DrawImage(Resources.ResourceManager.GetObject((new[] { "マイユニ男1", "マイユニ男2", "マイユニ女1", "マイユニ女2" })[0] + "_bu_アクセサリ1_" + 0) as Image, new Point(0, 0));
                }
                for (int i = 1; i < Emos.Length; i++)
                {
                    string exresname = CName + "_bu_" + Emos[i];
                    if (Emos[i] == "汗" && ResourceList.Contains(exresname))
                    {
                        g.DrawImage(Resources.ResourceManager.GetObject(exresname) as Image, new Point(BitConverter.ToUInt16(FaceData[dat_id], 0x40), BitConverter.ToUInt16(FaceData[dat_id], 0x42)));
                    }
                    else if (Emos[i] == "照" && ResourceList.Contains(exresname))
                    {
                        g.DrawImage(Resources.ResourceManager.GetObject(exresname) as Image, new Point(BitConverter.ToUInt16(FaceData[dat_id], 0x38), BitConverter.ToUInt16(FaceData[dat_id], 0x3A)));
                    }
                }
                if (ResourceList.Contains(hairname))
                {
                    Bitmap hair = Resources.ResourceManager.GetObject(hairname) as Bitmap;
                    g.DrawImage(ColorHair(hair, HairColor), new Point(0, 0));
                }
                if (USER && 0 > 0)
                {
                    Point Acc = new[] { new Point(66, 5), new Point(65, 21) }[0 - 2];
                    g.DrawImage(Resources.ResourceManager.GetObject((new[] { "マイユニ男1", "マイユニ男2", "マイユニ女1", "マイユニ女2" })[0] + "_bu_アクセサリ2_" + 0) as Image, Acc);
                }
            }
            if (Crop)
            {
                Bitmap Cropped = new Bitmap(BitConverter.ToUInt16(FaceData[dat_id], 0x34), BitConverter.ToUInt16(FaceData[dat_id], 0x36));
                using (Graphics g = Graphics.FromImage(Cropped))
                {
                    g.DrawImage(C, new Point(-BitConverter.ToUInt16(FaceData[dat_id], 0x30), -BitConverter.ToUInt16(FaceData[dat_id], 0x32)));
                }
                C = Cropped;
            }
            C.RotateFlip(RotateFlipType.RotateNoneFlipX);
            return C;
        }

        private Image ColorHair(Image Hair, Color C)
        {
            Bitmap bmp = Hair as Bitmap;
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbaValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbaValues, 0, bytes);

            for (int i = 0; i < rgbaValues.Length; i += 4)
            {
                if (rgbaValues[i + 3] > 0)
                {
                    rgbaValues[i + 2] = BlendOverlay(C.R, rgbaValues[i + 2]);
                    rgbaValues[i + 1] = BlendOverlay(C.G, rgbaValues[i + 1]);
                    rgbaValues[i + 0] = BlendOverlay(C.B, rgbaValues[i + 0]);
                }
            }
            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbaValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        private static byte BlendOverlay(byte Src, byte Dst)
        {
            return ((Dst < 128) ? (byte)Math.Max(Math.Min((Src / 255.0f * Dst / 255.0f) * 255.0f * 2, 255), 0) : (byte)Math.Max(Math.Min(255 - ((255 - Src) / 255.0f * (255 - Dst) / 255.0f) * 255.0f * 2, 255), 0));
        }

        private Image Fade(Image BaseImage)
        {

            Bitmap bmp = BaseImage as Bitmap;
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbaValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbaValues, 0, bytes);

            const double BLACK_A = 113.0 / 255.0;

            for (int i = 0; i < rgbaValues.Length; i += 4)
            {
                if (rgbaValues[i + 3] <= 0) continue;
                double DST_A = rgbaValues[i + 3] / 255.0;
                // double FINAL_A = BLACK_A + (DST_A) * (1.0 - BLACK_A);
                // rgbaValues[i + 3] = (byte)Math.Round((FINAL_A) * 255.0);
                rgbaValues[i + 2] = (byte)Math.Round((((rgbaValues[i + 2] / 255.0)) * (DST_A) * (1.0 - BLACK_A)) * 255.0);
                rgbaValues[i + 1] = (byte)Math.Round((((rgbaValues[i + 1] / 255.0)) * (DST_A) * (1.0 - BLACK_A)) * 255.0);
                rgbaValues[i + 0] = (byte)Math.Round((((rgbaValues[i + 0] / 255.0)) * (DST_A) * (1.0 - BLACK_A)) * 255.0);
            }
            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbaValues, 0, ptr, bytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        private void RefreshText()
        {
            for(int i = 0; i <= CUR_INDEX; i++)
            {
                UpdateParse(Messages[i]);
            }

            PB_TextBox.Image = RenderBox(Messages[CUR_INDEX].spokenText);
        }

        private void TB_CurrentLine_TextChanged(object sender, EventArgs e)
        {
            Messages[CUR_INDEX].spokenText = TB_CurrentLine.Text;
            PB_TextBox.Image = RenderBox(Messages[CUR_INDEX].spokenText);
        }

        private void TB_CurrentLine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                TB_CurrentLine.SelectAll();
        }

        /// <summary>
        /// Handles drag-and-drop of background images to text box.
        /// </summary>
        private void PB_TextBox_DragDrop(object sender, DragEventArgs e)
        {
            string file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            if (File.Exists(file))
            {
                try
                {
                    Image img = Image.FromFile(file);
                    if (img.Width > 1 && img.Height > 1)
                    {
                        BACKGROUND_IMAGE = img;
                    }
                }
                catch
                {
                    // Do nothing
                }
            }
        }

        private void PB_TextBox_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            Image ToSave;
            if ((e as MouseEventArgs).Button == MouseButtons.Right)
            {
                if (Prompt(MessageBoxButtons.YesNo, "Save the current conversation?") != DialogResult.Yes)
                    return;
                sfd.FileName = "FE_Conversation.png";
                List<Image> Images = new List<Image>();
                int stored = CUR_INDEX;
                ResetParameters();
                for (CUR_INDEX = 0; CUR_INDEX < Messages.Count; CUR_INDEX++)
                {
                    UpdateParse(Messages[CUR_INDEX]);
                    string parsed = Messages[CUR_INDEX].spokenText;
                    if (!string.IsNullOrWhiteSpace(parsed) && !string.IsNullOrEmpty(parsed))
                    {
                        Images.Add(RenderBox(parsed));
                    }
                }
                CUR_INDEX = stored;
                ResetParameters();
                for (int i = 0; i <= CUR_INDEX; i++)
                {
                    UpdateParse(Messages[i]);
                }
                Bitmap bmp = new Bitmap(Images.Max(i => i.Width), Images.Sum(i => i.Height));
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    int h = 0;
                    foreach (Image img in Images)
                    {
                        g.DrawImage(img, new Point(0, h));
                        h += img.Height;
                    }
                }
                ToSave = bmp;
            }
            else
            {
                if (Prompt(MessageBoxButtons.YesNo, "Save the current image?") != DialogResult.Yes)
                    return;
                ToSave = PB_TextBox.Image;
                sfd.FileName = "FE_Conversation_" + CUR_INDEX + ".png";
            }
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                ToSave.Save(sfd.FileName, ImageFormat.Png);
            }
        }

        internal static DialogResult Prompt(MessageBoxButtons btn, params string[] lines)
        {
            SystemSounds.Question.Play();
            string msg = string.Join(Environment.NewLine + Environment.NewLine, lines);
            return MessageBox.Show(msg, "Prompt", btn, MessageBoxIcon.Asterisk);
        }

        /// <summary>
        /// Opens the script import form as a modal window.
        /// </summary>
        private void B_LoadScript_Click(object sender, EventArgs e)
        {
            ScriptInput inputForm = new ScriptInput(ValidCharacters, RAW_MESSAGE);

            DialogResult dialogResult = inputForm.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                //Pass on the new script data to the message strings
                MODIFIED_MESSAGE = RAW_MESSAGE = inputForm.enteredData.Replace("\\n", "\n").Replace("$k\n", "$k\\n");
                ReloadScript(true);
            }

            inputForm.Dispose();
        }

        /// <summary>
        /// Opens Settings form as a modal window.
        /// </summary>
        private void B_Settings_Click(object sender, EventArgs e)
        {
            HalfBoxTester halfbox = new HalfBoxTester(ValidCharacters, Characters);
            DialogResult dialogResult = halfbox.ShowDialog();

            halfbox.Dispose();
        }

        private void B_Next_Click(object sender, EventArgs e)
        {
            if (CUR_INDEX < Messages.Count - 1)
                CUR_INDEX++;
            B_Prev.Enabled = (CUR_INDEX > 0);
            B_Next.Enabled = (CUR_INDEX < Messages.Count - 1);

            UpdateParse(Messages[CUR_INDEX]);
            
            if(TB_CurrentLine.Text != Messages[CUR_INDEX].spokenText)
            {
                TB_CurrentLine.Text = Messages[CUR_INDEX].spokenText;
            }
            else
            {
                PB_TextBox.Image = RenderBox(Messages[CUR_INDEX].spokenText);
            }
        }

        private void B_Prev_Click(object sender, EventArgs e)
        {
            if (CUR_INDEX > 0)
                CUR_INDEX--;

            ResetParameters();

            for(int i = 0; i <= CUR_INDEX; i++)
            {
                UpdateParse(Messages[i]);
            }

            B_Prev.Enabled = (CUR_INDEX > 0);
            B_Next.Enabled = (CUR_INDEX < Messages.Count - 1);

            if (TB_CurrentLine.Text != Messages[CUR_INDEX].spokenText)
            {
                TB_CurrentLine.Text = Messages[CUR_INDEX].spokenText;
            }
            else
            {
                PB_TextBox.Image = RenderBox(Messages[CUR_INDEX].spokenText);
            }
        }

        private void B_EditLineScript_Click(object sender, EventArgs e)
        {
            DirectEdit directEdit = new DirectEdit(ValidCharacters, Messages[CUR_INDEX]);
            DialogResult dialogResult = directEdit.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                string newScript = string.Empty;
                foreach(Message msg in Messages)
                {
                    Tools.SaveLineToRaw(msg);
                }

                Messages[CUR_INDEX].rawLine = directEdit.enteredData;

                foreach (Message msg in Messages)
                {
                    newScript += msg.rawLine;
                }

                newScript = newScript.Replace("\\n", "\n").Replace("$k\n", "$k\\n");
                MODIFIED_MESSAGE = newScript;
                ReloadScript(false);
                PB_TextBox.Image = RenderBox(Messages[CUR_INDEX].spokenText);
            }

            directEdit.Dispose();
        }

        private void B_ExportNewScript_Click(object sender, EventArgs e)
        {
            string newScript = string.Empty;
            foreach(Message msg in Messages)
            {
                Tools.SaveLineToRaw(msg);
                newScript += msg.rawLine;
            }

            ScriptExport scriptExport = new ScriptExport(ValidCharacters, newScript);
            DialogResult dialogResult = scriptExport.ShowDialog();

            scriptExport.Dispose();
        }

        private void TB_ProtagName_TextChanged(object sender, EventArgs e)
        {
            PLAYER_NAME = TB_ProtagName.Text;
            ResetParameters();
            RefreshText();
            //TB_CurrentLine.Text = MESSAGES[CUR_INDEX].spokenText;
        }

        private void CB_Textbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int curIndex = TextboxIndex;
            TextboxIndex = CB_Textbox.SelectedIndex;
            
            if(curIndex != TextboxIndex)
            {
                ResetParameters();
                RefreshText();
                TB_CurrentLine.Text = Messages[CUR_INDEX].spokenText;
            }
        }

        private void CHK_Backgrounds_CheckedChanged(object sender, EventArgs e)
        {
            bool curChoice = USE_BACKGROUNDS;
            USE_BACKGROUNDS = CHK_Backgrounds.Checked;

            if (USE_BACKGROUNDS)
            {
                BACKGROUND_IMAGE = Resources.SupportBG.Clone() as Bitmap;

                MessageBox.Show("Backgrounds enabled." + Environment.NewLine + "Drag/drop an image onto the Picture Box to change the background." + Environment.NewLine + "Uncheck and Recheck this box to reset the background to the default one.", "Alert");
            }

            if(curChoice != USE_BACKGROUNDS)
            {
                ResetParameters();
                RefreshText();
                TB_CurrentLine.Text = Messages[CUR_INDEX].spokenText;
            }
        }
    }
}
