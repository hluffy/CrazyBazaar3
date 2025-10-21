using Assets.script.nav;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMoveCtl : MonoBehaviour
{
    #region ����

    private NavMeshAgent agent;
    private LineRenderer lineRenderer; // �L�u̓��

    public GameObject targetPrefab;
    private GameObject target;

    private Animator animator;
  


    private bool isMoveToStartPos = false;

    private PassJump passJump;
    private PassLouti passLouti;

    #endregion

    #region Unity��������


    private void Start()
    {
        agent = transform.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError(" û�в�ѯ�� NavMeshAgent! ");
        }
        lineRenderer = transform.GetComponent<LineRenderer>();

        animator = transform.GetComponent<Animator>();
       
        passJump = new PassJump();
        passLouti = new PassLouti();
    }

    private void Update()
    {
      
        // �������w����
        DrawPath();
        if (target != null)
        {
            target.SetActive(agent.hasPath); // ���w�Ƅӵ�Ŀ����Ԥ������Ϣ
        }
        if (animator != null)
        {
            animator.SetFloat("speed", agent.velocity.magnitude); // ��nav����ٶȴ��ݸ�����
        }
        MoveOffMeshLink();
    }

    private void OnAnimatorMove()
    {
        // ��д�������������Ϊ�����ھ�ֹ��ʱ���ƶ�
    }

    #endregion

    #region ����


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
            // �����ƶ�  �ж�����¥�� ���� ����
            if (isMoveToStartPos == false)
            {

                // �ƶ�����¥�ݿ�ʼ��λ�ã��������
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
                    //  ��¥�����Զ����  ����
                    Debug.Log("��¥��");
                    passLouti.Move(data, transform, OnPassFinish);

                    break;
                case OffMeshLinkType.LinkTypeDropDown:
                    break;
                case OffMeshLinkType.LinkTypeJumpAcross:
                    // ��Ծ
                    Debug.Log("��Ծ");
                    passJump.Move(data, transform, OnPassFinish);
                    break;
                default:
                    Debug.Log("���� ");
                    if (data.offMeshLink == null)
                    {
                        Debug.Log("OffMeshLink is null");
                        NavMeshLink navMeshLink5 = agent.navMeshOwner as NavMeshLink;
                        if (navMeshLink5 != null)
                        {
                            // yidong 
                        }
                    }
                    Debug.Log("δ�����ƶ�����");
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
