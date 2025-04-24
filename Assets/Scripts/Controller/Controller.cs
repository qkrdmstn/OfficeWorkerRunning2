using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerType
{
    PLAYER,
    BOSS_AI
}

public abstract class Controller : MonoBehaviour
{
    public static Vector3[] dir = { Vector3.forward, Vector3.right, Vector3.back, Vector3.left };

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

    public Animator animator;
    public Rigidbody rb { get; private set; }
    public Command curCommand;

    public StateMachine stateMachine { get; private set; }
    public MoveState moveState { get; private set; }
    public RotateState rotateState { get; private set; }
    public JumpState jumpState { get; private set; }


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        playerLayer = LayerMask.NameToLayer("Player");
        jumpLayer = LayerMask.NameToLayer("PlayerJump");

        stateMachine = new StateMachine();
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        moveState = new MoveState(this, rb, animator, stateMachine);
        rotateState = new RotateState(this, rb, animator, stateMachine);
        jumpState = new JumpState(this, rb, animator, stateMachine);

    }

    private void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    private void FixedUpdate()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        stateMachine.currentState.Update();
    }

    public abstract State GetStateByCurrentCommand();

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
