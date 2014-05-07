using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;               // Vectors
using System.Windows.Media.Imaging; // BitmapImage

using System.ComponentModel;        // INotifyPropertyChanged

namespace SpritePacker.Model
{
    class Packer : INotifyPropertyChanged
    {
        // Data Stores
        public BitmapImage Atlas;   // cached atlas
        private Vector targetDims;  // x and y correspond to atlas width and height
        private List<Subsprite> subspriteList = new List<Subsprite>();  // list of subsprites
        
        // Settings
        public int Offset = 0;          // pixel offset between sprites
        public bool PowerOfTwo = false; // whether atlas will be power of two
        public enum SortingAlgos        // different sorting algorithms
        {
            None
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

        /// <summary>
        /// Build sprite atlas from list of subsprites
        /// </summary>
        /// <returns>Returns sprite atlas as image</returns>
        public BitmapImage BuildAtlas()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Adds a subsprite into the list
        /// </summary>
        /// <param name="addSub"></param>
        public void AddSubsprite(Subsprite addSub)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Removes a subsprite from the list
        /// </summary>
        /// <param name="removeSub"></param>
        public void RemoveSubsprite(Subsprite removeSub)
        {
            throw new NotImplementedException();
        }
    }
}
