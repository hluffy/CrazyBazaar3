using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Vector3 worldPositon;
    private Quaternion worldRotation;
    private Vector3 worldScale;

    void Start()
    {
        worldPositon = transform.position;
        worldRotation = transform.rotation;
        worldScale = transform.lossyScale;
    }

    void LateUpdate()
    {
        transform.SetPositionAndRotation(worldPositon, worldRotation);
        transform.localScale = worldScale;
    }
}
