using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private ItemData[] possibleDrop;


    public virtual void GenerateDrop()
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            DropItem(possibleDrop[i]);
        }
    }

    private void DropItem(ItemData _itemData)
    {
        if (_itemData == null || _itemData.gameModel == null)
            return;

        Debug.Log("Drop item: " + _itemData.name);
        GameObject newDrop = Instantiate(_itemData.gameModel, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-80, 80), Random.Range(250, 300));
        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity);
    }
}
