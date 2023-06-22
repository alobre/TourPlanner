using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.BL.Models;

namespace TourPlanner.PL.Command
{
    internal class GenerateReportCMD : ICommand
    {
        private MainVM _viewModel;

        public GenerateReportCMD(MainVM viewModel)
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
            _viewModel.GenerateReport();
        }
    }
}

