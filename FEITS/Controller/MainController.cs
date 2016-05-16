using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FEITS.Model;
using FEITS.Properties;
using System.Windows.Forms;

namespace FEITS.Controller
{
    public class MainController
    {
        private IMainView mainView;
        private ConversationModel conv;
        private ParsedFileContainer fileCont = new ParsedFileContainer();

        public MainController(IMainView view, ConversationModel model)
        {
            mainView = view;
            conv = model;
            mainView.SetController(this);
            mainView.SetMessageList(fileCont.MessageList);
            SetOptions();
        }

        #region Menu Bar
        public void NewFile()
        {
            fileCont.EmptyFileData();
        }

        public void OpenFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            openDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (fileCont.FilePathAndName != string.Empty)
                openDialog.FileName = fileCont.FilePathAndName;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if(fileCont.LoadFromFile(openDialog.FileName))
                    {
                        Console.WriteLine("File opened successfully.");
                        mainView.SetMessageList(fileCont.MessageList);
                    }
                    else
                    {
                        Console.WriteLine("File could not be opened successfully.");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: Could not read the file. Original error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// If file path known, save to file.
        /// If not, open Save As dialog.
        /// </summary>
        public void SaveFile()
        {
            if(fileCont.FilePathAndName != string.Empty)
            {
                fileCont.SaveToFile(fileCont.FilePathAndName);
            }
            else
            {
                SaveFileAs();
            }
        }

        public void SaveFileAs()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Text files (*.txt)|*.txt|All files(*.*)|*.*";
            saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            if (fileCont.FilePathAndName != string.Empty)
                saveDialog.FileName = fileCont.FilePathAndName;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if(fileCont.SaveToFile(saveDialog.FileName))
                    {
                        Console.WriteLine("File saved successfully.");
                    }
                    else
                    {
                        Console.WriteLine("File could not be saved successfully.");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: Could not save file. Original error: " + ex.Message);
                }
            }
        }

        public void ImportMessageScript()
        {

        }

        public void ExportMessageScript()
        {

        }

        public void EditMessageScript()
        {

        }

        public void EditMessageLineScript()
        {

        }

        public void OpenHalfBoxEditor()
        {

        }

        public void ShowFriendlyReminder()
        {

        }
        #endregion

        private void SetOptions()
        {
            mainView.ProtagonistName = conv.PlayerName;
            mainView.CurrentTextbox = conv.TextboxIndex;
            mainView.EnableBackgrounds = conv.EnableBackgrounds;
        }

        public void SetCurrentMessage()
        {
            conv.CurrentMessage = fileCont.MessageList[mainView.MsgListIndex];
            SetCurrentLine();
        }

        public void NextLine()
        {
            conv.LineIndex++;
            SetCurrentLine();
        }

        public void PreviousLine()
        {
            conv.LineIndex--;
            conv.GetCommandsBeforeIndex();
            SetCurrentLine();
        }

        private void SetCurrentLine()
        {
            mainView.CurrentLine = conv.GetParsedCommands(conv.CurrentMessage.MessageLines[conv.LineIndex]);
            mainView.PrevLine = (conv.LineIndex > 0) ? true : false;
            mainView.NextLine = (conv.LineIndex < conv.CurrentMessage.MessageLines.Count - 1) ? true : false;
            mainView.ActiveCharacter = conv.CharActive;
            mainView.CharacterPortrait = (conv.CharSide == (0 | 2 | 3)) ? conv.CharA : conv.CharB;
            mainView.Emotion = (conv.CharActive == conv.CharA) ? conv.EmotionA : conv.EmotionB;
        }

        public void OnMsgLineChanged()
        {
            conv.CurrentMessage.MessageLines[conv.LineIndex].SpokenText = mainView.CurrentLine;

            if(conv.CurrentMessage != null)
                mainView.PreviewImage = conv.RenderPreviewBox(mainView.CurrentLine);
        }

        public void OnNameChanged()
        {
            conv.PlayerName = mainView.ProtagonistName;

            if (conv.CurrentMessage != null)
                mainView.PreviewImage = conv.RenderPreviewBox(mainView.CurrentLine);
        }

        public void OnTextboxChanged()
        {
            conv.TextboxIndex = mainView.CurrentTextbox;

            if (conv.CurrentMessage != null)
                mainView.PreviewImage = conv.RenderPreviewBox(mainView.CurrentLine);
        }

        public void OnBackgroundEnabledChanged()
        {
            conv.EnableBackgrounds = mainView.EnableBackgrounds;

            if (conv.CurrentMessage != null)
                mainView.PreviewImage = conv.RenderPreviewBox(mainView.CurrentLine);
        }
    }
}
