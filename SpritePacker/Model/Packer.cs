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

using System.Xml.Linq;              // XDocument

namespace SpritePacker.Model
{
    class Packer : INotifyPropertyChanged
    {
        // = Data Stores =
        private RenderTargetBitmap _atlas;
        public BitmapImage Atlas
        {
            get
            {
                BitmapImage atlasImg = new BitmapImage();

                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(_atlas));

                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    atlasImg.BeginInit();
                    atlasImg.CacheOption = BitmapCacheOption.OnLoad;
                    atlasImg.StreamSource = stream;
                    atlasImg.EndInit();
                }

                return atlasImg;
            }
            private set
            {
                OnPropertyChanged("Atlas");
            }
        }
        private XDocument _atlasXML;
        public XDocument AtlasXML
        {
            get
            {
                return _atlasXML;
            }
            set
            {
                _atlasXML = value;
                OnPropertyChanged("AtlasXML");
            }
        }
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
                // must be zero or greater
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
            STRIP,
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

                // Add the pixel offset between each sprite
                iTotalWidth += Offset;
            }

            // Get the biggest height
            int iTotalHeight = SubspriteList[0].bitmapData.PixelHeight;

            for (int i = 0; i < SubspriteList.Count; i++)
            {
                iTotalHeight = Math.Max(iTotalHeight,
                                        SubspriteList[i].bitmapData.PixelHeight);
            }

            // @terrehbyte: When should the offset be added in?
            //              We'll do it here for now, but figure out
            //              where it should go in the process...

            if (PowerOfTwo)
            {
                // Add offset of sprite going down
                iTotalHeight += Offset;

                // Get the area
                int iAtlasArea = iTotalHeight * iTotalWidth;

                // Square the area to get the length for the sides
                int iTotalRooted = (int)Math.Sqrt((double)iAtlasArea);

                bool isPowTwo = false;

                // checks for pow of two
                isPowTwo = (iTotalRooted & (iTotalRooted - 1)) == 0;

                if (!isPowTwo)
                {
                    iTotalRooted--;
                    iTotalRooted |= iTotalRooted >> 1;
                    iTotalRooted |= iTotalRooted >> 2;
                    iTotalRooted |= iTotalRooted >> 4;
                    iTotalRooted |= iTotalRooted >> 8;
                    iTotalRooted |= iTotalRooted >> 16;
                    iTotalRooted++;
                }

                AtlasDims = new Vector(iTotalRooted, iTotalRooted);
            }
            else
            {
                switch (DesiredSort)
                {
                    // Strip Atlas
                    case (SortingAlgos.STRIP):
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
        /// Sort the subsprites using the current algo
        /// </summary>
        public void SortSubsprites()
        {
            // Determine atlas target resolution
            targetDims = calculateAtlasDims();

            // Create a SubspriteSorter
            SubspriteSorter sorter;
            
            // assign it the proper algorithm's function
            switch (DesiredSort)
            {
                case (SortingAlgos.STRIP):
                    {
                        sorter = stripSorter;
                        break;
                    }
                default:
                    {
                        throw new Exception("No Sort Algo!");
                    }
            }

            // Sort the Subsprites
            sorter(SubspriteList);
        }

        /// <summary>
        /// Saves current result of sorting to filesystem
        /// </summary>
        /// <returns>Sorted sprite atlas</returns>
        public BitmapImage BuildAtlas()
        {
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
                for (int i = 0; i < frames.Length; i++)
                {
                    drawingContext.DrawImage(frames[i], new Rect((Point)SubspriteList[i].Pos, SubspriteList[i].Dims));
                }
            }

            // Convert DrawingVisual into a BitmapSource
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)targetDims.X, (int)targetDims.Y, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);
            bmp.Freeze();   // finalize the image

            // store internally
            _atlas = bmp;
            Atlas = Atlas;

            // Return it too
            return Atlas;
        }
        /// <summary>
        /// Builds the XML document of the atlas
        /// </summary>
        public void BuildXML(string imagePath=null)
        {
            // def
            if (imagePath == null)
            {
                imagePath = "null";
            }

            // Populate with subsprites
            Object[] XMLelem = new Object[SubspriteList.Count];
            for (int i = 0; i < SubspriteList.Count; i++)
            {
                // Create the subsprite
                XElement node = new XElement("SubTexture");

                // Cache this shit so I don't have to type it every time
                Subsprite curSub = SubspriteList[i];

                // Add the attributes of the subsprite
                node.SetAttributeValue("name", curSub.Name);
                node.SetAttributeValue("x", curSub.Pos.X);
                node.SetAttributeValue("y", curSub.Pos.Y);
                node.SetAttributeValue("width", curSub.Dims.Width);
                node.SetAttributeValue("height", curSub.Dims.Height);

                // Stuff shit into array
                XMLelem[i] = node;
            }

            XElement XMLRootNode = new XElement("TextureAtlas", XMLelem);  // nest the subsprites
            XMLRootNode.SetAttributeValue("imagePath", imagePath);   // this will come after

            // Create Declaration - this will be hard coded for now
            XDeclaration XMLdec = new XDeclaration("1.0", "utf-8", "yes");

            // Create Doc
            XDocument XMLdoc = new XDocument(XMLdec, XMLRootNode);

            // Record doc
            AtlasXML = XMLdoc;
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

        #region Sorters
        private delegate void SubspriteSorter(List<Subsprite> subsprites);
        private void stripSorter(List<Subsprite> subsprites)
        {
            // Value to offset by
            int xOffset = 0;
            int yOffset = 0;

            int bigHeight = 0;

            for (int i = 0; i < SubspriteList.Count; i++)
            {
                // Store current subsprite
                Subsprite curSub = SubspriteList[i];

                // Wrap when necessary
                if (xOffset + curSub.Dims.Width > targetDims.X)
                {
                    xOffset = 0;
                    yOffset += bigHeight;

                    // reset bigHeight
                    bigHeight = 0;
                }

                // Write position to Subsprite
                SubspriteList[i].Pos = new Vector(xOffset, yOffset);

                // Cumulative offset
                xOffset += SubspriteList[i].bitmapData.PixelWidth + Offset;

                if ((int)SubspriteList[i].Dims.Height + Offset > bigHeight)
                {
                    bigHeight = (int)curSub.Dims.Height + Offset;
                }

                Debug.Assert(yOffset < targetDims.Y);
            }
        }
        #endregion
    }
}