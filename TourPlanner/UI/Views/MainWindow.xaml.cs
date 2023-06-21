using TourPlanner.UI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using TourPlanner.DL.DB;
using System.ComponentModel.DataAnnotations.Schema;

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainVM viewModel;
        /*private readonly TourPlannerDbContext _context = new TourPlannerDbContext();*/
        private TourPlannerDbContext _context = new TourPlannerDbContext();
        private CollectionViewSource ToursViewSource;
        private CollectionViewSource ViewSource;

        public MainWindow()
        {
            InitializeComponent();
            ViewSource = (CollectionViewSource)FindResource(nameof(ViewSource));
            ToursViewSource = (CollectionViewSource)FindResource(nameof(ToursViewSource));
            viewModel = new MainVM(); // Create an instance of MainVM
            DataContext = viewModel;
            /*DataContext = new MainVM(); //Connecting the view with viewmodel*/

        }
        private void OpenTourDialog()
        {
            NewTourDialog newTourDialog = new NewTourDialog(viewModel);
            newTourDialog.Owner = this; // Set the owner of the dialog to the MainWindow
            newTourDialog.ShowDialog();
        }

        private void bt_newTourDialog(object sender, RoutedEventArgs e)
        {
            // Show the new window
            OpenTourDialog();
        }

        private void DeleteTour_Click(object sender, RoutedEventArgs e)
        {
           /* string messageBoxText = "Do you want to save changes?";
            string caption = "Word Processor";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);*/
        }

        private void WndwMain_Loaded(object sender, RoutedEventArgs e)
        {
            // this is for demo purposes only, to make it easier
            // to get up and running
            _context.Database.EnsureCreated();

            // load the entities into EF Core
            _context.Tours.Load();
            _context.TourLogs.Load();
            

            // bind to the source
            ViewSource.Source =
                _context.TourLogs.Local.ToObservableCollection();
            ToursViewSource.Source =
                _context.Tours.Local.ToObservableCollection();
        }

        private void WndwMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _context.Dispose();
            base.OnClosing(e);
        }
    }
}
