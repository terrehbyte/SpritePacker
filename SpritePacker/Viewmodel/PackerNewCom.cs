using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Input;

namespace SpritePacker.Viewmodel
{
    class PackerNewCom : ICommand
    {
        private PackerViewmodel _packerView;

        public PackerNewCom(PackerViewmodel packerView)
        {
            _packerView = packerView;
        }

        public bool CanExecute(object parameter)
        {
            return _packerView.CanNew;
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
            _packerView.NewAtlas();
        }
    }
}
