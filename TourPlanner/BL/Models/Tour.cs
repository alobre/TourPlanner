using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Haley.Models;
using Newtonsoft.Json;

public class Tour : ChangeNotifier
{
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

    private TimeSpan _estimatedTime;
    public TimeSpan EstimatedTime
    {
        get { return _estimatedTime; }
        set { _estimatedTime = value; OnPropertyChanged(nameof(EstimatedTime)); }
    }

    private string _routeInformationImageUrl;
    public string RouteInformationImageUrl
    {
        get { return _routeInformationImageUrl; }
        set { _routeInformationImageUrl = value; OnPropertyChanged(nameof(RouteInformationImageUrl)); }
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
}

public class TourLog : ChangeNotifier
{
    private DateTime _dateTime;
    public DateTime DateTime
    {
        get { return _dateTime; }
        set { _dateTime = value; OnPropertyChanged(nameof(DateTime)); }
    }

    private string _comment;
    public string Comment
    {
        get { return _comment; }
        set { _comment = value; OnPropertyChanged(nameof(Comment)); }
    }

    private string _difficulty;
    public string Difficulty
    {
        get { return _difficulty; }
        set { _difficulty = value; OnPropertyChanged(nameof(Difficulty)); }
    }

    private TimeSpan _totalTime;
    public TimeSpan TotalTime
    {
        get { return _totalTime; }
        set { _totalTime = value; OnPropertyChanged(nameof(TotalTime)); }
    }

    private int _rating;
    public int Rating
    {
        get { return _rating; }
        set { _rating = value; OnPropertyChanged(nameof(Rating)); }
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
            tour.EstimatedTime = updatedTour.EstimatedTime;
            tour.RouteInformationImageUrl = updatedTour.RouteInformationImageUrl;
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
