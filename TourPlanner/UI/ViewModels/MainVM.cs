using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Haley.MVVM;
using Haley.Models;
using Haley.Abstractions;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.BL.Services.Logging;
using TourPlanner.BL.Services.MapQuest;
using TourPlanner.PL.DialogServices;
using TourPlanner.PL.Command;

namespace TourPlanner
{
    public class MainVM : ChangeNotifier
    {
        public ICommand AddNewTour
        {
            get;
            private set;
        }
        public MainVM()
        {
            try
            {
                Tours = new ObservableCollection<Tour>();
                TargetTour = new Tour();
                AddNewTour = new AddNewTourCMD(this);
                Log.LogInfo("Start Logging");
                /*initCommands();*/
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
            }
        }


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

        private bool _canAdd = true;
        public bool CanAdd
        {
            get { return _canAdd; }
            set { _canAdd = value; OnPropertyChanged(nameof(CanAdd)); }
        }

        private void DeleteTour(Tour tour)
        {
            if (tour == null) return;
            if (!Tours.Contains(tour)) return;

            Tours.Remove(tour);
        }
        /*private void initCommands()
        {
            AddNewTour = new AddNewTourCMD(this);
        }*/
        public ObservableCollection<Tour> AllTours { get; private set; }

        PL.DialogServices.IDialogService _dialogService = new DialogService();
        internal async Task CreateNewTour()
        {
            try
            {
                var result = _dialogService.ShowDialog(callback =>
                {
                    var test = callback;
                });

                var route = await MapQuestRequestHandler.GetRouteAsync(result.start, result.dest);
                Tour tour = new Tour();
                tour.Distance = route.route.route.distance;
                tour.Time = route.route.route.time;
                tour.Description = $"From {result.start.Address} {result.start.AreaCode} {result.start.City} to {result.dest.Address} {result.dest.AreaCode} {result.dest.City}";
                tour.TransportType = route.route.route.options.routeType;
                tour.From = $"{result.start.Address}";
                tour.To = result.dest.Address;
                tour.RouteImage = route.URL;
                tour.Name = $"{result.start.Address}TO{result.dest.Address}";
                tour.RouteDetails = route.image;
                Log.LogInfo("Neue Tour erstellt Name: " + tour.Name);


                // Add the new tour to the collection
                Tours.Add(tour);
                Log.LogInfo("New tour created with name: " + tour.Name);
            }
            catch (AggregateException ex)
            {
                foreach (Exception ex2 in ex.Flatten().InnerExceptions)
                {
                    Log.LogError(ex2.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Log.LogError(ex.Message);
            }
        }
        /*public bool CanAdd { get; internal set; } = true;*/
    }
}