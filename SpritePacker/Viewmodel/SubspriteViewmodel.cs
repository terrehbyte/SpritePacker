using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using SpritePacker.Model;       // Subsprite

namespace SpritePacker.Viewmodel
{
    class SubspriteViewmodel : INotifyPropertyChanged
    {
        private Subsprite _subsprite;
        public Subsprite Subsprite
        {
            get
            {
                return _subsprite;
            }
            set
            {
                _subsprite = value;
            }
        }

        public SubspriteViewmodel(Subsprite model)
        {
            Subsprite = model;
        }

        public string Name
        {
            get
            {
                return Subsprite.Name;
            }
            private set
            {
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
