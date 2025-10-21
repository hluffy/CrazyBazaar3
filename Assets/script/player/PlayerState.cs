using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerState
{
    protected PlayerStateMachine stateMachine;
    protected Player player;
    protected string animBoolName;
    protected Vector2 targetDirection = Vector2.zero;

    protected bool triggerCalled;


    public PlayerState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);

        triggerCalled = false;
    }

    public virtual void Update()
    {
        bool click = player.inputActions.Player.Click.ReadValue<float>() > 0.1f;
        if (click)
        {
            PointerEventData eventDataCurrentPosition = new(EventSystem.current);
            eventDataCurrentPosition.position = Input.mousePosition;
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            // 过滤掉 Canvas 本身和其他非交互式元素
            bool isClickingOnInteractiveUI = false;
            foreach (RaycastResult result in results)
            {
                // 检查是否点击在真正的交互式 UI 元素上
                if (PlayerManager.instance.IsInteractiveUIElement(result.gameObject))
                {
                    isClickingOnInteractiveUI = true;
                    break;
                }
            }

            if (isClickingOnInteractiveUI)
            {
                return; // 点击在交互式 UI 上，不执行角色控制
            }

            // 获取从摄像机通过鼠标位置的射线
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // 计算射线与XY平面（Z=0）的交点
            Plane xyPlane = new(Vector3.forward, Vector3.zero);
            if (xyPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                Vector2 worldMousePosition = (Vector2)worldPoint;

                // 现在使用worldMousePosition进行2D射线检测
                RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.down, 10f);
                if (hit.collider != null)
                {
                    if (hit.collider.GetComponent<Objects>() != null)
                    {
                        player.target = hit.collider.gameObject;
                    }
                    else
                    {
                        Vector2 worldPosition = hit.point;
                        targetDirection = (worldPosition - new Vector2(player.transform.position.x, player.transform.position.y)).normalized;

                        ItemSlotData itemSlotData = InventoryManager.Instance.selectItemSlotData;
                        if (itemSlotData != null && itemSlotData.itemData != null)
                        {
                            
                        }
                    }
                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (player.target == null)
                {
                    Objects obj = player.GetNearestObject();
                    if (obj != null)
                    {
                        player.target = obj.gameObject;
                    }
                }
            }
        }

        if (!click)
        {
            targetDirection = player.inputActions.Player.Move.ReadValue<Vector2>();
            if (targetDirection.magnitude > 0.1f)
            {
                player.target = null;
                if (stateMachine.currentState != player.walkState)
                {
                    SetAnimInput(targetDirection);
                    stateMachine.ChangeState(player.walkState);
                    return;
                }
            }
        }

        if (player.target != null)
        {
            targetDirection = player.target.transform.position - player.transform.position;

            float stopDistance = player.target.GetComponent<Objects>().stopDistance;
            float distance = Vector2.Distance(new Vector2(player.target.transform.position.x, player.target.transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));
            if (distance < stopDistance)
            {
                targetDirection = Vector2.zero;
                Objects objs = player.target.GetComponent<Objects>();
                
                if (objs is TreeController)
                {
                    if (stateMachine.currentState != player.attackState)
                    {
                        SetAnimInput(targetDirection);
                        stateMachine.ChangeState(player.attackState);
                        return;
                    }
                }
                else
                {
                    if (stateMachine.currentState != player.pickupState)
                    {
                        SetAnimInput(targetDirection);
                        stateMachine.ChangeState(player.pickupState);
                        return;
                    }
                }
            }
        }
        targetDirection = targetDirection.normalized;
        SetAnimInput(targetDirection);
        player.Flip(targetDirection.x);
    }

    private void SetAnimInput(Vector2 targetDirection)
    {
        if (targetDirection != Vector2.zero)
        {
            targetDirection = targetDirection.normalized;

            player.anim.SetFloat("xInput", targetDirection.x);
            player.weaponAnim.SetFloat("xInput", targetDirection.x);

            player.anim.SetFloat("yInput", targetDirection.y);
            player.weaponAnim.SetFloat("yInput", targetDirection.y);
            player.overlayAnim.SetFloat("yInput", targetDirection.y);
        }
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
