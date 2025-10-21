using System.Collections;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InteractableObject : MonoBehaviour
// , IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler 
{ 

    public ItemData item;
    // public CropBehaviour parentCrop;

    //   [SerializeField] private GameObject parent;

    UITips uITips;
    private void Start()
    {
        uITips = FindObjectOfType<UITips>();
    }
    public virtual void PickUp()
    {

        Debug.Log("-----------------InteractableObject");
        InventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
        //  if (!parentCrop.plantData.regrowable)
        //  {
        //     harvestable.transform.parent = null;
        //     Destroy(gameObject);
        //     return; 
        // }
    }
        
    //  }
    // public virtual void PickUp()
    // {
    //     ItemSlotData itemSlotData = new ItemSlotData(item, 1);
    //     EquipmentData equip = item as EquipmentData;

    //     InventoryManager.Instance.AddInventory(itemSlotData);
    //     if (item as EquipmentData != null)
    //     {
    //         // 捡武器，装备栏为空的话，直接装备
    //         if (InventoryManager.Instance.equippedTool == null)
    //         {

    //             BagManager.Instance.EquippedTool(itemSlotData);
    //         }
    //         else
    //         {
    //             //添加到背包
    //             BagManager.Instance.OnTitleClick(0);
    //         }
    //     }
    //     else if (item as SeedData != null)
    //     {
    //         // 捡物品，种子到种子栏
    //         BagManager.Instance.OnTitleClick(1);
    //     }
    //     else if (item as FoodData != null)
    //     {
    //         // 捡物品，种子到种子栏
    //         BagManager.Instance.OnTitleClick(2);
    //     }
    //     else
    //     {
    //         // 捡物品，食物到食物栏
    //         BagManager.Instance.OnTitleClick(2);
    //     }
    //     Destroy(gameObject);
    // }

    // public void OnPointerClick(PointerEventData eventData)
    // {
    //     print("点击OnPointerClick");

    //     if (Input.GetMouseButtonUp(0))
    //     {

    //         OnMouseLeftClick();


    //     }
    //     else if (Input.GetMouseButtonUp(1))
    //     {

    //         OnMouseRightClick();

    //     }
    // }
    // void OnMouseLeftClick()
    // {
 
    //     print("左键 玩家走进植物  植物还没长好");
    //     uITips.ShowHeadTips(true, "采摘植物");
    
    //     PickUp();
      
       
    // }
  
    // void OnMouseRightClick()
    // {
    //     // 玩家走进
    //     print("右键 玩家走进植物");
       
    // }

    // public void OnPointerEnter(PointerEventData eventData)
    // {
        
    // }
    // public void OnPointerExit(PointerEventData eventData)
    // {

      
    // }
   
 
}