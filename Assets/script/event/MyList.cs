using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyList : MonoBehaviour
{
    //public
    public GameObject originObject;
    public Transform parentTransForm;

    /// <summary>
    /// ��¡һ��GameObject
    /// </summary>
    public void InstantiateList()
    {
        GameObject.Instantiate(originObject, parentTransForm);
    } 
}
