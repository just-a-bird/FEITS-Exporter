namespace FEITS
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directBlockEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directLineEditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.halfBoxEditorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.B_NextBlock = new System.Windows.Forms.Button();
            this.B_PrevBlock = new System.Windows.Forms.Button();
            this.LB_BlockList = new System.Windows.Forms.ListBox();
            this.CHK_Backgrounds = new System.Windows.Forms.CheckBox();
            this.CB_Textbox = new System.Windows.Forms.ComboBox();
            this.LBL_Textbox = new System.Windows.Forms.Label();
            this.LBL_ProtagName = new System.Windows.Forms.Label();
            this.TB_ProtagName = new System.Windows.Forms.TextBox();
            this.LBL_Emotion = new System.Windows.Forms.Label();
            this.TB_Emotion = new System.Windows.Forms.TextBox();
            this.LBL_Portrait = new System.Windows.Forms.Label();
            this.TB_Portrait = new System.Windows.Forms.TextBox();
            this.TB_ActiveChar = new System.Windows.Forms.TextBox();
            this.LBL_ActiveChar = new System.Windows.Forms.Label();
            this.B_Prev = new System.Windows.Forms.Button();
            this.B_Next = new System.Windows.Forms.Button();
            this.LBL_MessageInfo = new System.Windows.Forms.Label();
            this.TB_CurrentLine = new System.Windows.Forms.TextBox();
            this.PB_TextBox = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_TextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(564, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem1
            // 
            this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
            this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem1.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.openToolStripMenuItem.Text = "&Open Script File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.saveToolStripMenuItem.Text = "&Save File";
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+S";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.saveAsToolStripMenuItem.Text = "Save File &As";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(209, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.printToolStripMenuItem.Text = "&Import Script Block";
            // 
            // printPreviewToolStripMenuItem
            // 
            this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
            this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.printPreviewToolStripMenuItem.Text = "&Export Script Block";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(209, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(212, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.directBlockEditToolStripMenuItem,
            this.directLineEditToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "&Edit";
            // 
            // directBlockEditToolStripMenuItem
            // 
            this.directBlockEditToolStripMenuItem.Name = "directBlockEditToolStripMenuItem";
            this.directBlockEditToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.directBlockEditToolStripMenuItem.Text = "Direct &Block Edit";
            // 
            // directLineEditToolStripMenuItem
            // 
            this.directLineEditToolStripMenuItem.Name = "directLineEditToolStripMenuItem";
            this.directLineEditToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.directLineEditToolStripMenuItem.Text = "Direct &Line Edit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.halfBoxEditorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // halfBoxEditorToolStripMenuItem
            // 
            this.halfBoxEditorToolStripMenuItem.Name = "halfBoxEditorToolStripMenuItem";
            this.halfBoxEditorToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.halfBoxEditorToolStripMenuItem.Text = "&Half-Box Editor";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.aboutToolStripMenuItem.Text = "Friendly &Reminder";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.B_NextBlock);
            this.splitContainer1.Panel1.Controls.Add(this.B_PrevBlock);
            this.splitContainer1.Panel1.Controls.Add(this.LB_BlockList);
            this.splitContainer1.Panel1MinSize = 130;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.CHK_Backgrounds);
            this.splitContainer1.Panel2.Controls.Add(this.CB_Textbox);
            this.splitContainer1.Panel2.Controls.Add(this.LBL_Textbox);
            this.splitContainer1.Panel2.Controls.Add(this.LBL_ProtagName);
            this.splitContainer1.Panel2.Controls.Add(this.TB_ProtagName);
            this.splitContainer1.Panel2.Controls.Add(this.LBL_Emotion);
            this.splitContainer1.Panel2.Controls.Add(this.TB_Emotion);
            this.splitContainer1.Panel2.Controls.Add(this.LBL_Portrait);
            this.splitContainer1.Panel2.Controls.Add(this.TB_Portrait);
            this.splitContainer1.Panel2.Controls.Add(this.TB_ActiveChar);
            this.splitContainer1.Panel2.Controls.Add(this.LBL_ActiveChar);
            this.splitContainer1.Panel2.Controls.Add(this.B_Prev);
            this.splitContainer1.Panel2.Controls.Add(this.B_Next);
            this.splitContainer1.Panel2.Controls.Add(this.LBL_MessageInfo);
            this.splitContainer1.Panel2.Controls.Add(this.TB_CurrentLine);
            this.splitContainer1.Panel2.Controls.Add(this.PB_TextBox);
            this.splitContainer1.Panel2MinSize = 430;
            this.splitContainer1.Size = new System.Drawing.Size(564, 512);
            this.splitContainer1.SplitterDistance = 130;
            this.splitContainer1.TabIndex = 1;
            // 
            // B_NextBlock
            // 
            this.B_NextBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_NextBlock.Location = new System.Drawing.Point(65, 469);
            this.B_NextBlock.Name = "B_NextBlock";
            this.B_NextBlock.Size = new System.Drawing.Size(60, 30);
            this.B_NextBlock.TabIndex = 3;
            this.B_NextBlock.Text = "Next";
            this.B_NextBlock.UseVisualStyleBackColor = true;
            // 
            // B_PrevBlock
            // 
            this.B_PrevBlock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_PrevBlock.Location = new System.Drawing.Point(3, 469);
            this.B_PrevBlock.Name = "B_PrevBlock";
            this.B_PrevBlock.Size = new System.Drawing.Size(60, 30);
            this.B_PrevBlock.TabIndex = 2;
            this.B_PrevBlock.Text = "Prev";
            this.B_PrevBlock.UseVisualStyleBackColor = true;
            // 
            // LB_BlockList
            // 
            this.LB_BlockList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LB_BlockList.FormattingEnabled = true;
            this.LB_BlockList.Location = new System.Drawing.Point(3, 3);
            this.LB_BlockList.Name = "LB_BlockList";
            this.LB_BlockList.Size = new System.Drawing.Size(122, 459);
            this.LB_BlockList.TabIndex = 0;
            // 
            // CHK_Backgrounds
            // 
            this.CHK_Backgrounds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CHK_Backgrounds.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CHK_Backgrounds.Location = new System.Drawing.Point(266, 354);
            this.CHK_Backgrounds.Name = "CHK_Backgrounds";
            this.CHK_Backgrounds.Size = new System.Drawing.Size(150, 20);
            this.CHK_Backgrounds.TabIndex = 35;
            this.CHK_Backgrounds.Text = "Use Backgrounds:";
            this.CHK_Backgrounds.UseVisualStyleBackColor = true;
            // 
            // CB_Textbox
            // 
            this.CB_Textbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Textbox.FormattingEnabled = true;
            this.CB_Textbox.Items.AddRange(new object[] {
            "Standard",
            "Nohr",
            "Hoshido"});
            this.CB_Textbox.Location = new System.Drawing.Point(101, 354);
            this.CB_Textbox.Name = "CB_Textbox";
            this.CB_Textbox.Size = new System.Drawing.Size(121, 21);
            this.CB_Textbox.TabIndex = 34;
            // 
            // LBL_Textbox
            // 
            this.LBL_Textbox.AutoSize = true;
            this.LBL_Textbox.Location = new System.Drawing.Point(10, 357);
            this.LBL_Textbox.Name = "LBL_Textbox";
            this.LBL_Textbox.Size = new System.Drawing.Size(85, 13);
            this.LBL_Textbox.TabIndex = 43;
            this.LBL_Textbox.Text = "Current Textbox:";
            // 
            // LBL_ProtagName
            // 
            this.LBL_ProtagName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LBL_ProtagName.AutoSize = true;
            this.LBL_ProtagName.Location = new System.Drawing.Point(216, 322);
            this.LBL_ProtagName.Name = "LBL_ProtagName";
            this.LBL_ProtagName.Size = new System.Drawing.Size(94, 13);
            this.LBL_ProtagName.TabIndex = 42;
            this.LBL_ProtagName.Text = "Protagonist Name:";
            // 
            // TB_ProtagName
            // 
            this.TB_ProtagName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_ProtagName.Location = new System.Drawing.Point(316, 319);
            this.TB_ProtagName.Name = "TB_ProtagName";
            this.TB_ProtagName.Size = new System.Drawing.Size(100, 20);
            this.TB_ProtagName.TabIndex = 33;
            // 
            // LBL_Emotion
            // 
            this.LBL_Emotion.AutoSize = true;
            this.LBL_Emotion.Location = new System.Drawing.Point(10, 322);
            this.LBL_Emotion.Name = "LBL_Emotion";
            this.LBL_Emotion.Size = new System.Drawing.Size(48, 13);
            this.LBL_Emotion.TabIndex = 41;
            this.LBL_Emotion.Text = "Emotion:";
            // 
            // TB_Emotion
            // 
            this.TB_Emotion.Location = new System.Drawing.Point(64, 319);
            this.TB_Emotion.Name = "TB_Emotion";
            this.TB_Emotion.ReadOnly = true;
            this.TB_Emotion.Size = new System.Drawing.Size(100, 20);
            this.TB_Emotion.TabIndex = 40;
            // 
            // LBL_Portrait
            // 
            this.LBL_Portrait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LBL_Portrait.AutoSize = true;
            this.LBL_Portrait.Location = new System.Drawing.Point(218, 286);
            this.LBL_Portrait.Name = "LBL_Portrait";
            this.LBL_Portrait.Size = new System.Drawing.Size(92, 13);
            this.LBL_Portrait.TabIndex = 39;
            this.LBL_Portrait.Text = "Character Portrait:";
            // 
            // TB_Portrait
            // 
            this.TB_Portrait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Portrait.Location = new System.Drawing.Point(316, 283);
            this.TB_Portrait.Name = "TB_Portrait";
            this.TB_Portrait.ReadOnly = true;
            this.TB_Portrait.Size = new System.Drawing.Size(100, 20);
            this.TB_Portrait.TabIndex = 38;
            // 
            // TB_ActiveChar
            // 
            this.TB_ActiveChar.Location = new System.Drawing.Point(105, 283);
            this.TB_ActiveChar.Name = "TB_ActiveChar";
            this.TB_ActiveChar.ReadOnly = true;
            this.TB_ActiveChar.Size = new System.Drawing.Size(100, 20);
            this.TB_ActiveChar.TabIndex = 37;
            // 
            // LBL_ActiveChar
            // 
            this.LBL_ActiveChar.AutoSize = true;
            this.LBL_ActiveChar.Location = new System.Drawing.Point(10, 286);
            this.LBL_ActiveChar.Name = "LBL_ActiveChar";
            this.LBL_ActiveChar.Size = new System.Drawing.Size(89, 13);
            this.LBL_ActiveChar.TabIndex = 36;
            this.LBL_ActiveChar.Text = "Active Character:";
            // 
            // B_Prev
            // 
            this.B_Prev.Location = new System.Drawing.Point(13, 399);
            this.B_Prev.Name = "B_Prev";
            this.B_Prev.Size = new System.Drawing.Size(100, 20);
            this.B_Prev.TabIndex = 32;
            this.B_Prev.Text = "<-----";
            this.B_Prev.UseVisualStyleBackColor = true;
            // 
            // B_Next
            // 
            this.B_Next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Next.Location = new System.Drawing.Point(316, 399);
            this.B_Next.Name = "B_Next";
            this.B_Next.Size = new System.Drawing.Size(100, 20);
            this.B_Next.TabIndex = 31;
            this.B_Next.Text = "----->";
            this.B_Next.UseVisualStyleBackColor = true;
            // 
            // LBL_MessageInfo
            // 
            this.LBL_MessageInfo.AutoSize = true;
            this.LBL_MessageInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_MessageInfo.Location = new System.Drawing.Point(10, 251);
            this.LBL_MessageInfo.Name = "LBL_MessageInfo";
            this.LBL_MessageInfo.Size = new System.Drawing.Size(152, 16);
            this.LBL_MessageInfo.TabIndex = 2;
            this.LBL_MessageInfo.Text = "Message Information";
            // 
            // TB_CurrentLine
            // 
            this.TB_CurrentLine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_CurrentLine.Location = new System.Drawing.Point(13, 425);
            this.TB_CurrentLine.Multiline = true;
            this.TB_CurrentLine.Name = "TB_CurrentLine";
            this.TB_CurrentLine.Size = new System.Drawing.Size(403, 73);
            this.TB_CurrentLine.TabIndex = 1;
            // 
            // PB_TextBox
            // 
            this.PB_TextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PB_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_TextBox.Location = new System.Drawing.Point(16, 3);
            this.PB_TextBox.Name = "PB_TextBox";
            this.PB_TextBox.Size = new System.Drawing.Size(400, 240);
            this.PB_TextBox.TabIndex = 0;
            this.PB_TextBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 536);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(580, 575);
            this.Name = "MainForm";
            this.Text = "FEITS File Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PB_TextBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directBlockEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directLineEditToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem halfBoxEditorToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox PB_TextBox;
        private System.Windows.Forms.TextBox TB_CurrentLine;
        private System.Windows.Forms.ListBox LB_BlockList;
        private System.Windows.Forms.Button B_PrevBlock;
        private System.Windows.Forms.Button B_NextBlock;
        private System.Windows.Forms.Label LBL_MessageInfo;
        private System.Windows.Forms.CheckBox CHK_Backgrounds;
        private System.Windows.Forms.ComboBox CB_Textbox;
        private System.Windows.Forms.Label LBL_Textbox;
        private System.Windows.Forms.Label LBL_ProtagName;
        private System.Windows.Forms.TextBox TB_ProtagName;
        private System.Windows.Forms.Label LBL_Emotion;
        private System.Windows.Forms.TextBox TB_Emotion;
        private System.Windows.Forms.Label LBL_Portrait;
        private System.Windows.Forms.TextBox TB_Portrait;
        private System.Windows.Forms.TextBox TB_ActiveChar;
        private System.Windows.Forms.Label LBL_ActiveChar;
        private System.Windows.Forms.Button B_Prev;
        private System.Windows.Forms.Button B_Next;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}