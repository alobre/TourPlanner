using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Haley.MVVM;
using Haley.Models;
using Haley.Abstractions;

namespace TourPlanner
{
    public class MainVM : ChangeNotifier
    {
        private Tour _targetTour;
        public Tour TargetTour
        {
            get { return _targetTour; }
            set { _targetTour = value; OnPropertyChanged(nameof(TargetTour)); }
        }

        private ObservableCollection<Tour> _tours;
        public ObservableCollection<Tour> Tours
        {
            get { return _tours; }
            set { _tours = value; }
        }

        public void AddTour()
        {
            Tours.Add(TargetTour); // Add it to the collection
            TargetTour = new Tour(); // Resetting it
        }

        public ICommand CMDAdd => new DelegateCommand(AddTour);
        public ICommand CMDDelete => new DelegateCommand<Tour>(DeleteTour);

        private void DeleteTour(Tour tour)
        {
            if (tour == null) return;
            if (!Tours.Contains(tour)) return;

            Tours.Remove(tour);
        }

        public MainVM()
        {
            Tours = new ObservableCollection<Tour>();
            TargetTour = new Tour();


        }
    }
}