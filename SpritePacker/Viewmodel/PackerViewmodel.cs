using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input; // ICommand

using SpritePacker.Model;   // SpritePacker

namespace SpritePacker.Viewmodel
{
    internal class PackerViewmodel
    {
        PackerViewmodel(Packer packer)
        {
            // record packer
            Packer = packer;

            // Assign ICommand
            AddCommand = new PackerAddCom(this);
            RemoveCommand = new PackerRemoveCom(this);
            ExportCommand = new PackerExportCom(this);
            PreviewCommand = new PackerPreviewCom(this);
        }

        public ICommand AddCommand;
        public ICommand RemoveCommand;
        public ICommand PreviewCommand;
        public ICommand ExportCommand;

        // References to sprite Packer
        private Packer _packer;
        public Packer Packer
        {
            get
            {
                return _packer;
            }
            set
            {
                _packer = value;
            }
        }

        public bool CanAdd
        {
            get
            {
                return (Packer.SubspriteList != null);
            }
        }
        public bool CanRemove
        {
            get
            {
                return (Packer.SubspriteList.Count > 0);
            }
        }
        public bool CanPreview
        {
            get
            {
                return (Packer.SubspriteList.Count > 0);
            }
        }
        public bool CanExport
        {
            get
            {
                return (Packer.SubspriteList.Count > 0);
            }
        }

        public void AddSubsprite()
        {
        }

        public void RemoveSubsprite()
        {
        }

        public void PreviewAtlas()
        {
        }

        public void ExportAtlas()
        {
        }
    }
}
