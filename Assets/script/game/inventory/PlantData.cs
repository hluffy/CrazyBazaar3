using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Plant")]
public class PlantData : ItemData
{

    [Header("Sprite:Growth process")]
    public Sprite seed; // 发芽期 默认是刚种下带土的种子
    public Sprite seedling; // 幼苗期，长大一点的植株
    public Sprite harvestable; // 收获的植株
    public Sprite wilted; // 枯萎

    [Header("Plant")]
    public int health;
    public int maxHealth;
    public int dayToGrow; // 种子到收获的时间


    public bool regrowable; // 采摘后重新生长

    public int dayToRegrow;
    
    public int harvestCount; // 结果数量
                             // public GameObject wilt; // ��ή
    
    public ItemData harvest;
}
