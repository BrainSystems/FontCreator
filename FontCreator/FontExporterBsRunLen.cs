using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FontCreator
{
    internal class FontExporterBsRunLen : FontExporter
    {

        public class BitStream
        {
            private List<byte> byteList;
            private int currentByte;
            private int bitPosition;

            public BitStream()
            {
                byteList = new List<byte>();
                currentByte = 0;
                bitPosition = 0;
            }

            public void AddBits(int bits, int num_bits)
            {
                if (num_bits < 0 || num_bits > 32)
                {
                    throw new ArgumentOutOfRangeException(nameof(num_bits), "num_bits must be between 0 and 32.");
                }

                for (int i = 0; i < num_bits; i++)
                {
                    int bit = (bits >> (num_bits - 1 - i)) & 1;
                    currentByte = (currentByte << 1) | bit;
                    bitPosition++;

                    if (bitPosition == 8)
                    {
                        byteList.Add((byte)currentByte);
                        currentByte = 0;
                        bitPosition = 0;
                    }
                }
            }

            public byte[] ToArray()
            {
                if (bitPosition > 0)
                {
                    currentByte <<= (8 - bitPosition);
                    byteList.Add((byte)currentByte);
                }

                return byteList.ToArray();
            }
        }

        public FontExporterBsRunLen(CharCollection charCollection) : base(charCollection)
        {

        }

        public static int GetBitLength(int x)
        {
            return x == 0 ? 1 : (int)Math.Log2(x) + 1;
        }

        private Byte[] RunLengthEncodeChar(Character ch)
        {

            if (ch.Excluded)
            {
                Byte[] bytes = new byte[1];
                bytes[0] = 0;
                return bytes;
            }
            else
            {
                int height = ch.HeightCommon;
                int width = ch.Width;
                int currRunLen = 0;
                bool lastPixel = false;
                int maxRunLen = 0;
                bool initialPixel = false;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if ((x == 0) && (y == 0))
                        {
                            lastPixel = ch.GetPixel(0, 0);
                            initialPixel = lastPixel;
                            currRunLen = 0;
                        }
                        else
                        {
                            bool pixel = ch.GetPixel(x, y);

                            if (pixel == lastPixel)
                            {
                                currRunLen++;
                            }
                            else
                            {
                                if (currRunLen > maxRunLen)
                                {
                                    maxRunLen = currRunLen;
                                }

                                lastPixel = pixel;
                                currRunLen = 0;
                            }
                        }
                    }
                }
                if (currRunLen > maxRunLen)
                {
                    maxRunLen = currRunLen;
                }

                int bit_len = GetBitLength(maxRunLen);

                BitStream bitStream = new BitStream();



                bitStream.AddBits(initialPixel ? 1 : 0, 1);       //inital color
                bitStream.AddBits(bit_len, 4);                  //4 bits meas bit length is between 1 and 16 bits allowing run lentgth of 1 to 65536 pixels
                bitStream.AddBits(width, 11);                   //character width, allows up to 2047 pixel wide characters

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        if ((x == 0) && (y == 0))
                        {
                            lastPixel = ch.GetPixel(0, 0);
                            currRunLen = 0;
                        }
                        else
                        {
                            bool pixel = ch.GetPixel(x, y);

                            if (pixel == lastPixel)
                            {
                                currRunLen++;
                            }
                            else
                            {
                                bitStream.AddBits(currRunLen, bit_len);
                                lastPixel = pixel;
                                currRunLen = 0;
                            }
                        }
                    }
                }
                if (currRunLen > 0)
                {
                    bitStream.AddBits(currRunLen, bit_len);
                }

                Byte[] bytes = bitStream.ToArray();
                return bytes;
            }
        }


        class RawCharData
        {
            int asciiIndex;
            String character;
            Byte[] data;
            int offset;

            public RawCharData(int asciiIndex, String character, byte[] data, int offset)
            {
                this.asciiIndex = asciiIndex;
                this.character = character;
                this.data = data;
                this.offset = offset;
            }

            public int AsciiIndex { get { return asciiIndex; } }

            public String Character { get { return character; } }
            public Byte[] Data { get { return data; } }

            public int Offset { get { return offset; } }

        }

        protected override void ExportDataToFile(FileStream file)
        {
            int charCount = mCharCollection.GetCount;
            List<RawCharData> rawCharacterDataArray = new List<RawCharData>();
            bool mustAddEmptyChar=false;
            int widest_char = 0;
            int font_data_length = 0;
            int offset = 0;
            for (int i = 0; i < charCount; i++)
            {
                Character character = mCharCollection.GetCharacter(i);
                if (character.Excluded)
                {
                    if (character.SubstituteCharIndex<0)
                    {
                        //we have at least one excluded character without a substitute, so we must add an empty char to the list
                        mustAddEmptyChar = true;
                    }
                    else
                    {
                        Character? substChar = mCharCollection.GetCharacterByAsciiIndex(character.SubstituteCharIndex);

                        if (substChar != null)
                        {
                            if (substChar.Excluded)
                            {
                                mustAddEmptyChar = true;
                            }
                        }
                        else
                        {
                            mustAddEmptyChar = true;
                        }
                    }
                }
                else
                {
                    Byte[] data = RunLengthEncodeChar(character);
                    font_data_length += data.Length;
                    rawCharacterDataArray.Add(new RawCharData(character.GetAsciiIndex,character.GetCharacter, data, offset));

                    offset += data.Length;
                }


                if (character.Width > widest_char)
                {
                    widest_char = character.Width;
                }

            }


            if (mustAddEmptyChar)
            {
                font_data_length++;
            }




            int num_offset_bits = GetBitLength(font_data_length);


            int currentOffset = 0;

            BitStream bitStreamOffsets = new BitStream();

            for (int i = 0; i < charCount; i++)
            {
                Character ch=mCharCollection.GetCharacter(i);

                currentOffset = 0;
                if ((!ch.Excluded) || (ch.SubstituteCharIndex >= 0))
                {
                    //must have subsititue
                    if (ch.SubstituteCharIndex >= 0)
                    {
                        Character? substChar = mCharCollection.GetCharacterByAsciiIndex(ch.SubstituteCharIndex);

                        if (substChar != null)
                        {
                            ch = substChar;
                        }
                    }

                    if (!ch.Excluded)
                    {
                        for (int jj = 0; jj < rawCharacterDataArray.Count; jj++)
                        {
                            if (rawCharacterDataArray[jj].AsciiIndex == ch.GetAsciiIndex)
                            {
                                currentOffset = rawCharacterDataArray[jj].Offset;
                                if (mustAddEmptyChar)
                                {
                                    currentOffset++;
                                }
                                break;
                            }
                        }
                    }
                }


                bitStreamOffsets.AddBits(currentOffset, num_offset_bits);
            }


            Byte[] offsets = bitStreamOffsets.ToArray();


            mTotalDataLength = +font_data_length + offsets.Length + 5;



            AddStringToFile(file, "/*\r\n");
            AddStringToFile(file, "Created with BrainSystems Font Creator\r\n");
            AddStringToFile(file, "\r\n");
            AddStringToFile(file, "https://github.com/BrainSystems/FontCreator/\r\n");
            AddStringToFile(file, "\r\n");            
            AddStringToFile(file, "File Format: BrainSystems Run Length\r\n");
            AddStringToFile(file, "*/\r\n\r\n\r\n");


            AddStringToFile(file, String.Format("#ifndef {0}_H\r\n#define {0}_H\r\n\r\n\r\nconst uint8_t {1}[{2}] = \r\n{{\r\n", mFontName.ToUpper(), mFontName, mTotalDataLength));

            AddStringToFile(file, String.Format("    0x{0:X2},  //height, common height in pixels of all characters\r\n", mCharCollection.GetCommonHeight));

            AddStringToFile(file, String.Format("    0x{0:X2},  //first char (ASCII code of first character)\r\n", mCharCollection.GetFirstChar));

            AddStringToFile(file, String.Format("    0x{0:X2},  //char count, number of characters contained in this file\r\n", mCharCollection.GetCount));

            AddStringToFile(file, String.Format("    0x{0:X2},  //char spacing, number of pixels between characters\r\n", mCharCollection.CharSpacing));

            AddStringToFile(file, String.Format("    0x{0:X2},  //num bits (x) per offset in character data offset table below\r\n\r\n", num_offset_bits));

            AddStringToFile(file, "    //character data offsets table, bitwise encoded, x bits per offset\r\n");
            AddStringToFile(file, "    //links a character code (ASCII code) to a glyph in the glyph table below, offsets are bytes from the first data byte of the glyph table\r\n    ");


            int cnt = 0;
            foreach (byte x in offsets)
            {
                AddStringToFile(file, String.Format("0x{0:X2}, ", x));

                if (cnt++ % 10 == 9)
                {
                    AddStringToFile(file, "\r\n    ");
                }
            }

            AddStringToFile(file, "\r\n\r\n");
            AddStringToFile(file, "    //Glyph data table (header data plus run length data for each glyph), each line is one glyph\r\n");
            AddStringToFile(file, "    //Data is bitwise encoded, each glyph consists of:\r\n");
            AddStringToFile(file, "    //bit 0: first pixel color (black or white):\r\n");
            AddStringToFile(file, "    //bit 1..4: bit length of each run (n):\r\n");
            AddStringToFile(file, "    //bit 5..15 character width\r\n");
            AddStringToFile(file, "    //bit 16..16+n length of the first run -1 (as length of 0 can't exist, so when decoding, add 1 to the length) \r\n");
            AddStringToFile(file, "    //bit 16+n+1..bit 17+2n second run lenght and so on\r\n");
            AddStringToFile(file, "    //After each run the color is inverted (staring from color int bit 0)\r\n");
            AddStringToFile(file, "\r\n");


            if (mustAddEmptyChar)
            {
                AddStringToFile(file, "    0x00,    //Empty char\r\n");
            }

            for (int i = 0; i < rawCharacterDataArray.Count; i++)
            {
                RawCharData rawCharData = rawCharacterDataArray[i];

                Byte[] charData = rawCharData.Data;
                AddStringToFile(file, "    ");
                for (int j = 0; j < charData.Length; j++)
                {
                    AddStringToFile(file, String.Format("0x{0:X2}, ", charData[j]));
                }

                AddStringToFile(file, String.Format("// {0} ({1})\r\n", rawCharData.AsciiIndex, rawCharData.Character));
            }

            AddStringToFile(file, "};\r\n\r\n#endif\r\n");
        }
    }
}