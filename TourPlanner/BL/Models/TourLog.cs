using Haley.Models;
using System;

public class TourLog : ChangeNotifier
{
    public TourLog(string comment, string difficulty, int rating, DateTime dateTime, int totalTime) 
    { 
        Comment = comment;
        Difficulty = difficulty;
        Rating = rating;
        DateTime = dateTime;
        TotalTime = totalTime;
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