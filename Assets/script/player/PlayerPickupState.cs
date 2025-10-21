using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupState : PlayerState
{
    public PlayerPickupState(PlayerStateMachine _stateMachine, Player _player, string _animBoolName) : base(_stateMachine, _player, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (player.target != null)
        {
            if (player.target.TryGetComponent<CropBehaviour>(out CropBehaviour crop))
            {
                if (crop.owned != null && crop.owned.TryGetComponent<NpcController>(out NpcController npc))
                {
                    npc.SetAttackTarget(player.gameObject);
                }
            }
        }
    }

    public override void Update()
    {
        base.Update();

        // 由于没有pickup动画，所以直接进入idel状态
        // stateMachine.ChangeState(player.idleState);

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
