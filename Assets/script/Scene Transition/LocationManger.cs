using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManger : MonoBehaviour
{

   public List<StartPoint> startPoints;



    public static LocationManger Instance { get; private set; }
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
       
    }

    public Transform GetPlayerStartPosition(SceneTransitionManager.Location enteringFrom)
    {
       
        StartPoint startingPoint = startPoints.Find(x=>x.enteringFrom==enteringFrom);
        print("----------GetPlayerStartPosition1:" + enteringFrom);
        print("----------GetPlayerStartPosition2:" + startingPoint.playerStart.name);
        return startingPoint.playerStart;
    }
    
}
