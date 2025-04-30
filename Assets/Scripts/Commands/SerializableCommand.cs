using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableCommand 
{
    public int frame;
    public string commandType;
}

[System.Serializable]
public class CommandWrapper
{
    public List<SerializableCommand> commands;
}