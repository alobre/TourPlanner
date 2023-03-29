using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;

namespace TourPlanner
{
    public class Route : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this.PropertyChanged, new PropertyChangedEventArgs(propName));
        }

        private string _Start;
		public string Start
		{
			get { return _Start; }
			set { _Start = value; OnPropertyChanged(nameof(Start)); }
		}
        private string _Destination;
        public string Destination
        {
            get { return _Destination; }
            set { _Destination = value; OnPropertyChanged(nameof(Destination)); }
        }

    }
}
