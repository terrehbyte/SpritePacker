using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpritePacker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnExitExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnAboutProjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            return;
        }
    }


    static class CustomCommands
    {
        static CustomCommands()
        {
            exitCommand = new RoutedCommand("Exit", typeof(CustomCommands));
            aboutProjectCommand = new RoutedCommand("AboutProject", typeof(CommandBinding));
        }

        public static RoutedCommand Exit
        {
            get
            {
                return (exitCommand);
            }
        }

        public static RoutedCommand AboutProject
        {
            get
            {
                return (aboutProjectCommand);
            }
        }

        static RoutedCommand exitCommand;
        static RoutedCommand aboutProjectCommand;
    }
}