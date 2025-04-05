using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommand : ICommand
{

    public void Execute(Controller controller)
    {
        if(!controller.isRotate && !controller.isJump)
            controller.stateMachine.ChangeState(controller.jumpState);
    }

    public void Undo(Controller controller)
    {
    }
}
