using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{
    public PlayerAttackState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (player.weapon != null && player.weapon.animName != null)
        {
            player.weaponAnim.SetBool(player.weapon.animName, true);
            float yInput = player.overlayAnim.GetFloat("yInput");
            if (yInput <= -1f)
            {
                player.overlayAnim.SetBool(base.animBoolName, true);
            }
        }
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        if (player.weapon != null && player.weapon.animName != null)
        {
            player.weaponAnim.SetBool(player.weapon.animName, false);
            player.overlayAnim.SetBool(base.animBoolName, false);
        }
        base.Exit();
    }
}
