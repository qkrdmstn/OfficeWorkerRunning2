using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
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


    public PlayerJumpState(PlayerController controller, Rigidbody rb, StateMachine stateMachine)
        : base(controller, rb, stateMachine)
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
        controller.ChangeCollisionLayer(jumpLayer);

        // 초기 위치 설정
        startPos = controller.transform.position;
        moveDir = controller.GetMoveDir();
        endPos = startPos + moveDir * jumpDistance;

        curDist = 0f;
        jumpDuration = jumpDistance / moveSpeed;

        // 포물선 공식 기반 초기 vy, 중력 계산
        vy = (4f * jumpHeight) / jumpDuration;
        gravity = (8f * jumpHeight) / (jumpDuration * jumpDuration);
    }

    public override void Update()
    {
        base.Update();

        float dt = Time.deltaTime;

        // 이동 거리, 속도 업데이트
        curDist += moveSpeed * dt;
        vy -= gravity * dt;

        Vector3 velocity = moveDir * moveSpeed + Vector3.up * vy;
        controller.SetVelocity(velocity);

        // 착지 판정
        if (curDist >= jumpDistance)
        {
            Vector3 landPos = startPos + moveDir * jumpDistance;
            landPos.y = startPos.y;

            controller.transform.position = landPos;
            controller.SetVelocity(Vector3.zero);

            controller.curCommand = Command.MOVE;
            stateMachine.ChangeState(controller.playerMoveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        controller.isJump = false;
        controller.ChangeCollisionLayer(playerLayer);
    }

    public void RecoverMoveDir()
    {
        if (controller.moveDir < 0 && !controller.isBump)
        {
            // 방향 및 거리, 속도 반전
            (startPos, endPos) = (endPos, startPos); // swap
            curDist = jumpDistance - curDist;
            controller.ToggleMoveDir();              // 내부 moveDir 플래그 반전
            moveDir = controller.GetMoveDir();       // 실제 방향 업데이트
            vy *= -1;                                // 수직 속도 반전
        }
    }
}
