using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCommand : ICommand
{
    private Command command;

    public RotateCommand(Command _command)
    {
        this.command = _command;
    }

    public void Execute(Controller controller)
    {
        controller.curCommand = command;
    }

    public void Undo(Controller controller)
    {
    }
}
