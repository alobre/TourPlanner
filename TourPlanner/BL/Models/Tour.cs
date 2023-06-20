using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Haley.Models;
using Haley.Abstractions;
using Haley.MVVM;
using Newtonsoft.Json;
using System.Net;
using System.Windows.Media.Imaging;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace TourPlanner.BL.Models
{
    public class Tour : ChangeNotifier
    {
        public Tour(string name, string tour_desc, string from, string to, string transport_type, float distance, int time, BitmapImage route_information, string image_link, ObservableCollection<TourLog> tourLogs = null)
        {
            Name = name;
            Description = tour_desc;
            From = from;
            To = to;
            TransportType = transport_type;
            Distance = distance;
            Time = time;
            RouteDetails = route_information;
            RouteImage = image_link;
            TourLogs = tourLogs != null ? new ObservableCollection<TourLog>(tourLogs) : new ObservableCollection<TourLog>();
        }
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }
        private string _from;
        public string From
        {
            get { return _from; }
            set { _from = value; OnPropertyChanged(nameof(From)); }
        }
        private string _to;
        public string To
        {
            get { return _to; }
            set { _to = value; OnPropertyChanged(nameof(To)); }
        }
        private string _transportType;
        public string TransportType
        {
            get { return _transportType; }
            set { _transportType = value; OnPropertyChanged(nameof(TransportType)); }
        }

        private double _distance;
        public double Distance
        {
            get { return _distance; }
            set { _distance = value; OnPropertyChanged(nameof(Distance)); }
        }

        private int _time;
        public int Time
        {
            get { return _time; }
            set { _time = value; OnPropertyChanged(nameof(Time)); }
        }

        private string _routeImage;
        public string RouteImage
        {
            get { return _routeImage; }
            set { _routeImage = value; OnPropertyChanged(nameof(RouteImage)); }
        }
        [JsonIgnore]
        private BitmapImage _routeDetails;
        public BitmapImage RouteDetails {
            get { return _routeDetails; }
            set { _routeDetails = value; OnPropertyChanged(nameof(_routeDetails)); }
        }

        private List<TourLog> _logs;
        public List<TourLog> Logs
        {
            get { return _logs; }
            set { _logs = value; OnPropertyChanged(nameof(Logs)); }
        }

        public Tour()
        {
            _logs = new List<TourLog>();
        }
        private ObservableCollection<TourLog> _tourLogs;
        public ObservableCollection<TourLog> TourLogs
        {
            get { return _tourLogs; }
            set { _tourLogs = value; OnPropertyChanged(nameof(TourLogs)); }
        }
    }


    public class TourManager
    {
        private List<Tour> _tours;

        public TourManager()
        {
            _tours = new List<Tour>();
        }

        public void CreateTour(Tour tour)
        {
            _tours.Add(tour);
        }

        public void UpdateTour(string tourName, Tour updatedTour)
        {
            var tour = GetTourByName(tourName);
            if (tour != null)
            {
                tour.Name = updatedTour.Name;

                tour.Description = updatedTour.Description;
                tour.TransportType = updatedTour.TransportType;
                tour.Distance = updatedTour.Distance;
                tour.Time = updatedTour.Time;
                tour.RouteImage = updatedTour.RouteImage;
            }
        }

        public void DeleteTour(string tourName)
        {
            var tour = GetTourByName(tourName);
            if (tour != null)
            {
                _tours.Remove(tour);
            }
        }

        public Tour GetTourByName(string tourName)
        {
            return _tours.FirstOrDefault(t => t.Name.Equals(tourName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Tour> GetTourDataFromAPIAsync(string start, string destination)
        {
            // Make REST request to MapQuest API to retrieve tour data
            // Implement your own logic to call the MapQuest Directions and Static Map APIs
            // and parse the response to populate the tour object

            // Example code using HttpClient and JSON deserialization
            using (var client = new HttpClient())
            {
                // Make a request to MapQuest API
                HttpResponseMessage response = await client.GetAsync("https://api.example.com/..."); // Replace with the actual API endpoint

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Deserialize the response JSON into a Tour object
                    Tour tour = JsonConvert.DeserializeObject<Tour>(responseBody);

                    return tour;
                }
                else
                {
                    // Handle error case
                    throw new Exception("Failed to retrieve tour data from the API.");
                }
            }
        }
    }
}
