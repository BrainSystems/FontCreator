using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FontCreator
{
    internal class Character
    {

        BitImage? characterImage;

        int mEmptyTopRows = 0;
        int mEmptyBottomRows = 0;
        int mEmptyLeftRows = 0;
        int mEmptyRightRows = 0;
        int mCharWidth=0;
        int mCharHeight=0;

        int mAsciiIndex = 0;
        String mChar;
        bool excluded;
        int mCommonEmptyTopRows = 0;
        int mCommmonHeight = 0;
        int mSubstituteCharIndex = -1;
        private Bitmap GetStringBitmap(Font font, String s)
        {
            // get the size needed to paint the string in one line with the font given
            Bitmap fontImage = new Bitmap(1000, 1000);// biggg to have all printed on one line which is needed for long strings and large fonts
            Graphics g = Graphics.FromImage(fontImage);
            StringFormat newStringFormat = new StringFormat();
            newStringFormat.Alignment = StringAlignment.Near;
            newStringFormat.LineAlignment = StringAlignment.Near;
            newStringFormat.Trimming = StringTrimming.None;
            Size preferredSize = Size.Ceiling(g.MeasureString(s, font, new Size(1000, 1000), newStringFormat));
            g.Dispose();

            // get a new bitmap with optimal size for painting stuff
            fontImage = new Bitmap(preferredSize.Width, preferredSize.Height);
            g = Graphics.FromImage(fontImage);
            g.ResetClip();
            g.Clear(Color.White);
            // set text drawing props to make the best capturing later
            g.TextContrast = 0;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString(s, font, Brushes.Black, new Point(0, 0)); // draw red on black for capturing
            g.Flush(System.Drawing.Drawing2D.FlushIntention.Sync); // get things finished
            g.Dispose();

            //fontImage.Save( "TEST.png" ); // save the temp result
            return fontImage;
        }

        public Character(Font font, String ch, int asciiIndex, int widthAdjustValue, bool exclude, int substituteCharIndex)
        {
            mChar = ch;
            mAsciiIndex =asciiIndex;
            excluded = exclude;
            if (!exclude)
            {
                Bitmap bmp = GetStringBitmap(font, ch);

                if (widthAdjustValue!=100)
                {
                    int width = bmp.Width;

                    width = width * widthAdjustValue / 100;
                    if (width == 0)
                    {
                        width = 1;
                    }
                    //resize bitmap
                    bmp = new Bitmap(bmp, new Size(width, bmp.Height));
                }
 
                characterImage = new BitImage(bmp);

                bool pixelFound = false;

                //find empty rows on top
                for (int y = 0; y < characterImage.Height; y++)
                {
                    for (int x = 0; x < characterImage.Width; x++)
                    {
                        if (characterImage.GetPixel(x, y))
                        {
                            mEmptyTopRows = y;

                            pixelFound = true;
                            goto break1;
                        }
                    }
                }

            break1: if (pixelFound)
                {
                    //find empty rows from bottom
                    for (int y = characterImage.Height - 1; y >= 0; y--)
                    {
                        for (int x = 0; x < characterImage.Width; x++)
                        {
                            if (characterImage.GetPixel(x, y))
                            {
                                mEmptyBottomRows = characterImage.Height - 1 - y;
                                goto break2;
                            }
                        }
                    }


                //find empty rows on left
                break2: for (int x = 0; x < characterImage.Width; x++)
                    {
                        for (int y = 0; y < characterImage.Height; y++)
                        {
                            if (characterImage.GetPixel(x, y))
                            {
                                mEmptyLeftRows = x;
                                goto break3;
                            }
                        }
                    }


                //find empty rows from bottom
                break3: for (int x = characterImage.Width - 1; x >= 0; x--)
                    {
                        for (int y = 0; y < characterImage.Height; y++)
                        {

                            if (characterImage.GetPixel(x, y))
                            {
                                mEmptyRightRows = characterImage.Width - 1 - x;
                                goto break4;
                            }
                        }
                    }

                break4:
                    //     mEmptyBottomRows =0;

                    mCharWidth = characterImage.Width - mEmptyLeftRows - mEmptyRightRows;

                    if (mCharWidth < 0) mCharWidth = 0;
                    mCharHeight = characterImage.Height;

                }
                else //emtpy char (eg space)
                {
                    mEmptyTopRows =  mEmptyBottomRows= characterImage.Height / 2;

                    mCharHeight = characterImage.Height;
                    mCharWidth = characterImage.Width;
                }
            }
            else //excluded char (eg space)
            {
                mCharHeight = 0;
                mCharWidth = 0;
                mSubstituteCharIndex=substituteCharIndex;
            }
        }

        public bool GetPixel(int x, int y)
        {
            if (characterImage != null)
            {
                return characterImage.GetPixel(x + mEmptyLeftRows, y + mCommonEmptyTopRows);
            }
            else
            {
                return false;
            }
        }

        public int Width { get => mCharWidth;  }
        public int HeightIndividual { get => mCharHeight; set => mCharHeight = value; }
        public int HeightCommon { get => mCommmonHeight; }

        public int EmptyTopRows { get => mEmptyTopRows; set => mEmptyTopRows = value; }
        public int EmptyBottomRows { get => mEmptyBottomRows; set => mEmptyBottomRows = value; }

        public int GetAsciiIndex {  get => mAsciiIndex; }

        public String GetCharacter { get => mChar;  }


        public bool Excluded { get => excluded; }
        public int SubstituteCharIndex { get => mSubstituteCharIndex; } 
        public void SetCommonEmptyTopBottomRows(int topRows, int bottomRows)
        {
            mCommonEmptyTopRows = topRows;
            mCommmonHeight = mCharHeight - topRows - bottomRows;
            
            if (mCommmonHeight<0) mCommmonHeight = 0;
        }
    }
}
