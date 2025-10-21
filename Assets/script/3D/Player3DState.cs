using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DState
{
    protected Player3DStateMachine stateMachine;
    protected Player3D player;
    protected string animBoolName;

    public Player3DState(Player3DStateMachine _stateMachine,Player3D _player,string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }


    public virtual void Enter()
    {
        player.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {
        
    }
    
    public virtual void Exit()
    {
        player.anim.SetBool(animBoolName, false);
    }
}
