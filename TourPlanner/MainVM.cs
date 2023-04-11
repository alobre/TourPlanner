using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace TourPlanner
{
    internal class MainVM : ChangeNotifier
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propName));
        }

        private Route route;

        public Route TargetRoute
        {
            get { return route; }
            set { route = value; OnPropertyChanged(nameof(TargetRoute)); }
        }
        private ObservableCollection<Route> _routes;

        public ObservableCollection<Route> Routes
        {
            get { return _routes; }
            set { _routes = value; }
        }

        public void AddRoute()
        {
            Routes.Add(TargetRoute); //Add it to the collection
            TargetRoute = new Route(); //resetting it
        }

        public ICommand CMDAdd { get; set; }
        public MainVM()
        {
            CMDAdd = new AddCommand(this);
            Routes = new ObservableCollection<Route>();
        }

    }
}
