using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class ItemSlotData
{
    public ItemData itemData;
    public int quantity;

    // 需要花费的金额
    public int cost;


    public ItemSlotData(ItemData _itemData, int _quantity)
    {
        itemData = _itemData;
        quantity = _quantity;
        cost = 0;
    }

    public ItemSlotData(ItemData _itemData, int _quantity, int _cost):this(_itemData,_quantity)
    {
        cost = _cost;
    }

    public ItemSlotData(ItemData _itemData)
    {
        this.itemData = _itemData;
        quantity = 1;
        cost = 0;
    }

    public void AddQuantity(int amountToAdd)
    {
        quantity += amountToAdd;
    }
    public void Remove()
    {
        quantity--;
    }

    public void ValidateQuantity()
    {
        if (quantity <= 0 || itemData == null)
        {
            Empty();
        }
    }
    public void Empty()
    {
        itemData = null;
        quantity = 0;
    }
  
    public ItemSlotData CloneNew()
    {
        ItemData newItemData = new ItemData();

        newItemData.desc = itemData.desc;
        newItemData.thumbnail = itemData.thumbnail;
        newItemData.gameModel = itemData.gameModel;

        ItemSlotData newData = new ItemSlotData(newItemData,quantity);
        return newData;
    }
}
