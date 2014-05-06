using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input; // ICommand

using SpritePacker.Model;   // SpritePacker

namespace SpritePacker.Viewmodel
{
    class PackerViewmodel
    {
        PackerViewmodel(Packer packer)
        {
            Packer = packer;

            AddCommand = new PackerAddCom(this);
            RemoveCommand = new PackerRemoveCom(this);
            ExportCommand = new PackerExportCom(this);
            PreviewCommand = new PackerPreviewCom(this);
        }

        public ICommand AddCommand;
        public ICommand RemoveCommand;
        public ICommand PreviewCommand;
        public ICommand ExportCommand;

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

        public bool CanAdd;
        public bool CanRemove;
        public bool CanPreview;
        public bool CanExport;
    }
}
