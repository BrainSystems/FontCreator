using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FontCreator
{
    internal class FontExporterGLCD2 : FontExporter
    {
        public FontExporterGLCD2(CharCollection charCollection) : base(charCollection)
        {

        }
    

        protected override void ExportDataToFile(FileStream file)
        {
            int charCount = mCharCollection.GetCount;
            List<Byte[]> arr = new List<Byte[]>();

            int font_data_length = 0;
            int widest_char = 0;
            bool hasLinkedChars = false;
            for (int i = 0; i < charCount; i++)
            {
                Character character = mCharCollection.GetCharacter(i);
                Byte[] data = GetCharBytes(character);
                font_data_length += data.Length;
                arr.Add(data);

                if (character.SubstituteCharIndex>0)
                {
                    hasLinkedChars = true;
                }


                if (character.Width>widest_char)
                {
                    widest_char=character.Width;
                }    
            }

            if (hasLinkedChars)
            {
                MessageBox.Show("GLCD2 Format does not support linked characters, all linked characters are exported as an empty charcter", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            mTotalDataLength = font_data_length + charCount + 6; //data + char width table + 6 header bytes


            AddStringToFile(file, "/*\r\n");
            AddStringToFile(file, "Created with BrainSystems Font Creator\r\n");
            AddStringToFile(file, "\r\n");
            AddStringToFile(file, "https://github.com/BrainSystems/FontCreator/\r\n");
            AddStringToFile(file, "\r\n");
            AddStringToFile(file, "File Format: GLCD2\r\n");
            AddStringToFile(file, "*/\r\n");

            AddStringToFile(file, String.Format("#ifndef {0}_H\r\n#define {0}_H\r\n\r\n#define {0}_WIDTH {2}\r\n#define {0}_HEIGHT {3}\r\n\r\nconst uint8_t {1}[{4}] = \r\n{{\r\n", mFontName.ToUpper(), mFontName, widest_char, mCharCollection.GetCommonHeight, mTotalDataLength));

            AddStringToFile(file, String.Format("    0x{0:X2}, 0x{1:X2},  //size\r\n", (mTotalDataLength >> 8)&0xFF, mTotalDataLength & 0xFF));

            AddStringToFile(file, String.Format("    0x{0:X2},  //max width\r\n", widest_char));

            AddStringToFile(file, String.Format("    0x{0:X2},  //max height\r\n", mCharCollection.GetCommonHeight));

            AddStringToFile(file, String.Format("    0x{0:X2},  //first char\r\n", mCharCollection.GetFirstChar));

            AddStringToFile(file, String.Format("    0x{0:X2},  //char count\r\n", mCharCollection.GetCount));

            AddStringToFile(file, "\r\n\r\n    // char widths\r\n    ");

            for (int i = 0; i < charCount; i++)
            {
                AddStringToFile(file, String.Format("0x{0:X2}, ", mCharCollection.GetCharacter(i).Width));

                if (i%10==9)
                {
                    AddStringToFile(file, "\r\n    ");
                }
            }

            AddStringToFile(file, "\r\n\r\n    // font data\r\n    ");
            for (int i = 0; i < charCount; i++)
            {
                Byte[] charData = arr[i];

                for (int j = 0; j < charData.Length; j++)
                {
                    AddStringToFile(file, String.Format("0x{0:X2}, ", charData[j]));
                }

                AddStringToFile(file, String.Format("// {0} ({1})\r\n    ", mCharCollection.GetCharacter(i).GetAsciiIndex, mCharCollection.GetCharacter(i).GetCharacter));
            }

            AddStringToFile(file, "};\r\n\r\n#endif\r\n");
        }

        private Byte[] GetCharBytes(Character ch)
        {
            int idx = 0;

            // from Top to bottom - then from left to right
            int bytesPerCol = (ch.HeightCommon + 7) / 8; // get height in bytes

            Byte[] char_bytes = new Byte[ch.Width * bytesPerCol];

            // per byte row
            for (int b = 0; b < bytesPerCol; b++)
            {
                for (int x = 0; x < ch.Width; x++)
                {
                  // left -> right
                    Byte col = 0;

                    int offset = b * 8;

                    Byte bit = 1;

                    for (int y = offset; y < offset + 8; y++)
                    {
                        if (y < ch.HeightCommon)
                        {
                            if (ch.GetPixel(x, y))     //black pixel
                            {
                                col |= bit;
                            }
                            bit <<= 1; // shift in
                        }
                    } // for y
                      // Adjust for partial bytes at the end of the character height
                    if ((ch.HeightCommon > 7) && (ch.HeightCommon < (b + 1) * 8))
                    {
                        col <<= (b + 1) * 8 - ch.HeightCommon ;
                    }

                    char_bytes[idx++] = col;


                }// for b
            }// for x

            return char_bytes;
        }


    }
}
