using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ArtificialObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerClickHandler,ISaveManager
{
    public ItemSlotData[] itemSlotDatas;
    public string nametext;
    public Sprite sprite;

    private SpriteRenderer render;
    // 从存档中获取的
    private List<ItemSlotData> loadSlotDatas;
    

    private string helloBlock = "HelloBlock";

    void OnValidate()
    {
        gameObject.name = "vendor" + nametext;
    }

    void Awake()
    {
        loadSlotDatas = new List<ItemSlotData>();
    }

    void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        var material = render.material;
        material.SetFloat("_IsSelect", 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var material = render.material;
        material.SetFloat("_IsSelect", 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerEventData.InputButton button = eventData.button;
        if (button == PointerEventData.InputButton.Left)
        {
            PlayerManager.instance.SetBossCharacter(nametext,sprite);

            PlayerManager.instance.flowchart.ExecuteBlock(helloBlock);

            InventoryManager.Instance.InitVendor(ref itemSlotDatas, sprite);
        }
    }

    public void LoadData(GameData _data)
    {
        
        foreach (KeyValuePair<string, SerializableDictionary<string, string>> pair in _data.vendor)
        {
            string nametext = pair.Key;
            if (nametext != this.nametext)
                break;

            foreach (KeyValuePair<string, string> dataPair in pair.Value)
            {
                foreach (var item in InventoryManager.Instance.GetItemDataBase())
                {
                    if (item != null && item.itemId == dataPair.Key)
                    {
                        string[] values = dataPair.Value.Split(",");
                        loadSlotDatas.Add(new ItemSlotData(item, int.Parse(values[0]), int.Parse(values[1])));
                        break;

                    }
                }
            }
        }
        if (loadSlotDatas.Count > 0)
        {
            itemSlotDatas = loadSlotDatas.ToArray();
        }
    }

    public void SaveData(ref GameData _data)
    {
        if (string.IsNullOrEmpty(nametext) || itemSlotDatas.Length <= 0)
            return;

        SerializableDictionary<string, string> vendorData = new SerializableDictionary<string, string>();
        foreach (ItemSlotData item in itemSlotDatas)
        {
            if (item != null)
            {
                vendorData.Add(item.itemData.itemId, item.quantity + "," + item.cost);
            }
        }

        if (vendorData.Count > 0)
        {
            _data.vendor.Add(nametext, vendorData);
        }
    }
}
