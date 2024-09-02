using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FontCreator
{
    public class BitImage
    {
        BitArray mBits;

        int mWidth;
        int mHeight;

        public BitImage(int width, int height)
        {
            mWidth = width;
            mHeight = height;

            mBits = new BitArray(mWidth * mHeight);
        }

        public BitImage(Bitmap bitmap)
        {
            mWidth = bitmap.Width;
            mHeight = bitmap.Height;

            mBits = new BitArray(mWidth * mHeight);

            for (int x = 0; x < mWidth; x++)
            {
                for (int y = 0; y < mHeight; y++)
                {
                    Color pix = bitmap.GetPixel(x, y);
                    SetPixel(x, y, pix.GetBrightness() < 0.5);
                }
            }
        }

        public void SetPixel(int x, int y, bool val)
        {
            int idx = x * mHeight + y;

            mBits.Set(idx, val);
        }

        public bool GetPixel(int x, int y)
        {
            int idx = x * mHeight + y;

            return mBits.Get(idx);
        }

        public int Width { get { return mWidth; } }
        public int Height { get { return mHeight; } } 

    }
}
