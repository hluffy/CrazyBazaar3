using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLowerPlayer : MonoBehaviour
{
    private Vector3 offset = Vector3.zero;
    void Start()
    {
        offset = transform.position - PlayerManager.instance.player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = PlayerManager.instance.player.transform.position + offset;
    }
}
