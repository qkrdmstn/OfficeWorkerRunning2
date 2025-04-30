using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateState : ControllerState
{
    Quaternion targetRotation;
    private int nextDir;
    public RotateState(Controller _controller, Rigidbody _rb, Animator _anim,  StateMachine _stateMachine) : base(_controller, _rb, _anim, _stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();

        controller.isRotate = true;
        if (controller.controllerType == ControllerType.PLAYER) animator.SetBool("IsMove", true);
        controller.SetVelocity(Vector3.zero);
        if (controller.curCommand == CommandType.ROTATE_LEFT)
            nextDir = controller.facingDir + 3;
        else
            nextDir = controller.facingDir + 1;
        nextDir %= 4;

        // ��ǥ ������ ���ϴ� ȸ��
        targetRotation = Quaternion.LookRotation(Controller.dir[nextDir]);
    }

    public override void Update()
    {
        base.Update();

        // ���� ���⿡��
        Quaternion currentRotation = controller.transform.rotation;

        // ���� ȸ�� ����
        controller.transform.rotation = Quaternion.RotateTowards(currentRotation, targetRotation, controller.rotateSpeed * Time.fixedDeltaTime);

        if (Quaternion.Angle(controller.transform.rotation, targetRotation) <= 0.05f)
            stateMachine.ChangeState(controller.moveState);
    }

    public override void Exit()
    {
        base.Exit();
        controller.facingDir = nextDir;
        controller.curCommand = CommandType.MOVE;
        controller.isRotate = false;
        if (controller.controllerType == ControllerType.PLAYER) animator.SetBool("IsMove", false);
    }
}
