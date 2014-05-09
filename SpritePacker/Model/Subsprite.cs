using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media.Imaging;

namespace SpritePacker.Model
{
    class Subsprite
    {
        /// <summary>
        /// Constructs a Subsprite from a BitmapImage
        /// </summary>
        /// <param name="sourceImage">BitmapImage to be handled</param>
        public Subsprite(BitmapImage sourceImage)
        {
            bitmapData = sourceImage;
        }

        /// <summary>
        /// Constructs a Subsprite from a Uri to an image
        /// </summary>
        /// <param name="sourceUri">Uri pointing to image</param>
        public Subsprite(Uri sourceUri)
        {
            // TODO: handle invalid Uri leading to not image
            bitmapData = new BitmapImage(sourceUri);
        }

        /// <summary>
        /// Constructs a Subsprite from a path to an image
        /// </summary>
        /// <param name="sourcePath">String containing path to image</param>
        public Subsprite(string sourcePath)
        {
            // TODO: handle invalid string leading to not image
            bitmapData = new BitmapImage(new Uri(sourcePath));
        }

        public string Name = null;          // Name (for use in XML doc creation)
        public BitmapImage bitmapData;      // reference to actual image

        // add variables for position
            // this will be useful in building the XML sheet
    }
}