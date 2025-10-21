using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEditor.SceneView;
using static UnityEngine.GraphicsBuffer;

public class camera_forword : MonoBehaviour
{
    public Transform player;


    // private float rotationAndel = 45f;   // ���D�ĽǶ�

    private Vector3 offset; // ����ͷ������Ĳ�ֵ
    private Vector3 offset2;// �������Ļ���ĵĲ�ֵ

    private Animator animator;

    private Vector3 initScreenCenter; // ��ʼ����Ļ����


    // public int panSpeed;
    // public float rotationAmount;


    private void Start()
    {
        offset = transform.position - player.position;
        animator = player.GetComponent<Animator>();
        initScreenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        offset2 = player.position - initScreenCenter;


        // panSpeed = 1;
        // rotationAmount = 0.1f;
    }


    private void LateUpdate()
    {

        // �����߶�ʱ��������Ÿ�����
        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKeyDown(KeyCode.E))
        {
            return;
        }
        transform.position = player.position + offset;
       
    }
    private void Update()
    {
       
        // ��������������̵��Ѿ���ʾ��������qe����zoom

        if (User.userState!=UserState.Chat)
        {
            QE2();
         
            camerazoom();
        }

       
        ClickGame();
      

    }
   
   
    private void QE2()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            // ����ͷΧ��������ת
            transform.RotateAround(player.position, Vector3.up * 5f, -45f);
            offset = transform.position - player.position;
            // ����ͷ����ͷ������ת
            /*   transform.Rotate(0, 45, 0);
               transform.rotation = Quaternion.Euler(25f, transform.rotation.y, 0);*/
        }
        // ������ת45��
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.RotateAround(player.position, Vector3.up * 5f, 45f);
            offset = transform.position - player.position;
            return;
        }
    }
   
    private void camerazoom() //�������������
    {

       
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            //Debug.Log("camerazoom��" + offset);
            transform.Translate(Vector3.forward * 0.5f);
            offset = transform.position - player.position;
        }else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            //Debug.Log("camerazoom��" + offset);
            transform.Translate(Vector3.forward * -0.5f);
            offset = transform.position - player.position;
        }
            
    }

    private void ClickGame()
    {

      
        // �ж��ǵ����ui�ϻ��ǵ��������λ��
        if (ClickOnUi())
        {
            print("�����UI��");
            return;
        }
  
       // ClickScene();
    }
    private bool ClickOnUi()
    {
        return false;
    }
    private void ClickScene()
    {
        // �����Ϸ�������

        if (Input.GetMouseButtonUp(0))
        {
            List<RaycastResult> list = new List<RaycastResult>();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                GameObject go = hit.collider.gameObject;
                print("hit:" + go.tag );

                if(go == null)
                {
                    return;
                }
              
            }
        }
    }
}

