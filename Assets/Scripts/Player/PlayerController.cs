using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum Command
{
    MOVE,
    ROTATE_LEFT,
    ROTATE_RIGHT,
    JUMP
}

public class PlayerController : MonoBehaviour
{
    public Vector3[] dir = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

    [Header("Move info")]
    public int moveDir = 1;
    public int facingDir = 0;
    public float moveSpeed;
    public float rotateSpeed;

    [Header("Jump info")]
    public int playerLayer;
    public int jumpLayer;
    public float jumpDistance;
    public float jumpHeight;

    [Header("State boolean")]
    public bool isBump;
    public bool isJump;
    public bool isRotate;

    public Command curCommand; 
    private Rigidbody rb { get; set; }

    #region State
    public StateMachine stateMachine { get; private set; }
    public PlayerMoveState playerMoveState { get; private set; }
    public PlayerRotateState playerRotateState { get; private set; }
    public PlayerJumpState playerJumpState { get; private set; }
    public PlayerDeadState playerDeadState { get; private set; }

    #endregion 

    void Awake()
    {
        rb = GetComponent<Rigidbody> ();
        playerLayer = LayerMask.NameToLayer("Player");
        jumpLayer = LayerMask.NameToLayer("PlayerJump");

        stateMachine = new StateMachine();
        playerMoveState = new PlayerMoveState(this, rb, stateMachine);
        playerRotateState = new PlayerRotateState(this, rb, stateMachine);
        playerJumpState = new PlayerJumpState(this, rb, stateMachine);
        playerDeadState = new PlayerDeadState(this, rb, stateMachine);
    }

    private void Start()
    {
        stateMachine.Initialize(playerMoveState);
        curCommand = Command.MOVE;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.currentState.Update();

        //InputControl();
    }

    void InputControl()
    {
        if (!GameManager.instance.isGameOver)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                curCommand = Command.ROTATE_LEFT;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                curCommand = Command.ROTATE_RIGHT;
            if (!isRotate && !isJump && Input.GetKeyDown(KeyCode.Space))
                stateMachine.ChangeState(playerJumpState);

            if (Input.GetKeyDown(KeyCode.UpArrow) && isJump)
                playerJumpState.RecoverMoveDir();
            else if (Input.GetKeyDown(KeyCode.UpArrow) && !isRotate)
                playerMoveState.RecoverMoveDir();
        }

    }

    public Vector3 GetNextGridCenter()
    {
        Vector3 pos = transform.position;
        Vector2Int index = StageManager.instance.GetGridIndex(pos);

        Vector3 curMoveDir = GetMoveDir();
        if (isBump)
        {
            if (curMoveDir.x < 0.0f)
                index.x++;
            if (curMoveDir.z < 0.0f)
                index.y++;
        }

        int nextX = index.x + Mathf.RoundToInt(curMoveDir.x);
        int nextZ = index.y + Mathf.RoundToInt(curMoveDir.z);
        return StageManager.instance.GetGridPos(nextX, pos.y, nextZ);
    }

    public State GetStateByCurrentCommand()
    {
        switch (curCommand)
        {
            case Command.ROTATE_LEFT:
            case Command.ROTATE_RIGHT:
                return playerRotateState;
            case Command.JUMP:
                return playerJumpState;
            default:
                return playerMoveState;
        }
    }

    public void ToggleMoveDir()
    {
        moveDir *= -1;
    }

    public Vector3 GetMoveDir()
    {
        return moveDir * dir[facingDir];
    }

    public void SetVelocity(float _speed)
    {
        rb.velocity = dir[facingDir] * _speed;
    }

    public void SetVelocity(Vector3 _velocity)
    {
        rb.velocity = _velocity;
    }

    public void ChangeCollisionLayer(int _layer)
    {
        gameObject.layer = _layer;
    }
}
