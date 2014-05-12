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

using System.Diagnostics;           // Assert

namespace SpritePacker.Model
{
    class Packer : INotifyPropertyChanged
    {
        // = Data Stores =
        public BitmapImage Atlas;   // cached atlas
        private Vector targetDims;  // x and y correspond to atlas width and height
        public  List<Subsprite> SubspriteList = new List<Subsprite>();  // list of subsprites
        
        // = Settings =
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
                OnPropertyChanged("Offset");
            }

        }          // pixel offset between sprites

        private bool _powerOfTwo;
        public bool PowerOfTwo
        {
            get
            {
                return _powerOfTwo;
            }
            set
            {
                _powerOfTwo = value;
                OnPropertyChanged("PowerOfTwo");
            }
        } // whether atlas will be power of two
        
        private SortingAlgos _desiredSort;
        public SortingAlgos DesiredSort
        {
            get
            {
                return _desiredSort;
            }
            set
            {
                _desiredSort = value;
                OnPropertyChanged("DesiredSort");
            }
        }

        public enum SortingAlgos        // different sorting algorithms
        {
            Strip
        };

        // = Events =
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // = Functions =

        /// <summary>
        /// Calculates the target size of the atlas
        /// </summary>
        /// <returns>Returns a Vector with width and height in X and Y respectively</returns>
        private Vector calculateAtlasDims()
        {
            // Create variable to store dims
            Vector AtlasDims = new Vector(0.0, 0.0);

            int iTotalWidth = 0;

            // Get the total width
            for (int i = 0; i < SubspriteList.Count; i++)
            {
                // Add each individual subsprite's width
                iTotalWidth += SubspriteList[i].bitmapData.PixelWidth;

                // Add the offset between each sprite
                iTotalWidth += Offset;
            }

            int iTotalHeight = 0;

            // Get the biggest height

            // edge case of one image
            if (SubspriteList.Count == 1)
            {
                iTotalHeight = SubspriteList[0].bitmapData.PixelHeight;
            }
            else
            {
                for (int i = 1; i < SubspriteList.Count - 1; i++)
                {
                    iTotalHeight = Math.Max(SubspriteList[i - 1].bitmapData.PixelHeight,
                                            SubspriteList[i].bitmapData.PixelHeight);
                }
            }

            // Add offset of all subsprites going down
            iTotalHeight += SubspriteList.Count * Offset;

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

            Debug.Assert(AtlasDims.X != 0 &&
                         AtlasDims.Y != 0);

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
                        sorter = stripSorter;
                        break;
                    }
                default:
                    {
                        throw new Exception("No Sort Algo!");
                    }
            }

            // Sort the Subsprites & Store the Returned Value
            Atlas = sorter(SubspriteList);

            // Return the stored value
            return Atlas;
        }

        /// <summary>
        /// Adds a subsprite into the list
        /// </summary>
        /// <param name="addSub">Subsprite to be added to the list</param>
        public void AddSubsprite(Subsprite addSub)
        {
            SubspriteList.Add(addSub);
        }
        /// <summary>
        /// Removes a subsprite from the list
        /// </summary>
        /// <param name="removeSub">Subsprite to be removed from the list</param>
        public void RemoveSubsprite(Subsprite removeSub)
        {
            SubspriteList.Remove(removeSub);
        }

        /// <summary>
        /// Builds the XML document of the atlas
        /// </summary>
        public void BuildXML()
        {
            throw new NotImplementedException();
        }

        private delegate BitmapImage SubspriteSorter(List<Subsprite> subsprites);
        private BitmapImage stripSorter(List<Subsprite> subsprites)
        {
            BitmapImage imageResult;

            BitmapFrame[] frames = new BitmapFrame[SubspriteList.Count];

            for (int i = 0; i < SubspriteList.Count; i++)
            {
                // create bitmap frames
                frames[i] = BitmapDecoder.Create(SubspriteList[i].bitmapData.UriSource,
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

                    // Write position to Subsprite
                    SubspriteList[i].Pos = new Vector(prevFrames, 0);

                    prevFrames += frames[i].PixelWidth;
                }
            }

            // Convert DrawingVisual into a BitmapSource
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)targetDims.X, (int)targetDims.Y, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            bmp.Freeze();   // finalize the image

            PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
            pngEncoder.Frames.Add(BitmapFrame.Create(bmp));

            // Stuff BitmapSource into encoder
            ImageIO imageIOHandler = new ImageIO();

            // Get working directory
            string cd = Directory.GetCurrentDirectory();

            // Save to temporary file
            using (Stream stream = File.Create(cd + @"\temp.bmp"))
            {
                pngEncoder.Save(stream);
                stream.Close();
            }

            try
            {
                Uri tempPath = new Uri(cd + "\\temp.bmp");
                imageResult = new BitmapImage(tempPath);
            }
            catch (System.ArgumentNullException)
            {
                throw new Exception();
            }
            catch (System.UriFormatException)
            {
                throw new Exception();
            }

            // Record resulting bitmap into imageResult
            return imageResult;
        }
    }
}