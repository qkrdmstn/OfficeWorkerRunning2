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
    public int moveDir = 1;
    public int facingDir = 0;
    public Vector3[] dir = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

    [Header("Move info")]
    public float moveSpeed;
    public float rotateSpeed;

    public float jumpDistance;
    public float jumpHeight;
    public float jumpDuration;

    [Header("State boolean")]
    public bool isBump;
    public bool isJump;
    public bool isRotate;
    
    public Command curCommand { get; set; }
    private Rigidbody rb { get; set; }

    #region State
    public StateMachine stateMachine { get; private set; }
    public PlayerMoveState playerMoveState { get; private set; }
    public PlayerRotateState playerRotateState { get; private set; }
    public PlayerJumpState playerJumpState { get; private set; }

    #endregion 

    void Awake()
    {
        rb = GetComponent<Rigidbody> ();

        stateMachine = new StateMachine();
        playerMoveState = new PlayerMoveState(this, rb, stateMachine);
        playerRotateState = new PlayerRotateState(this, rb, stateMachine);
        playerJumpState = new PlayerJumpState(this, rb, stateMachine);

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

        InputControl();

    }

    public void SetVelocity(float _speed)
    {
        rb.velocity = dir[facingDir] * _speed;
    }

    public void SetVelocity(Vector3 _velocity)
    {
        rb.velocity = _velocity;
    }

    void InputControl()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
            curCommand = Command.ROTATE_LEFT;
        if (Input.GetKeyDown(KeyCode.RightArrow))
            curCommand = Command.ROTATE_RIGHT;
        if (!isRotate && !isJump && Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(playerJumpState);
    }

    public State CheckCommand()
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

    public Vector3 GetNextGridCenter()
    {
        Vector3 pos = transform.position;
        float d = GameManager.instance.dist;
        Vector3 curMoveDir = GetMoveDir();

        int xIndex = Mathf.FloorToInt((pos.x - d / 2f) / d);
        int zIndex = Mathf.FloorToInt((pos.z - d / 2f) / d);
        
        if(isBump)
        {
            if (curMoveDir.x < 0.0f)
                xIndex++;
            if (curMoveDir.z < 0.0f)
                zIndex++;
        }

        int nextX = xIndex + Mathf.RoundToInt(curMoveDir.x);
        int nextZ = zIndex + Mathf.RoundToInt(curMoveDir.z);

        return new Vector3(
            d / 2f + nextX * d,
            pos.y,
            d / 2f + nextZ * d
        );
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Bumper"))
        {
            isBump = true;
            ToggleMoveDir();
            stateMachine.ChangeState(playerMoveState);
        }
    }
}
