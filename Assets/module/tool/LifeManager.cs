using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLife
{
    public int id;
    public int age;
    public string name;
    public int type;
    public System.Action action;
    //public float time;
    public float delay_time;

    public float timer = 0;
}
// 生命管理器
public class LifeManager : MonoBehaviour
{
    #region 静态字段
    #endregion


    #region 静态字段
    // npc 的人生
    private void createNPC()
    {

    }
    // 获取今日的npc事件，
    private void createNPCEvent()
    {
        // 农事，战事，
    }
    // 获取今日的npc行为，
    private void createNPCDaily()
    {
        //  买菜：目的地：早市，金币，菜：随机，触发回家：
        //  种地：目的地：家中菜园，下个事件触发：劳累，
        //  卖菜：目的地：早市，携带：菜

        //  菜：买，自己种，野外采，偷盗

    }
    #region 静态方法

    // player的人生，10，20,60
    private void createPlayer()
    {

    }
    #endregion
    // 动物的人生
    private void createAnimal()
    {

    }

    // 植物的人生
    private void createPlant()
    {

    }
    #endregion
}