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
        curCommand = Command.MOVE;

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
            case Command.ROTATE_LEFT:
            case Command.ROTATE_RIGHT:
                return rotateState;
            case Command.JUMP:
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
