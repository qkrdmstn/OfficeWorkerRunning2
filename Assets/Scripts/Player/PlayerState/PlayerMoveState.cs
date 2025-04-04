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

        controller.SetVelocity(controller.GetMoveDir() * controller.moveSpeed);

        //��ǥ ��ġ�� �����ϸ�, ���� ��� Ȯ��
        if (Vector3.Distance(controller.transform.position, nextGridCenter) < 0.1f)
        {
            controller.isBump = false;
            controller.transform.position = nextGridCenter;
            controller.SetVelocity(Vector3.zero);
            stateMachine.ChangeState(controller.GetStateByCurrentCommand());
        }
    }

    public override void Exit()
    {
        base.Exit();

    }

    public void RecoverMoveDir()
    {
        if (!controller.isBump && controller.moveDir < 0)
        {
            controller.ToggleMoveDir();
            stateMachine.ChangeState(controller.playerMoveState);
        }
    }
}
