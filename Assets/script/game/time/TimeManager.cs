
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    [Header("Internel Clock")]
    public GameTimestamp timestamp;
    public float timeScale = 1.0f;

    [Header("Day and Night Cycle")]
    public Transform sunTransform;


    [Header("time UI modify")]
    public Text seasonUI;
    public Text timeUI;
    List<ITimeTrack> listeners = new List<ITimeTrack>();
    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        timestamp = new GameTimestamp(0, GameTimestamp.Season.Spring, timestamp.day, timestamp.hour, timestamp.minite);
        StartCoroutine(TimeUpdate());
       
    }

    public GameTimestamp GetGameTimestamp()
    {
        // 返回一个clone对象
        return new GameTimestamp(timestamp);
    }
    IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(1 / timeScale);
        
        }
    }
    public void Tick()
    {
        timestamp.UpdateClock();

        // 每次tick 都更新作物
        foreach(ITimeTrack listener in listeners)
        {
            listener.ClockUpdate(timestamp);
        }
        UpdateUi(timestamp);
        UpdateSunMove();
    }
    void UpdateSunMove() {
        // 日光根据时间移动
        int timeInMinutes = GameTimestamp.HoursToMinuts(timestamp.hour) + timestamp.minite;
        float sunAngle = .25f * timeInMinutes - 90;

        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);

    }

    // 处理listenrder
    public void RegisterTracker(ITimeTrack listener)
    {
        listeners.Add(listener);
    }
    public void UnRegisterTracker(ITimeTrack listener)
    {
     
        listeners.Remove(listener);
    }


    void UpdateUi(GameTimestamp timestamp) {

        int hour = timestamp.hour;
        int minutes = timestamp.minite;
        string prefix = "AM";

        if (hour > 12) {
            prefix = "PM";
            hour = hour- 12;
        }
        timeUI.text = prefix + hour + ":" + minutes.ToString("00");

        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfWeek = timestamp.GetDayOfTheWeek().ToString();

        seasonUI.text = season + " " + day + " (" + dayOfWeek + ")";
        
    }
}