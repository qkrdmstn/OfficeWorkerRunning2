using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerState : State
{
    protected Controller controller;
    protected Rigidbody rb;
    protected StateMachine stateMachine;

    public ControllerState(Controller _controller, Rigidbody _rb, StateMachine _stateMachine)
    {
        controller = _controller;
        rb = _rb;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
    }
}
