using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;               // Vectors
using System.Windows.Controls;      // ???
using System.Windows.Media;         // DrawingContext
using System.Windows.Media.Imaging; // BitmapImage

using System.ComponentModel;        // INotifyPropertyChanged

using System.IO;

namespace SpritePacker.Model
{
    class Packer : INotifyPropertyChanged
    {
        // Data Stores
        public BitmapImage Atlas;   // cached atlas
        private Vector targetDims;  // x and y correspond to atlas width and height
        private List<Subsprite> subspriteList = new List<Subsprite>();  // list of subsprites
        
        // Settings
        private int _offset = 0;
        public int Offset
        {
            get
            {
                return _offset;
            }
            set
            {
                _offset = value >= 0 ? value : 0;
            }

        }          // pixel offset between sprites
        public bool PowerOfTwo = false; // whether atlas will be power of two
        public SortingAlgos DesiredSort;

        public enum SortingAlgos        // different sorting algorithms
        {
            Strip
        };

        // Events
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Functions

        private Vector calculateAtlasDims()
        {
            // Create variable to store dims
            Vector AtlasDims = new Vector(0.0, 0.0);

            int iTotalWidth = 0;

            // Get the total width
            for (int i = 0; i < subspriteList.Count - 1; i++)
            {
                // Add each individual subsprite's width
                iTotalWidth += subspriteList[i].bitmapData.PixelWidth;

                // Add the offset between each sprite
                iTotalWidth += Offset;
            }

            int iTotalHeight = 0;

            // Get the biggest height
            for (int i = 1; i < subspriteList.Count - 1; i++)
            {
                iTotalHeight = Math.Max(subspriteList[i - 1].bitmapData.PixelHeight,
                                        subspriteList[i].bitmapData.PixelHeight);
            }

            // Add offset of all subsprites going down
            iTotalHeight += subspriteList.Count * Offset;

            // @terrehbyte: When should the offset be added in?
            //              We'll do it here for now, but figure out
            //              where it should go in the process...

            if (PowerOfTwo)
            {
                // Get the area
                int iAtlasArea = iTotalHeight * iTotalWidth;

                // Square the area to get the length for the sides
                double iTotalRooted = Math.Sqrt((double)iAtlasArea);

                AtlasDims = new Vector(iTotalRooted, iTotalRooted);
            }
            else
            {
                switch (DesiredSort)
                {
                    // Strip Atlas
                    case (SortingAlgos.Strip):
                        {
                            AtlasDims = new Vector((double)iTotalWidth, (double)iTotalHeight);
                            break;
                        }
                    // Should Never Be Reached
                    default:
                        {
                            throw new Exception("No Sort Algo!");
                        }
                }
            }

            return AtlasDims;
        }

        /// <summary>
        /// Build sprite atlas from list of subsprites
        /// </summary>
        /// <returns>Returns sprite atlas as image</returns>
        public BitmapImage BuildAtlas()
        {
            // Determine atlas target resolution
            targetDims = calculateAtlasDims();

            // Create a SubspriteSorter
            SubspriteSorter sorter;
            
            // assign it the proper algorithm's function
            switch (DesiredSort)
            {
                case (SortingAlgos.Strip):
                    {
                        sorter = stripSort;
                        break;
                    }
                default:
                    {
                        throw new Exception("No Sort Algo!");
                    }
            }

            // Sort the Subsprites & Store the Returned Value
            Atlas = sorter(subspriteList);

            // Return the stored value
            return Atlas;
        }

        private delegate BitmapImage SubspriteSorter(List<Subsprite> subsprites);

        private BitmapImage stripSort(List<Subsprite> subsprites)
        {
            BitmapImage imageResult;

            BitmapFrame[] frames = new BitmapFrame[subspriteList.Count];

            for (int i = 0; i < subspriteList.Count; i++)
            {
                // create bitmap frames
                frames[i] = BitmapDecoder.Create(subspriteList[i].bitmapData.UriSource,
                                                 BitmapCreateOptions.DelayCreation,
                                                 BitmapCacheOption.OnLoad).Frames.First();
            }

            // Create the drawing visual
            DrawingVisual drawingVisual = new DrawingVisual();

            // Draw into it
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                int prevFrames = 0;
                for (int i = 0; i < frames.Length; i++)
                {
                    drawingContext.DrawImage(frames[i], new Rect(prevFrames, 0, frames[i].PixelWidth, frames[i].PixelHeight));
                    prevFrames += frames[i].PixelWidth;
                }
            }

            // Convert DrawingVisual into a BitmapSource
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)targetDims.X, (int)targetDims.Y, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            BmpBitmapEncoder bmpEncoder = new BmpBitmapEncoder();
            bmpEncoder.Frames.Add(BitmapFrame.Create(bmp));

            // Stuff BitmapSource into encoder
            ImageIO imageIOHandler = new ImageIO();

            // Get working directory
            string cd = Directory.GetCurrentDirectory();

            // Save to temporary file
            using (Stream stream = File.Create(cd + @"\temp.bmp"))
            {
                bmpEncoder.Save(stream);
                stream.Close();
            }

            imageResult = new BitmapImage(new Uri(cd + "\temp.bmp"));

            // Record resulting bitmap into imageResult
            return imageResult;
        }

        /// <summary>
        /// Adds a subsprite into the list
        /// </summary>
        /// <param name="addSub"></param>
        public void AddSubsprite(Subsprite addSub)
        {
            subspriteList.Add(addSub);
        }
        /// <summary>
        /// Removes a subsprite from the list
        /// </summary>
        /// <param name="removeSub"></param>
        public void RemoveSubsprite(Subsprite removeSub)
        {
            subspriteList.Remove(removeSub);
        }
    }
}