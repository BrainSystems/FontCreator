#include "pch.h"
#include "FontPictureControl.h"
#include "BrainSystemsRunLenghFontDecoder.h"


 CFontPictureControl::CFontPictureControl()
{
     m_font_data = NULL;
}



 extern "C"  void SetPixelFcn(int16_t x, int16_t y, bool value, void* set_pixel_param)
{
    CDC *pDc=(CDC*) set_pixel_param;

    COLORREF clr;
    if (value)
    {
        clr = 0xFFFFFF;
    }
    else
    {
        clr = 0;
    }

    pDc->SetPixel(x, y, clr);
}


void CFontPictureControl::DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct)
{
    CDC dc;
    dc.Attach(lpDrawItemStruct->hDC);

    CDC compatDC;
    CBitmap compatBitmap;

    CRect rect;
    GetClientRect(&rect);

    // draw to a compatible device context and then bitblt to screen
    compatDC.CreateCompatibleDC(&dc);
    compatDC.SaveDC();
    compatBitmap.CreateCompatibleBitmap(&dc, rect.Width(), rect.Height());
    compatDC.SelectObject(&compatBitmap);

    // if we have just created a compatible bitmap then it will be full of random data
    compatDC.FillSolidRect(0, 0, rect.Width(), rect.Height(), 0xFFFFFF);

    if (m_font_data)
    {
        int32_t cx = 0;
        int32_t cy = 0;
        for (int ch = 0; ch < 256; ch++)
        {
            //check if next char will still fit in the row
            if (cx+ gdi_get_char_width(ch, m_font_data, TRUE) > rect.right)
            {
                //does not fit in the row, start a new row...
                cx = 0;
                cy += gdi_get_font_height(m_font_data);
            }

            cx+=gdi_draw_char(cx, cy, ch, m_font_data, TRUE, SetPixelFcn, (void*) &compatDC);
        }
    }

    // bitblt the buffered graph to the display
    dc.BitBlt(0, 0, rect.Width(), rect.Height(), &compatDC, 0, 0, SRCCOPY);
    compatDC.RestoreDC(-1);
    compatBitmap.DeleteObject();
    compatDC.DeleteDC();

    dc.Detach();
}



void CFontPictureControl::SetFontData(uint8_t* font_data)
{
    m_font_data = font_data;

    Invalidate();
}