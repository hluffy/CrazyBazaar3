using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Land")]
public class LandData : ItemData
{
    
    public TypedTile.TileType tileType;

    public int maxStackingCount; // 最大堆叠数

    public TypedTile tile;
}
