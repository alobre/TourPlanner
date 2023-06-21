using System;
using System.Windows;
using System.Windows.Input;
using TourPlanner.UI.ViewModels;

namespace TourPlanner.UI.Views
{
    public partial class NewTourDialog : Window
    {
        private MainVM mainViewModel;
        private NewTourDialogVM dViewModel;

        public string Start_Address
        {
            get { return tb_start_address.Text; }
            set { tb_start_address.Text = value; }
        }

        public string Start_AreaCode
        {
            get { return tb_start_areacode.Text; }
            set { tb_start_areacode.Text = value; }
        }

        public string Start_City
        {
            get { return tb_start_city.Text; }
            set { tb_start_city.Text = value; }
        }

        public string Start_Country
        {
            get { return tb_start_state.Text; }
            set { tb_start_state.Text = value; }
        }

        public string Dest_Address
        {
            get { return tb_dest_address.Text; }
            set { tb_dest_address.Text = value; }
        }

        public string Dest_AreaCode
        {
            get { return tb_dest_areacode.Text; }
            set { tb_dest_areacode.Text = value; }
        }

        public string Dest_City
        {
            get { return tb_dest_city.Text; }
            set { tb_dest_city.Text = value; }
        }

        public string Dest_Country
        {
            get { return tb_dest_state.Text; }
            set { tb_dest_state.Text = value; }
        }

        public NewTourDialog(MainVM mainViewModel, NewTourDialogVM dialogViewModel)
        {
            InitializeComponent();

            this.mainViewModel = mainViewModel;
            dViewModel = dialogViewModel;
            DataContext = dViewModel;

            // Set initial values
            if (DataContext != null)
            {
                dViewModel.Start_Address = "Schumanngasse 77";
                dViewModel.Start_AreaCode = "1170";
                dViewModel.Start_City = "Vienna";
                dViewModel.Start_Country = "Austria";
                dViewModel.Dest_Address = "Hetzendorferstraße 163";
                dViewModel.Dest_AreaCode = "1120";
                dViewModel.Dest_City = "Vienna";
                dViewModel.Dest_Country = "Austria";
            }
        }

        public bool DialogResult { get; private set; }

        private void b_finish_Click(object sender, RoutedEventArgs e)
        {
            if (mainViewModel != null && mainViewModel.AddNewTourCommand.CanExecute(null))
            {
                mainViewModel.AddNewTourCommand.Execute(null);
            }
            DialogResult = true;
            Close(); // Close the current dialog
        }
    }
}
