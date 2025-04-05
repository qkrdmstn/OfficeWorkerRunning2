using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand 
{
    void Execute(Controller controller);
    void Undo(Controller controller);

}
