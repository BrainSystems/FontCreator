# FontCreator
Run-length compressed font creator for use in embedded systems

This font creator is loosely based on some code from GLCD_FontCreator and has some features inspired by GLCDFontCreator2 but mostly written from scratch. It can export a run-length compressed font file or the GLCD2 format as used in GLCDFontCreator2.

It also adds the follwoing features:

- Code page support: Create a font with a charset of a specific code page, making it easy to use non ASCII characters
- Run-Length encoded font data format (BrainSystems Run Length Font):
  - Save up to 70% of codespace compared to GLCD2 format, especially when using larger fonts
  - Ultra fast redering of characters
  - Allowing of linked characters, you can link any ASCII code to any glyph, saving even more space eg: you can link a character Â to the character A if you don't need it
  - Exluding characters: exclude any character you don't need
  - Character spacing information is included in the font data file

- optional export to legacy  GLCD2 format. Note this has not ben extensively tested.


The description of the run-length format is in the generated header file.

There are two projects in the repo:

FontCreator: Creates the fonts as described above
FontCreatorTest: A small windows test utility in C that loads the generated font header file and renders the font for testing. The font decoder code is in file "BrainSystemsRunLengthDeoder.c"

Credits:

GLCD_FontCreator - Copyright 2015 Martin Burri

Font Dialog derived from CustomFontDialog by Syed Umar Anis (http://umaranis.wordpress.com/)

Note: Fonts are copyrighted materials. Before using any derived bitmap make sure you are compliant with the license. There are a number of great free to use fonts out there. But don't distribute/sell derived fonts if the license is not explicitly allowing you this.
