using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static FontCreator.FontCreatorForm;

namespace FontCreator
{
    internal class CharCollection
    {

        int mFirstChar;
        int mCharCount;
        String mCharString;
        Character[] mCharacters;
        int mCommonEmptyTopRows;
        int mCommonEmptyBottomRows;

        int mCommonHeight;

        int mCharSpacing;
        public CharCollection(Font font, int firstChar, int charCount, int spaceCharAdjustValue, int charSpacing, List<ExcludedChar> excludedChars, int encloding)
        {
            int i = 0;

            mFirstChar = firstChar;
            mCharCount = charCount;
            mCharSpacing = charSpacing;
            Encoding DestEncoding = Encoding.GetEncoding(encloding);
            Encoding asciiEncoding = Encoding.ASCII;


            byte[] byteArray = new byte[mCharCount];
            for (i = 0; i < mCharCount; i++)
            {
                byteArray[i] = (byte)(i + firstChar);
            }

            mCharString = DestEncoding.GetString(byteArray);

            if (mCharString.Length< mCharCount)
            {
                mCharCount = mCharString.Length;
            }

            mCharacters = new Character[mCharCount];

            try
            {
                for (i = 0; i < mCharCount; i++)
                {
                    String character = mCharString.Substring(i, 1);

                    bool excludeChar = false;
                    int substituteCharIndex = -1;
                    foreach (ExcludedChar excludedChar in excludedChars)
                    {
                        if (excludedChar.Index== i + firstChar)
                        {
                            excludeChar = true;
                            substituteCharIndex= excludedChar.SubstituteCharIndex;
                        }
                    }

                    int widthAdjustValue;
                    if (i+firstChar == 32)      //space char
                    {
                        widthAdjustValue = spaceCharAdjustValue;
                    }
                    else
                    {
                        widthAdjustValue = 100;
                    }

                    mCharacters[i] = new Character(font, character, i + mFirstChar, widthAdjustValue, excludeChar, substituteCharIndex);
                }
            }
            catch 
            {
                MessageBox.Show(String.Format("Error rendering character {0}", i), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mCharCount = 0;
                return;
            }

            //find smallest height char
            mCommonHeight = int.MaxValue;
            for (i = 0; i < mCharCount; i++)
            {
                if (mCharacters[i].HeightIndividual != 0)
                {
                    if (mCharacters[i].HeightIndividual < mCommonHeight)
                    {
                        mCommonHeight = mCharacters[i].HeightIndividual;
                    }
                }
            }

            //cut lines of characters higher than smallest char
            for (i = 0; i < mCharCount; i++)
            {
                if (mCharacters[i].HeightIndividual > 0)
                {
                    int num_lines_to_remove = mCharacters[i].HeightIndividual - mCommonHeight;
                    if (mCharacters[i].EmptyBottomRows >= num_lines_to_remove)
                    {
                        mCharacters[i].EmptyBottomRows -= num_lines_to_remove;
                    }
                    else if (mCharacters[i].EmptyTopRows >= num_lines_to_remove)
                    {
                        mCharacters[i].EmptyTopRows -= num_lines_to_remove;
                    }

                    mCharacters[i].HeightIndividual -= num_lines_to_remove;
                }
            }






            mCommonEmptyTopRows = int.MaxValue;
            mCommonEmptyBottomRows = int.MaxValue;
            for (i = 0; i < mCharCount; i++)
            {
                if (mCharacters[i].HeightIndividual > 0)
                {
                    if (mCharacters[i].EmptyTopRows < mCommonEmptyTopRows)
                    {
                        mCommonEmptyTopRows = mCharacters[i].EmptyTopRows;
                    }
                    if (mCharacters[i].EmptyBottomRows < mCommonEmptyBottomRows)
                    {
                        mCommonEmptyBottomRows = mCharacters[i].EmptyBottomRows;
                    }
                }
            }

            for (i = 0; i < mCharCount; i++)
            {
                mCharacters[i].SetCommonEmptyTopBottomRows(mCommonEmptyTopRows, mCommonEmptyBottomRows);
            }

            mCommonHeight = mCommonHeight - mCommonEmptyTopRows - mCommonEmptyBottomRows;
            if (mCommonHeight < 0) mCommonHeight = 0;
        }

 

        public String GetCharString { get => mCharString; }

        public int GetCount { get => mCharCount; }
        public int GetFirstChar { get => mFirstChar; }
        public Character GetCharacter(int index)
        {
            return mCharacters[index];
        }


        public Character? GetCharacterByAsciiIndex(int index)
        {
            int idx=index-mFirstChar;

            if ((idx >= 0) && (idx < GetCount))
            {
                return mCharacters[idx];
            }
            
            return null;
        }

        public int GetCommonHeight { get => mCommonHeight; }


        public int CharSpacing { get => mCharSpacing; }
    }
}
