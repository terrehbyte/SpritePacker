using System;
using System.Security.Permissions;
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

namespace SpritePacker.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SpritePacker.Model.Packer packerSprite;
        SpritePacker.Viewmodel.PackerViewmodel packerViewmodel;

        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public MainWindow()
        {
            InitializeComponent();

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnExHandler);


            packerSprite = new SpritePacker.Model.Packer();
            packerViewmodel = new SpritePacker.Viewmodel.PackerViewmodel(packerSprite);
            
            DataContext = packerViewmodel;
        }

        private void OnExitExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnAboutProjectExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBoxResult msg = MessageBox.Show("- TBYTE SpritePacker -\n\nAIE Project 2013-2014\n\n Terry Nguyen",
                                                    "About TBYTE SpritePacker",
                                                    MessageBoxButton.OK,
                                                    MessageBoxImage.Information);
            return;
        }

        static void UnExHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            Console.WriteLine("UnExHandler caught: " + e.Message);
            Console.WriteLine("Runtime terminating: {0}", args.IsTerminating);
        }

        #region Commands
        private void OnNewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            packerViewmodel.NewAtlas();
        }
        private void OnOpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            packerViewmodel.AddSubsprite();
        }
        private void OnSaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            packerViewmodel.ExportAtlas();
        }
        private void OnSaveAsExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            packerViewmodel.ExportAtlas();
        }
        
        private void NewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (packerViewmodel.CanNew)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        private void OpenCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (packerViewmodel.CanAdd)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (packerViewmodel.CanExport)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        private void SaveAsCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (packerViewmodel.CanExport)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }
        #endregion
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