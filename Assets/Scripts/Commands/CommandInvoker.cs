using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{

    public bool isRecording;
    public bool isReplaying;
    private SortedList<float, ICommand> recordedCommands = new SortedList<float, ICommand>();
    public PlayerController player;
    private float recordingTime;
    private float replayTime;

    public void InputHadle()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ExecuteCommand(new JumpCommand());

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ExecuteCommand(new RotateCommand(Command.ROTATE_LEFT));

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ExecuteCommand(new RotateCommand(Command.ROTATE_RIGHT));

        if (Input.GetKeyDown(KeyCode.UpArrow))
            ExecuteCommand(new RecoverDirCommand());
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute(player);

        if (isRecording)
        {
            recordedCommands.Add(recordingTime, command);
            //string str = "";
            //for(int i=0; i< recordedCommands.Count; i++)
            //{
            //    str += recordedCommands.ElementAt(i).ToString() + " ";
            //}
            //Debug.Log(str);
        }
    }

    private void Update()
    {
        InputHadle();
    }

    private void FixedUpdate()
    {
        if (isRecording)
            recordingTime += Time.fixedDeltaTime;

        if (isReplaying)
        {
            replayTime += Time.fixedDeltaTime;

            if (recordedCommands.Any())
            {
                if (Mathf.Approximately(replayTime, recordedCommands.Keys[0]))
                {
                    Debug.Log($"replay Time : {isReplaying}");
                    Debug.Log($"reply command : {recordedCommands.Values[0]}");

                    recordedCommands.Values[0].Execute(player);
                    recordedCommands.RemoveAt(0);
                }
            }
            else
            {
                isReplaying = false;
            }
        }
    }
}
