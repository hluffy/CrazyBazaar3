using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState
{
    protected CharacterStateMachine stateMachine;
    protected NpcController npc;
    protected string animBoolName;

    public CharacterState(CharacterStateMachine _stateMachine, NpcController _npc, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.npc = _npc;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        npc.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {
        npc.anim.SetBool(animBoolName, false);
    }
}
