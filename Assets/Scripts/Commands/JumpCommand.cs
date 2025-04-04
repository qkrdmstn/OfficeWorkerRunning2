using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : ICommand
{

    public void Execute(PlayerController player)
    {
        if(!player.isRotate && !player.isJump)
            player.stateMachine.ChangeState(player.playerJumpState);
    }

    public void Undo(PlayerController player)
    {
    }
}
