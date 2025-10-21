using System.Collections;
using UnityEngine;

[System.Serializable]
public class GameTimestamp
{

    public int year;
    public enum Season
    {
        Spring, Summer, Fall, Winter
    }

    public Season season;

    public enum DayOfWeek
    {
        Sunday,
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday
    }
    public int day;
    public int hour;
    public int minite;



    public GameTimestamp(int year, Season season, int day, int hour, int minite)
    {
        this.year = year;
        this.season = season;
        this.day = day;
        this.hour = hour;
        this.minite = minite;
    }
    public GameTimestamp(GameTimestamp times)
    {
        this.year = times.year;
        this.season = times.season;
        this.day = times.day;
        this.hour = times.hour;
        this.minite = times.minite;
    }
    public void UpdateClock()
    {
        minite++;
        if (minite >= 60)
        {
            minite = 0;
            hour++;
        }
        if (hour >= 24)
        {
            hour = 0;
            day++;
        }
        if (day > 30)
        {
            hour = day;
            if (season == Season.Winter)
            {
                season = Season.Spring;
                year++;
            }
            else
            {
                season++;
            }
        }
    }

    public DayOfWeek GetDayOfTheWeek()
    {
        int dayPass = YearsToDays(year) + SeasonsToDays(season) + day;
        int dayIndex = dayPass % 7;
        return (DayOfWeek)dayIndex;
    }
    public static int HoursToMinuts(int hour)
    {
        return hour * 60;
    }
    public static int DaysToHours(int days)
    {
        return days * 24;
    }

    public static int SeasonsToDays(Season season)
    {
        int seasonIndex = (int)season;
        return seasonIndex * 30;
    }
    public static int YearsToDays(int year)
    {
        return year * 4 * 30;
    }

    public static int CompareTimestamp(GameTimestamp timestamp1, GameTimestamp timestamp2)
    {
        int timestamp1Hours = DaysToHours(YearsToDays(timestamp1.year)) + DaysToHours(SeasonsToDays(timestamp1.season))
            + DaysToHours(timestamp1.day) + timestamp1.hour;
        int timestamp2Hours = DaysToHours(YearsToDays(timestamp2.year)) + DaysToHours(SeasonsToDays(timestamp2.season))
            + DaysToHours(timestamp2.day) + timestamp2.hour;

        return Mathf.Abs(timestamp2Hours - timestamp1Hours);

    }
}