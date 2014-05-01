using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.IO;

using System.Text.RegularExpressions;

namespace SpritePacker
{
    static class ImageIO
    {
        // - Filter Parameters -
        //  - 'p' for PNG
        //  - 'j' for JPG/JPEG
        //  - 'b' for BMP
        // Use any combination or none at all!

        // Used to build Win Open/SaveDialog filters
        private static string buildFilterStr(string customFilter)
        {
            string internalFilter = "";

            // PNG
            if (Regex.IsMatch(customFilter, "p", RegexOptions.IgnoreCase))
            {
                internalFilter += "Png Image (.png)|*.png";
            }

            // All subsequent appends must include checks for existing filters

            // JPEG/JPG
            if (Regex.IsMatch(customFilter, "j", RegexOptions.IgnoreCase))
            {
                if (internalFilter.Length != 0)
                {
                    internalFilter += "|";
                }
                internalFilter += "Jpeg Image (.jpg)|*.jpg;*jpeg";
            }

            // BMP
            if (Regex.IsMatch(customFilter, "b", RegexOptions.IgnoreCase))
            {
                if (internalFilter.Length != 0)
                {
                    internalFilter += "|";
                }
                internalFilter += "Bitmap Image (.bmp)|*.bmp";
            }

            // Return the resulting filter for Open/Save Dialogs
            return internalFilter;
        }

        // Opens a Windows Save Dialog
        public static string SaveDialog(string filter = "pjb")
        {
            // Create a save file dialog
            Microsoft.Win32.SaveFileDialog saveDiag = new Microsoft.Win32.SaveFileDialog();
            saveDiag.FileName = "";
            saveDiag.Filter = ImageIO.buildFilterStr(filter);
            saveDiag.FilterIndex = 0;

            // Show the dialog
            Nullable<bool> diagResult = saveDiag.ShowDialog();

            if (diagResult == true)
            {
                return saveDiag.FileName;
            }
            else
            {
                return null;
            }
        }

        // Opens a Windows Open Dialog
        public static string OpenDialog(string filter = "pjb")
        {
            Microsoft.Win32.OpenFileDialog openDiag = new Microsoft.Win32.OpenFileDialog();
            openDiag.Filter = ImageIO.buildFilterStr(filter);

            Nullable<bool> diagResult = openDiag.ShowDialog();

            if (diagResult == true)
            {
                return openDiag.FileName;
            }
            else
            {
                return null;
            }
        }

        // takes a bitmapimage and a path to save the image to
        public static void Save(BitmapImage newImage, string savePath)
        {
            string filePathName = savePath;
            string fileExt = System.IO.Path.GetExtension(filePathName);

            // Create encoder
            BitmapEncoder imageEnc;

            // Decide which encoder to use
            if (fileExt.ToLower() == ".png")
            {
                imageEnc = new PngBitmapEncoder();
            }
            else if (fileExt.ToLower() == ".jpg")
            {
                imageEnc = new JpegBitmapEncoder();
            }
            else if (fileExt.ToLower() == ".bmp")
            {
                imageEnc = new BmpBitmapEncoder();
            }
            else
            {
                // this probably isn't serious enough to crash the program, but I don't want to handle it yet
                throw new System.Exception("Invalid file format chosen!");
            }

            imageEnc.Frames.Add(BitmapFrame.Create(newImage));

            // Open a file stream to save things to
            FileStream saveStream = new FileStream(filePathName, FileMode.Create);

            imageEnc.Save(saveStream);
            saveStream.Close(); // always close the stream when you're done
        }
    };
}