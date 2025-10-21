using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ObjectRotation : MonoBehaviour
{
    Camera mainCamera;
    // public GameObject player;
    void Start()
    {
        GameObject gameObject = GameObject.FindWithTag("MainCamera");
        mainCamera = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {


        if (User.userState != UserState.Idil) return;
       


        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.rotation = mainCamera.transform.rotation;
        //    transform.LookAt(mainCamera.transform.position);
          

        //     transform.Rotate(0, 45, 0);

        //     transform.rotation = Quaternion.Euler( Camera.main.transform.rotation.x, transform.rotation.y, 0);
            return;
        }

        // ������ת45��
        if (Input.GetKeyDown(KeyCode.E))
        {
            // transform.position=new Vector3(6f, 0f, 0f);

            transform.rotation = mainCamera.transform.rotation;
        //    transform.LookAt(mainCamera.transform.position);
          

        //     transform.Rotate(0, -45, 0);

        //     transform.rotation = Quaternion.Euler( Camera.main.transform.rotation.x, transform.rotation.y, 0);

            return;
        }
    }
 
}
