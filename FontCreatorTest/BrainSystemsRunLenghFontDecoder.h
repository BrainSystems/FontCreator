#pragma once

#ifdef __cplusplus
extern "C"
{
#endif

	typedef void (*SET_PIXEL_FCN)(int16_t x, int16_t y, bool value, void* set_pixel_param);

	uint8_t gdi_get_font_height(const uint8_t* font);
	uint16_t gdi_draw_char(int16_t x0, int16_t y0, unsigned char ch, const uint8_t* font, bool addCharSpacing, SET_PIXEL_FCN set_pixel_fcn, void* set_pixel_param);
	uint16_t gdi_get_char_width(unsigned char ch, const uint8_t* font, bool addCharSpacing);
	int16_t gdi_draw_string(int16_t x0, int16_t y0, const unsigned char* string, const uint8_t* font, SET_PIXEL_FCN set_pixel_fcn, void* set_pixel_param);
	void gdi_get_text_extent(const unsigned char* string, const uint8_t* font, uint16_t* width, uint16_t* height);

#ifdef __cplusplus
}
#endif