using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class RegrowableHarvestBehaviour : InteractableObject 
{
 
    public CropBehaviour parentCrop;

    public override void PickUp()
    {
        Debug.Log("-----------------RegrowableHarvestBehaviour");

        if (parentCrop.plantData == null) return;
        InventoryManager.Instance.AddItem(parentCrop.plantData, parentCrop.plantData.harvestCount);

        parentCrop.Regrow();
    }
}
