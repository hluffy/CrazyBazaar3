using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Land;


// ������涼������ֲ��
public class Earthland : MonoBehaviour
{

    [Header("Crops")] // ����������ֲֲ��
    public GameObject cropPrefab;

    private GameObject mouseObj;

    public Image itemDisplayImg; // ��Ʒ����ͼ

    private Land seedLand; // ��ֲ�ĵ�Ƥ
   
    void Start()
    {

    }

    RaycastHit hit;
    void Update()
    {
        if (mouseObj != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 worldPosition = hit.point;
                worldPosition.y = transform.position.y;
                mouseObj.transform.position = worldPosition;
                Transform cant=  mouseObj.transform.Find("cant");
                print("------------1:"+(seedLand==null));

             // TerrainLayer[] ss=  TerrainData.terrainLayers;
             /*   if (seedLand!=null)
                {
                    print("------------2:" + (seedLand.landStatus == LandStatus.Soil));
                    print("------------2:" + (cant == null));
                    if (seedLand.landStatus == LandStatus.Soil)
                    {
                        // 材质变成红色
                        cant.gameObject.SetActive(true);
                    }
                    else
                    {
                        // 材质变成绿色
                        cant.gameObject.SetActive(false);
                    }
                    
                }*/

            }

        }
    }

    public void InstantiateMouseObj()
    {

        ItemSlotData itemSlotData = InventoryManager.Instance.mouse;
        if (itemSlotData == null || itemSlotData.itemData == null)
        {
            if (mouseObj != null)
            {
                Destroy(mouseObj);
                mouseObj = null;
            }
            return;
        }
       
        print("������ ");
        SeedData seed = InventoryManager.Instance.IsMouseSeedEquip();
       
        if (mouseObj != null)
        {
            Destroy(mouseObj);
        }
      
        if (seed != null)
        {
            // mouseObj = Instantiate(seed.seedling, transform);
            // Transform cant = mouseObj.transform.Find("cant");
            // cant.gameObject.SetActive(true);
        }
        else
        {
            mouseObj = Instantiate(itemSlotData.itemData.gameModel, transform);
        }
        BoxCollider boxCollider = mouseObj.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
       
        mouseObj.SetActive(true);
    }
    
    public void ActiveLand(Land selectedLand)
    {
        seedLand = selectedLand;
    }
    public void PlantSeed(SeedData seed)
    {
        print("-----------PlantSeed:"+ seed);
        seed.Print();
        // if (cropPlanteds == null) cropPlanteds = new List<CropBehaviour>();
        // ������

        GameObject cropObject = Instantiate(cropPrefab, transform);
        // cropObject.transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        cropObject.transform.position = mouseObj.transform.position;
        CropBehaviour cropPlanted = cropObject.GetComponent<CropBehaviour>();
        cropPlanted.Plant(seed);

        ItemSlotData itemSlotData = InventoryManager.Instance.mouse;

        itemSlotData.quantity--;
        if (itemSlotData.quantity == 0)
        {
            InventoryManager.Instance.mouse = null;
            Destroy(mouseObj);
            mouseObj = null;
        }

    }


}
