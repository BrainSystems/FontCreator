namespace FontCreator
{
    partial class FontCreatorForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            buttonLoadFont = new Button();
            pictureBoxCharacter = new PictureBox();
            listViewCharacters = new ListView();
            buttonExportFont = new Button();
            textBoxExcludeChars = new TextBox();
            labelExcludeChars = new Label();
            textBoxFirstCharcter = new TextBox();
            label1 = new Label();
            textBoxLastChar = new TextBox();
            label2 = new Label();
            textBoxFontName = new TextBox();
            label3 = new Label();
            textBoxSpaceCharAdjust = new TextBox();
            label4 = new Label();
            buttonSetExportDirectory = new Button();
            textBoxExportDir = new TextBox();
            textBoxInfo = new TextBox();
            label5 = new Label();
            fontListFont = new GLCD_FontCreator.CustomFontDialog.FontList();
            groupBox3 = new GroupBox();
            lblSelFont = new Label();
            groupBox2 = new GroupBox();
            pictureBoxSampleText = new PictureBox();
            groupBox1 = new GroupBox();
            chbStrikeout = new CheckBox();
            chbBold = new CheckBox();
            chbItalic = new CheckBox();
            textBoxFontSize = new TextBox();
            listBoxFontSIze = new ListBox();
            label6 = new Label();
            comboBoxCodePage = new ComboBox();
            label7 = new Label();
            comboBoxExportFormat = new ComboBox();
            label8 = new Label();
            pictureBoxString = new PictureBox();
            label9 = new Label();
            label10 = new Label();
            textBoxCharSpacing = new TextBox();
            label11 = new Label();
            buttonAbout = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCharacter).BeginInit();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSampleText).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxString).BeginInit();
            SuspendLayout();
            // 
            // buttonLoadFont
            // 
            buttonLoadFont.Location = new Point(26, 733);
            buttonLoadFont.Name = "buttonLoadFont";
            buttonLoadFont.Size = new Size(542, 49);
            buttonLoadFont.TabIndex = 0;
            buttonLoadFont.Text = "Load Font";
            buttonLoadFont.UseVisualStyleBackColor = true;
            buttonLoadFont.Click += buttonLoadFontClicked;
            // 
            // pictureBoxCharacter
            // 
            pictureBoxCharacter.Location = new Point(594, 16);
            pictureBoxCharacter.Name = "pictureBoxCharacter";
            pictureBoxCharacter.Size = new Size(688, 701);
            pictureBoxCharacter.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxCharacter.TabIndex = 2;
            pictureBoxCharacter.TabStop = false;
            pictureBoxCharacter.Paint += pictureBoxCharacter_Paint;
            // 
            // listViewCharacters
            // 
            listViewCharacters.FullRowSelect = true;
            listViewCharacters.Location = new Point(1288, 16);
            listViewCharacters.MultiSelect = false;
            listViewCharacters.Name = "listViewCharacters";
            listViewCharacters.Size = new Size(390, 701);
            listViewCharacters.TabIndex = 3;
            listViewCharacters.UseCompatibleStateImageBehavior = false;
            listViewCharacters.SelectedIndexChanged += listViewCharacters_SelectedIndexChanged;
            // 
            // buttonExportFont
            // 
            buttonExportFont.Enabled = false;
            buttonExportFont.Location = new Point(1338, 1124);
            buttonExportFont.Name = "buttonExportFont";
            buttonExportFont.Size = new Size(226, 71);
            buttonExportFont.TabIndex = 4;
            buttonExportFont.Text = "Export Font";
            buttonExportFont.UseVisualStyleBackColor = true;
            buttonExportFont.Click += buttonExportFont_Click;
            // 
            // textBoxExcludeChars
            // 
            textBoxExcludeChars.BorderStyle = BorderStyle.FixedSingle;
            textBoxExcludeChars.Location = new Point(26, 555);
            textBoxExcludeChars.Multiline = true;
            textBoxExcludeChars.Name = "textBoxExcludeChars";
            textBoxExcludeChars.Size = new Size(542, 76);
            textBoxExcludeChars.TabIndex = 5;
            // 
            // labelExcludeChars
            // 
            labelExcludeChars.AutoSize = true;
            labelExcludeChars.Location = new Point(26, 532);
            labelExcludeChars.Name = "labelExcludeChars";
            labelExcludeChars.Size = new Size(537, 20);
            labelExcludeChars.TabIndex = 6;
            labelExcludeChars.Text = "Exclude chars (eg. 127, 204-210, 215), Substitute chars (eg 155:127, 200-222:127)";
            // 
            // textBoxFirstCharcter
            // 
            textBoxFirstCharcter.Location = new Point(26, 421);
            textBoxFirstCharcter.Name = "textBoxFirstCharcter";
            textBoxFirstCharcter.Size = new Size(98, 27);
            textBoxFirstCharcter.TabIndex = 7;
            textBoxFirstCharcter.Text = "32";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 397);
            label1.Name = "label1";
            label1.Size = new Size(68, 20);
            label1.TabIndex = 8;
            label1.Text = "First char";
            // 
            // textBoxLastChar
            // 
            textBoxLastChar.Location = new Point(166, 421);
            textBoxLastChar.Name = "textBoxLastChar";
            textBoxLastChar.Size = new Size(100, 27);
            textBoxLastChar.TabIndex = 9;
            textBoxLastChar.Text = "255";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(166, 397);
            label2.Name = "label2";
            label2.Size = new Size(67, 20);
            label2.TabIndex = 10;
            label2.Text = "Last char";
            // 
            // textBoxFontName
            // 
            textBoxFontName.Location = new Point(580, 1147);
            textBoxFontName.Name = "textBoxFontName";
            textBoxFontName.Size = new Size(460, 27);
            textBoxFontName.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(580, 1124);
            label3.Name = "label3";
            label3.Size = new Size(306, 20);
            label3.TabIndex = 12;
            label3.Text = "Font name (will be exported into header file)";
            // 
            // textBoxSpaceCharAdjust
            // 
            textBoxSpaceCharAdjust.Location = new Point(235, 648);
            textBoxSpaceCharAdjust.Name = "textBoxSpaceCharAdjust";
            textBoxSpaceCharAdjust.Size = new Size(67, 27);
            textBoxSpaceCharAdjust.TabIndex = 13;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(26, 651);
            label4.Name = "label4";
            label4.Size = new Size(203, 20);
            label4.TabIndex = 14;
            label4.Text = "Space char width adjustment:";
            // 
            // buttonSetExportDirectory
            // 
            buttonSetExportDirectory.Location = new Point(26, 1124);
            buttonSetExportDirectory.Name = "buttonSetExportDirectory";
            buttonSetExportDirectory.Size = new Size(536, 38);
            buttonSetExportDirectory.TabIndex = 15;
            buttonSetExportDirectory.Text = "Set Export Directory";
            buttonSetExportDirectory.UseVisualStyleBackColor = true;
            buttonSetExportDirectory.Click += buttonSetExportDirectory_Click;
            // 
            // textBoxExportDir
            // 
            textBoxExportDir.Location = new Point(26, 1168);
            textBoxExportDir.Name = "textBoxExportDir";
            textBoxExportDir.ReadOnly = true;
            textBoxExportDir.Size = new Size(536, 27);
            textBoxExportDir.TabIndex = 16;
            // 
            // textBoxInfo
            // 
            textBoxInfo.Location = new Point(641, 755);
            textBoxInfo.Name = "textBoxInfo";
            textBoxInfo.ReadOnly = true;
            textBoxInfo.Size = new Size(1043, 27);
            textBoxInfo.TabIndex = 17;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(594, 755);
            label5.Name = "label5";
            label5.Size = new Size(35, 20);
            label5.TabIndex = 18;
            label5.Text = "Info";
            // 
            // fontListFont
            // 
            fontListFont.Location = new Point(26, 12);
            fontListFont.Margin = new Padding(4, 5, 4, 5);
            fontListFont.Name = "fontListFont";
            fontListFont.SelectedFontFamily = null;
            fontListFont.Size = new Size(240, 385);
            fontListFont.TabIndex = 19;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(lblSelFont);
            groupBox3.Location = new Point(293, 230);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(275, 55);
            groupBox3.TabIndex = 25;
            groupBox3.TabStop = false;
            groupBox3.Text = "Selected Font";
            // 
            // lblSelFont
            // 
            lblSelFont.Location = new Point(9, 18);
            lblSelFont.Name = "lblSelFont";
            lblSelFont.Size = new Size(219, 30);
            lblSelFont.TabIndex = 0;
            lblSelFont.Text = "label3";
            lblSelFont.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(pictureBoxSampleText);
            groupBox2.Location = new Point(293, 306);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(275, 142);
            groupBox2.TabIndex = 24;
            groupBox2.TabStop = false;
            groupBox2.Text = "Sample Text";
            // 
            // pictureBoxSampleText
            // 
            pictureBoxSampleText.Location = new Point(9, 28);
            pictureBoxSampleText.Name = "pictureBoxSampleText";
            pictureBoxSampleText.Size = new Size(260, 108);
            pictureBoxSampleText.TabIndex = 0;
            pictureBoxSampleText.TabStop = false;
            pictureBoxSampleText.Paint += pictureBoxSampleText_Paint;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chbStrikeout);
            groupBox1.Controls.Add(chbBold);
            groupBox1.Controls.Add(chbItalic);
            groupBox1.Location = new Point(455, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(113, 196);
            groupBox1.TabIndex = 23;
            groupBox1.TabStop = false;
            groupBox1.Text = "Font Style";
            // 
            // chbStrikeout
            // 
            chbStrikeout.AutoSize = true;
            chbStrikeout.Location = new Point(17, 142);
            chbStrikeout.Name = "chbStrikeout";
            chbStrikeout.Size = new Size(90, 24);
            chbStrikeout.TabIndex = 8;
            chbStrikeout.Text = "Strikeout";
            chbStrikeout.UseVisualStyleBackColor = true;
            chbStrikeout.CheckedChanged += chb_CheckedChanged;
            // 
            // chbBold
            // 
            chbBold.AutoSize = true;
            chbBold.Location = new Point(17, 46);
            chbBold.Name = "chbBold";
            chbBold.Size = new Size(62, 24);
            chbBold.TabIndex = 6;
            chbBold.Text = "Bold";
            chbBold.UseVisualStyleBackColor = true;
            chbBold.CheckedChanged += chb_CheckedChanged;
            // 
            // chbItalic
            // 
            chbItalic.AutoSize = true;
            chbItalic.Location = new Point(16, 93);
            chbItalic.Name = "chbItalic";
            chbItalic.Size = new Size(63, 24);
            chbItalic.TabIndex = 7;
            chbItalic.Text = "Italic";
            chbItalic.UseVisualStyleBackColor = true;
            chbItalic.CheckedChanged += chb_CheckedChanged;
            // 
            // textBoxFontSize
            // 
            textBoxFontSize.Location = new Point(293, 13);
            textBoxFontSize.Name = "textBoxFontSize";
            textBoxFontSize.Size = new Size(125, 27);
            textBoxFontSize.TabIndex = 22;
            textBoxFontSize.TextChanged += txtSize_TextChanged;
            // 
            // listBoxFontSIze
            // 
            listBoxFontSIze.BorderStyle = BorderStyle.FixedSingle;
            listBoxFontSIze.FormattingEnabled = true;
            listBoxFontSIze.Items.AddRange(new object[] { "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "36", "48", "72" });
            listBoxFontSIze.Location = new Point(293, 46);
            listBoxFontSIze.Name = "listBoxFontSIze";
            listBoxFontSIze.Size = new Size(125, 162);
            listBoxFontSIze.TabIndex = 21;
            listBoxFontSIze.SelectedIndexChanged += lstSize_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(330, 13);
            label6.Name = "label6";
            label6.Size = new Size(39, 20);
            label6.TabIndex = 20;
            label6.Text = "Size:";
            // 
            // comboBoxCodePage
            // 
            comboBoxCodePage.FormattingEnabled = true;
            comboBoxCodePage.Location = new Point(26, 491);
            comboBoxCodePage.Name = "comboBoxCodePage";
            comboBoxCodePage.Size = new Size(542, 28);
            comboBoxCodePage.TabIndex = 26;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(26, 468);
            label7.Name = "label7";
            label7.Size = new Size(80, 20);
            label7.TabIndex = 27;
            label7.Text = "Code Page";
            // 
            // comboBoxExportFormat
            // 
            comboBoxExportFormat.FormattingEnabled = true;
            comboBoxExportFormat.Items.AddRange(new object[] { "BrainSystems RunLength", "GLCD Font Creator 2" });
            comboBoxExportFormat.Location = new Point(1070, 1146);
            comboBoxExportFormat.Name = "comboBoxExportFormat";
            comboBoxExportFormat.Size = new Size(243, 28);
            comboBoxExportFormat.TabIndex = 28;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(1070, 1123);
            label8.Name = "label8";
            label8.Size = new Size(101, 20);
            label8.TabIndex = 29;
            label8.Text = "Export fromat";
            // 
            // pictureBoxString
            // 
            pictureBoxString.Location = new Point(26, 793);
            pictureBoxString.Name = "pictureBoxString";
            pictureBoxString.Size = new Size(1652, 311);
            pictureBoxString.TabIndex = 30;
            pictureBoxString.TabStop = false;
            pictureBoxString.Paint += pictureBoxString_Paint;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(302, 651);
            label9.Name = "label9";
            label9.Size = new Size(135, 20);
            label9.TabIndex = 31;
            label9.Text = "% of original width";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(26, 697);
            label10.Name = "label10";
            label10.Size = new Size(127, 20);
            label10.TabIndex = 32;
            label10.Text = "Character spacing";
            // 
            // textBoxCharSpacing
            // 
            textBoxCharSpacing.Location = new Point(235, 690);
            textBoxCharSpacing.Name = "textBoxCharSpacing";
            textBoxCharSpacing.Size = new Size(67, 27);
            textBoxCharSpacing.TabIndex = 33;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(302, 697);
            label11.Name = "label11";
            label11.Size = new Size(47, 20);
            label11.TabIndex = 34;
            label11.Text = "pixels";
            // 
            // buttonAbout
            // 
            buttonAbout.Location = new Point(1582, 1162);
            buttonAbout.Name = "buttonAbout";
            buttonAbout.Size = new Size(96, 33);
            buttonAbout.TabIndex = 35;
            buttonAbout.Text = "About";
            buttonAbout.UseVisualStyleBackColor = true;
            buttonAbout.Click += buttonAbout_Click;
            // 
            // FontCreatorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1696, 1215);
            Controls.Add(buttonAbout);
            Controls.Add(label11);
            Controls.Add(textBoxCharSpacing);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(pictureBoxString);
            Controls.Add(label8);
            Controls.Add(comboBoxExportFormat);
            Controls.Add(label7);
            Controls.Add(comboBoxCodePage);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(textBoxFontSize);
            Controls.Add(listBoxFontSIze);
            Controls.Add(label6);
            Controls.Add(fontListFont);
            Controls.Add(label5);
            Controls.Add(textBoxInfo);
            Controls.Add(textBoxExportDir);
            Controls.Add(buttonSetExportDirectory);
            Controls.Add(label4);
            Controls.Add(textBoxSpaceCharAdjust);
            Controls.Add(label3);
            Controls.Add(textBoxFontName);
            Controls.Add(label2);
            Controls.Add(textBoxLastChar);
            Controls.Add(label1);
            Controls.Add(textBoxFirstCharcter);
            Controls.Add(labelExcludeChars);
            Controls.Add(textBoxExcludeChars);
            Controls.Add(buttonExportFont);
            Controls.Add(listViewCharacters);
            Controls.Add(pictureBoxCharacter);
            Controls.Add(buttonLoadFont);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            Name = "FontCreatorForm";
            Text = "BrainSystems Font Creator";
            ((System.ComponentModel.ISupportInitialize)pictureBoxCharacter).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxSampleText).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxString).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonLoadFont;
        private PictureBox pictureBoxCharacter;
        private ListView listViewCharacters;
        private Button buttonExportFont;
        private TextBox textBoxExcludeChars;
        private Label labelExcludeChars;
        private TextBox textBoxFirstCharcter;
        private Label label1;
        private TextBox textBoxLastChar;
        private Label label2;
        private TextBox textBoxFontName;
        private Label label3;
        private TextBox textBoxSpaceCharAdjust;
        private Label label4;
        private Button buttonSetExportDirectory;
        private TextBox textBoxExportDir;
        private TextBox textBoxInfo;
        private Label label5;
        private GLCD_FontCreator.CustomFontDialog.FontList fontListFont;
        private GroupBox groupBox3;
        private Label lblSelFont;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private CheckBox chbStrikeout;
        private CheckBox chbBold;
        private CheckBox chbItalic;
        private TextBox textBoxFontSize;
        private ListBox listBoxFontSIze;
        private Label label6;
        private ComboBox comboBoxCodePage;
        private Label label7;
        private ComboBox comboBoxExportFormat;
        private Label label8;
        private PictureBox pictureBoxString;
        private PictureBox pictureBoxSampleText;
        private Label label9;
        private Label label10;
        private TextBox textBoxCharSpacing;
        private Label label11;
        private Button buttonAbout;
    }
}
