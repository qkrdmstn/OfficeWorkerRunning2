using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverDirCommand : ICommand
{
    public void Execute(PlayerController player)
    {
        if (player.isJump)
            player.playerJumpState.RecoverMoveDir();
        else if (!player.isRotate)
            player.playerMoveState.RecoverMoveDir();
    }

    public void Undo(PlayerController player)
    {
    }
}
