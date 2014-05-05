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
            MessageBoxResult msg = MessageBox.Show("- TBYTE SpritePacker -\n\nAIE Project 2013-2014\n\n Terry Nguyen", "About TBYTE SpritePacker", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
    }


    static class CustomCommands
    {
        // Define how custom commands will be referenced in the XAML
        static CustomCommands()
        {
            exitCommand = new RoutedCommand("Exit", typeof(CustomCommands));
            aboutProjectCommand = new RoutedCommand("AboutProject", typeof(CustomCommands));
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