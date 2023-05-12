using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Haley.Models;
using Haley.Abstractions;
using Haley.MVVM;

namespace TourPlanner
{
    public class MainVM : ChangeNotifier
    {
        private Route route;
        public Route TargetRoute
        {
            get { return route; }
            set { route = value; OnPropertyChanged(nameof(TargetRoute)); }
        }

        private ObservableCollection<Route> _routes; //ONLY WHEN ADDED/OR REMOVED (NOT FOR INTERNAL PROPERTY CHANGES)
        public ObservableCollection<Route> Routes
        {
            get { return _routes; }
            set { _routes = value; }
        }

        public void AddRoute()
        {
            Routes.Add(TargetRoute); //Add it to thecollection
            TargetRoute = new Route(); //resetting it.
        }

        public ICommand CMDAdd => new DelegateCommand(AddRoute);
        public ICommand CMDDelete => new DelegateCommand<Route>(DeleteRoute);

        private void DeleteRoute(Route obj)
        {
            if (obj == null) return;
            if (!Routes.Contains(obj)) return;

            Routes.Remove(obj);
        }

        public MainVM()
        {
            Routes = new ObservableCollection<Route>();
            TargetRoute = new Route();
        }
    }
}
