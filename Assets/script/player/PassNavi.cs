using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

    // 右键移动
    public class PassNavi : IPassable
    {

        private float moveSpeed = 6;

        private Vector3 targetDir;
        
        // private Rigidbody rb;




    public void Move(Vector3 dir, Transform transform, Action onFinsh)
    {


        // if (rb == null)
        // {
        //     rb = transform.GetComponent<Rigidbody>();
        // }
        User.userState = UserState.Navi;

        // transform.localPosition = Vector3.MoveTowards(transform.localPosition, dir, moveSpeed * Time.deltaTime);// 会穿过其他物体

        Vector3 dirP = targetDir - transform.position;
        dirP.y = 0;

        // // This is the distance to target to stop moving
        if (dirP.magnitude < 0.2F)
        {
            // Debug.Log("transform.localPosition == targetDirxxxxxx");
            onFinsh?.Invoke();
            return;
        }

        Vector3 movement = dirP.normalized * moveSpeed * Time.deltaTime;

        if (movement.magnitude > dirP.magnitude) movement = dirP;

        transform.GetComponent<CharacterController>().Move(movement);
        
        //  rb.velocity = moveSpeed * Time.deltaTime * movement;

    }

      

        private RaycastHit raycastHit;
        //   // 鼠标右键按下的时候发射射线，并且移动到右键位置
        public void DetectNavi(Transform transform,Action onFinsh )
        {
            if (User.userState == UserState.Navi)
            {
                Move(new Vector3(0, 0, 0), transform, onFinsh);

            }
            if (User.userState == UserState.Wsad)
            {
                // 正在wsad中的时候 不起作用
                return;
            }
          /*  if (Input.GetMouseButtonDown(1)) {
              
            }*/
                if (Input.GetMouseButton(1))
            {

                GameObject obj = GetFirstPickGameObject(Input.mousePosition);
                // if (obj == null) return;
                    //Debug.Log("鼠标右键按下 点击在UI上" + obj.name + "xxxx" + obj.layer);
                if (obj!=null && obj.layer == 5)
                {

                    return; // 5=UI 层
                }


               // Debug.Log("鼠标右键按下 GetMouseButtonDown");

                // 发射射线
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, 1000, 1 << 8))
                {
                 
                     //Debug.Log("鼠标右键按下 GetMouseButtonDown 2");
                     Vector3 sss = new Vector3(raycastHit.point.x, 0f, raycastHit.point.z);
                    if(sss== targetDir)
                    {
                        return;
                    }
                    User.userState = UserState.Navi;
                    transform.GetComponent<Animator>().SetBool("Walk", true);

                    targetDir = sss;
                 
                    LookRotation(targetDir, transform);

                }
                return;

            }
        }
        public void LookRotation(Vector3 targetDir, Transform transform)
        {
            Vector3 vec = (targetDir - transform.position);
            Quaternion targetDirQuaternion = Quaternion.LookRotation(vec);
            targetDirQuaternion.x = 0;
            targetDirQuaternion.z = 0;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetDirQuaternion, Time.deltaTime * 20f);
        }


        public GameObject GetFirstPickGameObject(Vector2 position)
        {
            EventSystem eventSystem = EventSystem.current;
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = position;
            //射线检测ui
            List<RaycastResult> uiRaycastResultCache = new List<RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, uiRaycastResultCache);
            if (uiRaycastResultCache.Count > 0)
                return uiRaycastResultCache[0].gameObject;
            return null;
        } 
    }
