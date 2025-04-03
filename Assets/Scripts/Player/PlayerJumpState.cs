using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
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

    private bool isReturning = false;

    public PlayerJumpState(PlayerController controller, Rigidbody rb, StateMachine stateMachine)
        : base(controller, rb, stateMachine)
    {
        jumpDistance = controller.jumpDistance;
        jumpHeight = controller.jumpHeight;
        moveSpeed = controller.moveSpeed;
    }

    public override void Enter()
    {
        base.Enter();

        controller.isJump = true;
        isReturning = false;

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

        // 회귀 입력 처리
        if (!isReturning && controller.moveDir < 0 && Input.GetKeyDown(KeyCode.UpArrow))
        {
            isReturning = true;

            // 방향 및 거리, 속도 반전
            (startPos, endPos) = (endPos, startPos); // swap
            curDist = jumpDistance - curDist;
            controller.ToggleMoveDir();              // 내부 moveDir 플래그 반전
            moveDir = controller.GetMoveDir();       // 실제 방향 업데이트
            vy *= -1;                                // 수직 속도 반전
        }

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
        isReturning = false;
    }
}
