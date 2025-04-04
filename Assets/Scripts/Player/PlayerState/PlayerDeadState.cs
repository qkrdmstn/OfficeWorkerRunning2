using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(PlayerController _controller, Rigidbody _rb, StateMachine _stateMachine) : base(_controller, _rb, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.GameOver();
        controller.SetVelocity(0.0f);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
