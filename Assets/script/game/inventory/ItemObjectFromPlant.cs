using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 植株采摘
public class ItemObjectFromPlant : ItemObject
{
    private CropBehaviour parentCrop;
 
    // private RegrowableHarvestBehaviour regroupable;


    public override void Start()
    {
        parentCrop = transform.GetComponent<CropBehaviour>();
    }
    public override void PickupItem()
    {
        InventoryManager.Instance.AddItem(parentCrop.plantData.harvest, parentCrop.plantData.harvestCount);
        if (parentCrop.plantData.regrowable)
        {
            // 采摘后，不一定销毁
            parentCrop.Regrow();
        }
        else
        {
            Destroy(gameObject);
        }
       
    }
}
