using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.StopMove();
    }

    public override void Update()
    {
        base.Update();

        if (targetDirection.magnitude > 0.1f)
            {
                stateMachine.ChangeState(player.walkState);
            }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
