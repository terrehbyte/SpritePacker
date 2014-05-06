using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input; // ICommand

namespace SpritePacker.Viewmodel
{
    class PackerRemoveCom : ICommand
    {
        private PackerViewmodel _packerView;

        public PackerRemoveCom(PackerViewmodel packerView)
        {
            _packerView = packerView;
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
