using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : ControllerState
{

    private int jumpLayer;
    private int playerLayer;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 moveDir;

    private float curDist;
    private float jumpDistance;
    private float jumpHeight;
    private float moveSpeed;

    private float jumpDuration;
    private float vy;
    private float gravity;


    public JumpState(Controller controller, Rigidbody rb, Animator anim, StateMachine stateMachine)
        : base(controller, rb, anim, stateMachine)
    {
        jumpLayer = controller.jumpLayer;
        playerLayer = controller.playerLayer;

        jumpDistance = controller.jumpDistance;
        jumpHeight = controller.jumpHeight;
        moveSpeed = controller.moveSpeed;
    }

    public override void Enter()
    {
        base.Enter();

        controller.isJump = true;
        if (controller.controllerType == ControllerType.PLAYER)
            animator.SetBool("IsJump", true);
        controller.ChangeCollisionLayer(jumpLayer);

        // �ʱ� ��ġ ����
        startPos = controller.transform.position;
        moveDir = controller.GetMoveDir();
        endPos = startPos + moveDir * jumpDistance;

        curDist = 0f;
        jumpDuration = jumpDistance / moveSpeed;

        // ������ ���� ��� �ʱ� vy, �߷� ���
        vy = (4f * jumpHeight) / jumpDuration;
        gravity = (8f * jumpHeight) / (jumpDuration * jumpDuration);

        SoundManager.instance.Play("JumpSound");
    }

    public override void Update()
    {
        base.Update();

        float dt = Time.fixedDeltaTime;

        // �̵� �Ÿ�, �ӵ� ������Ʈ
        curDist += moveSpeed * dt;
        vy -= gravity * dt;

        Vector3 velocity = moveDir * moveSpeed + Vector3.up * vy;
        controller.SetVelocity(velocity);

        // ���� ����
        if (curDist >= jumpDistance)
        {
            Vector3 landPos = startPos + moveDir * jumpDistance;
            landPos.y = startPos.y;

            controller.transform.position = landPos;
            controller.SetVelocity(Vector3.zero);

            //controller.curCommand = Command.MOVE;
            stateMachine.ChangeState(controller.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        controller.isJump = false;
        if (controller.controllerType == ControllerType.PLAYER) 
            animator.SetBool("IsJump", false);
        controller.ChangeCollisionLayer(playerLayer);
    }

    public void RecoverMoveDir()
    {
        if (controller.moveDir < 0 && !controller.isBump)
        {
            // ���� �� �Ÿ�, �ӵ� ����
            (startPos, endPos) = (endPos, startPos); // swap
            curDist = jumpDistance - curDist;
            controller.ToggleMoveDir();              // ���� moveDir �÷��� ����
            moveDir = controller.GetMoveDir();       // ���� ���� ������Ʈ
            vy *= -1;                                // ���� �ӵ� ����
        }
    }
}
