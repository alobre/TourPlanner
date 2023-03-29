using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;

namespace TourPlanner
{
    internal class MainVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propName));
        }

        private Route route;

        public Route Route
        {
            get { return route; }
            set { route = value; OnPropertyChanged(nameof(Route)); }
        }

        public ICommand CMDAdd { get; set; }

    }
}
