using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FEITS
{
    public partial class ScriptExport : Form
    {
        private bool[] m_validCharacters;
        private string m_enteredData;
        public string enteredData { get { return m_enteredData; } }

        public ScriptExport(bool[] validCharacters, string currentScript)
        {
            InitializeComponent();
            m_validCharacters = validCharacters;

            //Grab the current script from the main form
            m_enteredData = currentScript;
            m_enteredData = m_enteredData.Replace("\n", "\\n");

            //And update the textbox
            TB_ScriptInput.Text = m_enteredData;
            TB_ScriptInput.SelectAll();
        }

        private void TB_ScriptInput_TextChanged(object sender, EventArgs e)
        {
            if(TB_ScriptInput.Text != string.Empty)
            {
                LBL_CharInfo.Text = string.Format("Character count: {0}", TB_ScriptInput.Text.Count());
            }
            else
            {
                LBL_CharInfo.Text = string.Empty;
            }
        }

        private void TB_ScriptInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
                TB_ScriptInput.SelectAll();
        }
    }
}
