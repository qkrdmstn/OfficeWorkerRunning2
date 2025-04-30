using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum CommandType
{
    MOVE,
    ROTATE_LEFT,
    ROTATE_RIGHT,
    JUMP,
    RECOVER_DIR,
}

public class PlayerController : Controller
{
    #region State
    public DeadState deadState { get; private set; }
    public VictoryState victoryState { get; private set; }
    #endregion 

    public float animDelay = 0.0f;

    protected override void OnAwake()
    {
        base.OnAwake();
        deadState = new DeadState(this, rb, animator, stateMachine);
        victoryState = new VictoryState(this, rb, animator, stateMachine);
    }

    protected override void OnStart()
    {
        base.OnStart();
        stateMachine.Initialize(moveState);
        curCommand = CommandType.MOVE;

        moveSpeed += GameManager.instance.stageIndex * 0.2f;
        rotateSpeed += GameManager.instance.stageIndex * 1.0f;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override State GetStateByCurrentCommand()
    {
        switch (curCommand)
        {
            case CommandType.ROTATE_LEFT:
            case CommandType.ROTATE_RIGHT:
                return rotateState;
            case CommandType.JUMP:
                return jumpState;
            default:
                return moveState;
        }
    }

    public void GameEnd(bool isClear)
    {
        if (isClear)
            stateMachine.ChangeState(victoryState);
        else
            stateMachine.ChangeState(deadState);
    }
}
