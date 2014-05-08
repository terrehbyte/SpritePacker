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
        public PackerViewmodel(Packer packer)
        {
            // record packer
            Packer = packer;

            // Assign ICommand
            AddCommand = new PackerAddCom(this);
            RemoveCommand = new PackerRemoveCom(this);
            ExportCommand = new PackerExportCom(this);
            PreviewCommand = new PackerPreviewCom(this);
        }

        // NEVER FORGET TO DEFINE THESE ICOMMANDS
        // NOTHING WILL WORK

        public ICommand AddCommand
        {
            get;
            private set;
        }
        public ICommand RemoveCommand
        {
            get;
            private set;
        }
        public ICommand PreviewCommand
        {
            get;
            private set;
        }
        public ICommand ExportCommand
        {
            get;
            private set;
        }

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
            throw new NotImplementedException();
        }

        public void RemoveSubsprite()
        {
            throw new NotImplementedException();
        }

        public void PreviewAtlas()
        {
            throw new NotImplementedException();
        }

        public void ExportAtlas()
        {
            throw new NotImplementedException();
        }
    }
}
