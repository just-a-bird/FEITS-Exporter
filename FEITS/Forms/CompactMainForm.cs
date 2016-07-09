using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FEITS.Controller;
using FEITS.Model;

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

        #endregion

        private void LB_MessageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            cont.SetCurrentMessage();
        }

        private void RTB_CurrentLine_TextChanged(object sender, EventArgs e)
        {
            cont.OnMsgLineChanged();
        }

        private void B_PrevLine_Click(object sender, EventArgs e)
        {
            cont.PreviousLine();
        }

        private void B_NextLine_Click(object sender, EventArgs e)
        {
            cont.NextLine();
        }

        private void TB_CurrentPage_Leave(object sender, EventArgs e)
        {
            //Check for new number, change to page #
        }

        private void TB_PlayerName_TextChanged(object sender, EventArgs e)
        {
            cont.OnNameChanged();
        }

        private void MI_Open_Click(object sender, EventArgs e)
        {
            cont.OpenFile();
        }

        private void MI_Save_Click(object sender, EventArgs e)
        {
            cont.SaveFile();
        }

        private void MI_SaveAs_Click(object sender, EventArgs e)
        {
            cont.SaveFileAs();
        }

        private void MI_Import_Click(object sender, EventArgs e)
        {
            cont.ImportMessageScript();
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
    }
}
