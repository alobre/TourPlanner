using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.BL.Models;

namespace TourPlanner.PL.Command
{
    internal class AddTourLogCMD : ICommand
    {
        private MainVM _viewModel;

        public AddTourLogCMD(MainVM viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            await _viewModel.AddTourLog();
        }
    }
}
