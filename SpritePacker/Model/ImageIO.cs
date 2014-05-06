using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;                   // MessageBox
using System.Windows.Media.Imaging;     // BitmapImage

using System.IO;                        // Filestream

using System.Security;                  // Exceptions

using System.Text.RegularExpressions;   // Regex

using Microsoft.Win32;                  // Open/Save Dialog

namespace SpritePacker.Model
{
    /// <summary>
    /// Handles File I/O for images
    /// </summary>
    class ImageIO
    {
        public SaveFileDialog SaveDiag = new Microsoft.Win32.SaveFileDialog();
        public OpenFileDialog OpenDiag = new Microsoft.Win32.OpenFileDialog();

        // - Filter Parameters -
        //  - 'p' for PNG
        //  - 'j' for JPG/JPEG
        //  - 'b' for BMP
        // Use any combination or none at all!
        // Used to build Win Open/SaveDialog filters
        private static string buildFilterStr(string customFilter)
        {
            // create an empty string to append things to
            string internalFilter = "";

            // @terrehbyte: consider instead looping over the string
            // and appending based on the current letter
            // so the developer can decide what order they want to
            // set the filters up as

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
        public string SaveDialog()
        {
            // Show the dialog and store whether or not the user selected something
            //  false means cancel
            //  true means selection occurred
            Nullable<bool> diagResult = SaveDiag.ShowDialog();

            // Return file path if true, otherwise return null
            if (diagResult == true)
            {
                return SaveDiag.FileName;   // this is actually the full/absolute string for file path
                // (i.e. "C:\Users\Student\Pictures\testImage.png")
            }
            else
            {
                // other program should handle null
                return null;
            }
        }

        // Opens a Windows Open Dialog
        public string OpenDialog()
        {
            Nullable<bool> diagResult = OpenDiag.ShowDialog();

            // Return filename if true, otherwise return null
            if (diagResult == true)
            {
                return OpenDiag.FileName;   // this is actually the full/absolute string for file path
                // (i.e. "C:\Users\Student\Pictures\testImage.png")
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
            switch (fileExt.ToLower())
            {
                case (".png"):
                    {
                        imageEnc = new PngBitmapEncoder();
                        break;
                    }
                case (".jpg"):
                    {
                        imageEnc = new JpegBitmapEncoder();
                        break;
                    }
                case (".bmp"):
                    {
                        imageEnc = new BmpBitmapEncoder();
                        break;
                    }
                default:
                    {
                        throw new System.Exception("Invalid file type specified for saving!");
                    }
            }

            // Add the BitmapImage to the encoder's frame queue
            imageEnc.Frames.Add(BitmapFrame.Create(newImage));

            // Open a file stream to save things to
            FileStream saveStream;

            try
            {
                saveStream = new FileStream(filePathName, FileMode.Create);

                // Save into the stream
                imageEnc.Save(saveStream);
                saveStream.Close(); // always close the stream when you're done
            }
            catch (SecurityException ex)
            {
                MessageBox.Show("Try running with elevated permissions.\nError Message: " + ex.Message, "Security Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something is preventing Windows from saving this file.\nErrorMessage: " + ex.Message, "Unspecified Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    };
}
