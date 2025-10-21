using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectStats : Stats
{
    protected override void Start()
    {
        base.Start();

    }

    protected override void Die()
    {
        base.Die();

        if(TryGetComponent<ItemDrop>(out ItemDrop itemDrop))
            itemDrop.GenerateDrop();
    }
}
