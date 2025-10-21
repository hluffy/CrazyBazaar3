using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateGameObj : MonoBehaviour,IPointerDownHandler
{


    public GameObject prefab;
    private GameObject instance;
    private RaycastHit raycastHit;

    public void OnPointerDown(PointerEventData eventData)
    {
        instance = GameObject.Instantiate(prefab);
    }

    private void Update()
    {
        if(Input.GetMouseButton(0) && instance != null)
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),
                out raycastHit,1000,1<<8))
            {
                instance.transform.position = raycastHit.point;
            }
        }

        if(Input.GetMouseButtonUp(0) && instance != null)
        {
            NavMeshLink[] navMeshLinks = instance.transform.
                GetComponentsInChildren<NavMeshLink>();
            for(int i = 0; i < navMeshLinks.Length; i++)
            {
                navMeshLinks[i].UpdateLink();
            }
            instance = null;
        }
    }
}
