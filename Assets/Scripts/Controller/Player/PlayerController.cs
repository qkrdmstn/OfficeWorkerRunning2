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


    #endregion 

    protected override void OnAwake()
    {
        base.OnAwake();

    }

    protected override void OnStart()
    {
        base.OnStart();
        stateMachine.Initialize(moveState);
        curCommand = Command.MOVE;
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
}
