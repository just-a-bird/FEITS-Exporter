using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FEITS.Controller;
using FEITS.Model;

namespace FEITS.View
{
    public partial class MainForm : Form, IMainView
    {
        private MainController cont;

        public MainForm()
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
        public int CurrentPage { get; set; }

        public string CurrentLine
        {
            get { return TB_CurrentLine.Text; }
            set { TB_CurrentLine.Text = value; }
        }

        public string ActiveCharacter
        {
            get { return TB_ActiveChar.Text; }
            set { TB_ActiveChar.Text = value; }
        }

        public string CharacterPortrait
        {
            get { return TB_Portrait.Text; }
            set { TB_Portrait.Text = value; }
        }

        public string Emotion
        {
            get { return TB_Emotion.Text; }
            set { TB_Emotion.Text = value; }
        }

        public bool PrevLine
        {
            get { return B_Prev.Enabled; }
            set { B_Prev.Enabled = value; }
        }

        public bool NextLine
        {
            get { return B_Next.Enabled; }
            set { B_Next.Enabled = value; }
        }

        public Image PreviewImage
        {
            get { return PB_TextBox.Image; }
            set { PB_TextBox.Image = value; }
        }

        //Settings
        public string ProtagonistName
        {
            get { return TB_ProtagName.Text; }
            set { TB_ProtagName.Text = value; }
        }

        public bool EnableBackgrounds
        {
            get { return CHK_Backgrounds.Checked; }
            set { CHK_Backgrounds.Checked = value; }
        }

        public int PlayerGender
        {
            get;
            set;
        }

        public int CurrentTextbox
        {
            get { return CB_Textbox.SelectedIndex; }
            set { CB_Textbox.SelectedIndex = value; }
        }

        //Status
        public string FormName
        {
            set
            {
                if (value != string.Empty)
                    Text = value + " - FEITS Exporter";
                else
                    Text = "FEITS Exporter";
            }
        }

        public string ApplicationStatus { get; set; }
        public string PageCount { get; set; }
        public string CharacterCount { get; set; }
        #endregion

        private void LB_MessageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            cont.SetCurrentMessage();
        }

        private void TB_CurrentLine_TextChanged(object sender, EventArgs e)
        {
            cont.OnMsgLineChanged();
        }

        private void B_Prev_Click(object sender, EventArgs e)
        {
            cont.PreviousPage();
        }

        private void B_Next_Click(object sender, EventArgs e)
        {
            cont.NextPage();
        }

        private void TB_ProtagName_TextChanged(object sender, EventArgs e)
        {
            cont.OnNameChanged();
        }

        private void CB_Textbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            cont.OnTextboxChanged();
        }

        private void CHK_Backgrounds_CheckedChanged(object sender, EventArgs e)
        {
            cont.OnBackgroundEnabledChanged();
        }

        //private void openToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    cont.OpenFile();
        //}

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cont.SaveFile();
        }

        //private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    if(cont.SaveFileAs())
        //}
    }
}
