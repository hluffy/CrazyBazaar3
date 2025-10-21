using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTask
{
    public string key;
    public System.Action action;
    //public float time;
    public float delay_time;

    public float timer = 0;
}

// Fix ����

public class TimerUtils
{

 


    #region ��̬�ֶ�
    private static Dictionary<string, TimerTask> tasks = new Dictionary<string, TimerTask>();
    private static List<TimerTask> removed_task = new List<TimerTask>();
    private static List<string> need_remove_keys = new List<string>();
    private static Dictionary<string, TimerTask> need_add_tasks = new Dictionary<string, TimerTask>();
    #endregion

    #region ��̬����

    /// <summary>
    /// �ӳٵ���
    /// </summary>
    /// <param name="action">��ʱ������Ҫִ�еķ���</param>
    /// <param name="delay">�ӳٵ�ʱ��</param>
    public static void DelayInvoke(System.Action action, float delay = 0.01f)
    {
        DelayInvoke(System.Guid.NewGuid().ToString(), action, delay);
    }

    /// <summary>
    /// �ӳٵ���
    /// </summary>
    /// <param name="key">Ψһ��ʶ</param>
    /// <param name="action">��ʱ������Ҫִ�еķ���</param>
    /// <param name="delay">�ӳٵ�ʱ��</param>
    public static void DelayInvoke(string key, System.Action action, float delay = 0.01f)
    {
       if (tasks.ContainsKey(key))
        {
            // �����ʱ�����Ѿ����� ��ȡ������
            CancelInvoke(key);
        }
        if (need_add_tasks.ContainsKey(key))
        {
            // �����ʱ�����Ѿ����� ��ȡ������
            need_add_tasks.Remove(key);
        }

        TimerTask task = new TimerTask();
        task.key = key;
        task.action = action;
        //task.time = Time.time;
        task.delay_time = delay;
        //tasks.Add(key, task);

        need_add_tasks.Add(key, task);
    }

    /// <summary>
    /// ȡ���ӳٵ���
    /// </summary>
    /// <param name="key">Ψһ��ʶ</param>
    public static void CancelInvoke(string key)
    {
        if(tasks.ContainsKey(key))  
             tasks.Remove(key);
        if (need_add_tasks.ContainsKey(key))
            need_add_tasks.Remove(key);
        need_remove_keys.Add(key);
    }

    public static void Update()
    {
        // ��������
        foreach (var item in need_add_tasks.Keys)
        {
            if (tasks.ContainsKey(item))
                CancelInvoke(item); // �����ʱ�����Ѿ����� ��ȡ������
            tasks.Add(item, need_add_tasks[item]);
        }

        need_add_tasks.Clear();


        if (tasks.Count == 0) return;

        if (removed_task.Count != 0)
            removed_task.Clear();
        foreach (var item in tasks.Values)
        {
            item.timer += Time.unscaledDeltaTime;
            if (item.timer >= item.delay_time)
            {
                try
                {
                    // �����ص�
                    item.action?.Invoke();
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }
                finally
                {
                    // �Ƴ��ص�
                    removed_task.Add(item);
                }
                item.timer = 0;
            }
        }

        if (removed_task.Count != 0)
        {
            foreach (var item in removed_task)
            {
                if (tasks.ContainsKey(item.key))
                    tasks.Remove(item.key);
            }
            removed_task.Clear();
        }

        foreach (var item in need_remove_keys)
        {
            if (tasks.ContainsKey(item))
                tasks.Remove(item);
        }

        need_remove_keys.Clear();
    }

    /// <summary>
    /// �ȴ�Э��ִ�н�����ִ��
    /// </summary>
    /// <param name="coroutine">Э�̶���</param>
    /// <param name="onFinsh">�ص�</param>
    public static void WaitForCoroutine(Coroutine coroutine, System.Action onFinsh)
    {
        //CoroutineStarter.Start(WaitForCoroutineExcute(coroutine, onFinsh));
    }

    private static IEnumerator WaitForCoroutineExcute(Coroutine coroutine, System.Action onFinsh)
    {
        yield return coroutine;
        onFinsh?.Invoke();
    }

    #endregion
}