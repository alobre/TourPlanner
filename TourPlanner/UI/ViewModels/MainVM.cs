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
using TourPlanner.DL.DB;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TourPlanner
{
    public class MainVM : ChangeNotifier
    {
        private Tour _targetTour;
        private ObservableCollection<Tour> _tours;
        private Tour _selectedTour;
        private bool _canAdd = true;
        private DialogService _dialogService = new DialogService(null);

        private TourPlannerDbContext _context = new TourPlannerDbContext();

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

                var Distance = route.route.route.distance;
                var Time = route.route.route.time;
                var Description = $"From {result.start.Address} to {result.dest.Address}";
                var TransportType = route.route.route.options.routeType;
                var From = $"{result.start.Address}";
                var To = result.dest.Address;
                var RouteImage = route.URL;
                var Name = $"{result.start.Address} TO {result.dest.Address}";
                var TourLogs = new ObservableCollection<TourLog>(new List<TourLog>());
                
                Tour tour = new Tour(name: Name, tour_desc: Description, from: From, to: To, transport_type: TransportType, image_link: RouteImage, time: Time, distance: Distance, tourLogs: TourLogs);
                
                Log.LogInfo("Neue Tour erstellt Name: " + tour.Name);

                // Add the new tour to the collection
                if(tour != null)
                {
                    Tours.Add(tour);
                    AddTourToDatabase(tour);
                    _context.Tours.Load();
                }
                
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
        public async Task UpdateTours()
        {
            using (var context = new TourPlannerDbContext())
            {
                Tours = new ObservableCollection<Tour>(context.Tours.ToList());
            }
        }
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

            DeleteTourFromDataSource(tour);
            Tours.Remove(tour);
        }

        private void DeleteTourFromDataSource(Tour tour)
        {
            using (var context = new TourPlannerDbContext())
            {
                var tourToDelete = context.Tours.FirstOrDefault(t => t.Tour_id == tour.Tour_id);
                if (tourToDelete != null)
                {
                    context.Tours.Remove(tourToDelete);
                    context.SaveChanges();
                }
            }
        }

        public async Task SetSelectedTour(Tour tour)
        {
            try
            {
                SelectedTour = tour;
                using (var context = new TourPlannerDbContext())
                {
                    SelectedTour.TourLogs = new ObservableCollection<TourLog>(context.TourLogs.Where(log => log.Tour_id == SelectedTour.Tour_id).ToList());
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
        public async Task AddTourLog()
        {
            try
            {
                foreach (Tour tour in Tours)
                {
                    if(tour.Equals(SelectedTour))
                    {
                            TourLog tourLog = new TourLog(tour_id: SelectedTour.Tour_id, comment: TourLogViewModel.Comment, difficulty: TourLogViewModel.Difficulty, rating: TourLogViewModel.Rating + 1, dateTime: TourLogViewModel.Date, totalTime: int.Parse(TourLogViewModel.Time));
                            tour.TourLogs.Add(tourLog);
                            SelectedTour = tour;

                            AddTourLogToDatabase(tourLog);
                            
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
        private void AddTourToDatabase(Tour tour)
        {
            try
            {
                using (var context = new TourPlannerDbContext())
                {
                    context.Tours.Add(tour);
                    context.SaveChanges();
                    Tours = new ObservableCollection<Tour>(context.Tours.ToList());
                }
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
        private void AddTourLogToDatabase(TourLog tourLog)
        {
            // Convert the DateTime value to UTC if it's not already in UTC
            if (tourLog.DateTime.Kind != DateTimeKind.Utc)
            {
                tourLog.DateTime = tourLog.DateTime.ToUniversalTime();
            }

            using (var context = new TourPlannerDbContext())
            {
                context.TourLogs.Add(tourLog);
                context.SaveChanges();
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
            _context = new TourPlannerDbContext();

            ClearTours();
            /* SelectedTour = new Tour(name: "Tour 1", time: 1, tour_desc: "Description 1", from: "From 1", to: "To 1", transport_type: "Bus", distance: 123, image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg", route_information: null);
             SelectedTour.TourLogs.Add(new TourLog(comment: "sehr schön", "easy", 5, new DateTime(), 10));*/
            // Add some sample tours
            UpdateTours();
            Tours.Add(new Tour(
            name: "Tour 1",
            time: 1,
            tour_desc: "Description 1",
            from: "From 1",
            to: "To 1",
            transport_type: "Bus",
            distance: 123,
            image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg",
             tourLogs: new ObservableCollection<TourLog>(new List<TourLog>()
            {
                new TourLog(tour_id: 1234, comment: "sehr schön", difficulty: "easy", rating: 5, dateTime: DateTime.Now, totalTime: 10)
            })
            ));
            Tours.Add(new Tour(name: "Tour 2", time: 2, tour_desc: "Description 2", from: "From 2", to: "To 2", transport_type: "Car", distance: 456, image_link: "https://www.odtap.com/wp-content/uploads/2019/04/Route-optimization-software-odtap.jpg"));
        }
    }
}
