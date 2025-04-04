using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : State
{
    public static PlayerController controller;
    public static Rigidbody rb;
    public static StateMachine stateMachine;

    public PlayerState(PlayerController _controller, Rigidbody _rb, StateMachine _stateMachine)
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
