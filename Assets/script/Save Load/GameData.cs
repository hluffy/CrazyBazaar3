using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // 玩家信息
    public int currency;
    public Vector3 playerPosition;

    //库存信息
    public SerializableDictionary<string, int> inventory;
    public SerializableDictionary<string, int> equipment;

    // 摊贩信息
    public SerializableDictionary<string, SerializableDictionary<string,string>> vendor;



    public GameData()
    {
        currency = 0;
        playerPosition = Vector3.zero;

        inventory = new SerializableDictionary<string, int>();
        equipment = new SerializableDictionary<string, int>();
        vendor = new SerializableDictionary<string, SerializableDictionary<string,string>>();
    }
}
