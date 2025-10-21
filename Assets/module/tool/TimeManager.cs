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
//    //��һ�����ڱ�ʾ����ͺ�ҹѭ����Transform��ֵ��sunTransform������������һ��ITimeTracker�ӿ����͵��б�listeners�����ڱ�������ע���ʱ��׷������
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
//        //����ʱ���
//        timestamp.UpdateClock();
//        //֪ͨ����ע���ʱ��׷��������ʱ��
//        foreach (ITimeTracker listener in listeners)
//        {
//            listener.ClockUpdate(timestamp);
//        }
//        //����UpdateSunMovement��������̫����λ��
//        UpdateSunMovement();
//    }

//    void UpdateSunMovement()
//    {
//        //��ǰʱ��
//        int timeInMinutes = GameTimestamp.HoursToMinutes(timestamp.hour) + timestamp.minute;
//        //����̫���ĽǶ�
//        float sunAngle = .25f * timeInMinutes - 90;
//        //��̫���Ƕ�Ӧ�õ�sunTransform��ŷ�����ϣ�ʵ��̫�����˶�
//        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
//    }

//    //��ȡ�µ�ʱ���
//    public GameTimestamp GetGameTimestamp()
//    {
//        //���ص�ǰ��ʱ���
//        return new GameTimestamp(timestamp);
//    }
//    //��һ��ʱ��׷����ע�ᵽ�б���
//    public void RegisterTracker(ITimeTracker listener)
//    {
//        listeners.Add(listener);
//    }
//    //��һ��ʱ��׷�������б����Ƴ�
//    public void UnregisterTracker(ITimeTracker listener)
//    {
//        listeners.Remove(listener);
//    } 
//}
