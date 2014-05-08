using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input; // ICommand

namespace SpritePacker.Viewmodel
{
    class PackerExportCom : ICommand
    {
        // reference to PackerViewmodel
        private PackerViewmodel _packerView;

        public PackerExportCom(PackerViewmodel packerView)
        {
            _packerView = packerView;
        }

        public bool CanExecute(object parameter)
        {
            return _packerView.CanExport;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _packerView.ExportAtlas();
        }
    }
}
