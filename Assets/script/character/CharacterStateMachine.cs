using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateMachine
{
    public CharacterState currentState { get; private set; }

    public void Initialize(CharacterState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(CharacterState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
