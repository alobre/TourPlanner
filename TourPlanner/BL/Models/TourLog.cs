using Haley.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TourLog : ChangeNotifier
{
    public TourLog(int tour_id, string comment, string difficulty, int rating, DateTime dateTime, int totalTime) 
    {
        Tour_id = tour_id;
        TourLog_id = mod((tour_id + DateTime.Now.ToString("MM/dd/yyyy h:mm tt").GetHashCode()), 9999);
        Comment = comment;
        Difficulty = difficulty;
        Rating = rating;
        DateTime = dateTime;
        TotalTime = totalTime;
    }
    int mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
    private int _tour_id;
    public int Tour_id
    {
        get { return _tour_id; }
        set { _tour_id = value; OnPropertyChanged(nameof(Tour_id)); }
    }
    
    private int _tourLog_id;
    [Key]
    public int TourLog_id
    {
        get { return _tourLog_id; }
        set { _tourLog_id = value; OnPropertyChanged(nameof(TourLog_id)); }
    }
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

    private int _totalTime;
    public int TotalTime
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