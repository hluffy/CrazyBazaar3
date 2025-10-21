using Fungus;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorItemSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    private Image image;
    private TextMeshProUGUI text;
    private TextMeshProUGUI coinText;
    private ItemSlotData itemSlotData;

    private string buyBlockName = "BuyBlock";

    private void OnEnable()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        coinText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void SetItemSlot(ItemSlotData _itemSlotData)
    {
        itemSlotData = _itemSlotData;

        image.sprite = itemSlotData.itemData.thumbnail;
        image.enabled = true;
        text.text = itemSlotData.quantity.ToString();
        coinText.text = _itemSlotData.cost.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var material = image.material;
        if (material != null)
        {
            var instanceMaterial = new Material(material);
            image.material = instanceMaterial;

            instanceMaterial.SetFloat("_IsSelect", 1);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var material = image.material;
        if (material != null)
        {
            material.SetFloat("_IsSelect", 0);
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerEventData.InputButton button = eventData.button;
        if (button == PointerEventData.InputButton.Right)
        {
            Flowchart flowchart = PlayerManager.instance.flowchart;

            Variable fungusVariable = flowchart.GetVariable("selectItemData");
            if (fungusVariable != null && fungusVariable is ObjectVariable objectVar)
            {
                objectVar.Value = itemSlotData.itemData;
            }

            flowchart.SetStringVariable("itemName", itemSlotData.itemData.itemName);
            flowchart.SetIntegerVariable("itemNum", itemSlotData.quantity);
            flowchart.ExecuteBlock(buyBlockName);
        }
    }
}
