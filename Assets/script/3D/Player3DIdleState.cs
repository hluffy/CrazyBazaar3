using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DIdleState : Player3DState
{
    public Player3DIdleState(Player3DStateMachine _stateMachine, Player3D _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
