using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverDirCommand : Command
{
    public RecoverDirCommand()
    {
        command = CommandType.RECOVER_DIR;
    }

    public override void Execute(Controller controller)
    {
        if (controller.isJump)
            controller.jumpState.RecoverMoveDir();
        else if (!controller.isRotate)
            controller.moveState.RecoverMoveDir();
    }

    public override void Undo(Controller controller)
    {
    }
}
