#pragma once
#include <afxwin.h>
class CFontPictureControl :    public CStatic
{
public:
    CFontPictureControl();

    void CFontPictureControl::DrawItem(LPDRAWITEMSTRUCT lpDrawItemStruct);

    uint8_t* m_font_data;




    void SetFontData(uint8_t* font_data);

};

