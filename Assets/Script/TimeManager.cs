using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; set; }

    public UnityEvent OnDayPass = new UnityEvent();

    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter,
    }

    public Season currentSeaon = Season.Spring;

    private int daysPerSeaon = 30;
    private int daysInCurrentSeason = 1;

    public enum Weeks
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public Weeks currentWeek = Weeks.Monday ;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this; 
        }
    }

    public int dayInGame = 1;
    public int yearInGame = 1;

    public void TriggerNextDay()
    {
        dayInGame += 1;
        daysInCurrentSeason += 1;
        currentWeek = (Weeks)(((int)currentWeek + 1) % 7);
        
        if (daysInCurrentSeason > daysPerSeaon)
        {
            daysInCurrentSeason = 1;
            currentSeaon = GetNextSeason();
        }

        OnDayPass.Invoke();
    }

    private Season GetNextSeason()
    {
        int currentSeasonIndex = (int)currentSeaon;
        int NextSeasonIndex = (currentSeasonIndex + 1) % 4;

        if (NextSeasonIndex == 0)
        {
            yearInGame += 1;
        }

        return (Season)NextSeasonIndex;
    }
}
