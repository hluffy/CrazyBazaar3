using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class PlayerWalkState : PlayerState
{
    public PlayerWalkState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        if (targetDirection.magnitude <= 0.1f)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
        else
        {
            // Quaternion targetQuaternion = Quaternion.LookRotation(targetDirection);
            // player.transform.localRotation = Quaternion.Slerp(player.transform.localRotation, targetQuaternion, Time.deltaTime * 10f);
            Debug.Log("walk info: " + player.moveSpeed * Time.deltaTime * targetDirection.normalized);
            player.rb.velocity = player.moveSpeed * Time.deltaTime * targetDirection.normalized;
            // player.cc.Move(player.moveSpeed * Time.deltaTime * targetDirection);

            // player.rb.MovePosition(player.moveSpeed * Time.deltaTime * targetDirection);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
