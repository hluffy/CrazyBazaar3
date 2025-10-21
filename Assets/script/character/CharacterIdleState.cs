using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdleState : CharacterState
{
    private float enterTime;
    public CharacterIdleState(CharacterStateMachine _stateMachine, NpcController _npc, string _animBoolName) : base(_stateMachine, _npc, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        enterTime = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (npc.attackTarget != null)
        {
            stateMachine.ChangeState(npc.walkState);
            return;
        }
        else if ((Time.time - enterTime) > npc.idelWaitTimeout)
        {
            stateMachine.ChangeState(npc.walkState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
        enterTime = 0;
    }
}
