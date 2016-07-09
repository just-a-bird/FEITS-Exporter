using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FEITS.Model;
using FEITS.View;
using System.Windows.Forms;
using System.ComponentModel;

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
                    if (fileCont.LoadFromFile(openDialog.FileName))
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
                    MessageBox.Show(ex.Message, "Error: Could Not Read File");
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
            ScriptInput messageImporter = new ScriptInput();
            ImportExportController importCont = new ImportExportController(messageImporter, "");
            DialogResult dialogResult = messageImporter.ShowDialog();

            if(dialogResult == DialogResult.OK)
            {
                if (fileCont.LoadFromString(importCont.MessageScript))
                {
                    Console.WriteLine("Message Imported Successfully.");
                    mainView.SetMessageList(fileCont.MessageList);
                }
                else
                {
                    MessageBox.Show("There was a problem importing the message. Please double-check the text and try again.", "Error");
                }
            }

            messageImporter.Dispose();
        }

        public void ExportMessageScript()
        {
            ScriptExport messageExporter = new ScriptExport();

            try
            {
                ImportExportController exportCont = new ImportExportController(messageExporter, conv.CurrentMessage.CompileMessage(false));
                DialogResult dialogResult = messageExporter.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Nothing to export.", "Error",);
            }

            messageExporter.Dispose();

        }

        public void EditMessageScript()
        {
            DirectEdit messageEdit = new DirectEdit();
            ImportExportController editCont = new ImportExportController(messageEdit, conv.CurrentMessage.CompileMessage(false));
            DialogResult dialogResult = messageEdit.ShowDialog();

            if(dialogResult == DialogResult.OK)
            {
                try
                {
                    MessageBlock updatedBlock = new MessageBlock();
                    updatedBlock.ParseMessage(editCont.MessageScript);
                    fileCont.MessageList[mainView.MsgListIndex] = updatedBlock;
                    SetCurrentMessage();
                }
                catch
                {
                    MessageBox.Show("There was a problem updating the message with new edits. Please try again.", "Error: Could Not Apply Edits");
                }
            }

            messageEdit.Dispose();
        }

        public void EditMessageLineScript()
        {
            string rawLine = conv.CurrentMessage.MessageLines[conv.LineIndex].RawLine;
            rawLine = rawLine.Replace(Environment.NewLine, "\\n").Replace("\n", "\\n");
            Console.WriteLine(rawLine);

            DirectEdit lineEdit = new DirectEdit();
            ImportExportController editCont = new ImportExportController(lineEdit, rawLine);
            DialogResult dialogResult = lineEdit.ShowDialog();

            if(dialogResult == DialogResult.OK)
            {
                string newMessage = string.Empty;

                foreach (MessageLine msg in fileCont.MessageList[mainView.MsgListIndex].MessageLines)
                {
                    msg.UpdateRawWithNewDialogue();
                }

                fileCont.MessageList[mainView.MsgListIndex].MessageLines[conv.LineIndex].RawLine = editCont.MessageScript;
                
                foreach(MessageLine msg in fileCont.MessageList[mainView.MsgListIndex].MessageLines)
                {
                    newMessage += msg.RawLine;
                }

                MessageBlock updatedBlock = new MessageBlock();
                updatedBlock.ParseMessage(newMessage);
                fileCont.MessageList[mainView.MsgListIndex] = updatedBlock;
                SetCurrentLine();
            }

            lineEdit.Dispose();
        }

        public void OpenHalfBoxEditor()
        {

        }

        public void OpenSettingsMenu()
        {

        }

        public void ShowFriendlyReminder()
        {

        }

        public void ExitApplication()
        {
            Application.Exit();
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
        }

        public void OnMsgLineChanged()
        {
            conv.CurrentMessage.MessageLines[conv.LineIndex].SpokenText = mainView.CurrentLine;

            if (conv.CurrentMessage != null)
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
