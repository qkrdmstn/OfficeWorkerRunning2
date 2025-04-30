using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Command 
{
    public CommandType command;
    public abstract void Execute(Controller controller);
    public abstract void Undo(Controller controller);

}
