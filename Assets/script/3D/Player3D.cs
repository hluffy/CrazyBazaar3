using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D : MonoBehaviour
{
    [Header("Move Info")]
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 40.0f;

    #region 组件
    public CharacterController cc { get; private set; }
    public Animator anim { get; private set; }
    #endregion

    #region 状态
    public Player3DStateMachine stateMachine { get; private set; }
    public Player3DIdleState idleState { get; private set; }
    public Player3DWalkState walkState { get; private set; }
    public Player3DAttackState attackState{ get; private set; }
    #endregion

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        stateMachine = new Player3DStateMachine();
        idleState = new Player3DIdleState(stateMachine,this,"Idle");
        walkState = new Player3DWalkState(stateMachine, this, "Walk");
        attackState = new Player3DAttackState(stateMachine, this, "Attack");
    }

    // Start is called before the first frame update
    void Start()
    {
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();
    }
}
