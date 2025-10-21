//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public interface ITimeTracker
//{
//    void ClockUpdate(GameTimestamp timestamp);
//}

//public GameTimestamp(DateTime dateTime)
 
//public class TimeManager : MonoBehaviour
//{
//    public static TimeManager Instance { get; private set; }
//    private void Awake()
//    {
//        if (Instance != null && Instance != this) { Destroy(this); }
//        else { Instance = this; }
//    }

//    [SerializeField]
//    GameTimestamp timestamp;
//    public float timeScale = 1.0f;
//    //将一个用于表示白天和黑夜循环的Transform赋值给sunTransform变量，并声明一个ITimeTracker接口类型的列表listeners，用于保存所有注册的时间追踪器。
//    [Header("Day and Night cycle")]
//    public Transform sunTransform;
//    List<ITimeTracker> listeners = new List<ITimeTracker>();

//    void Start()
//    {
//        timestamp = new GameTimestamp(0, GameTimestamp.Season.Spring, 1, 6, 0);
//        StartCoroutine(TimeUpdate());
//    }
//    IEnumerator TimeUpdate()
//    {
//        while (true)
//        {
//            Tick();
//            yield return new WaitForSeconds(1 / timeScale);
//        }
//    }

//    public void Tick()
//    {
//        //更新时间戳
//        timestamp.UpdateClock();
//        //通知所有注册的时间追踪器更新时间
//        foreach (ITimeTracker listener in listeners)
//        {
//            listener.ClockUpdate(timestamp);
//        }
//        //调用UpdateSunMovement方法更新太阳的位置
//        UpdateSunMovement();
//    }

//    void UpdateSunMovement()
//    {
//        //当前时间
//        int timeInMinutes = GameTimestamp.HoursToMinutes(timestamp.hour) + timestamp.minute;
//        //计算太阳的角度
//        float sunAngle = .25f * timeInMinutes - 90;
//        //将太阳角度应用到sunTransform的欧拉角上，实现太阳的运动
//        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
//    }

//    //获取新的时间戳
//    public GameTimestamp GetGameTimestamp()
//    {
//        //返回当前的时间戳
//        return new GameTimestamp(timestamp);
//    }
//    //将一个时间追踪器注册到列表中
//    public void RegisterTracker(ITimeTracker listener)
//    {
//        listeners.Add(listener);
//    }
//    //将一个时间追踪器从列表中移除
//    public void UnregisterTracker(ITimeTracker listener)
//    {
//        listeners.Remove(listener);
//    } 
//}
