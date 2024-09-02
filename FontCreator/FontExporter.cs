using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FontCreator
{
    internal abstract class FontExporter
    {
        protected CharCollection mCharCollection;
        protected String mFontName;
        protected int mTotalDataLength = 0;
        public FontExporter(CharCollection charCollection)
        {
            mCharCollection = charCollection;
            mFontName = "";
        }
        protected abstract void ExportDataToFile(FileStream file);

        protected void AddStringToFile(FileStream file, String str)
        {
            file.Write(new UTF8Encoding(true).GetBytes(str));
        }


        public void ExportFont(String directory, String fontName)
        {
            mFontName = fontName;
            if (Directory.Exists(directory))
            {
                String filePath = directory + "/" + fontName + ".h";

                if (System.IO.File.Exists(filePath))
                {
                    DialogResult dialogResult = MessageBox.Show("The file " + filePath + " already exists\r\nDo you want to overwrite it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult != DialogResult.Yes)
                    {
                        return;
                    }
                }
                FileStream? file=null;
                try
                {
                    
                    file= File.Open(filePath, FileMode.Create);
                    ExportDataToFile(file);
                    file.Close();
                    MessageBox.Show(String.Format("Export successfull, total font data size: {0}", GetTotalDataLength), "File exported", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception e)
                {
                    if (file!=null)
                    {
                        file.Close();   
                    }
                    MessageBox.Show(String.Format("Error saving file\r\n{0}", e.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }



            }

            else
            {
                MessageBox.Show("Export Path does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    public int GetTotalDataLength { get => mTotalDataLength; }
    }
}
