using System;

namespace FEITS.Model
{
    /// <summary>
    /// Contains a single line of dialogue and its commands.
    /// </summary>
    public class MessageLine
    {
        public string RawLine = string.Empty;
        public string SpokenText = string.Empty;
        public int SpeechIndex = -1;

        //Don't know if this will work!
        public string CharacterA = string.Empty;
        public string CharacterB = string.Empty;
        public string CharacterActive = string.Empty;
        public int CharacterSide = -1;

        public const string DefaultEmotion = "通常,";
        public string Emotion = DefaultEmotion;

        public bool Wd;
        public bool HasPerms;
        public bool SetType;
        public bool CommandsParsed;

        public ConversationTypes ConversationType = ConversationTypes.Type1;

        private void ResetCommandFields()
        {
            CharacterActive = CharacterA = CharacterB = string.Empty;
            CharacterSide = -1;
            Emotion = DefaultEmotion;
            Wd = HasPerms = SetType = CommandsParsed = false;
        }

        public void ParseLineCommands()
        {
            ResetCommandFields();

            if(SpokenText != string.Empty)
            {
                UpdateRawWithNewDialogue();
                RawLine = RawLine.Replace("\\n", "\n").Replace("$k\n", "$k\\n");
            }

            SpokenText = RawLine;
            for(int i = 0; i < SpokenText.Length; i++)
            {
                if(SpokenText[i] == '$')
                {
                    Tuple<string, Command> res = MessageBlock.ParseCommand(SpokenText, i);
                    SpokenText = res.Item1;

                    if (res.Item2.numParams > 0)
                        if (res.Item2.Params[0] == "ベロア")
                            res.Item2.Params[0] = "べロア"; // Velour Fix

                    switch(res.Item2.cmd)
                    {
                        case "$E":
                            Emotion = res.Item2.Params[0];
                            break;
                        case "$Ws":
                            CharacterActive = res.Item2.Params[0];
                            break;
                        case "$Wm":
                            if(ConversationType == ConversationTypes.Type1)
                            {
                                if(res.Item2.Params[1] == "3")
                                {
                                    CharacterSide = 3;
                                    CharacterA = res.Item2.Params[0];
                                    Emotion = DefaultEmotion;
                                }
                                else if(res.Item2.Params[1] == "7")
                                {
                                    CharacterSide = 7;
                                    CharacterB = res.Item2.Params[0];
                                    Emotion = DefaultEmotion;
                                }
                            }
                            else
                            {
                                if(res.Item2.Params[1] == "0" || res.Item2.Params[1] == "2")
                                {
                                    CharacterSide = Convert.ToInt32(res.Item2.Params[1]);
                                    CharacterA = res.Item2.Params[0];
                                    Emotion = DefaultEmotion;
                                }
                                else if(res.Item2.Params[1] == "6")
                                {
                                    CharacterSide = 6;
                                    CharacterB = res.Item2.Params[0];
                                    Emotion = DefaultEmotion;
                                }
                            }
                            break;
                        case "$Wd":
                            
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
                            SpokenText = SpokenText.Substring(0, i) + "$Nu" + SpokenText.Substring(i);
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

            CommandsParsed = true;
            SpeechIndex = RawLine.IndexOf(SpokenText);
            RawLine = RawLine.Replace("\\n", "\n").Replace("$k\n", "$k\\n");
            SpokenText.Replace("\\n", "\n").Replace("$k\n", "$k\\n");
            SpokenText = SpokenText.Replace("\n", Environment.NewLine);
        }

        public void UpdateRawWithNewDialogue()
        {
            if(SpokenText != string.Empty)
            {
                SpokenText = SpokenText.Replace('’', '\'').Replace('~', '～').Replace("  ", " ");

                int lineIndex;
                if(RawLine.Contains("$Nu"))
                {
                    lineIndex = RawLine.IndexOf("$", RawLine.IndexOf("$Nu") + 2);
                }
                else
                {
                    lineIndex = RawLine.IndexOf("$", SpeechIndex);
                }

                string oldDialogue = string.Empty;
                if (lineIndex > SpeechIndex)
                    oldDialogue = RawLine.Substring(SpeechIndex, lineIndex - SpeechIndex);
                else
                    oldDialogue = RawLine.Substring(SpeechIndex);

                RawLine = RawLine.Replace(oldDialogue, SpokenText);
            }

            RawLine = RawLine.Replace(Environment.NewLine, "\\n");
        }
    }
}
