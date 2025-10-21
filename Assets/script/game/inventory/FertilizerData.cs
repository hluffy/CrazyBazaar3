using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Fertilizer")]
public class FertilizerData : ItemData
{
    // 腐烂物施加在植物上，可以加速生长
    [Header("Fertilizer")]
    public int rotTime; // 腐烂时间

    public int grow;
    public int maxStackingCount; // 最大堆叠数
}
