using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TourPlanner
{
    class AddCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private MainVM associatedVM;
        public bool CanExecute(object? parameter)
        {
            //throw new NotImplementedException();
            return true;
        }

        public void Execute(object? parameter)
        {
            //throw new NotImplementedException();
            associatedVM.AddRoute();
        }

        public AddCommand(MainVM vm)
        {
            associatedVM = vm;
        }
    }
}
