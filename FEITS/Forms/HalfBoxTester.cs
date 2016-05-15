using FEITS.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FEITS.Forms
{
    public partial class HalfBoxTester : Form
    {
        //private bool[] m_validCharacters;
        //private FontCharacter[] m_characters;

        //private Image[] m_textBoxes = { Resources.HalfBox, Resources.HalfBox_Nohr, Resources.HalfBox_Hoshido };

        //public HalfBoxTester(bool[] vc, FontCharacter[] c)
        //{
        //    InitializeComponent();

        //    m_validCharacters = vc;
        //    m_characters = c;
        //    CB_TB.Items.AddRange(new[] { "Standard", "Nohrian", "Hoshido" });
        //    CB_TB.SelectedIndex = 0;

        //    //LoadText
        //    string line = "This is an example\r\nmessage.";
        //    TB_Line.Text = line;
        //}

        //private void TB_Line_TextChanged(object sender, EventArgs e)
        //{
        //    HandleEmptyTextBox();
        //    UpdateBox();
        //}

        //private void UpdateBox()
        //{
        //    Image hb = m_textBoxes[CB_TB.SelectedIndex].Clone() as Bitmap;
        //    Image text = Tools.DrawString(m_characters, new Bitmap(165, 50), TB_Line.Text.Replace(Environment.NewLine, "\n"), 0, 22, Color.FromArgb(68, 8, 0)) as Bitmap;

        //    using (Graphics g = Graphics.FromImage(hb))
        //    {
        //        g.DrawImage(text, new Point(10, 0));
        //        g.DrawImage(Resources.KeyPress, new Point(PB_TextBox.Width - 30, PB_TextBox.Height - hb.Height + 32));
        //    }

        //    PB_TextBox.Image = hb;
        //}

        //private void HandleEmptyTextBox()
        //{
        //    if(TB_Line.Text == string.Empty)
        //    {
        //        B_Export.Enabled = false;
        //    }
        //    else
        //    {
        //        B_Export.Enabled = true;
        //    }
        //}

        //private void B_Export_Click(object sender, EventArgs e)
        //{
        //    if (TB_Line.Text == string.Empty)
        //        return;

        //    ScriptExport lineExport = new ScriptExport(m_validCharacters, TB_Line.Text.Replace(Environment.NewLine, "\n"));
        //    DialogResult dialogResult = lineExport.ShowDialog();

        //    lineExport.Dispose();
        //}

        //private void CB_TB_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    UpdateBox();
        //}

        //private void TB_Line_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Control && e.KeyCode == Keys.A)
        //        TB_Line.SelectAll();
        //}
    }
}
