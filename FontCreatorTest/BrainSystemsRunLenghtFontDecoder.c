

#include "stdint.h"
#include "stdbool.h"

#include "BrainSystemsRunLenghFontDecoder.h"

#define NUM_HEADER_BYTES    5


typedef struct {
    const uint8_t* data;
    int bit_offset;
} BitStreamDecoder;

static void init_bit_stream_decoder(BitStreamDecoder* decoder, const uint8_t* data)
{
    decoder->data = data;
    decoder->bit_offset = 0;
}

static uint32_t get_bits_from_offset(BitStreamDecoder* decoder, int no_bits, int offset)
{
    uint32_t result = 0;
    int bits_collected = 0;

    if (no_bits < 0 || no_bits > 32)
    {
        return 0;
    }

    while (bits_collected < no_bits)
    {
        int byte_position = offset >> 3;  // Divide by 8 using shift
        int bit_position = offset & 7;    // Modulo 8 using mask

        int bits_remaining_in_byte = 8 - bit_position;
        int bits_to_take = no_bits - bits_collected;

        if (bits_to_take > bits_remaining_in_byte)
        {
            bits_to_take = bits_remaining_in_byte;
        }

        result <<= bits_to_take;
        result |= (decoder->data[byte_position] >> (bits_remaining_in_byte - bits_to_take)) & ((1 << bits_to_take) - 1);

        offset += bits_to_take;
        bits_collected += bits_to_take;
    }

    return result;
}

static uint32_t get_bits(BitStreamDecoder* decoder, int no_bits)
{
    uint32_t result = get_bits_from_offset(decoder, no_bits, decoder->bit_offset);

    decoder->bit_offset += no_bits;

    return result;
}


static bool decode_font_header_and_get_bitstream(const uint8_t* font, unsigned char ch, BitStreamDecoder* decoder, uint8_t* char_spacing)
{
    unsigned int firstChar = font[1];
    unsigned int charCount = font[2];
    *char_spacing = font[3];
    unsigned int num_offset_bits = font[4];


    if (ch<firstChar || ch>=firstChar + charCount || num_offset_bits < 2)
    {
        return false;
    }

    uint8_t char_index = ch - firstChar;


    int bit_offset_index = char_index * num_offset_bits;

    BitStreamDecoder char_offset_decoder;

    init_bit_stream_decoder(&char_offset_decoder, font + NUM_HEADER_BYTES);

    int offset = get_bits_from_offset(&char_offset_decoder, num_offset_bits, bit_offset_index);

    //find the offset of the start of the font data, which is the [bit witdh * number of characters] number of bits, in bytes rouded up
    int font_data_start = (((num_offset_bits * charCount) + 7) >> 3) + NUM_HEADER_BYTES;

    const uint8_t* bitstream = font + font_data_start + offset;

    init_bit_stream_decoder(decoder, bitstream);

    return true;
}

uint8_t gdi_get_font_height(const uint8_t* font)
{
    return font[0];
}

uint16_t gdi_draw_char(int16_t x0, int16_t y0, unsigned char ch, const uint8_t* font, bool addCharSpacing, SET_PIXEL_FCN set_pixel_fcn, void* set_pixel_param)
{
    BitStreamDecoder decoder;
    uint8_t char_spacing;

    uint16_t total_char_witdh = 0;

    if (set_pixel_fcn == NULL)
    {
        return total_char_witdh;
    }

    if (decode_font_header_and_get_bitstream(font, ch, &decoder, &char_spacing))
    {
        int32_t x, y;

        uint16_t height = gdi_get_font_height(font);
        int16_t max_line_buf_length = height - 1;

        if (max_line_buf_length < 0) max_line_buf_length = 0; //sanity check

        bool current_pixel_color = get_bits(&decoder, 1);
        uint8_t bit_length = get_bits(&decoder, 4);

        if (bit_length != 0)//this is not empty character (just draw char spacing for empty characters)
        {
            int16_t width = get_bits(&decoder, 11);

            uint32_t run_length = 0;

            total_char_witdh += width;

            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    if (run_length == 0)
                    {
                        run_length = get_bits(&decoder, bit_length) + 1;     //we encode a run length of one run with 0 so add one here
                        if (current_pixel_color)
                        {
                            current_pixel_color = false;
                        }
                        else
                        {
                            current_pixel_color = true;
                        }
                    }

                    (set_pixel_fcn)(x0, y + y0, current_pixel_color, set_pixel_param);

                    run_length--;
                }

                x0++;
            }
        }

        if (addCharSpacing)
        {
            total_char_witdh += char_spacing;
            for (x = 0; x < char_spacing; x++)
            {
                for (y = 0; y < height; y++)
                {
                    (set_pixel_fcn)(x0, y + y0, true, set_pixel_param);
                }

                x0++;
            }
        }
    }

    return total_char_witdh;
}




uint16_t gdi_get_char_width(unsigned char ch, const uint8_t* font, bool addCharSpacing)
{
    BitStreamDecoder decoder;
    uint8_t char_spacing;
    uint16_t total_char_witdh = 0;

    if (decode_font_header_and_get_bitstream(font, ch, &decoder, &char_spacing))
    {
        get_bits(&decoder, 1);   //remove first bit (initial pixel color)

        uint8_t bit_length = get_bits(&decoder, 4);  //get bit_length (is 0 for an empty character)
 
        
        if (bit_length != 0)//this is an empty character
        {
            total_char_witdh += get_bits(&decoder, 11);

        }

        if (addCharSpacing)
        {
            total_char_witdh += char_spacing;
        }
    }

    return total_char_witdh;

}


int16_t gdi_draw_string(int16_t x0, int16_t y0, const unsigned char* string, const uint8_t* font, SET_PIXEL_FCN set_pixel_fcn, void* set_pixel_param)
{
    uint16_t i = 0;
    bool add_char_spacing = true;

    while (string[i] != '\0')
    {
        if (string[i + 1] == '\0') //if next char end of string, do not add char spacing
        {
            add_char_spacing = false;
        }

        x0 += gdi_draw_char(x0, y0, string[i], font, add_char_spacing, set_pixel_fcn, set_pixel_param);

        i++;
    }

    return x0;
}



void gdi_get_text_extent(const unsigned char* string, const uint8_t* font, uint16_t* width, uint16_t* height)
{
    uint16_t i = 0;
    uint16_t total_width = 0;
    bool add_char_spacing = true;

    *height = gdi_get_font_height(font);

    while (string[i] != '\0')
    {
        if (string[i + 1] == '\0') //if next char end of string, do not add char spacing
        {
            add_char_spacing = false;
        }

        total_width += gdi_get_char_width(string[i], font, add_char_spacing);
        i++;
    }

    *width = total_width;
}

