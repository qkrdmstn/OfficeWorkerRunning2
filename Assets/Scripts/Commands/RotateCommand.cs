using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCommand : Command
{
    public RotateCommand(CommandType _command)
    {
        this.command = _command;
    }

    public override void Execute(Controller controller)
    {
        controller.curCommand = command;
    }

    public override void Undo(Controller controller)
    {
    }
}
