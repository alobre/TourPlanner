using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Haley.Models;
using Haley.Abstractions;
using Haley.MVVM;

namespace TourPlanner
{
    public class Route : ChangeNotifier
    {
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

        private string _Age;
        public string Age
        {
            get { return _Age; }
            set { _Age = value; OnPropertyChanged(nameof(Age)); }
        }

        public Route() { }
    }
}
