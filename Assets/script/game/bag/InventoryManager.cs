using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static BagManager;

public class InventoryManager : MonoBehaviour, ISaveManager
{

    public List<ItemSlotData> startingItems;
    public List<PlantData> randomPlant;

    public int coin { get; private set; }

    [Header("equipment")]
    private List<ItemSlotData> equips = new List<ItemSlotData>();
    public Dictionary<EquipmentData, ItemSlotData> equipDictionary = new Dictionary<EquipmentData, ItemSlotData>();

    [Header("Inventory UI")]
    [SerializeField] private Transform slotsParent;
    [SerializeField] private Transform equipsParent;
    public Transform dragImageParent;

    [Header("Player stats UI")]
    [SerializeField] private Transform healthBar;
    [SerializeField] private Transform healthText;

    [SerializeField] private Transform vendorUI;
    private VendorUI vendor;

    private Dictionary<ItemData, List<int>> slotDictionary = new();
    public ItemSlotData[] itemSlots;
    public InventorySlot[] slotsItemSlot;
    private EquipmentSlot[] equipsItemSlot;

    private CharacterStats playerStats;
    private Slider healthSlider;
    private TextMeshProUGUI text;

    // 当前选中
    public ItemSlotData selectItemSlotData = null;
    public InventorySlot selectInventoySlot;

    [Header("quik equipped")]
    public ItemData equippedTool = null;
    public ItemSlotData mouse = null;
    public ItemData equippedItem = null;

    [Header("Player Hand Equip")]
    public Transform handPoint;

    public List<ItemData> loadItemData;

    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        slotsItemSlot = slotsParent.GetComponentsInChildren<InventorySlot>();
        itemSlots = new ItemSlotData[slotsItemSlot.Length];
        equipsItemSlot = equipsParent.GetComponentsInChildren<EquipmentSlot>();

    }

    void Start()
    {
        vendor = vendorUI.GetComponent<VendorUI>();

        AddStartingItems();

        playerStats = PlayerManager.instance.player.GetComponent<CharacterStats>();
        healthSlider = healthBar.GetComponent<Slider>();
        text = healthText.GetComponent<TextMeshProUGUI>();

        // playerStats.onHealthChanged += UpdateHealthUI;
        // UpdateHealthUI();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) || selectItemSlotData == null || selectItemSlotData.quantity <= 0)
        {
            StopDragging();
        }
    }

    //开始绘制，图片跟随鼠标移动
    public void StartDragging(Sprite sprite)
    {
        dragImageParent.gameObject.SetActive(true);
        dragImageParent.GetComponent<DragImage>().SetupDragImage(sprite);
    }

    //停止绘制
    public void StopDragging()
    {
        dragImageParent.gameObject.SetActive(false);
        if (selectItemSlotData != null && selectItemSlotData.itemData != null && selectItemSlotData.quantity >= 1)
        {
            selectInventoySlot.UpdateSlot(selectItemSlotData);

            ClearSelect();
        }
    }

    private void AddStartingItems()
    {
        if (SaveManager.instance.haveData)
            return;

        for (int i = 0; i < startingItems.Count; i++)
        {
            ItemSlotData itemSlotData = startingItems[i];

            AddItem(itemSlotData.itemData, itemSlotData.quantity);
        }

    }

    public void EquipItem(ItemData _item)
    {
        EquipmentData newEquipment = _item as EquipmentData;
        ItemSlotData newItem = new ItemSlotData(_item);


        EquipmentData oldEquipment = null;
        foreach (KeyValuePair<EquipmentData, ItemSlotData> item in equipDictionary)
        {
            if (item.Key.toolType == newEquipment.toolType)
            {
                oldEquipment = item.Key;
            }
        }

        if (oldEquipment != null)
        {
            UnequipItem(oldEquipment);
            AddItem(oldEquipment);
        }

        equips.Add(newItem);
        equipDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();

        PlayerManager.instance.player.SetWeapon(newEquipment.weapon);

        RemoveItem(_item);

        UpdateSlotUI();
    }

    public void UnequipItem(EquipmentData _item)
    {
        RemoveEquipItem(_item);
        _item.RemoveModifiers();
        PlayerManager.instance.player.SetWeapon(null);

        UpdateSlotUI();
    }

    private void RemoveEquipItem(EquipmentData _item)
    {
        if (equipDictionary.TryGetValue(_item, out ItemSlotData value))
        {
            equips.Remove(value);
            equipDictionary.Remove(_item);
        }
    }

    public void RenderHand()
    {
        if (handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }
        ItemData item = InventoryManager.Instance.equippedTool;
        if (item == null || item.gameModel == null)
        {
            return;
        }
        Instantiate(InventoryManager.Instance.equippedTool.gameModel, handPoint);
    }


    public int AddInventory(ItemSlotData itemSlotData)
    {
        if (itemSlotData == null || itemSlotData.itemData == null) return -1;

        ItemData itemData = itemSlotData.itemData;
        int remainingQuantity = itemSlotData.quantity;

        // 尝试将物品添加到已有的槽位中
        if (slotDictionary.TryGetValue(itemData, out List<int> indexs))
        {
            for (int i = 0; i < indexs.Count && remainingQuantity > 0; i++)
            {
                int index = indexs[i];
                ItemSlotData slotData = itemSlots[index];
                int availableSpace = slotData.itemData.maxStack - slotData.quantity;
                
                if (availableSpace > 0)
                {
                    int addAmount = Math.Min(remainingQuantity, availableSpace);
                    slotData.quantity += addAmount;
                    remainingQuantity -= addAmount;
                }
            }
        }

        // 如果还有剩余数量，尝试放入空槽位
        if (remainingQuantity > 0)
        {
            for (int i = 0; i < itemSlots.Length && remainingQuantity > 0; i++)
            {
                if (itemSlots[i] == null || itemSlots[i].itemData == null)
                {
                    int addAmount = Math.Min(remainingQuantity, itemData.maxStack);
                    itemSlots[i] = new ItemSlotData(itemData, addAmount);
                    remainingQuantity -= addAmount;

                    // 更新字典
                    List<int> indices = slotDictionary.GetValueOrDefault(itemData, new List<int>());
                    indices.Add(i);
                    slotDictionary[itemData] = indices;
                }
            }
        }

        return remainingQuantity;
    }


    private void UpdateSlotUI()
    {

        for (int i = 0; i < equipsItemSlot.Length; i++)
        {
            equipsItemSlot[i].ClearSlot();
            foreach (KeyValuePair<EquipmentData, ItemSlotData> item in equipDictionary)
            {
                if (item.Key.toolType == equipsItemSlot[i].toolType)
                {
                    equipsItemSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < slotsItemSlot.Length; i++)
        {
            slotsItemSlot[i].ClearSlot();
            ItemSlotData itemData = itemSlots[i];
            if (itemData == null) slotsItemSlot[i].ClearSlot();
            else slotsItemSlot[i].UpdateSlot(itemData);
        }
    }

    public void AddItem(ItemData _item)
    {
        AddItem(_item, 1);
    }

    public void AddItem(ItemData _item, int _num)
    {
        if (_item == null)
            return;
            
        AddInventory(new ItemSlotData(_item, _num));
        UpdateSlotUI();
    }

    public void RemoveItem(ItemData _item)
    {
        if (_item == null) return;
        if (slotDictionary.TryGetValue(_item, out List<int> indexs))
        {
            foreach (var i in indexs)
            {
                ItemSlotData slotData = itemSlots[i];
                if ((slotData.quantity - 1) == 0)
                {
                    indexs.Remove(i);
                    itemSlots[i] = null;
                }
                else
                {
                    itemSlots[i].quantity -= 1;
                }
                return;
            }
        }

        UpdateSlotUI();
    }

    public void DesreaseSelectItemSlotData(int _num)
    {
        if (selectItemSlotData != null)
        {
            selectItemSlotData.quantity -= _num;
            if (selectItemSlotData.quantity <= 0)
            {
                selectItemSlotData = null;
                selectInventoySlot = null;
            }
        }
    }

    void AddInventory(List<ItemSlotData> parents, ItemSlotData itemSlotData, int maxStackingCount)
    {
        int p = -1;
        int emptyP = -1;


        for (int i = 0; i < parents.Count; i++)
        {
            if (parents[i] != null && parents[i].itemData != null)
            {
                if (parents[i].itemData == itemSlotData.itemData)
                {
                    int all = parents[i].quantity + itemSlotData.quantity;

                    if (all <= maxStackingCount)
                    {
                        parents[i].quantity += itemSlotData.quantity;
                        p = i;
                        return;
                    }
                    else if (parents[i].quantity < maxStackingCount && all > maxStackingCount)
                    {


                        parents[i].quantity = maxStackingCount;

                        itemSlotData.quantity = all - maxStackingCount;

                        AddInventory(parents, itemSlotData, maxStackingCount);

                        p = i;
                        return;
                    }


                }
            }
            if (parents[i] == null || parents[i].itemData == null)
            {
                if (emptyP == -1) emptyP = i;
            }

        }
        if (p == -1 && emptyP != -1)
        {
            parents[emptyP] = itemSlotData;
            return;
        }
        if (p == -1 && emptyP == -1)
        {
            print("�������ˣ�û�еط��ţ���������������ڵ���");
        }
    }
    
    public bool IsMouseEquip()
    {
        return mouse != null && mouse.itemData != null;
    }
    public SeedData IsMouseSeedEquip()
    {
        if (!IsMouseEquip())
        {
            return null;
        }
        return mouse.itemData as SeedData;
    }

    public void ClearSelect()
    {
        selectItemSlotData = null;
        selectInventoySlot = null;
    }

    private void UpdateHealthUI()
    {
        healthSlider.maxValue = playerStats.maxHealth.GetValue();
        healthSlider.value = playerStats.currentHealth;

        text.text = playerStats.currentHealth + "/" + playerStats.maxHealth.GetValue();
    }

    public void InitVendor(ref ItemSlotData[] _itemSlotDatas, Sprite _sprite)
    {
        vendor?.InitVendorUI(ref _itemSlotDatas, _sprite);
    }

    public void CloseVendorUI()
    {
        vendor?.CloseVendorUI();
    }

    // fungus中调用 BuyConfirmedBlock
    public void RemoveVendorItemData(ItemData _itemData, int _num)
    {
        vendor?.RemoveItemData(_itemData, _num);
    }

    public void IncrementCoin(int _cost)
    {
        coin += _cost;
    }

    public void DecrementCoin(int _cost)
    {
        coin -= _cost;
    }


    void OnDestroy()
    {
        // playerStats.onHealthChanged -= UpdateHealthUI;
    }

    public void LoadData(GameData _data)
    {
        coin = _data.currency;

        loadItemData = GetItemDataBase();

        foreach (KeyValuePair<string, int> pair in _data.inventory)
        {
            foreach (ItemData item in loadItemData)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    AddItem(item);
                    break;
                }
            }
        }

        foreach (KeyValuePair<string, int> pair in _data.equipment)
        {
            foreach (ItemData item in loadItemData)
            {
                if (item != null && item.itemId == pair.Key)
                {
                    EquipItem(item);
                    break;
                }
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        _data.currency = coin;

        foreach (KeyValuePair<EquipmentData, ItemSlotData> pair in equipDictionary)
        {
            _data.equipment.Add(pair.Key.itemId, pair.Value.quantity);
        }
    }

    public List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assetNames = AssetDatabase.FindAssets("", new[] { "Assets/Data" });

        foreach (string SOName in assetNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            ItemData itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);

            itemDataBase.Add(itemData);
        }

        return itemDataBase;
    }
}
