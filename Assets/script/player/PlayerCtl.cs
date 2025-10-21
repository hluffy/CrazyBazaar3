using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static UnityEditor.SceneView;
using static UnityEngine.GraphicsBuffer;

public class PlayerCtl : MonoBehaviour
{


    private LineRenderer lineRenderer;

    [SerializeField] Animator animator;
   

    [SerializeField] CharacterController characterController;
    private Quaternion targetDirQuaternion;


    //[SerializeField] NavMeshAgent navMeshAgent;

    InteractableObject selectedInteractable = null;


    private PassWasd passWasd;
    private PassNavi passNavi;

    private void Start()
    {
        lineRenderer = transform.GetComponent<LineRenderer>();

        passWasd = new PassWasd();
        passNavi = new PassNavi();
        
       
    }
    private void Update()
    {
        passNavi.DetectNavi(transform, OnPassFinish);
        wsadMove();
        //DrawPath();
        if (Input.GetKey(KeyCode.P))
        {
            TimeManager.Instance.Tick();
        }

        pickUpObj();
    }
    private RaycastHit raycastHit;
    private void pickUpObj()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //  Debug.Log("---------------- "+( 1 << 9)  );
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 1000, 1 << 9))
            {
                Debug.Log("---------------- ");
                if (raycastHit.collider)
                {

                    Debug.Log("------------------" + raycastHit.transform.name);
                    Debug.Log("------------------" + raycastHit.transform.tag);
                    GameObject obj = GameObject.Find(raycastHit.transform.name);

                    if (obj.tag == "Item")
                    {
                        // selectedInteractable = obj.GetComponent<InteractableObject>();
                        // selectedInteractable.PickUp();
                        obj.GetComponent<ItemObject>().PickupItem();
                        return;
                    }
                    if (selectedInteractable != null)
                    {
                        selectedInteractable = null;
                    }
                }
            }
        }
    }
    public void DrawPath()
    {

  /*      if (lineRenderer != null)
        {
            lineRenderer.positionCount = navMeshAgent.path.corners.Length;
            lineRenderer.SetPositions(navMeshAgent.path.corners);
        }*/

    }
     
 
   
    private void wsadMove()
    {
     

 
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            gameObject.GetComponent<Animator>().SetBool("Walk", true);
            User.userState = UserState.Wsad;
            //passWasd.Move(new Vector3(h, 0, v), transform, OnPassFinish);

            passWasd.Move3(new Vector3(h, 0, v).normalized, transform, OnPassFinish);
            return;
        }
        if (User.userState == UserState.Wsad) OnPassFinish();
 

    }
    public void OnPassFinish()
    {
        transform.GetComponent<Animator>().SetBool("Walk", false);
        User.userState = UserState.Idil;
        passWasd.stop();
    }
}
