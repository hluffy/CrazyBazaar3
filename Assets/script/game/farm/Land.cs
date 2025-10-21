using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// С���
public class Land : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ITimeTrack
{
    public enum LandStatus
    {
        Soil, Farmland, Water,
    }

    public GameObject baseLand;
    public GameObject selected; // ������ʾ

    public LandStatus landStatus;

    public List<Material> soilMat, farmlandMat, waterMat;

    GameTimestamp timeWatered;

    /*[Header("Crops")] // ����������ֲֲ��
    public GameObject cropPrefab;


    [Header("Other")]
    private GameObject mouseObj;*/

   // List<CropBehaviour> cropPlanteds = null;

    Renderer render;

    public Vector3 targetPlantAdd; //���ѡ�е��ֵص�

    Earthland earthland;
    void Start()
    {
        render = GetComponent<Renderer>();
        SwitchLandStatus(landStatus);
        TimeManager.Instance.RegisterTracker(this);

        earthland = FindObjectOfType<Earthland>();// ��ȡ��ײ��ũ��
    }

    // Update is called once per frame
    void Update()
    {
       /* if (mouseObj != null)
        {
            // ��ȡ��������λ��
           
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 worldPosition = hit.point;
                worldPosition.y = transform.position.y;
                mouseObj.transform.position = worldPosition;
            }

        }*/
       
    }

    public void SwitchLandStatus(LandStatus landStatus)
    {
        if (render == null)
        {
            render = baseLand.GetComponent<Renderer>();
        }
        Material material = soilMat[Random.Range(0, soilMat.Count)];
        switch (landStatus)
        {
            case LandStatus.Soil:
                material = soilMat[Random.Range(0, soilMat.Count)];
                break;
            case LandStatus.Farmland:
                material = farmlandMat[Random.Range(0, farmlandMat.Count)];
                break;
            case LandStatus.Water:
                material = waterMat[Random.Range(0, waterMat.Count)];
                timeWatered = TimeManager.Instance.GetGameTimestamp();
                break;
        }
        render.material = material;
        this.landStatus = landStatus;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        print("------------:xxxxxxxxxxxxxxxxx0000");
        if (InventoryManager.Instance == null) return;

        //�������������һ�����ӣ���ô�����ֵأ�
        SeedData seed = InventoryManager.Instance.IsMouseSeedEquip();
        if (seed!=null)
        {
            if ( landStatus != LandStatus.Soil)
            {
                // PlantSeed(seed);
                print("------------:xxxxxxxxxxxxxxxxx1111");
                earthland.PlantSeed(seed);
                return;
            }
        }

        if (InventoryManager.Instance.equippedTool == null) return;

        EquipmentData equippedTool = InventoryManager.Instance.equippedTool as EquipmentData;
        if (equippedTool != null)
        {
            switch (equippedTool.toolType)
            {
                case EquipmentData.ToolType.Hoe:// ����
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:// ��ˮ
                    SwitchLandStatus(LandStatus.Water);

                    break;
                case EquipmentData.ToolType.Shove:// ����
                    // ���Ӳ������
                   /* if (cropPlanted != null)
                    {
                        Destroy(cropPlanted.gameObject);
                    }*/
                    break;
            }

            return;
        }


    }


    public LandStatus GetStatus()
    {
        return landStatus;
    }

    public void Selected(bool toggle)
    {
        if (selected == null) return;
        selected.SetActive(toggle);
    }

   /* void PlantSeed(SeedData seed)
    {
       // if (cropPlanteds == null) cropPlanteds = new List<CropBehaviour>();
        // ������

        GameObject cropObject = Instantiate(cropPrefab, transform);
        // cropObject.transform.position = new Vector3(transform.position.x, 0.2f, transform.position.z);
        cropObject.transform.position = mouseObj.transform.position;
        CropBehaviour cropPlanted = cropObject.GetComponent<CropBehaviour>();
        cropPlanted.Plant(seed);

        print("-----------cropObject.GetInstanceID():" + cropObject.GetInstanceID());

        // cropPlanteds.Add(cropPlanted);

        ItemSlotData itemSlotData= InventoryManager.Instance.mouseEquipped;

        itemSlotData.quantity--;
        if (itemSlotData.quantity ==0)
        {
            InventoryManager.Instance.mouseEquipped = null;
            Destroy(mouseObj);
            mouseObj = null;
        }
   
    }*/
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (InventoryManager.Instance == null) return;
        //�������������һ�����ӣ���ô�����ֵأ�
        SeedData seed = InventoryManager.Instance.IsMouseSeedEquip();
        if (seed != null )
        {
            earthland.ActiveLand(this);
            return;
        }
        /* SeedData seed = InventoryManager.Instance.IsMouseSeedEquip();
         if (seed!=null && mouseObj == null)
         {
             mouseObj = Instantiate(seed.seedling, transform);
             mouseObj.SetActive(true);
             return;
         }*/
        if (InventoryManager.Instance.equippedTool == null)
        {
            return;
        }
        EquipmentData equippedTool = InventoryManager.Instance.equippedTool as EquipmentData;
        if (equippedTool == null)
        {
            return;
        }

        switch (equippedTool.toolType) {
            case EquipmentData.ToolType.Hoe:// ����
                Selected(true);
                break;
            case EquipmentData.ToolType.WateringCan:// ��ˮ
                Selected(true);
                break;
          
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        /* if (mouseObj != null)
         {
             Destroy(mouseObj);
             mouseObj = null;
         }*/
        SeedData seed = InventoryManager.Instance.IsMouseSeedEquip();
        if (seed != null)
        {
            earthland.ActiveLand(null);
        }
        if (!selected.activeSelf) return;
        Selected(false);
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {

       /* if (landStatus == LandStatus.Water || landStatus == LandStatus.Farmland)
        {
            // ��ˮ�ĵػ��߲���ˮ�ĵض�����ֲ
            if (cropPlanted != null)
            {
                cropPlanted.Grow();
            }
           
        }

        if (landStatus == LandStatus.Water ) {
            int hourElapsed = GameTimestamp.CompareTimestamp(timeWatered,timestamp);
            print("---------:"+hourElapsed);
            if (hourElapsed > 6)
            {
                SwitchLandStatus(LandStatus.Farmland);
            }
        }

        if (landStatus != LandStatus.Water && cropPlanted != null)
        {
            if(cropPlanted.cropState != CropBehaviour.CropState.Seed)
            {
                cropPlanted.Wither();
            } 
        }
         */
    }
}
