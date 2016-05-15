using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FEITS
{
    public partial class ScriptInput : Form
    {
        //private bool[] m_validCharacters;
        //private string m_enteredData;
        //public string enteredData { get { return m_enteredData; } }

        //public ScriptInput(bool[] validCharacters, string currentScript)
        //{
        //    InitializeComponent();
        //    m_validCharacters = validCharacters;

        //    //Grab the current script from the main form
        //    m_enteredData = currentScript;
        //    m_enteredData = m_enteredData.Replace("\n", "\\n");

        //    //And update the textbox
        //    TB_ScriptInput.Text = m_enteredData;
        //    TB_ScriptInput.SelectAll();
        //}

        //private void TB_ScriptInput_TextChanged(object sender, EventArgs e)
        //{
        //    bool containsInvalids = false;
        //    List<char> inv = new List<char>();
        //    foreach (char c in TB_ScriptInput.Text.Where(c => !m_validCharacters[Tools.GetValue(c)]))
        //    {
        //        if (!containsInvalids)
        //            containsInvalids = true;
        //        if (!inv.Contains(c))
        //            inv.Add(c);
        //    }

        //    if (containsInvalids)
        //    {
        //        LBL_Warning.Text = string.Format("Warning: Text contains one or more unsupported characters: {0}", string.Join(",", inv));
        //        LBL_Warning.Visible = true;
        //        B_LoadScript.Enabled = false;
        //        B_LoadScript.DialogResult = DialogResult.None;
        //    }
        //    else
        //    {
        //        LBL_Warning.Visible = false;
        //        B_LoadScript.Enabled = true;
        //        B_LoadScript.DialogResult = DialogResult.OK;
        //    }

        //    m_enteredData = TB_ScriptInput.Text;
        //    HandleEmptyTextBox();
        //}

        ///// <summary>
        ///// Checks if the text box is empty, and disables the Load button if so.
        ///// </summary>
        //private void HandleEmptyTextBox()
        //{
        //    if (m_enteredData == string.Empty)
        //    {
        //        B_LoadScript.Enabled = false;
        //    }
        //    else
        //    {
        //        B_LoadScript.Enabled = true;
        //    }
        //}

        //private void TB_ScriptInput_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Control && e.KeyCode == Keys.A)
        //        TB_ScriptInput.SelectAll();
        //}
    }
}
