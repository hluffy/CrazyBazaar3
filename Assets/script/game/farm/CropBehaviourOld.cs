using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropBehaviourOld : MonoBehaviour, ITimeTrack
{

    public PlantData plantData;

    public GameObject germination; // 发芽期 默认是刚种下带土的种子
    public GameObject seedling; // 幼苗期，长大一点的植株
    public GameObject harvestable; // 收获的植株
    public GameObject wilted; // 枯萎

    int growth;
    int maxGrowth;

    int maxHealth = GameTimestamp.HoursToMinuts(48);
    int health;

    int water = GameTimestamp.HoursToMinuts(96);

    // public Transform ccamera;
    // public Transform dir;

    public enum CropState
    {
        Seed, Seedling, Harvestable, Wilted
    }

    public CropState cropState=CropState.Seed;

    private void Start()
    {
        TimeManager.Instance.RegisterTracker(this);
        // 获取当前状态，初始化的时候，有可能不是种子，直接就是植株
        // if (cropState == CropState.Seed)
        // {
        //     return;
        // }
        int hourToGrow = GameTimestamp.DaysToHours(plantData.dayToGrow);
        maxGrowth = GameTimestamp.HoursToMinuts(hourToGrow);

        
        // if (cropState==CropState.Harvestable)
        // {
        //     // �ջ���ܼ�������
        //     RegrowableHarvestBehaviour2 regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour2>();
        //     regrowableHarvest.SetParent(this);
        // }

        SwitchState(cropState);

        foreach (Transform child in transform)
        {
            // 原地绕Y轴旋转
            child.rotation = Camera.main.transform.rotation;
        }
       
    }
     void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        {
            foreach (Transform child in transform)
            {
                // 原地绕Y轴旋转
                child.rotation = Camera.main.transform.rotation;
            }
        }

      
    }

    public void Plant(SeedData seedToGrow)
    {
        PlantData cropToYild = seedToGrow.cropToYield;
        int hourToGrow = GameTimestamp.DaysToHours(cropToYild.dayToGrow);
        maxGrowth = GameTimestamp.HoursToMinuts(hourToGrow);

        this.plantData = cropToYild;
        // seedling = Instantiate(seedToGrow.seedling,transform);

        // PlantData cropToYild = seedToGrow.cropToYield;

        // harvestable = Instantiate(cropToYild.gameModel,transform);

        // wilted = Instantiate(cropToYild.wilt, transform);
        if (plantData.regrowable)
        {
            // �ջ���ܼ�������
            // RegrowableHarvestBehaviour regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour>();
            // regrowableHarvest.SetParent(this);
        }


        SwitchState(CropState.Seed);
    }
    
    public void Grow()
    {
    //    print("-----------growth:" + growth+ ",  maxGrowth:"+ maxGrowth);
        if (cropState == CropState.Wilted) return;

        growth++;

        if (health < maxHealth)
        {
            health++;
        }

        if (growth >= maxGrowth / 2 && cropState == CropState.Seed && cropState != CropState.Seedling) {
            SwitchState(CropState.Seedling);
        }

        if (growth >= maxGrowth && cropState!= CropState.Harvestable)
        {
            SwitchState(CropState.Harvestable);
        }
    }

    public void SwitchState(CropState stateToSwitch)
    {
        print("-----------SwitchState1:"+ stateToSwitch);
       
        
        germination.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        wilted.SetActive(false);
       
        switch (stateToSwitch)
        {
            case CropState.Seed:
                germination.SetActive(true);
                SetSortingOrder(germination);
                // seedling.SetActive(true);
                break;
            case CropState.Seedling:
                seedling.SetActive(true);
                SetSortingOrder(seedling);
                health = maxHealth;
                break;
            case CropState.Harvestable:
                harvestable.SetActive(true);
                SetSortingOrder(harvestable);

                // if (!plantData.regrowable)
                // {
                //     harvestable.transform.parent = null;
                //     Destroy(gameObject);
                // }
                break;
            case CropState.Wilted:
                wilted.SetActive(true);
                SetSortingOrder(wilted);
                break;

        }
        cropState = stateToSwitch;
    }

    private void SetSortingOrder(GameObject _gameObject)
    {
        SpriteRenderer sr = _gameObject.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = -((int)_gameObject.transform.position.y * 10 + (int)_gameObject.transform.position.x);
        }
    }

    public void Regrow()
    {
        // print("----------------Regrow");
        int hourToGrow = GameTimestamp.DaysToHours(plantData.dayToRegrow);
        growth = maxGrowth - GameTimestamp.HoursToMinuts(hourToGrow);
        SwitchState(CropState.Seedling);
    }

    public void Wither()
    {
       // print("-----------Wither:" + health);
        health--;
        if(health<=0 && cropState != CropState.Seed)
        {
            SwitchState(CropState.Wilted);
        }
    }

    public void Water()
    {
        water--;
        if (water <= 0 && cropState != CropState.Seed)
        {
            SwitchState(CropState.Wilted);
        }
    }
    public void ClockUpdate(GameTimestamp timestamp)
    {
        if (germination == null || seedling == null) return;
        Grow();
        Water();
    }

    private void OnDestroy()
    {
        TimeManager.Instance.UnRegisterTracker(this);
    }
}
