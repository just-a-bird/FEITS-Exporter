using FEITS.Model;
using FEITS.View;
using System;
using System.ComponentModel;
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
            StartLoadingAssets();
        }

        #region Menu Bar
        public void NewFile()
        {
            fileCont.EmptyFileData();
        }

        public bool OpenFile(string filePath)
        {
            try
            {
                if (fileCont.LoadFromFile(filePath))
                {
                    //Console.WriteLine("File opened successfully.");
                    mainView.SetMessageList(fileCont.MessageList);
                    return true;
                }
                else
                {
                    //Console.WriteLine("File could not be opened successfully.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error: Could Not Read File");
                return false;
            }
        }

        /// <summary>
        /// Tries to save to a previous filepath.
        /// Returns true if successful, false if not.
        /// </summary>
        public bool SaveFile()
        {
            if(fileCont.FilePath != string.Empty)
            {
                fileCont.SaveToFile(fileCont.FilePath);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SaveFileAs(string filePath)
        {
            try
            {
                if (fileCont.SaveToFile(filePath))
                {
                    //Console.WriteLine("File saved successfully.");
                    return true;
                }
                else
                {
                    //Console.WriteLine("File could not be saved successfully.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error: File Not Saved");
                return false;
            }
        }

        public bool ImportMessageScript()
        {
            ScriptInput messageImporter = new ScriptInput();
            try
            {
                ImportExportController importCont = new ImportExportController(messageImporter, "");
                DialogResult dialogResult = messageImporter.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    if (fileCont.LoadFromString(importCont.MessageScript))
                    {
                        mainView.SetMessageList(fileCont.MessageList);
                    }
                    else
                    {
                        MessageBox.Show("There was a problem parsing the message. Please double-check the text and try again.", "Error");
                        return false;
                    }
                }
            }
            finally
            {
                if (messageImporter != null)
                    messageImporter.Dispose();
            }
            return true;
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
                MessageBox.Show("Nothing to export.", "Error");
            }
            finally
            {
                if(messageExporter != null)
                    messageExporter.Dispose();
            }

        }

        public void EditMessageScript()
        {
            using (DirectEdit messageEdit = new DirectEdit())
            {
                ImportExportController editCont = new ImportExportController(messageEdit, conv.CurrentMessage.CompileMessage(false));
                DialogResult dialogResult = messageEdit.ShowDialog();

                if (dialogResult == DialogResult.OK)
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
            }
        }

        public void EditMessageLineScript()
        {
            string rawLine = conv.CurrentMessage.MessageLines[conv.LineIndex].RawLine;
            rawLine = rawLine.Replace(Environment.NewLine, "\\n").Replace("\n", "\\n");

            using (DirectEdit lineEdit = new DirectEdit())
            {
                ImportExportController editCont = new ImportExportController(lineEdit, rawLine);
                DialogResult dialogResult = lineEdit.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    string newMessage = string.Empty;

                    foreach (MessageLine msg in fileCont.MessageList[mainView.MsgListIndex].MessageLines)
                    {
                        msg.UpdateRawWithNewDialogue();
                    }

                    fileCont.MessageList[mainView.MsgListIndex].MessageLines[conv.LineIndex].RawLine = editCont.MessageScript;

                    foreach (MessageLine msg in fileCont.MessageList[mainView.MsgListIndex].MessageLines)
                    {
                        newMessage += msg.RawLine;
                    }

                    MessageBlock updatedBlock = new MessageBlock();
                    updatedBlock.ParseMessage(newMessage);
                    fileCont.MessageList[mainView.MsgListIndex] = updatedBlock;
                    SetCurrentLine();
                }
            }
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

        private void StartLoadingAssets()
        {
            LoadingPopup loader = new LoadingPopup();
            loader.BeginLoading();
            loader.ShowDialog();
        }

        public void SetCurrentMessage()
        {
            conv.CurrentMessage = fileCont.MessageList[mainView.MsgListIndex];
            SetCurrentLine();
        }

        public void NextPage()
        {
            conv.LineIndex++;
            SetCurrentLine();
        }

        public void PreviousPage()
        {
            conv.LineIndex--;
            conv.GetCommandsBeforeIndex();
            SetCurrentLine();
        }

        public void GotoPage(int page)
        {
            Console.WriteLine("New page number typed in; changing page!");
            if (page >= conv.CurrentMessage.MessageLines.Count)
                page = conv.CurrentMessage.MessageLines.Count - 1;

            conv.LineIndex = page;
            SetCurrentLine();
        }

        private void SetCurrentLine()
        {
            mainView.CurrentPage = conv.LineIndex;
            mainView.PageCount = conv.CurrentMessage.MessageLines.Count.ToString();
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
