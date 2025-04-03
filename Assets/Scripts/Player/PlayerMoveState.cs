using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    private Vector3 nextGridCenter; 

    public PlayerMoveState(PlayerController _controller, Rigidbody _rb, StateMachine _stateMachine) : base(_controller, _rb, _stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        nextGridCenter = controller.GetNextGridCenter();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKey(KeyCode.UpArrow) && !controller.isBump && controller.moveDir < 0)
        {
            controller.ToggleMoveDir();
            stateMachine.ChangeState(controller.playerMoveState);
        }

        controller.SetVelocity(controller.GetMoveDir() * controller.moveSpeed);
        if (Vector3.Distance(controller.transform.position, nextGridCenter) < 0.1f)
        {
            controller.isBump = false;
            controller.transform.position = nextGridCenter;
            stateMachine.ChangeState(controller.CheckCommand());
        }
    }

    public override void Exit()
    {
        base.Exit();
        controller.SetVelocity(Vector3.zero);
    }
}
