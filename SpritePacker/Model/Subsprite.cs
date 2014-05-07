using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media.Imaging;

namespace SpritePacker.Model
{
    class Subsprite
    {
        public string Name = null;  // Name (for use in XML doc creation)
        public BitmapImage bitmapData;     // reference to actual image

        // add variables for position
            // this will be useful in building the XML sheet
    }
}