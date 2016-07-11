using FEITS.Controller;
using FEITS.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FEITS.View
{
    public partial class CompactMainForm : Form, IMainView
    {
        private MainController cont;

        public CompactMainForm()
        {
            InitializeComponent();
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
            get;
            set;
        }

        public int CurrentTextbox
        {
            get;
            set;
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
        }

        private void RTB_CurrentLine_TextChanged(object sender, EventArgs e)
        {
            if (RTB_CurrentLine.TextLength > 0)
            {
                RTB_CurrentLine.Enabled = true;
                cont.OnMsgLineChanged();
            }
            else
            {
                RTB_CurrentLine.Enabled = false;
            }
        }

        private void B_PrevLine_Click(object sender, EventArgs e)
        {
            cont.PreviousPage();
        }

        private void B_NextLine_Click(object sender, EventArgs e)
        {
            cont.NextPage();
        }

        private void TB_CurrentPage_Leave(object sender, EventArgs e)
        {
            cont.GotoPage(int.Parse(TB_CurrentPage.Text) - 1);
        }

        private void TB_PlayerName_TextChanged(object sender, EventArgs e)
        {
            cont.OnNameChanged();
        }

        private void MI_Open_Click(object sender, EventArgs e)
        {
            FD_Open.ShowDialog();
        }

        private void MI_Save_Click(object sender, EventArgs e)
        {
            if (!cont.SaveFile())
                FD_Save.ShowDialog();
            else
                ApplicationStatus = "File saved at " + DateTime.Now.ToShortTimeString();
        }

        private void MI_SaveAs_Click(object sender, EventArgs e)
        {
            FD_Save.ShowDialog();
        }

        private void MI_Import_Click(object sender, EventArgs e)
        {
            if (cont.ImportMessageScript())
            {
                FormName = "";
                FD_Save.FileName = string.Empty;
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

        private void MI_EditLine_Click(object sender, EventArgs e)
        {
            cont.EditMessageLineScript();
        }

        private void MI_Message_Click(object sender, EventArgs e)
        {
            cont.EditMessageScript();
        }

        private void MI_HalfBox_Click(object sender, EventArgs e)
        {
            cont.OpenHalfBoxEditor();
        }

        private void MI_Options_Click(object sender, EventArgs e)
        {
            cont.OpenSettingsMenu();
        }

        private void MI_Reminder_Click(object sender, EventArgs e)
        {
            cont.ShowFriendlyReminder();
        }

        private void M_Exit_Click(object sender, EventArgs e)
        {
            cont.ExitApplication();
        }

        private void FD_Open_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cont.OpenFile(FD_Open.FileName))
            {
                string fileName = FD_Open.SafeFileName.Replace(".txt", "");
                FD_Save.FileName = FormName = fileName;
                ApplicationStatus = "File opened successfully";
            }
            else
            {
                ApplicationStatus = "Error opening file";
            }
        }

        private void FD_Save_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cont.SaveFileAs(FD_Save.FileName))
                ApplicationStatus = "File saved at " + DateTime.Now.ToShortTimeString();
            else
                ApplicationStatus = "Could not save file";
        }

        private void TB_CurrentPage_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                cont.GotoPage(int.Parse(TB_CurrentPage.Text) - 1);
        }
    }
}
