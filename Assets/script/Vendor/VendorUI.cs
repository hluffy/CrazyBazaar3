using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class VendorUI : MonoBehaviour
{
    private Dictionary<ItemData, ItemSlotData> itemDataDictionary;

    public Transform bgVendor;
    public Transform itemSlotParent;

    public TextMeshProUGUI coinText;

    private Sprite sprite;
    private Transform[] itemSlots;

    private ItemSlotData[] itemSlotData;

    private void OnEnable()
    {
        itemSlots = new Transform[itemSlotParent.childCount];

        for (int i = 0; i < itemSlotParent.childCount; i++)
        {
            itemSlots[i] = itemSlotParent.GetChild(i);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseVendorUI();
        }
    }

    public void InitVendorUI(ref ItemSlotData[] _itemSlotDatas, Sprite _sprite)
    {
        itemSlotData = _itemSlotDatas;

        sprite = _sprite;

        gameObject.SetActive(true);

        itemDataDictionary = new Dictionary<ItemData, ItemSlotData>();

        for (int i = 0; i < _itemSlotDatas.Length; i++)
        {
            itemDataDictionary.Add(_itemSlotDatas[i].itemData, _itemSlotDatas[i]);
        }

        bgVendor.GetComponent<Image>().sprite = sprite;

        UpdateVendorUI();
    }

    public void RemoveItemData(ItemData _itemData,int _num)
    {
        if (itemDataDictionary.TryGetValue(_itemData, out ItemSlotData value))
        {
            if (-1 == _num)
            {
                _num = value.quantity;
            }

            int coin = InventoryManager.Instance.coin;
            int cost = value.cost;
            cost = _num * cost;

            if (coin < cost)
            {
                PlayerManager.instance.flowchart.SetBooleanVariable("buyResult", false);
                return;
            }
            InventoryManager.Instance.DecrementCoin(cost);

            if (value.quantity <= _num)
            {
                itemDataDictionary.Remove(_itemData);
                foreach (var item in itemSlotData)
                {
                    if (item == value)
                    {
                        item.quantity = 0;
                        break;
                    }
                }
            }
            else
            {
                value.quantity -= _num;
                foreach (var item in itemSlotData)
                {
                    if (item == value)
                    {
                        item.quantity = value.quantity;
                        break;
                    }
                }
            }

            InventoryManager.Instance.AddItem(_itemData, _num);
            PlayerManager.instance.flowchart.SetBooleanVariable("buyResult", true);

            UpdateVendorUI();
        }
    }

    private void UpdateVendorUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].GetComponent<Image>().enabled = false;
            itemSlots[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
            itemSlots[i].GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
        }

        int index = 0;
        foreach (KeyValuePair<ItemData, ItemSlotData> pair in itemDataDictionary)
        {
            VendorItemSlot vendorItemSlot = itemSlots[index].GetComponent<VendorItemSlot>();
            vendorItemSlot?.SetItemSlot(pair.Value);

            index++;
        }
        coinText.text = InventoryManager.Instance.coin.ToString();
        
    }


    public void CloseVendorUI()
    {
        gameObject.SetActive(false);
    }
}
