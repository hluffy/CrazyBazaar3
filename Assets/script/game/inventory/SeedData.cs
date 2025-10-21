using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Items/Seed")]
public class SeedData : ItemData
{
   
     [Header("Seed")]
    public int maxStackingCount; // 最大堆数量

    public PlantData cropToYield;
    public ItemData rot; // 腐烂
    
    public void Print()
    {
        // Debug.Log("dayToGrow:" + dayToGrow + ",regrowable:" + regrowable + ",dayToRegrow:" + dayToRegrow + ",maxStackingCount" + maxStackingCount);
    }

}
