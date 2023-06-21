using System.ComponentModel;
using System.Runtime.CompilerServices;
using Haley.MVVM;
using Haley.Models;
using Haley.Abstractions;

namespace TourPlanner.UI.ViewModels
{
    public class NewTourDialogVM : ChangeNotifier
    {
        // Properties
        private string _start_Address;
        public string Start_Address
        {
            get { return _start_Address; }
            set
            {
                _start_Address = value;
                OnPropertyChanged();
            }
        }

        private string _start_AreaCode;
        public string Start_AreaCode
        {
            get { return _start_AreaCode; }
            set
            {
                _start_AreaCode = value;
                OnPropertyChanged();
            }
        }

        private string _start_City;
        public string Start_City
        {
            get { return _start_City; }
            set
            {
                _start_City = value;
                OnPropertyChanged();
            }
        }

        private string _start_Country;
        public string Start_Country
        {
            get { return _start_Country; }
            set
            {
                _start_Country = value;
                OnPropertyChanged();
            }
        }

        private string _dest_Address;
        public string Dest_Address
        {
            get { return _dest_Address; }
            set
            {
                _dest_Address = value;
                OnPropertyChanged();
            }
        }

        private string _dest_AreaCode;
        public string Dest_AreaCode
        {
            get { return _dest_AreaCode; }
            set
            {
                _dest_AreaCode = value;
                OnPropertyChanged();
            }
        }

        private string _dest_City;
        public string Dest_City
        {
            get { return _dest_City; }
            set
            {
                _dest_City = value;
                OnPropertyChanged();
            }
        }

        private string _dest_Country;
        public string Dest_Country
        {
            get { return _dest_Country; }
            set
            {
                _dest_Country = value;
                OnPropertyChanged();
            }
        }
    }
}