using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;
using Haley.MVVM;
using Haley.Models;
using Haley.Abstractions;
using TourPlanner.BL.Services.Logging;
using TourPlanner.BL.Services.MapQuest;
using TourPlanner.PL.DialogServices;
using TourPlanner.PL.Command;
using System.Windows.Data;
using TourPlanner.BL.Models;
using System.Collections.Generic;
using System.Xml.Linq;
using TourPlanner.UI.ViewModels;

namespace TourPlanner
{
    public class MainVM : ChangeNotifier
    {
        private Tour _targetTour;
        private ObservableCollection<Tour> _tours;
        private Tour _selectedTour;
        private bool _canAdd = true;
        private DialogService _dialogService = new DialogService(null);

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

        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                _selectedTour = value;
                OnPropertyChanged(nameof(SelectedTour));
            }
        }
        public Tour TargetTour
        {
            get { return _targetTour; }
            set { _targetTour = value; OnPropertyChanged(nameof(TargetTour)); }
        }

        public ObservableCollection<Tour> Tours
        {
            get { return _tours; }
            set { _tours = value; OnPropertyChanged(nameof(Tours)); }
        }
        public void ClearTours()
        {
            Tours.Clear();
        }
        public bool CanAdd
        {
            get { return _canAdd; }
            set { _canAdd = value; OnPropertyChanged(nameof(CanAdd)); }
        }
        private TourLogVM _tourLogViewModel;
        public TourLogVM TourLogViewModel
        {
            get { return _tourLogViewModel; }
            set
            {
                _tourLogViewModel = value;
                OnPropertyChanged(nameof(TourLogViewModel));
            }
        }

        public ICommand AddNewTourCommand { get; private set; }
        public ICommand DeleteTourCommand { get; private set; }
        public ICommand ChangeSelectedTourCommand { get; private set; }
        public ICommand AddTourLogCommand { get; private set; }


        public async Task CreateNewTour()
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
                tour.Name = $"{result.start.Address} TO {result.dest.Address}";
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
        public event PropertyChangedEventHandler PropertyChanged;

        public async Task DeleteTourAsync(Tour tour)
        {
            try
            {
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

            if (tour == SelectedTour) SelectedTour = null;

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

        public async Task SetSelectedTour(Tour tour)
        {
            try
            {
                SelectedTour = tour;
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
        public async Task AddTourLog()
        {
            try
            {

                TourLog tourLog = new TourLog(comment: TourLogViewModel.Comment, difficulty: TourLogViewModel.Difficulty, rating: TourLogViewModel.Rating, dateTime: TourLogViewModel.Date, totalTime: int.Parse(TourLogViewModel.Time));
                foreach (Tour tour in Tours)
                {
                    if(tour.Equals(SelectedTour))
                    { 
                        tour.TourLogs.Add(tourLog);
                        // Also add in DB
                    }
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
        private void InitCommands()
        {
            Tours = new ObservableCollection<Tour>();
            TargetTour = new Tour();
            AddNewTourCommand = new AddNewTourCMD(this);
            DeleteTourCommand = new DeleteTourCMD(this);
            ChangeSelectedTourCommand = new ChangeSelectedTourCMD(this);
            AddTourLogCommand = new AddTourLogCMD(this);
            TourLogViewModel = new TourLogVM();

            ClearTours();
           /* SelectedTour = new Tour(name: "Tour 1", time: 1, tour_desc: "Description 1", from: "From 1", to: "To 1", transport_type: "Bus", distance: 123, image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg", route_information: null);
            SelectedTour.TourLogs.Add(new TourLog(comment: "sehr schön", "easy", 5, new DateTime(), 10));*/
            // Add some sample tours
            Tours.Add(new Tour(
            name: "Tour 1",
            time: 1,
            tour_desc: "Description 1",
            from: "From 1",
            to: "To 1",
            transport_type: "Bus",
            distance: 123,
            image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg",
            route_information: null,
             tourLogs: new ObservableCollection<TourLog>(new List<TourLog>()
            {
                new TourLog(comment: "sehr schön", difficulty: "easy", rating: 5, dateTime: DateTime.Now, totalTime: 10)
            })
            ));
            Tours.Add(new Tour(name: "Tour 2", time: 2, tour_desc: "Description 2", from: "From 2", to: "To 2", transport_type: "Car", distance: 456, image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg", route_information: null));
        }
    }
}
