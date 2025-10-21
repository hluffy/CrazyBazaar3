using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Food")]
public class FoodData : ItemData
{
    public enum FoodType
    {
        // Cook 为合成蔬菜，比如红烧肉
        Vegetable, Fruit, Fish, Cook
    }

     [Header("Food")]
    public FoodType foodType; // 食物类型

    public int hunger;   // 增加饥饿
    public int blood;    // 加血

    public ItemData rot; // 腐烂
    public int maxStackingCount; // 最大堆叠数

    [Header("health")]
    public int health;
    public int maxHealth;


      public override CharacterStats AddModifiers()
    {
        CharacterStats playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();

        playerStats.maxHealth.AddModifier(maxHealth);
        playerStats.IncreaseHealthBy(health);

        return playerStats;
    }

    public override CharacterStats RemoveModifiers()
    {
        CharacterStats playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();

        playerStats.maxHealth.RemoveModifier(maxHealth);
        playerStats.IncreaseHealthBy(0);

        return playerStats;
    }
}
