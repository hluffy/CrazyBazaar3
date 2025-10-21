using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DState
{
    protected Player3DStateMachine stateMachine;
    protected Player3D player;
    protected string animBoolName;

    protected Vector3 targetDirection;

    protected bool triggerCalled;

    public Player3DState(Player3DStateMachine _stateMachine,Player3D _player,string _animBoolName)
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
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        targetDirection = new Vector3(horizontal, 0f, vertical);

        if (Input.GetKey(KeyCode.F) && stateMachine.currentState != player.attackState){
            stateMachine.ChangeState(player.attackState);
            return;
        }
    }

    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
    
    public virtual void AnimationFinishTrigger()
    {
        // Debug.Log("Attack finish trigger");
        triggerCalled = true;
    }
}
