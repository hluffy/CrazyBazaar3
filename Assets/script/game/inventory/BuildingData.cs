using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Building")]
public class BuildingData : ItemData
{
    public enum BuildingType
    {
        House, Farm
    }
    public BuildingType buildingType;

    public List<ItemData> composites;
}
