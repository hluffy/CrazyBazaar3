using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamara : MonoBehaviour
{
    List<Transform> childs;

    void Start()
    {
        childs = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<ParticleSystem>() == null)
                childs.Add(child);
        }
    }

    void Update()
    {
        for (int i = 0; i < childs.Count; i++)
        {
            if (childs[i] != null)
                childs[i].rotation = Camera.main.transform.rotation;
        }
    }

    public void AddTransform(Transform _transform)
    {
        childs.Add(_transform);
    }
}
