namespace FEITS
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.PB_TextBox = new System.Windows.Forms.PictureBox();
            this.TB_CurrentLine = new System.Windows.Forms.TextBox();
            this.B_ExportNewScript = new System.Windows.Forms.Button();
            this.B_LoadScript = new System.Windows.Forms.Button();
            this.B_HalfBox = new System.Windows.Forms.Button();
            this.B_Next = new System.Windows.Forms.Button();
            this.B_Prev = new System.Windows.Forms.Button();
            this.B_EditLineScript = new System.Windows.Forms.Button();
            this.LBL_ActiveChar = new System.Windows.Forms.Label();
            this.TB_ActiveChar = new System.Windows.Forms.TextBox();
            this.TB_Portrait = new System.Windows.Forms.TextBox();
            this.LBL_Portrait = new System.Windows.Forms.Label();
            this.LBL_Emotion = new System.Windows.Forms.Label();
            this.TB_Emotion = new System.Windows.Forms.TextBox();
            this.LBL_Title = new System.Windows.Forms.Label();
            this.TB_ProtagName = new System.Windows.Forms.TextBox();
            this.LBL_ProtagName = new System.Windows.Forms.Label();
            this.LBL_Textbox = new System.Windows.Forms.Label();
            this.CB_Textbox = new System.Windows.Forms.ComboBox();
            this.CHK_Backgrounds = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.PB_TextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // PB_TextBox
            // 
            this.PB_TextBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.PB_TextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_TextBox.Image = global::FEITS.Properties.Resources.TextBox;
            this.PB_TextBox.InitialImage = global::FEITS.Properties.Resources.TextBox;
            this.PB_TextBox.Location = new System.Drawing.Point(7, 12);
            this.PB_TextBox.MaximumSize = new System.Drawing.Size(400, 240);
            this.PB_TextBox.MinimumSize = new System.Drawing.Size(400, 240);
            this.PB_TextBox.Name = "PB_TextBox";
            this.PB_TextBox.Size = new System.Drawing.Size(400, 240);
            this.PB_TextBox.TabIndex = 0;
            this.PB_TextBox.TabStop = false;
            this.PB_TextBox.Click += new System.EventHandler(this.PB_TextBox_Click);
            this.PB_TextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.PB_TextBox_DragDrop);
            // 
            // TB_CurrentLine
            // 
            this.TB_CurrentLine.AcceptsReturn = true;
            this.TB_CurrentLine.AllowDrop = true;
            this.TB_CurrentLine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_CurrentLine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TB_CurrentLine.Location = new System.Drawing.Point(12, 416);
            this.TB_CurrentLine.Multiline = true;
            this.TB_CurrentLine.Name = "TB_CurrentLine";
            this.TB_CurrentLine.Size = new System.Drawing.Size(390, 77);
            this.TB_CurrentLine.TabIndex = 1;
            this.TB_CurrentLine.TextChanged += new System.EventHandler(this.TB_CurrentLine_TextChanged);
            this.TB_CurrentLine.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TB_CurrentLine_KeyDown);
            // 
            // B_ExportNewScript
            // 
            this.B_ExportNewScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_ExportNewScript.Location = new System.Drawing.Point(322, 499);
            this.B_ExportNewScript.Name = "B_ExportNewScript";
            this.B_ExportNewScript.Size = new System.Drawing.Size(80, 25);
            this.B_ExportNewScript.TabIndex = 5;
            this.B_ExportNewScript.Text = "Export";
            this.B_ExportNewScript.UseVisualStyleBackColor = true;
            this.B_ExportNewScript.Click += new System.EventHandler(this.B_ExportNewScript_Click);
            // 
            // B_LoadScript
            // 
            this.B_LoadScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.B_LoadScript.Location = new System.Drawing.Point(236, 499);
            this.B_LoadScript.Name = "B_LoadScript";
            this.B_LoadScript.Size = new System.Drawing.Size(80, 25);
            this.B_LoadScript.TabIndex = 4;
            this.B_LoadScript.Text = "Import";
            this.B_LoadScript.UseVisualStyleBackColor = true;
            this.B_LoadScript.Click += new System.EventHandler(this.B_LoadScript_Click);
            // 
            // B_HalfBox
            // 
            this.B_HalfBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_HalfBox.Location = new System.Drawing.Point(98, 499);
            this.B_HalfBox.Name = "B_HalfBox";
            this.B_HalfBox.Size = new System.Drawing.Size(80, 25);
            this.B_HalfBox.TabIndex = 7;
            this.B_HalfBox.Text = "Half-Box Edit";
            this.B_HalfBox.UseVisualStyleBackColor = true;
            this.B_HalfBox.Click += new System.EventHandler(this.B_Settings_Click);
            // 
            // B_Next
            // 
            this.B_Next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B_Next.Location = new System.Drawing.Point(302, 390);
            this.B_Next.Name = "B_Next";
            this.B_Next.Size = new System.Drawing.Size(100, 20);
            this.B_Next.TabIndex = 2;
            this.B_Next.Text = "----->";
            this.B_Next.UseVisualStyleBackColor = true;
            this.B_Next.Click += new System.EventHandler(this.B_Next_Click);
            // 
            // B_Prev
            // 
            this.B_Prev.Location = new System.Drawing.Point(12, 390);
            this.B_Prev.Name = "B_Prev";
            this.B_Prev.Size = new System.Drawing.Size(100, 20);
            this.B_Prev.TabIndex = 3;
            this.B_Prev.Text = "<-----";
            this.B_Prev.UseVisualStyleBackColor = true;
            this.B_Prev.Click += new System.EventHandler(this.B_Prev_Click);
            // 
            // B_EditLineScript
            // 
            this.B_EditLineScript.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.B_EditLineScript.Location = new System.Drawing.Point(12, 499);
            this.B_EditLineScript.Name = "B_EditLineScript";
            this.B_EditLineScript.Size = new System.Drawing.Size(80, 25);
            this.B_EditLineScript.TabIndex = 6;
            this.B_EditLineScript.Text = "Direct Edit";
            this.B_EditLineScript.UseVisualStyleBackColor = true;
            this.B_EditLineScript.Click += new System.EventHandler(this.B_EditLineScript_Click);
            // 
            // LBL_ActiveChar
            // 
            this.LBL_ActiveChar.AutoSize = true;
            this.LBL_ActiveChar.Location = new System.Drawing.Point(12, 287);
            this.LBL_ActiveChar.Name = "LBL_ActiveChar";
            this.LBL_ActiveChar.Size = new System.Drawing.Size(89, 13);
            this.LBL_ActiveChar.TabIndex = 21;
            this.LBL_ActiveChar.Text = "Active Character:";
            // 
            // TB_ActiveChar
            // 
            this.TB_ActiveChar.Location = new System.Drawing.Point(107, 284);
            this.TB_ActiveChar.Name = "TB_ActiveChar";
            this.TB_ActiveChar.ReadOnly = true;
            this.TB_ActiveChar.Size = new System.Drawing.Size(100, 20);
            this.TB_ActiveChar.TabIndex = 22;
            // 
            // TB_Portrait
            // 
            this.TB_Portrait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_Portrait.Location = new System.Drawing.Point(302, 284);
            this.TB_Portrait.Name = "TB_Portrait";
            this.TB_Portrait.ReadOnly = true;
            this.TB_Portrait.Size = new System.Drawing.Size(100, 20);
            this.TB_Portrait.TabIndex = 23;
            // 
            // LBL_Portrait
            // 
            this.LBL_Portrait.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LBL_Portrait.AutoSize = true;
            this.LBL_Portrait.Location = new System.Drawing.Point(209, 287);
            this.LBL_Portrait.Name = "LBL_Portrait";
            this.LBL_Portrait.Size = new System.Drawing.Size(92, 13);
            this.LBL_Portrait.TabIndex = 24;
            this.LBL_Portrait.Text = "Character Portrait:";
            // 
            // LBL_Emotion
            // 
            this.LBL_Emotion.AutoSize = true;
            this.LBL_Emotion.Location = new System.Drawing.Point(12, 320);
            this.LBL_Emotion.Name = "LBL_Emotion";
            this.LBL_Emotion.Size = new System.Drawing.Size(48, 13);
            this.LBL_Emotion.TabIndex = 26;
            this.LBL_Emotion.Text = "Emotion:";
            // 
            // TB_Emotion
            // 
            this.TB_Emotion.Location = new System.Drawing.Point(66, 317);
            this.TB_Emotion.Name = "TB_Emotion";
            this.TB_Emotion.ReadOnly = true;
            this.TB_Emotion.Size = new System.Drawing.Size(100, 20);
            this.TB_Emotion.TabIndex = 25;
            // 
            // LBL_Title
            // 
            this.LBL_Title.AutoSize = true;
            this.LBL_Title.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LBL_Title.Location = new System.Drawing.Point(12, 258);
            this.LBL_Title.Name = "LBL_Title";
            this.LBL_Title.Size = new System.Drawing.Size(109, 16);
            this.LBL_Title.TabIndex = 27;
            this.LBL_Title.Text = "Message Data";
            // 
            // TB_ProtagName
            // 
            this.TB_ProtagName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TB_ProtagName.Location = new System.Drawing.Point(302, 317);
            this.TB_ProtagName.Name = "TB_ProtagName";
            this.TB_ProtagName.Size = new System.Drawing.Size(100, 20);
            this.TB_ProtagName.TabIndex = 8;
            this.TB_ProtagName.TextChanged += new System.EventHandler(this.TB_ProtagName_TextChanged);
            // 
            // LBL_ProtagName
            // 
            this.LBL_ProtagName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LBL_ProtagName.AutoSize = true;
            this.LBL_ProtagName.Location = new System.Drawing.Point(202, 320);
            this.LBL_ProtagName.Name = "LBL_ProtagName";
            this.LBL_ProtagName.Size = new System.Drawing.Size(94, 13);
            this.LBL_ProtagName.TabIndex = 29;
            this.LBL_ProtagName.Text = "Protagonist Name:";
            // 
            // LBL_Textbox
            // 
            this.LBL_Textbox.AutoSize = true;
            this.LBL_Textbox.Location = new System.Drawing.Point(12, 351);
            this.LBL_Textbox.Name = "LBL_Textbox";
            this.LBL_Textbox.Size = new System.Drawing.Size(85, 13);
            this.LBL_Textbox.TabIndex = 30;
            this.LBL_Textbox.Text = "Current Textbox:";
            // 
            // CB_Textbox
            // 
            this.CB_Textbox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_Textbox.FormattingEnabled = true;
            this.CB_Textbox.Items.AddRange(new object[] {
            "Standard",
            "Nohr",
            "Hoshido"});
            this.CB_Textbox.Location = new System.Drawing.Point(103, 348);
            this.CB_Textbox.Name = "CB_Textbox";
            this.CB_Textbox.Size = new System.Drawing.Size(121, 21);
            this.CB_Textbox.TabIndex = 9;
            this.CB_Textbox.SelectedIndexChanged += new System.EventHandler(this.CB_Textbox_SelectedIndexChanged);
            // 
            // CHK_Backgrounds
            // 
            this.CHK_Backgrounds.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CHK_Backgrounds.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CHK_Backgrounds.Location = new System.Drawing.Point(252, 348);
            this.CHK_Backgrounds.Name = "CHK_Backgrounds";
            this.CHK_Backgrounds.Size = new System.Drawing.Size(150, 20);
            this.CHK_Backgrounds.TabIndex = 10;
            this.CHK_Backgrounds.Text = "Use Backgrounds:";
            this.CHK_Backgrounds.UseVisualStyleBackColor = true;
            this.CHK_Backgrounds.CheckedChanged += new System.EventHandler(this.CHK_Backgrounds_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(414, 536);
            this.Controls.Add(this.CHK_Backgrounds);
            this.Controls.Add(this.CB_Textbox);
            this.Controls.Add(this.LBL_Textbox);
            this.Controls.Add(this.LBL_ProtagName);
            this.Controls.Add(this.TB_ProtagName);
            this.Controls.Add(this.LBL_Title);
            this.Controls.Add(this.LBL_Emotion);
            this.Controls.Add(this.TB_Emotion);
            this.Controls.Add(this.LBL_Portrait);
            this.Controls.Add(this.TB_Portrait);
            this.Controls.Add(this.TB_ActiveChar);
            this.Controls.Add(this.LBL_ActiveChar);
            this.Controls.Add(this.B_EditLineScript);
            this.Controls.Add(this.B_Prev);
            this.Controls.Add(this.B_Next);
            this.Controls.Add(this.B_HalfBox);
            this.Controls.Add(this.B_LoadScript);
            this.Controls.Add(this.B_ExportNewScript);
            this.Controls.Add(this.TB_CurrentLine);
            this.Controls.Add(this.PB_TextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(430, 575);
            this.Name = "MainForm";
            this.Text = "FEITS Exporter";
            ((System.ComponentModel.ISupportInitialize)(this.PB_TextBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PB_TextBox;
        private System.Windows.Forms.TextBox TB_CurrentLine;
        private System.Windows.Forms.Button B_ExportNewScript;
        private System.Windows.Forms.Button B_LoadScript;
        private System.Windows.Forms.Button B_HalfBox;
        private System.Windows.Forms.Button B_Next;
        private System.Windows.Forms.Button B_Prev;
        private System.Windows.Forms.Button B_EditLineScript;
        private System.Windows.Forms.Label LBL_ActiveChar;
        private System.Windows.Forms.TextBox TB_ActiveChar;
        private System.Windows.Forms.TextBox TB_Portrait;
        private System.Windows.Forms.Label LBL_Portrait;
        private System.Windows.Forms.Label LBL_Emotion;
        private System.Windows.Forms.TextBox TB_Emotion;
        private System.Windows.Forms.Label LBL_Title;
        private System.Windows.Forms.Label LBL_ProtagName;
        private System.Windows.Forms.TextBox TB_ProtagName;
        private System.Windows.Forms.Label LBL_Textbox;
        private System.Windows.Forms.ComboBox CB_Textbox;
        private System.Windows.Forms.CheckBox CHK_Backgrounds;
    }
}

