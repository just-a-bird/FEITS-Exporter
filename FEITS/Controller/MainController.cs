using FEITS.Model;
using FEITS.View;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace FEITS.Controller
{
    public class MainController
    {
        private IMainView mainView;
        private ConversationModel conv;
        private ParsedFileContainer fileCont = new ParsedFileContainer();

        private OpenFileDialog ofd = new OpenFileDialog();
        private SaveFileDialog sfd = new SaveFileDialog();

        private bool reminderOpen;
        public bool ReminderOpen { set { reminderOpen = value; } }

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

        public bool OpenFile()
        {
            ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.FileName = string.Empty;

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (fileCont.LoadFromFile(ofd.FileName))
                    {
                        mainView.FormName = fileCont.FileName = Path.GetFileNameWithoutExtension(ofd.FileName);
                        mainView.SetMessageList(fileCont.MessageList);
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error: Could Not Read File");
                    return false;
                }
            }

            return false;
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

        public bool SaveFileAs()
        {
            sfd.Filter = "Text files (*.txt)|*.txt";
            sfd.FilterIndex = 1;

            if (fileCont.FileName != string.Empty)
                sfd.FileName = fileCont.FileName;

            if(sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (fileCont.SaveToFile(sfd.FileName))
                    {
                        mainView.FormName = fileCont.FileName = Path.GetFileNameWithoutExtension(sfd.FileName);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error: File Not Saved");
                    return false;
                }
            }

            return false;
        }

        public bool ImportMessageScript()
        {
            ScriptImport messageImporter = new ScriptImport();
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
                messageExporter.ShowDialog();
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

        public void ExportAllMessages()
        {
            ScriptExport messageExporter = new ScriptExport();
            try
            {
                ImportExportController exportCont = new ImportExportController(messageExporter, fileCont.CompileFileText());
                messageExporter.ShowDialog();
            }
            catch
            {
                MessageBox.Show("Nothing to export.", "Error");
            }
            finally
            {
                if (messageExporter != null)
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
                        fileCont.MessageList[mainView.MsgListIndex].MessageLines.Clear();
                        fileCont.MessageList[mainView.MsgListIndex].ParseMessage(editCont.MessageScript);
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
            conv.CurrentMessage.MessageLines[conv.LineIndex].UpdateRawWithNewDialogue();
            string rawLine = conv.CurrentMessage.MessageLines[conv.LineIndex].RawLine;
            rawLine = rawLine.Replace(Environment.NewLine, "\\n").Replace("\n", "\\n");

            using (DirectEdit lineEdit = new DirectEdit())
            {
                ImportExportController editCont = new ImportExportController(lineEdit, rawLine);
                DialogResult dialogResult = lineEdit.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    string newMessage = string.Empty;

                    //foreach (MessageLine msg in fileCont.MessageList[mainView.MsgListIndex].MessageLines)
                    //{
                    //    msg.UpdateRawWithNewDialogue();
                    //}

                    fileCont.MessageList[mainView.MsgListIndex].MessageLines[conv.LineIndex].RawLine = editCont.MessageScript;

                    foreach (MessageLine msg in fileCont.MessageList[mainView.MsgListIndex].MessageLines)
                    {
                        newMessage += msg.RawLine;
                    }

                    fileCont.MessageList[mainView.MsgListIndex].MessageLines.Clear();
                    fileCont.MessageList[mainView.MsgListIndex].ParseMessage(newMessage);
                    SetCurrentLine();
                }
            }
        }

        public void OpenHalfBoxEditor()
        {
            using (HalfBoxTester halfBox = new HalfBoxTester())
            {
                HalfBoxController hbCont = new HalfBoxController(halfBox);
                halfBox.ShowDialog();
            }
        }

        public void ShowFriendlyReminder()
        {
            if(!reminderOpen)
            {
                reminderOpen = true;
                FriendlyReminder reminder = new FriendlyReminder();
                reminder.SetController(this);
                reminder.Show();
            }
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

        public void StartLoadingAssets()
        {
            LoadingPopup loader = new LoadingPopup();
            loader.BeginLoading();
            loader.ShowDialog();
        }

        public void SetCurrentMessage()
        {
            conv.CurrentMessage = fileCont.MessageList[mainView.MsgListIndex];
            SetCurrentLine();

            mainView.PlayerGender = conv.CurrentMessage.Prefix.Contains("PCF") ? 1 : 0;
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
            try
            {
                conv.CurrentMessage.MessageLines[conv.LineIndex].SpokenText = mainView.CurrentLine;

                if (conv.CurrentMessage != null)
                    mainView.PreviewImage = conv.RenderPreviewBox(mainView.CurrentLine);
            }
            catch
            {
                
            }
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

        public void OnPlayerGenderChanged()
        {
            conv.PlayerGender = mainView.PlayerGender;

            if (conv.CurrentMessage != null)
                mainView.PreviewImage = conv.RenderPreviewBox(mainView.CurrentLine);
        }

        public void OnBackgroundEnabledChanged()
        {
            conv.EnableBackgrounds = mainView.EnableBackgrounds;

            if (conv.CurrentMessage != null)
                mainView.PreviewImage = conv.RenderPreviewBox(mainView.CurrentLine);

            if (mainView.EnableBackgrounds)
            {
                MessageBox.Show("Backgrounds enabled." + Environment.NewLine + "Drag/drop an image onto the Picture Box to change the background." + Environment.NewLine + "Disable and then re-enable to reset the background to the default one.", "Alert");
            }
        }

        public void HandleNewBackgroundImage(DragEventArgs e)
        {
            string file = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            if(File.Exists(file))
            {
                try
                {
                    Image img = Image.FromFile(file);
                    if(img.Width > 1 && img.Height > 1)
                    {
                        conv.BackgroundImage = img;

                        if (conv.CurrentMessage != null)
                            mainView.PreviewImage = conv.RenderPreviewBox(mainView.CurrentLine);
                    }
                }
                catch
                {
                    MessageBox.Show("This is not a valid background image.", "Error");
                }
            }
            else
            {
                //e.Effect = DragDropEffects.None;
            }
        }

        public void SavePreview(bool fullConversation)
        {
            sfd.Filter = "PNG Files (*.png)|*.png";
            
            if(fullConversation)
            {
                sfd.FileName = fileCont.FileName + "_Conversation";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    Image imageFile = conv.RenderConversation();
                    conv.GetCommandsBeforeIndex();
                    SetCurrentLine();

                    imageFile.Save(sfd.FileName, ImageFormat.Png);
                }
            }
            else
            {
                sfd.FileName = fileCont.FileName + "_Page" + mainView.CurrentPage.ToString();
                
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    Image imageFile = mainView.PreviewImage;
                    imageFile.Save(sfd.FileName, ImageFormat.Png);
                }
            }
        }
    }
}
