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
        public BitmapImage Atlas;
        private Vector targetDims;
        private List<Subsprite> subspriteList = new List<Subsprite>();
        
        // Settings
        public int Offset= 0;
        public bool PowerOfTwo = false;
        public enum SortingAlgos
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
        public BitmapImage BuildAtlas();
        public void AddSubsprite(Subsprite addSub);
        public void RemoveSubsprite(Subsprite removeSub);
    }
}
