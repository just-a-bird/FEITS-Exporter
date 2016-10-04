using FEITS.Controller;
using FEITS.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace FEITS.View
{
    public partial class CompactMainForm : Form, IMainView
    {
        private MainController cont;

        public CompactMainForm()
        {
            InitializeComponent();
            PB_PreviewBox.AllowDrop = true;
        }

        public void SetController(MainController controller)
        {
            cont = controller;
        }

        public void SetMessageList(List<MessageBlock> messages)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = messages;

            LB_MessageList.ValueMember = "Prefix";
            LB_MessageList.DataSource = bs;
        }

        #region Properties
        //Message controls
        public int MsgListIndex
        {
            get { return LB_MessageList.SelectedIndex; }
            set { LB_MessageList.SelectedIndex = value; }
        }

        public int CurrentPage
        {
            get { return int.Parse(TB_CurrentPage.Text); }
            set { TB_CurrentPage.Text = (value + 1).ToString(); }
        }

        public string CurrentLine
        {
            get { return RTB_CurrentLine.Text; }
            set { RTB_CurrentLine.Text = value; }
        }

        public bool PrevLine
        {
            get { return B_PrevLine.Enabled; }
            set { B_PrevLine.Enabled = value; }
        }

        public bool NextLine
        {
            get { return B_NextLine.Enabled; }
            set { B_NextLine.Enabled = value; }
        }

        public Image PreviewImage
        {
            get { return PB_PreviewBox.Image; }
            set { PB_PreviewBox.Image = value; }
        }

        //Settings
        public string ProtagonistName
        {
            get { return TB_PlayerName.Text; }
            set { TB_PlayerName.Text = value; }
        }

        public bool EnableBackgrounds
        {
            get { return MI_EnableBackgrounds.Checked; }
            set { MI_EnableBackgrounds.Checked = value; }
        }

        public int CurrentTextbox
        {
            get
            {
                foreach(ToolStripMenuItem mi in MI_TBStyles.DropDownItems)
                {
                    if (mi.Checked)
                        return MI_TBStyles.DropDownItems.IndexOf(mi);
                }

                return -1;
            }
            set
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)MI_TBStyles.DropDownItems[value];

                foreach(ToolStripMenuItem mi in MI_TBStyles.DropDownItems)
                {
                    if (mi == menuItem)
                        mi.Checked = true;
                    else
                        mi.Checked = false;
                }
            }
        }

        //Status
        public string FormName
        {
            get { return Text; }
            set
            {
                if (value != string.Empty)
                    Text = value + " - FEITS Exporter";
                else
                    Text = "FEITS Exporter";
            }
        }

        public string ApplicationStatus
        {
            set { TSL_Status.Text = value; }
        }

        public string PageCount
        {
            set { TSL_PageNumber.Text = "Page: " + CurrentPage.ToString() + " / " + value; }
        }

        #endregion

        private void LB_MessageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            cont.SetCurrentMessage();
            RTB_CurrentLine.Font = new Font(Font.OriginalFontName, 12f, FontStyle.Regular);
        }

        private void RTB_CurrentLine_TextChanged(object sender, EventArgs e)
        {
            if (RTB_CurrentLine.TextLength <= 0 && LB_MessageList.Items.Count <= 0)
            {
                RTB_CurrentLine.Enabled = false;
            }
            else
            {
                RTB_CurrentLine.Enabled = true;
                cont.OnMsgLineChanged();
                //Console.WriteLine(RTB_CurrentLine.Rtf);
            }
        }

        private void B_PrevLine_Click(object sender, EventArgs e)
        {
            cont.PreviousPage();
            RTB_CurrentLine.Font = new Font(Font.OriginalFontName, 12f, FontStyle.Regular);
        }

        private void B_NextLine_Click(object sender, EventArgs e)
        {
            cont.NextPage();
            RTB_CurrentLine.Font = new Font(Font.OriginalFontName, 12f, FontStyle.Regular);
        }

        private void TB_CurrentPage_Leave(object sender, EventArgs e)
        {
            cont.GotoPage(int.Parse(TB_CurrentPage.Text) - 1);
            RTB_CurrentLine.Font = new Font(Font.OriginalFontName, 12f, FontStyle.Regular);
        }

        private void TB_PlayerName_TextChanged(object sender, EventArgs e)
        {
            cont.OnNameChanged();
        }

        private void MI_Open_Click(object sender, EventArgs e)
        {
            if(cont.OpenFile())
                ApplicationStatus = "File opened successfully";
            else
                ApplicationStatus = "Error opening file";
        }

        private void MI_Save_Click(object sender, EventArgs e)
        {
            if (CurrentLine != string.Empty)
            {
                if (!cont.SaveFile())
                {
                    if (!cont.SaveFileAs())
                    {
                        MessageBox.Show("File could not be saved.", "Error");
                        ApplicationStatus = "Could not save file";
                        return;
                    }
                }

                ApplicationStatus = "File saved at " + DateTime.Now.ToShortTimeString();
            }
            else
            {
                MessageBox.Show("There is no file to save.", "Alert");
                return;
            }
        }

        private void MI_SaveAs_Click(object sender, EventArgs e)
        {
            if(CurrentLine != string.Empty)
            {
                if (cont.SaveFileAs())
                    ApplicationStatus = "File saved at " + DateTime.Now.ToShortTimeString();
                else
                    ApplicationStatus = "Could not save file";
            }
            else
            {
                MessageBox.Show("There is no file to save.", "Alert");
            }
        }

        private void MI_Import_Click(object sender, EventArgs e)
        {
            if (cont.ImportMessageScript())
            {
                FormName = "";
                //FD_Save.FileName = string.Empty;
                ApplicationStatus = "Import successful";
            }
            else
            {
                ApplicationStatus = "Import failed";
            }
        }

        private void MI_Export_Click(object sender, EventArgs e)
        {
            cont.ExportMessageScript();
        }

        private void exportAllMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cont.ExportAllMessages();
        }

        private void MI_EditLine_Click(object sender, EventArgs e)
        {
            cont.EditMessageLineScript();
        }

        private void MI_Message_Click(object sender, EventArgs e)
        {
            cont.EditMessageScript();
        }

        private void MI_TBItem_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            if (!item.Checked)
                return;

            foreach (ToolStripMenuItem mi in MI_TBStyles.DropDownItems)
            {
                if(mi != item)
                {
                    mi.Checked = false;
                }
            }

            cont.OnTextboxChanged();
        }

        private void MI_HalfBox_Click(object sender, EventArgs e)
        {
            cont.OpenHalfBoxEditor();
        }

        private void MI_Reminder_Click(object sender, EventArgs e)
        {
            cont.ShowFriendlyReminder();
        }

        private void M_Exit_Click(object sender, EventArgs e)
        {
            cont.ExitApplication();
        }

        private void TB_CurrentPage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                cont.GotoPage(int.Parse(TB_CurrentPage.Text) - 1);
        }

        private void CompactMainForm_Shown(object sender, EventArgs e)
        {
            cont.StartLoadingAssets();
        }

        private void MI_EnableBackgrounds_CheckedChanged(object sender, EventArgs e)
        {
            cont.OnBackgroundEnabledChanged();
        }

        private void PB_PreviewBox_Click(object sender, EventArgs e)
        {
            if((e as MouseEventArgs).Button == MouseButtons.Right)
            {
                if (cont.Prompt(MessageBoxButtons.YesNo, "Save the current conversation?") != DialogResult.Yes)
                    return;

                cont.SavePreview(true);
            }
            else if((e as MouseEventArgs).Button == MouseButtons.Left)
            {
                if (cont.Prompt(MessageBoxButtons.YesNo,"Save the current image?") != DialogResult.Yes)
                    return;

                cont.SavePreview(false);
            }
        }

        private void PB_PreviewBox_DragDrop(object sender, DragEventArgs e)
        {
            cont.HandleNewBackgroundImage(e);
        }

        private void PB_PreviewBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void LB_MessageList_MouseDown(object sender, MouseEventArgs e)
        {
            LB_MessageList.Focus();
        }

        private void RTB_CurrentLine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.Shift && e.KeyCode == Keys.Enter)
            {
                if (!RTB_CurrentLine.ReadOnly)
                    RTB_CurrentLine.ReadOnly = true;

                if (PrevLine)
                    cont.PreviousPage();
            }
            else if (e.Shift && e.KeyCode == Keys.Enter)
            {
                if (!RTB_CurrentLine.ReadOnly)
                    RTB_CurrentLine.ReadOnly = true;

                if (NextLine)
                    cont.NextPage();
            }
            //else if((e.Control || e.Shift) && e.KeyCode != Keys.Enter)
            //{
            //    if (RTB_CurrentLine.ReadOnly)
            //        RTB_CurrentLine.ReadOnly = false;
            //}
            //else if(e.Control || e.Shift)
            //{
            //    if(!RTB_CurrentLine.ReadOnly)
            //        RTB_CurrentLine.ReadOnly = true;
            //}
            else
            {
                if (RTB_CurrentLine.ReadOnly)
                    RTB_CurrentLine.ReadOnly = false;
            }
        }
    }
}
