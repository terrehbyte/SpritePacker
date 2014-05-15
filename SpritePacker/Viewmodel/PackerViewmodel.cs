using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input; // ICommand

using SpritePacker.Model;   // SpritePacker

using System.IO;            // FileStream
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;            

// @terry - consider having the viewmodel completely wrap the model

namespace SpritePacker.Viewmodel
{
    internal class PackerViewmodel : INotifyPropertyChanged
    {
        // Bindable commands
        public ICommand NewCommand
        {
            get;
            private set;
        }
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
        private Packer _packerMan;
        public Packer PackerMan
        {
            get
            {
                return _packerMan;
            }
            set
            {
                _packerMan = value;
            }
        }

        // Properties
        public bool CanNew
        {
            get
            {
                return true;
            }
        }
        public bool CanAdd
        {
            get
            {
                return (PackerMan.SubspriteList != null);
            }
        }
        public bool CanRemove
        {
            get
            {
                return (PackerMan.SubspriteList.Count > 0);
            }
        }
        public bool CanPreview
        {
            get
            {
                return (PackerMan.SubspriteList.Count > 0);
            }
        }
        public bool CanExport
        {
            get
            {
                return (PackerMan.SubspriteList.Count > 0);
            }
        }

        private string savePath;

        private SubspriteViewmodel _selectedSubsprite;
        public SubspriteViewmodel SelectedSubsprite
        {
            set
            {
                _selectedSubsprite = value;
                OnPropertyChanged("SelectedSubsprite");
            }
            get
            {
                return _selectedSubsprite;
            }
        }

        public IEnumerable<string> SortAlgoEnum
        {
            get;
            set;
        }

        // Bindable Collections
        private ObservableCollection<SubspriteViewmodel> _subspriteList;
        public ObservableCollection<SubspriteViewmodel> SubspriteList
        {
            get
            {
                return _subspriteList;
            }
            set
            {
                // prevent self-assignment
                if (value == _subspriteList)
                {
                    return;
                }
                _subspriteList = value;
                OnPropertyChanged("SubspriteList");
            }
        }

        /// <summary>
        /// Constructs the packer view model
        /// </summary>
        /// <param name="packer">Packer hidden by viewmodel</param>
        public PackerViewmodel(Packer packer)
        {
            // Record reference to packer
            PackerMan = packer;

            // Assign ICommand
            AddCommand = new PackerAddCom(this);
            RemoveCommand = new PackerRemoveCom(this);
            ExportCommand = new PackerExportCom(this);
            PreviewCommand = new PackerPreviewCom(this);
            NewCommand = new PackerNewCom(this);

            // Initialize SortAlgoEnum
            string[] enumNames = Enum.GetNames(typeof(SpritePacker.Model.Packer.SortingAlgos));
            SortAlgoEnum = enumNames;

            _subspriteList = new ObservableCollection<SubspriteViewmodel>();
        }

        public void NewAtlas()
        {
            PackerMan = new Packer();
            SubspriteList = new ObservableCollection<SubspriteViewmodel>();
        }

        // Internal Calls to Model
        public void AddSubsprite()
        {
            ImageIO imgIO = new ImageIO();

            imgIO.OpenDiag.Filter = "Image Files (.png, .jpg, .bmp)|*.png;*.jpg;*.bmp";
            imgIO.OpenDiag.Multiselect = true;

            if (imgIO.CreateOpenDialog() != null)
            {
                string[] selectedSubs = imgIO.OpenDiag.FileNames;

                List<Subsprite> toAdd = new List<Subsprite>();

                for (int i = 0; i < selectedSubs.Length; i++)
                {
                    Subsprite tempSub = new Subsprite(selectedSubs[i]);
                    tempSub.DeriveNameFromSource();

                    SubspriteList.Add(new SubspriteViewmodel(tempSub));

                    // Queue this to be added to the packer's list
                    toAdd.Add(tempSub);
                }

                // add things to the packer's list
                for (int i = 0; i < toAdd.Count; i++)
                {
                    PackerMan.SubspriteList.Add(toAdd[i]);
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
            if (SelectedSubsprite != null)
            {
                PackerMan.SubspriteList.Remove(SelectedSubsprite.Subsprite);
                SubspriteList.Remove(SelectedSubsprite);
            }
        }
        public void PreviewAtlas()
        {
            PackerMan.SortSubsprites();
            PackerMan.BuildAtlas();
        }
        public void ExportAtlas()
        {
            if (savePath == null)
            {

                // prompt for save
                ImageIO imageHandler = new ImageIO();

                // create save dialog
                Microsoft.Win32.SaveFileDialog saveDiag = new Microsoft.Win32.SaveFileDialog();
                saveDiag.FileName = "atlas.png";
                saveDiag.AddExtension = true;
                saveDiag.Filter = ImageIO.BuildFilterStr("p");

                Nullable<bool> diagResult = saveDiag.ShowDialog();

                // User selected a save location
                if (diagResult == true)
                {
                    // Write Save Path for future saves
                    savePath = saveDiag.FileName;
                }
                // User did not select a save location
                else
                {
                    return;
                }
            }

            // Save BitmapImage
            PackerMan.SortSubsprites();
            PackerMan.BuildAtlas();

            ImageIO.Save(PackerMan.Atlas, savePath);

            Path.GetFileNameWithoutExtension(savePath);

            // Save XML
            PackerMan.BuildXML(Path.GetFileName(savePath));
            string xmlSavepath = Path.ChangeExtension(savePath, ".xml");
            FileStream xmlStream = new FileStream(xmlSavepath, FileMode.Create);
            PackerMan.AtlasXML.Save(xmlStream);
            xmlStream.Close();
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
