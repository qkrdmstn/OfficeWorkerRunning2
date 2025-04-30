using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : Command
{
    public JumpCommand()
    {
        command = CommandType.JUMP;
    }

    public override void Execute(Controller controller)
    {
        if(!controller.isRotate && !controller.isJump)
            controller.stateMachine.ChangeState(controller.jumpState);
    }

    public override void Undo(Controller controller)
    {
    }
}
