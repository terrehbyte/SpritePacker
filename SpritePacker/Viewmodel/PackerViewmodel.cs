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
            // Record reference to packer
            Packer = packer;

            // Assign ICommand
            AddCommand      = new PackerAddCom(this);
            RemoveCommand   = new PackerRemoveCom(this);
            ExportCommand   = new PackerExportCom(this);
            PreviewCommand  = new PackerPreviewCom(this);
        }

        // Bindable commands
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

        // Properties
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

        // Internal Calls to Model
        public void AddSubsprite()
        {
            ImageIO imgIO = new ImageIO();

            imgIO.OpenDiag.Filter = ImageIO.BuildFilterStr("pjb");
            imgIO.OpenDiag.Multiselect = true;

            if (imgIO.CreateOpenDialog() != null)
            {
                string[] selectedSubs = imgIO.OpenDiag.FileNames;

                for (int i = 0; i < selectedSubs.Length; i++)
                {
                    Packer.AddSubsprite(new Subsprite(selectedSubs[i]));
                }

                int Dummy;
                Packer.BuildAtlas();
            }

            
            else
            {
                return;
            }
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
