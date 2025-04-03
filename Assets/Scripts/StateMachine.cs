using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State currentState { get; private set; }
    
    public StateMachine()
    {

    }

    public void Initialize(State _initState)
    {
        currentState = _initState;
        currentState.Enter();
    }

    public void ChangeState(State state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }

}
