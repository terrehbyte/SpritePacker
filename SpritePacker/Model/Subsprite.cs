using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media.Imaging; // BitmapImage
using System.Windows;

using System.IO;                    // Path

namespace SpritePacker.Model
{
    class Subsprite
    {
        #region Ctor
        /// <summary>
        /// Default constructor (does nothing)
        /// </summary>
        public Subsprite()
        {
        }

        /// <summary>
        /// Constructs a Subsprite from a BitmapImage
        /// </summary>
        /// <param name="sourceImage">BitmapImage to be handled</param>
        public Subsprite(BitmapImage sourceImage)
        {
            bitmapData = sourceImage;
        }

        /// <summary>
        /// Constructs a Subsprite from a BitmapImage and string
        /// </summary>
        /// <param name="sourceImage">BitmapImage to be handled</param>
        /// <param name="name">Name in XML</param>
        public Subsprite(BitmapImage sourceImage, string name) : this(sourceImage)
        {
            Name = name;
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
        /// Constructs a Subsprite from a Uri to an image
        /// </summary>
        /// <param name="sourceUri">Uri pointing to image</param>
        /// <param name="name">Name in XML</param>
        public Subsprite(Uri sourceUri, string name) : this(sourceUri)
        {
            Name = name;
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

        /// <summary>
        /// Constructs a Subsprite from a path to an image
        /// </summary>
        /// <param name="sourcePath">String containing path to image</param>
        /// <param name="name">Name in XML</param>
        public Subsprite(string sourcePath, string name) : this(sourcePath)
        {
            Name = name;
        }
        #endregion
        public string Name = null;          // Name (for use in XML doc creation)
        public BitmapImage bitmapData;      // reference to actual image

        /// <summary>
        /// Sets the name as its filename w/o extension
        /// </summary>
        public void DeriveNameFromSource()
        {
            Name = Path.GetFileNameWithoutExtension(bitmapData.UriSource.LocalPath);
        }

        // origin is top left
        public Vector Pos;
        public Vector Dims
        {
            get
            {
                return new Vector(bitmapData.PixelWidth, bitmapData.PixelHeight);
            }
            private set
            {
                
            }
        }
    }
}