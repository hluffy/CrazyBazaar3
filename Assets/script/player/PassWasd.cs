using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

// 前后左右移动
public class PassWasd : IPassable
{


    Quaternion targetDirQuaternion;

    private float moveSpeed = 6;


    public int panSpeed = 6;
    public float rotationAmount = 0.1f;

    private Rigidbody rb;

    public void Move3(Vector3 dirTarget, Transform transform, Action onFinsh)
    {

        if (rb == null)
        {
            rb = transform.GetComponent<Rigidbody>();
        }
        // 修改朝向 方向不变的话不变


        Vector3 targetDirection = dirTarget;
        targetDirection.y = 0;
        float y = Camera.main.transform.rotation.eulerAngles.y;
        targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;
        // transform.Translate(targetDirection * Time.deltaTime * moveSpeed, Space.World);

        // rb.velocity = moveSpeed * Time.deltaTime * dirTarget;
        //  rb.MovePosition(transform.position+ dirTarget * moveSpeed * Time.deltaTime);

         transform.GetComponent<CharacterController>().Move( moveSpeed * Time.deltaTime * dirTarget);

        if (dirTarget.magnitude >= 0.1f)
        {
            targetDirQuaternion = Quaternion.LookRotation(targetDirection);
        }

        if (transform.localRotation != targetDirQuaternion)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetDirQuaternion, Time.deltaTime * 10f);

        }
    }
    public void Move(Vector3 dirTarget, Transform transform, Action onFinsh)
    {

        // h=1 v=0 向右
        // h=-1 v=0 向左
        // h=0 v=1 向上
        // h=0 v=-1 向下
        // 玩家有操作

        dirTarget = dirTarget.normalized;
        transform.GetComponent<Animator>().SetBool("Walk", true);

        // 修改朝向 方向不变的话不变

        if (dirTarget.magnitude >= 0.1f)
        {
            targetDirQuaternion = Quaternion.LookRotation(dirTarget);
        }

        if (transform.localRotation != targetDirQuaternion)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetDirQuaternion, Time.deltaTime * 10f);

        }

        // 处理实际的移动

        //transform.GetComponent<CharacterController>().Move(dirTarget.normalized * moveSpeed);
        transform.GetComponent<CharacterController>().SimpleMove(dirTarget.normalized * moveSpeed);
    }

    public void DetectKeyWSAD(Action onFinsh)
    {
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            onFinsh?.Invoke();
        }
    }


    public void stop()
    {
        if (rb == null)
        {
            return;
        }
        rb.velocity = Vector3.zero;
    }

}
