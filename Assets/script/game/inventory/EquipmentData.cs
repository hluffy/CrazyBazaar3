using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


[CreateAssetMenu(menuName = "Items/Equipment")]
public class EquipmentData : ItemData
{
    [Serializable]
    public enum ToolType
    {
        Hoe, WateringCan, Axe, Pickaxe, Shove,Sword, Any
    }
    public ToolType toolType;

    public int durable; // 耐久
    public int strength;
    public int consume;

    [Header("Weapon info")]
    public Weapon weapon;

    public Vector3 initPosition;
    public Quaternion initRotation;

    public int damage;

    public ItemData waste;
    public void Use()
    {

        if (durable > 0)
        {
            durable--;
            if (durable == 0)
            {
                /* InventoryManager.Instance.RemoveInventory(this);
                 InventoryManager.Instance.AddInventory(waste);*/
            }
        }
    }

    public override CharacterStats AddModifiers()
    {
        CharacterStats playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();

        playerStats.damage.AddModifier(damage);

        return playerStats;
    }

    public override CharacterStats RemoveModifiers()
    {
        CharacterStats playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();

        playerStats.damage.RemoveModifier(damage);

        return playerStats;
    }
}
