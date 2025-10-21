using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackState : CharacterState
{
    public CharacterAttackState(CharacterStateMachine _stateMachine, NpcController _npc, string _animBoolName) : base(_stateMachine, _npc, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        stateMachine.ChangeState(npc.idleState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
