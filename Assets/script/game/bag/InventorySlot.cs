using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static BagManager;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
{
    public ItemSlotData itemSlotData;
    public Image itemDisplayImg;
    public Text name_;
    public Text count_;
    public Text durable_;

    Earthland farmland;

    //是否能放入当前slot
    protected bool canPut = true;
    protected Color canPutColor = new(156/255f, 211/255f, 108/255f, 110/255f);
    protected Color canNotPutColor = new(177/255f, 109/255f, 80/255f, 110/255f);


    protected virtual void Start()
    {
        farmland = FindObjectOfType<Earthland>();
    }

    public void Display(ItemSlotData itemToDisplay)
    {
        if (itemToDisplay == null)
        {
            itemDisplayImg.gameObject.SetActive(false);
            return;
        }
        if (itemToDisplay.itemData == null)
        {
            print("-----------------itemToDisplay.itemData = null");
            return;
        }
        itemDisplayImg.sprite = itemToDisplay.itemData.thumbnail;
        this.itemSlotData = itemToDisplay;
        itemDisplayImg.gameObject.SetActive(true);

        count_.gameObject.SetActive(true);
        count_.text = "" + itemSlotData.quantity;

        EquipmentData data = itemToDisplay.itemData as EquipmentData;
        if (data != null)
        {
            if (data.durable == -1) return;
            if (data.durable == 0)
            {
                return;
            }
            durable_.gameObject.SetActive(true);
            durable_.text = "" + data.durable + "%";
            count_.gameObject.SetActive(false);
        }
    }

    public void Reset()
    {

    }

    public void ResetQuantity()
    {
        if (itemSlotData != null)
        {
            count_.gameObject.SetActive(true);
            count_.text = "" + itemSlotData.quantity;

        }
        else
        {
            ClearSlot();
        }

    }

    public void AddCount(int count)
    {
        itemSlotData.AddQuantity(count);
        count_.gameObject.SetActive(true);
        count_.text = "" + itemSlotData.quantity;
    }
    public void DeleteCount(int count)
    {
        itemSlotData.Remove();
        if (itemSlotData.quantity <= 1)
        {
            count_.gameObject.SetActive(false);
        }
        else
        {
            count_.gameObject.SetActive(true);
            count_.text = "" + itemSlotData.quantity;
        }
    }
    public virtual void ClearSlot()
    {
        itemSlotData = null;
        itemDisplayImg.sprite = null;
        count_.gameObject.SetActive(false);
        durable_.gameObject.SetActive(false);
    }

    public virtual void UpdateSlot(ItemSlotData _newItem)
    {
        if (_newItem == null)
            return;
        itemSlotData = _newItem;
        itemDisplayImg.sprite = itemSlotData.itemData.thumbnail;
        name_.text = itemSlotData.itemData.itemName;
        count_.text = itemSlotData.quantity.ToString();
        if (itemSlotData.quantity > 1)
        {
            count_.gameObject.SetActive(true);
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        name_.gameObject.SetActive(true);
        canPut = true;

        ItemSlotData selectItemSlotData = InventoryManager.Instance.selectItemSlotData;
        if (selectItemSlotData != null && selectItemSlotData.itemData != null)
        {
            ItemData itemData = selectItemSlotData.itemData;
            
            if (canPut)
                SetItemDisplayImgColor(canPutColor);
            else
                SetItemDisplayImgColor(canNotPutColor);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        name_.gameObject.SetActive(false);
        itemDisplayImg.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public bool IsThis(ItemSlotData justyData)
    {
        if (itemSlotData == null) return false;
        return justyData.itemData == itemSlotData.itemData;
    }
    public bool IsNull()
    {
        return itemSlotData == null || itemSlotData.itemData == null;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        PointerEventData.InputButton inputButton = eventData.button;
        if (inputButton == PointerEventData.InputButton.Left && canPut)
        {
            ClearItemDisplayImgColor();

            //按下左键
            ItemSlotData tmp = itemSlotData;

            ItemSlotData selectItemSlotData = InventoryManager.Instance.selectItemSlotData;
            InventorySlot selectInventorySlot = InventoryManager.Instance.selectInventoySlot;

            if (selectItemSlotData == null || selectItemSlotData.itemData == null)
            {
                if (tmp == null || tmp.itemData == null)
                    return;

                InventoryManager.Instance.selectItemSlotData = tmp;
                InventoryManager.Instance.selectInventoySlot = this;
                ClearSlot();
                InventoryManager.Instance.StartDragging(tmp.itemData.thumbnail);
            }
            else
            {
                if (tmp == null || tmp.itemData == null)
                {
                    selectInventorySlot.ClearSlot();
                    UpdateSlot(selectItemSlotData);
                    InventoryManager.Instance.ClearSelect();
                    InventoryManager.Instance.StopDragging();
                }
                else
                {
                    UpdateSlot(selectItemSlotData);
                    InventoryManager.Instance.selectItemSlotData = tmp;
                    InventoryManager.Instance.selectInventoySlot = this;
                    InventoryManager.Instance.StartDragging(tmp.itemData.thumbnail);
                }
            }
        }
        else if (inputButton == PointerEventData.InputButton.Right)
        {
            //按下右键
            if (itemSlotData == null || itemSlotData.itemData == null)
                return;
            ItemData itemData = itemSlotData.itemData;
            if (itemData is EquipmentData)
            {
                InventoryManager.Instance.EquipItem(itemData);
            }
            else if (itemData.itemType == ItemType.Food)
            {
                itemData.AddModifiers();
                InventoryManager.Instance.RemoveItem(itemData);
            }
        }
    }

    protected void SetItemDisplayImgColor(Color _color)
    {
        itemDisplayImg.color = _color;
    }

    protected void ClearItemDisplayImgColor()
    {
        SetItemDisplayImgColor(Color.white);
    }
    
}
