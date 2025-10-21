using Assets.script.nav;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveCtl : MonoBehaviour
{
    #region 变量

    private NavMeshAgent agent;
    private LineRenderer lineRenderer; // 繪製虛線

    public GameObject targetPrefab;
    private GameObject target;

    private Animator animator;
  


    private bool isMoveToStartPos = false;

    private PassJump passJump;
    private PassLouti passLouti;

    #endregion

    #region Unity生命周期


    private void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError(" 没有查询到 NavMeshAgent! ");
        }
        lineRenderer = transform.GetComponent<LineRenderer>();

        animator = transform.GetComponent<Animator>();
       
        passJump = new PassJump();
        passLouti = new PassLouti();
    }

    private void Update()
    {
      
        // 如果按下w案件
        DrawPath();
        if (target != null)
        {
            target.SetActive(agent.hasPath); // 物體移動到目标点后，预制体消息
        }
        if (animator != null)
        {
            animator.SetFloat("speed", agent.velocity.magnitude); // 把nav里的速度传递给动画
        }
        MoveOffMeshLink();
    }

    private void OnAnimatorMove()
    {
        // 重写这个方法，是因为人物在静止的时候还移动
    }

    #endregion

    #region 方法


    public void MoveToTarget(Vector3 targetPos)
    {
        Debug.Log("MoveToTargetxxxxxxxxxxxxxxxxxxxxxx");
        agent.SetDestination(targetPos);
        if (target == null)
        {
            target = GameObject.Instantiate(targetPrefab);
        }
        target.transform.position = targetPos;

    }
    public void DrawPath()
    {

        if (lineRenderer != null)
        {
            lineRenderer.positionCount = agent.path.corners.Length;
            lineRenderer.SetPositions(agent.path.corners);
        }

    }

    public void MoveOffMeshLink()
    {
        if (agent.isOnOffMeshLink)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            // 进行移动  判断是爬楼梯 还是 过河
            if (isMoveToStartPos == false)
            {

                // 移动到爬楼梯开始的位置，解决闪速
                transform.position = Vector3.MoveTowards(transform.position, data.startPos, 5 * Time.deltaTime);

                if (Vector3.Equals(transform.position, data.startPos))
                {
                    Debug.Log("xxxxxxxxxxxxxxx  isMoveToStartPos = true");
                    isMoveToStartPos = true;
                }
                return;
            }
            switch (data.linkType)
            {
                case OffMeshLinkType.LinkTypeManual:
                    //  爬楼梯是自定义的  上桥
                    Debug.Log("爬楼梯");
                    passLouti.Move(data, transform, OnPassFinish);

                    break;
                case OffMeshLinkType.LinkTypeDropDown:
                    break;
                case OffMeshLinkType.LinkTypeJumpAcross:
                    // 跳跃
                    Debug.Log("跳跃");
                    passJump.Move(data, transform, OnPassFinish);
                    break;
                default:
                    Debug.Log("其他 ");
                    if (data.offMeshLink == null)
                    {
                        Debug.Log("OffMeshLink is null");
                        NavMeshLink navMeshLink5 = agent.navMeshOwner as NavMeshLink;
                        if (navMeshLink5 != null)
                        {
                            // yidong 
                        }
                    }
                    Debug.Log("未发现移动技能");
                    break;
            }

        }
    }

    public void OnPassFinish()
    {
        agent.CompleteOffMeshLink();
        isMoveToStartPos = false;
    }
    #endregion
}
