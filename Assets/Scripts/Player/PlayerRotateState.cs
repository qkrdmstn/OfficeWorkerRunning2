using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotateState : PlayerState
{
    Quaternion targetRotation;
    private int nextDir;
    public PlayerRotateState(PlayerController _controller, Rigidbody _rb, StateMachine _stateMachine) : base(_controller, _rb, _stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        controller.isRotate = true;
        if (controller.curCommand == Command.ROTATE_LEFT)
            nextDir = controller.facingDir + 3;
        else
            nextDir = controller.facingDir + 1;
        nextDir %= 4;

        // 목표 방향을 향하는 회전
        targetRotation = Quaternion.LookRotation(controller.dir[nextDir]);
    }

    public override void Update()
    {
        base.Update();

        // 현재 방향에서
        Quaternion currentRotation = controller.transform.rotation;

        // 실제 회전 적용
        controller.transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, controller.rotateSpeed * Time.deltaTime);

        if (Quaternion.Angle(controller.transform.rotation, targetRotation) <= 0.05f)
            stateMachine.ChangeState(controller.playerMoveState);
    }

    public override void Exit()
    {
        base.Exit();
        controller.facingDir = nextDir;
        controller.curCommand = Command.MOVE;
        controller.isRotate = false;
    }
}
