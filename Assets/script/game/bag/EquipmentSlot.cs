using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : InventorySlot
{
    public EquipmentData.ToolType toolType;

    void OnValidate()
    {
        gameObject.name = "Equipment slot - " + toolType.ToString();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        canPut = true;

        ItemSlotData selectItemSlotData = InventoryManager.Instance.selectItemSlotData;
        if (selectItemSlotData != null && selectItemSlotData.itemData != null)
        {
            ItemData itemData = selectItemSlotData.itemData;

            // 如果物品类型是武器
            if (toolType != EquipmentData.ToolType.Any)
            {
                EquipmentData equipmentData = itemData as EquipmentData;
                if (equipmentData.toolType != toolType)
                    canPut = false;

            }
            
            if (canPut)
                SetItemDisplayImgColor(canPutColor);
            else
                SetItemDisplayImgColor(canNotPutColor);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {

        PointerEventData.InputButton inputButton = eventData.button;
        if (inputButton == PointerEventData.InputButton.Right)
        {
            //按下右键
            if (itemSlotData == null || itemSlotData.itemData == null)
                return;
            ItemData itemData = itemSlotData.itemData;
            InventoryManager.Instance.UnequipItem(itemData as EquipmentData);
            InventoryManager.Instance.AddItem(itemData);
        }
    }
}
