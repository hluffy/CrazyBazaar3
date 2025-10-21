using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public enum ItemType
{
    Tool,
    Seed,
    Food,
    Land,
    Coin,
    Wood
}

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public string desc;
    public Sprite thumbnail;

    public GameObject gameModel;

    //最大堆叠数量
    public int maxStack = 1;

    public string itemId;

  
    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        itemId = AssetDatabase.AssetPathToGUID(path);
#endif
    }

    public virtual CharacterStats AddModifiers()
    {
        // CharacterStats playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();

        // playerStats.maxHealth.AddModifier(maxHealth);
        // playerStats.IncreaseHealthBy(health);

        return null;
    }

    public virtual CharacterStats RemoveModifiers()
    {
        // CharacterStats playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();

        // playerStats.maxHealth.RemoveModifier(maxHealth);
        // playerStats.IncreaseHealthBy(0);

        return null;
    }
}
