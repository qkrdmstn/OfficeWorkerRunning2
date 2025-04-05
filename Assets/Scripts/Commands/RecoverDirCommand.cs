using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverDirCommand : ICommand
{
    public void Execute(Controller controller)
    {
        if (controller.isJump)
            controller.jumpState.RecoverMoveDir();
        else if (!controller.isRotate)
            controller.moveState.RecoverMoveDir();
    }

    public void Undo(Controller controller)
    {
    }
}
