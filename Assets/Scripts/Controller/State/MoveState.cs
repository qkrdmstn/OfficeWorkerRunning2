using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : ControllerState
{
    private Vector3 nextGridCenter; 

    public MoveState(Controller _controller, Rigidbody _rb, Animator _anim, StateMachine _stateMachine) : base(_controller, _rb, _anim, _stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        animator.SetBool("IsMove", true);
        nextGridCenter = controller.GetNextGridCenter();
    }

    public override void Update()
    {
        base.Update();

        controller.SetVelocity(controller.GetMoveDir() * controller.moveSpeed);

        //목표 위치에 도달하면, 다음 명령 확인
        if (Vector3.Distance(controller.transform.position, nextGridCenter) < 0.1f)
        {
            controller.isBump = false;
            controller.transform.position = nextGridCenter;
            stateMachine.ChangeState(controller.GetStateByCurrentCommand());
        }
    }

    public override void Exit()
    {
        base.Exit();
        animator.SetBool("IsMove", false);
    }

    public void RecoverMoveDir()
    {
        if (!controller.isBump && controller.moveDir < 0)
        {
            controller.ToggleMoveDir();
            stateMachine.ChangeState(controller.moveState);
        }
    }
}
