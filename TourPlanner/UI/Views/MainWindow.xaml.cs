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

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM(); //Connecting the view with viewmodel

        }
        private void OpenTourDialog()
        {
            NewTourDialog newTourDialog = new NewTourDialog();
            newTourDialog.Owner = this; // Set the owner of the dialog to the MainWindow
            newTourDialog.ShowDialog();
        }

        private void bt_newTourDialog(object sender, RoutedEventArgs e)
        {


            // Show the new window
            OpenTourDialog();
        }
    }
}
