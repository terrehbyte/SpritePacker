using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input; // ICommand

using SpritePacker.Model;   // SpritePacker

using System.IO;            // FileStream

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
                    Packer.SubspriteList.Last().DeriveNameFromSource();
                }
            }

            // User didn't select anything, just stop
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
            Packer.SortSubsprites();
            Packer.BuildAtlas();
            Packer.BuildXML();
        }
        public void ExportAtlas()
        {
            Packer.SortSubsprites();
            Packer.BuildAtlas();
            Packer.BuildXML();

            // prompt for save
            ImageIO imageHandler = new ImageIO();

            Microsoft.Win32.SaveFileDialog saveDiag = new Microsoft.Win32.SaveFileDialog();
            saveDiag.FileName = "atlas.png";
            saveDiag.AddExtension = true;
            saveDiag.Filter = ImageIO.BuildFilterStr("p");

            Nullable<bool> diagResult = saveDiag.ShowDialog();
            if (diagResult == true)
            {
                // Save BitmapImage
                ImageIO.Save(Packer.Atlas, saveDiag.FileName);

                // Save XML
                string xmlSavepath = Path.ChangeExtension(saveDiag.FileName, ".xml");
                FileStream xmlStream = new FileStream(xmlSavepath, FileMode.Create);
                Packer.AtlasXML.Save(xmlStream);
                xmlStream.Close();
            }
            else
            {
                return;
            }



            //throw new NotImplementedException();
        }
    }
}
