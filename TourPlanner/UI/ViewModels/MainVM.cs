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
        public ICommand DeleteTour
        {
            get;
            private set;
        }
        public MainVM()
        {
            try
            {
                InitCommands();
                Log.LogInfo("Start Logging");
                
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

        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set { _selectedTour = value; OnPropertyChanged(nameof(SelectedTour)); }
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
        /* public ICommand CMDDelete => new DelegateCommand<Tour>(DeleteTourAsync);*/

        private RelayCommandAsync<Tour> _deleteTourCommand;
        public ICommand DeleteTourCommand
        {
            get { return _deleteTourCommand; }
            set { _deleteTourCommand = (RelayCommandAsync<Tour>)value; OnPropertyChanged(nameof(DeleteTourCommand)); }
        }

        private bool _canAdd = true;
        public bool CanAdd
        {
            get { return _canAdd; }
            set { _canAdd = value; OnPropertyChanged(nameof(CanAdd)); }
        }
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

        internal async Task DeleteTourAsync(Tour tour)
        {
            try
            {
                // Perform the deletion logic here
                // Example:
                if (Tours.Contains(tour))
                {
                    // Assuming Tour has an ID property
                    // Delete the tour from the data source
                   // await DeleteTourFromDataSource(tour);

                    // Remove the tour from the Tours collection
                    DeleteTourFromTours(tour);
                }
            }
            catch (NullReferenceException n)
            {
                MessageBox.Show(n.Message);
                Log.LogError(n.Message);
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
                MessageBox.Show(ex.Message);
            }
        }
        private void DeleteTourFromTours(Tour tour)
        {
            if (tour == null) return;
            if (!Tours.Contains(tour)) return;

            Tours.Remove(tour);
        }
        private Task DeleteTourFromDataSource(Tour tour)
        {
            // TODO: Replace this code with your data source deletion logic

            // For example, if you are using a database:
            // 1. Establish a connection to the database
            // 2. Execute a DELETE query to remove the tour with the corresponding ID

            // Return a Task representing the completion of the deletion operation
            return Task.CompletedTask;
        }
        private void InitCommands()
        {
            Tours = new ObservableCollection<Tour>();
            TargetTour = new Tour();
            AddNewTour = new AddNewTourCMD(this);
            DeleteTourCommand = new RelayCommandAsync<Tour>(DeleteTourAsync);


            // Add some sample tours
            Tours.Add(new Tour(name: "Tour 1", time: 1, tour_desc: "Description 1", from: "From 1", to: "To 1", transport_type: "Bus", distance: 123, image_link: "image1", route_information: null));
            Tours.Add(new Tour(name: "Tour 2", time: 2, tour_desc: "Description 2", from: "From 2", to: "To 2", transport_type: "Car", distance: 456, image_link: "image2", route_information: null));
        }
    }

}