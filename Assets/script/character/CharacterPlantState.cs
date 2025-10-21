using System.Collections;
using System.Collections.Generic;
using Mono.Reflection;
using UnityEngine;

public class CharacterPlantState : CharacterState
{
    private float lastEnterTime;
    public CharacterPlantState(CharacterStateMachine _stateMachine, NpcController _npc, string _animBoolName) : base(_stateMachine, _npc, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        lastEnterTime = Time.time;

        npc.PlantGameObject();

    }

    public override void Update()
    {
        base.Update();

        if ((Time.time - lastEnterTime) > 3)
        {
            stateMachine.ChangeState(npc.walkState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
