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
        BitmapImage bitmapData;     // reference to actual image
    }
}
