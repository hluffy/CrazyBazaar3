using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DManager : MonoBehaviour
{
    public static Player3DManager instance;

    public Player3D player;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }
}
