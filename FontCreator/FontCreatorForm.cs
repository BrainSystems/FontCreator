using GLCD_FontCreator.CustomFontDialog;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Collections;
using System.Linq.Expressions;
using FontCreator.Properties;
using System.Configuration;
using System.IO;
using static System.Net.WebRequestMethods;
using System.Reflection;
using Microsoft.VisualBasic;
using static System.Windows.Forms.DataFormats;
using System.Drawing;

namespace FontCreator
{
    public partial class FontCreatorForm : Form
    {
        public class EncodingItem : Object
        {
            String mEncodingName;
            int mEncoding;
            public EncodingItem(int encoding, String encodingName)
            {
                mEncoding = encoding;
                mEncodingName = encodingName;
            }

            public override string ToString()
            {
                return String.Format("{0} [{1}]", mEncodingName, mEncoding);
            }


            public int Encoding { get => mEncoding; }
        }


        int selectedRow = -1;
        private Font mSelectedFont;

        CharCollection? mCharCollection;
        public FontCreatorForm()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            fontListFont.LoadFonts();

            fontListFont.SelectedFontFamilyChanged += lstFont_SelectedFontFamilyChanged;
            fontListFont.SelectedFontFamily = FontFamily.GenericSansSerif;
            textBoxFontSize.Text = Convert.ToString(10);


            listViewCharacters.View = View.Details;
            listViewCharacters.Columns.Add("Dec");
            listViewCharacters.Columns.Add("Hex");
            listViewCharacters.Columns.Add("Char");
            listViewCharacters.Columns.Add("Width");
            listViewCharacters.Columns.Add("Excl/Linked");
            listViewCharacters.Columns[4].Width = -2;

            listViewCharacters.GridLines = true;


            textBoxExcludeChars.Text = (String)Settings.Default["ExcludedChars"];
            textBoxFirstCharcter.Text = (String)Settings.Default["FirstChar"];
            textBoxLastChar.Text = (String)Settings.Default["NumChars"];
            textBoxSpaceCharAdjust.Text = (String)Settings.Default["EmptyCharWidth"];
            textBoxCharSpacing.Text = (String)Settings.Default["DefaultCharSpacing"];
            textBoxExportDir.Text = (String)Settings.Default["ExportDir"];
            comboBoxExportFormat.SelectedIndex = (int)Settings.Default["ExportFormat"];

            mSelectedFont = (Font)Settings.Default["SelectedFont"];

            if (mSelectedFont == null)
            {
                mSelectedFont = new Font(FontFamily.GenericSansSerif, 10);
            }

            fontListFont.SelectedFontFamily = mSelectedFont.FontFamily;
            textBoxFontSize.Text = mSelectedFont.Size.ToString();
            chbBold.Checked = mSelectedFont.Bold;
            chbItalic.Checked = mSelectedFont.Italic;
            chbStrikeout.Checked = mSelectedFont.Strikeout;


            int code_page = (int)Settings.Default["CodePage"];

            var codepages = Encoding.GetEncodings().Select(x => x.GetEncoding()).ToList();
            bool selected = false;
            foreach (var codepage in codepages)
            {
                EncodingItem item = new EncodingItem(codepage.CodePage, codepage.EncodingName);
                comboBoxCodePage.Items.Add(item);

                if (code_page == codepage.CodePage)
                {
                    comboBoxCodePage.SelectedItem = item;
                    selected = true;
                }
            }

            if (!selected)
            {
                comboBoxCodePage.SelectedIndex = 0;
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            // Code
            Settings.Default["ExcludedChars"] = textBoxExcludeChars.Text;
            Settings.Default["FirstChar"] = textBoxFirstCharcter.Text;
            Settings.Default["NumChars"] = textBoxLastChar.Text;
            Settings.Default["EmptyCharWidth"] = textBoxSpaceCharAdjust.Text;
            Settings.Default["DefaultCharSpacing"] = textBoxCharSpacing.Text;
            Settings.Default["ExportDir"] = textBoxExportDir.Text;
            Settings.Default["ExportFormat"] = comboBoxExportFormat.SelectedIndex;

            if (comboBoxCodePage.SelectedItem != null)
            {
                Settings.Default["CodePage"] = ((EncodingItem)comboBoxCodePage.SelectedItem).Encoding;
            }
            Settings.Default["SelectedFont"] = mSelectedFont;
            Settings.Default.Save(); // Saves settings in application configuration file
        }
        private void lstFont_SelectedFontFamilyChanged(object? sender, EventArgs? e)
        {
            UpdateSampleText();
        }
        private void lstSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxFontSIze.SelectedItem != null)
                textBoxFontSize.Text = listBoxFontSIze.SelectedItem.ToString();
        }
        private void txtSize_TextChanged(object sender, EventArgs e)
        {
            if (listBoxFontSIze.Items.Contains(textBoxFontSize.Text))
                listBoxFontSIze.SelectedItem = textBoxFontSize.Text;
            else
                listBoxFontSIze.ClearSelected();

            UpdateSampleText();
        }
        private void chb_CheckedChanged(object sender, EventArgs e)
        {
            UpdateSampleText();
        }



        private void UpdateSampleText()
        {
            if (fontListFont.SelectedFontFamily == null) return; // more init issues


            float size = textBoxFontSize.Text != "" ? float.Parse(textBoxFontSize.Text) : 1;
            // determine what is available and what is choosen
            bool fRegular = fontListFont.SelectedFontFamily.IsStyleAvailable(FontStyle.Regular);
            bool fBold = fontListFont.SelectedFontFamily.IsStyleAvailable(FontStyle.Bold);
            bool fItalic = fontListFont.SelectedFontFamily.IsStyleAvailable(FontStyle.Italic);
            bool fStrike = fontListFont.SelectedFontFamily.IsStyleAvailable(FontStyle.Strikeout);

            FontStyle style = FontStyle.Regular; // invalid to check later..

            //        if (fRegular) style = FontStyle.Regular;
            if (chbBold.Checked && fBold) style |= FontStyle.Bold;
            if (chbItalic.Checked && fItalic) style |= FontStyle.Italic;
            if (chbStrikeout.Checked && fStrike) style |= FontStyle.Strikeout; // this is OR'ed (underline would be too)


            mSelectedFont = new Font(fontListFont.SelectedFontFamily, size, style, GraphicsUnit.Point);
            lblSelFont.Text = mSelectedFont.Name;

            pictureBoxSampleText.Invalidate();
        }


        public class ExcludedChar
        {
            int mIndex;
            int mSubstitutedCharIndex;

            public ExcludedChar(int index, int substitutedChar)
            {
                mIndex = index;
                mSubstitutedCharIndex = substitutedChar;
            }

            public int Index { get { return mIndex; } }
            public int SubstituteCharIndex { get { return mSubstitutedCharIndex; } }
        }


        public static List<ExcludedChar> ParseRange(string input)
        {
            var result = new List<ExcludedChar>();

            foreach (var part in input.Split(new string[] { ",", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                try
                {
                    var pairs = part.Split(':');
                    int substChar = -1;

                    if (pairs.Length >= 2)
                    {
                        substChar = int.Parse(pairs[1]);
                    }

                    if (pairs.Length >= 1)
                    {
                        var rangeParts = pairs[0].Split('-');
                        int start = int.Parse(rangeParts[0]);
                        int end = rangeParts.Length > 1 ? int.Parse(rangeParts[1]) : start;
                        for (int i = start; i <= end; i++)
                        {
                            result.Add(new ExcludedChar(i, substChar));
                        }
                    }
                }
                catch { }
            }

            return result;
        }


        private void buttonLoadFontClicked(object sender, EventArgs e)
        {
            try
            {
                List<ExcludedChar> excludedChars = ParseRange(textBoxExcludeChars.Text);

                int lastChar = int.Parse(textBoxLastChar.Text);
                int firstChar = int.Parse(textBoxFirstCharcter.Text);
                int spaceBarAdjust = int.Parse(textBoxSpaceCharAdjust.Text);
                int charSpacing = int.Parse(textBoxCharSpacing.Text);

                int numChars = lastChar - firstChar + 1;

                if ((spaceBarAdjust < 0) || (spaceBarAdjust > 500))
                {
                    MessageBox.Show("Space char adjust value must be bewteen 0% and 500%", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (charSpacing > 255)
                {
                    MessageBox.Show("Char spacing must be smaller than 256", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if ((firstChar < 0) || (numChars < 1) || (firstChar + numChars > 256))
                {
                    MessageBox.Show("Char span must be between 0 and 255, Last char needs to be greater than First Char", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (comboBoxCodePage.SelectedItem == null)
                {
                    MessageBox.Show("Select encoding", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int previouslySelectedRow = 0;

                    if (listViewCharacters.SelectedIndices.Count > 0)
                    {
                        previouslySelectedRow = listViewCharacters.SelectedIndices[0];
                    }

                    EncodingItem encodingItem = (EncodingItem)comboBoxCodePage.SelectedItem;

                    mCharCollection = new CharCollection(mSelectedFont, firstChar, numChars, spaceBarAdjust, charSpacing, excludedChars, encodingItem.Encoding);

                    listViewCharacters.Items.Clear();

                    textBoxInfo.Text = String.Format("Character Height: {0}, Number of characters: {1}", mCharCollection.GetCommonHeight.ToString(), mCharCollection.GetCount);

                    listViewCharacters.BeginUpdate();
                    for (int i = 0; i < mCharCollection.GetCount; i++)
                    {
                        Character character = mCharCollection.GetCharacter(i);

                        String exclSubsStr;

                        Color itemBackColor;

                        if (!character.Excluded)
                        {
                            exclSubsStr = "No";
                            itemBackColor = Color.White;
                        }
                        else
                        {
                            int substCharIndex = character.SubstituteCharIndex;
                            if (substCharIndex < 0)
                            {
                                exclSubsStr = "Yes";
                                itemBackColor = Color.LightGray;
                            }
                            else
                            {
                                Character? substChar = mCharCollection.GetCharacterByAsciiIndex(substCharIndex);

                                if ((substChar != null) && (substChar.Excluded == false))
                                {
                                    exclSubsStr = String.Format("LNK: {0} [{1}]", substCharIndex, substChar.GetCharacter);
                                    itemBackColor = Color.LightGreen;
                                }
                                else //can't link to an excluded or not available char
                                {
                                    itemBackColor = Color.Red;
                                    exclSubsStr = String.Format("INV: {0}", substCharIndex);
                                }
                            }
                        }


                        ListViewItem item = new ListViewItem(new string[] { String.Format("{0}", character.GetAsciiIndex), String.Format("0x{0:X2}", character.GetAsciiIndex), character.GetCharacter, String.Format("{0}", character.Width), exclSubsStr });
                        item.BackColor = itemBackColor;
                        listViewCharacters.Items.Add(item);
                    }
                    listViewCharacters.EndUpdate();
                    buttonExportFont.Enabled = true;

                    if (listViewCharacters.Items.Count <= previouslySelectedRow)
                    {
                        previouslySelectedRow = 0;
                    }

                    if (listViewCharacters.Items.Count >= 0)
                    {
                        try
                        {
                            selectedRow = previouslySelectedRow;
                            listViewCharacters.Items[previouslySelectedRow].Selected = true;
                            listViewCharacters.Items[previouslySelectedRow].Focused = true;
                            listViewCharacters.EnsureVisible(previouslySelectedRow);
                        }
                        catch { }
                    }
                    else
                    {
                        selectedRow = -1;
                    }
                    pictureBoxCharacter.Invalidate();
                    pictureBoxString.Invalidate();


                    String strModifier = "";
                    if (mSelectedFont.Bold)
                    {
                        strModifier += "_bold";
                    }

                    if (mSelectedFont.Italic)
                    {
                        strModifier += "_italic";
                    }

                    if (mSelectedFont.Strikeout)
                    {
                        strModifier += "_strikeout";
                    }
                    textBoxFontName.Text = String.Format("{0}{1}_{2}", mSelectedFont.Name.Replace(' ', '_'), strModifier, mSelectedFont.Size);
                }
            }
            catch
            {
                MessageBox.Show("Enter numbers only", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void listViewCharacters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewCharacters.SelectedItems.Count > 0)
            {
                if (listViewCharacters.FocusedItem != null)
                {
                    selectedRow = listViewCharacters.FocusedItem.Index;
                    pictureBoxCharacter.Invalidate();
                }
            }
        }


        private void pictureBoxCharacter_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBoxCharacter = (PictureBox)sender;
            Graphics g = e.Graphics;
            g.Clear(Color.DarkGray);
            Size s = pictureBoxCharacter.Size;

            s.Width -= 1;
            s.Height -= 1;

            Rectangle rc = new Rectangle();
            rc.Size = s;

            Pen pen = new Pen(Color.Black, 1);
            pen.Alignment = PenAlignment.Inset; //<-- this
            g.DrawRectangle(pen, rc);


            s.Width -= 2;
            s.Height -= 2;

            if ((mCharCollection != null) && (selectedRow >= 0) && (selectedRow < mCharCollection.GetCount))
            {
                Pen penLine = new Pen(Color.LightGray, 1);
                SolidBrush brushBoxes = new SolidBrush(Color.Black);

                Character character = mCharCollection.GetCharacter(selectedRow);

                if (character.Excluded == true)
                {

                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    if (character.SubstituteCharIndex >= 0)
                    {
                        Character? substChar = mCharCollection.GetCharacterByAsciiIndex(character.SubstituteCharIndex);

                        if ((substChar != null) && (substChar.Excluded == false))
                        {
                            //substitute char
                            character = substChar;
                        }
                        else
                        {
#pragma warning disable CS8604 // Possible null reference argument.
                            g.DrawString("Invalid Link - empty", SystemFonts.IconTitleFont, Brushes.Black, s.Width / 2, s.Height / 2, stringFormat);
#pragma warning restore CS8604 // Possible null reference argument.
                            return;
                        }
                    }
                    else
                    {
#pragma warning disable CS8604 // Possible null reference argument.
                        g.DrawString("Excluded - empty", SystemFonts.IconTitleFont, Brushes.Black, s.Width / 2, s.Height / 2, stringFormat);
#pragma warning restore CS8604 // Possible null reference argument.
                        return;
                    }
                }




                if (character.Width > 0)
                {
                    float hSpacing = (float)s.Width / character.Width;
                    float vSpacing = (float)s.Height / character.HeightCommon;

                    if (hSpacing > vSpacing)
                    {
                        hSpacing = vSpacing;
                    }

                    if (vSpacing > hSpacing)
                    {
                        vSpacing = hSpacing;
                    }


                    int total_char_width = (int)(character.Width * vSpacing);
                    int total_char_height = (int)(character.HeightCommon * hSpacing);

                    int x_offset = (s.Width - total_char_width) / 2;
                    if (x_offset < 0) x_offset = 0;

                    int y_offset = (s.Height - total_char_height) / 2;
                    if (y_offset < 0) y_offset = 0;


                    g.FillRectangle(new SolidBrush(Color.White), x_offset + 1, y_offset + 1, total_char_width, total_char_height);


                    for (int x = 0; x < character.Width; x++)
                    {
                        int xxFrom = (int)(x * hSpacing);
                        int xxTo = (int)((x + 1) * hSpacing);
                        g.DrawLine(penLine, xxFrom + x_offset + 1, y_offset + 1, xxFrom + x_offset + 1, y_offset + 1 + total_char_height);
                        for (int y = 0; y < character.HeightCommon; y++)
                        {
                            int yyFrom = (int)(y * vSpacing);
                            int yyTo = (int)((y + 1) * vSpacing);

                            if (character.GetPixel(x, y))
                            {
                                g.FillRectangle(brushBoxes, xxFrom + 3 + x_offset, y_offset + yyFrom + 3, xxTo - xxFrom - 3, yyTo - yyFrom - 3);
                            }
                        }
                    }

                    int xx = (int)(character.Width * hSpacing);
                    g.DrawLine(penLine, xx + x_offset + 1, y_offset + 1, xx + x_offset + 1, y_offset + 1 + total_char_height);

                    for (int y = 0; y < character.HeightCommon; y++)
                    {
                        int yy = (int)(y * vSpacing);

                        g.DrawLine(penLine, x_offset + 1, y_offset + yy + 1, xx + x_offset + 1, y_offset + yy + 1);
                    }
                }
            }
        }



        private void buttonExportFont_Click(object sender, EventArgs e)
        {
            FontExporter exporter;

            if (mCharCollection != null)
            {
                if (comboBoxExportFormat.SelectedIndex == 1)
                {
                    exporter = new FontExporterGLCD2(mCharCollection);
                }
                else
                {
                    exporter = new FontExporterBsRunLen(mCharCollection);
                }

                exporter.ExportFont(textBoxExportDir.Text, textBoxFontName.Text);
            }
        }

        private void buttonSetExportDirectory_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            if (DialogResult.OK == dlg.ShowDialog())
            {
                textBoxExportDir.Text = dlg.SelectedPath;
            }
        }

        private void pictureBoxString_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBoxCharacter = (PictureBox)sender;
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            Size s = pictureBoxCharacter.Size;
            Rectangle rc = new Rectangle();

            rc.Size = s;

            rc.Inflate(-1, -1);

            Pen pen = new Pen(Color.Black, 1);
            pen.Alignment = PenAlignment.Inset; //<-- this
            g.DrawRectangle(pen, rc);

            SolidBrush blackBrush = new SolidBrush(Color.Black);


            if (mCharCollection != null)
            {
                int cx = 4, cy = 4;
                for (int ch = 0; ch < mCharCollection.GetCount; ch++)
                {
                    Character character = mCharCollection.GetCharacter(ch);
                    if ((character.SubstituteCharIndex < 0))   //do not display substituted or exluded characters
                    {
                        if (character.Excluded == false)
                        {
                            if (cx + character.Width + 4 > s.Width)
                            {
                                cx = 4;
                                cy += character.HeightCommon;
                            }

                            for (int x = 0; x < character.Width; x++)
                            {
                                for (int y = 0; y < character.HeightCommon; y++)
                                {
                                    if (character.GetPixel(x, y))
                                    {
                                        e.Graphics.FillRectangle(blackBrush, cx + x, cy + y, 1, 1);
                                    }
                                }
                            }
                            cx += character.Width;
                        }
                        cx += mCharCollection.CharSpacing;
                    }
                }
            }
        }

        private void pictureBoxSampleText_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pictureBoxCharacter = (PictureBox)sender;
            Graphics g = e.Graphics;
            g.Clear(SystemColors.Control);
            Size s = pictureBoxCharacter.Size;
            Rectangle rc = new Rectangle();

            rc.Size = s;

            rc.Inflate(-1, -1);

            e.Graphics.DrawString("AaBbCcXxYyZz", mSelectedFont, Brushes.Black, rc, System.Drawing.StringFormat.GenericDefault);
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("BrainSystems FontCreator\n\rReleased under GLP3.0\n\rFind documentation and source under https://github.com/BrainSystems/FontCreator/", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
