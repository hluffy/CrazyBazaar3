using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
 
using UnityEngine.UI;
using static UnityEditor.Progress;

public class BagManager : MonoBehaviour, ITimeTrack
{
    public enum BagType
    {
        Equipmment, Seed, Food, Land
    }


    [Header("Bag Child")]
    public Transform bag;
    public Transform top;

    public Transform inventoryAllSlot;


    [Header("Status Bar")]
    public InventorySlot toolEquipSlot;
    public InventorySlot itemEquipSlot;


    [Header("Inventory System")]
    public BagType bagType;



    public static BagManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        OnTitleClick(0);

        EquippedTool(new ItemSlotData(InventoryManager.Instance.equippedTool, 1));

        TimeManager.Instance.RegisterTracker(this);
    }
 
    void InitSlotData(List<ItemSlotData> slotsData, Transform slotsParent)
    {
        if (slotsData == null  || slotsData.Count == 0) return;

        for (int i = 0; i < slotsParent.childCount; i++)
        {
            if (i >= slotsData.Count)
            {
                return;
            }
            ItemSlotData data = slotsData[i];

            InventorySlot inventorySlot = slotsParent.GetChild(i).transform.GetComponent<InventorySlot>();

            if (data != null && data.itemData != null)
            {
                inventorySlot.Display(data);
            }
 
        }

    }
    public void ClearSlots(Transform slotsParent)
    {
        for (int i = 0; i < slotsParent.childCount; i++)
        {
            InventorySlot inventorySlot = slotsParent.GetChild(i).transform.GetComponent<InventorySlot>();
            inventorySlot.ClearSlot();
        }
    }
    public void ClearSlots()
    {
        ClearSlots(inventoryAllSlot);
     
    }
    
    public void RefreshSlotData()
    {
        // switch (bagType)
        // {
        //     case BagType.Equipmment:
        //         InitSlotData(InventoryManager.Instance.tools, inventoryAllSlot);
        //         break;
        //     case BagType.Seed:
        //         InitSlotData(InventoryManager.Instance.seeds, inventoryAllSlot);
        //         break;
        //     case BagType.Food:
        //         InitSlotData(InventoryManager.Instance.foods, inventoryAllSlot);
        //         break;
        //     case BagType.Land:
        //         break;
        // }
    }
    
    public void EquippedTool(ItemSlotData itemSlotData)
    {

        if (itemSlotData == null || itemSlotData.itemData == null) return;
       
        InventoryManager.Instance.equippedTool = itemSlotData.itemData;
        toolEquipSlot.Display(itemSlotData);
 
    }
      
    public void OnBagClick()
    {
        bag.gameObject.SetActive(!bag.gameObject.activeSelf);

    }
  
   
    public void OnTitleClick(int type)
    {
        // if (InventoryManager.Instance.IsMouseEquip())
        // {
        //     return;
        // }
     
        // if (top == null) return;
        // for (int i = 0; i < top.childCount; i++)
        // {
        //     Transform child = top.GetChild(i);
           
        //     if (type == i)
        //     {
        //         child.gameObject.GetComponent<Text>().fontSize = 16;
        //         child.gameObject.GetComponent<Text>().color = Color.red;

        //     }
        //     else
        //     {
        //         child.gameObject.GetComponent<Text>().fontSize = 14;
        //         child.gameObject.GetComponent<Text>().color = Color.black;

        //     }
        // }

        // ClearSlots(inventoryAllSlot);
        // switch (type)
        // {
        //     case 0:
              
        //         InitSlotData(InventoryManager.Instance.tools, inventoryAllSlot);
        //         bagType = BagType.Equipmment;

        //         break;
        //     case 1:
                
        //         InitSlotData(InventoryManager.Instance.seeds, inventoryAllSlot);
        //         bagType = BagType.Seed;
        //         break;
        //     case 2:
                
        //         InitSlotData(InventoryManager.Instance.foods, inventoryAllSlot);
        //         bagType = BagType.Food;
        //         break;
        //     case 3:
                
        //         InitSlotData(InventoryManager.Instance.foods, inventoryAllSlot);
        //         bagType = BagType.Food;
        //         break;
        // }
    }




    public void ClockUpdate(GameTimestamp timestamp)
    {
        
    }

    public BagType GetBagType(ItemSlotData item)
    {
        if(item==null || item.itemData == null)
        {
            return BagType.Equipmment;
        }
        if (item.itemData as EquipmentData != null) return BagType.Equipmment;
        if (item.itemData as SeedData != null) return BagType.Seed;
        return BagType.Food;


    }

    public bool isCurrentBag(ItemSlotData itemSlotData)
    {
        if (itemSlotData == null || itemSlotData.itemData == null) return false;
        if (itemSlotData.itemData as EquipmentData != null && bagType == BagType.Equipmment) return true;
        if (itemSlotData.itemData as SeedData != null && bagType == BagType.Seed) return true;
        if (itemSlotData.itemData as ItemData != null && bagType == BagType.Food) return true;
        return false;
    }
}
