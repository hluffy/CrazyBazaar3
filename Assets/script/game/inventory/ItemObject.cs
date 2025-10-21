using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : Objects
{
 
     [SerializeField] private Rigidbody2D rb;
    [SerializeField] public ItemData itemData;

    // private RegrowableHarvestBehaviour regroupable;
 

    float initY;

    // public override void Start()
    // {
    //     regroupable = transform.GetComponent<RegrowableHarvestBehaviour>();
    // }
      

    public override void Start()
    {
        initY = transform.position.y;
    }

    private void SetupVisuals()
    {
        if (itemData == null)
            return;
        gameObject.name = "Item object - " + itemData.itemName;
    }

    public void SetupItem(ItemData _itemData, Vector2 _velocity)
    {

        itemData = _itemData;
        SetupVisuals();

        rb.AddForce(_velocity);

        StartCoroutine(CheckLanding());
    }

    private IEnumerator CheckLanding()
    {
        float maxWaitTime = 5f;
        float elapsedTime = 0f;

        while (transform.position.y >= initY && elapsedTime < maxWaitTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;

        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
            sr.sortingOrder = -((int)transform.position.y * 10 + (int)transform.position.x);
    }

    public virtual void PickupItem()
    {
        // regroupable?.PickUp();
        // 捡起来后，销毁
        InventoryManager.Instance.AddItem(itemData);
       
        Destroy(gameObject);
       
    }
}
