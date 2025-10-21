using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3DStateMachine
{
    public Player3DState currentState { get; private set; }

    public void Initialize(Player3DState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }
    
    public void ChangeState(Player3DState _newState)
    {
         Debug.Log(currentState + "=>" + _newState);
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
