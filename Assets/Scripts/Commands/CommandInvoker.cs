using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CommandInvoker : MonoBehaviour
{
    public Controller[] controller;

    [Header("Replaying info")]
    [SerializeField] private int replayFrame;
    [SerializeField] private int replayIdx;

    private void Update()
    {
        InputHadle();
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.IsPlaying())
            return;

        if(GameManager.instance.gameStateFlag[(int)GameState.REPLAY])
        {
            replayFrame++;

            if (DataManager.instance.recordedCommands.Count > 0 && DataManager.instance.recordedCommands.Count > replayIdx)
            {
                if (replayFrame == DataManager.instance.recordedCommands.Keys[replayIdx])
                {
                    //Debug.Log($"replay Time : {replayFrame}");
                    //Debug.Log($"reply command : {DataManager.instance.recordedCommands.Values[replayIdx]}");

                    DataManager.instance.recordedCommands.Values[replayIdx++].Execute(controller[(int)ControllerType.PLAYER]);
                    //recordedCommands.RemoveAt(0);
                }
            }
            //else
            //{
            //    isReplaying = false;
            //}
        }
    }

    public void InputHadle()
    {
        if (GameManager.instance.IsPlaying() && !GameManager.instance.gameStateFlag[(int)GameState.REPLAY])
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ExecuteCommand(new JumpCommand());

            if (Input.GetKeyDown(KeyCode.LeftArrow))
                ExecuteCommand(new RotateCommand(CommandType.ROTATE_LEFT));

            if (Input.GetKeyDown(KeyCode.RightArrow))
                ExecuteCommand(new RotateCommand(CommandType.ROTATE_RIGHT));

            if (Input.GetKeyDown(KeyCode.UpArrow))
                ExecuteCommand(new RecoverDirCommand());
        }
    }

    public void ExecuteCommand(Command command)
    {
        command.Execute(controller[(int)ControllerType.PLAYER]);

        if (!GameManager.instance.gameStateFlag[(int)GameState.REPLAY])
            DataManager.instance.SaveExcuteCommand(command);
    }

    public void OnJumpButtonPressed()
    {
        if (GameManager.instance.IsPlaying() && !GameManager.instance.gameStateFlag[(int)GameState.REPLAY])
            ExecuteCommand(new JumpCommand());
    }

    public void OnLeftRotateButtonPressed()
    {
        if (GameManager.instance.IsPlaying() && !GameManager.instance.gameStateFlag[(int)GameState.REPLAY])
            ExecuteCommand(new RotateCommand(CommandType.ROTATE_LEFT));
    }

    public void OnRightRotateButtonPressed()
    {
        if (GameManager.instance.IsPlaying() && !GameManager.instance.gameStateFlag[(int)GameState.REPLAY])
            ExecuteCommand(new RotateCommand(CommandType.ROTATE_RIGHT));
    }

    public void OnMoveButtonPressed()
    {
        if (GameManager.instance.IsPlaying() && !GameManager.instance.gameStateFlag[(int)GameState.REPLAY])
            ExecuteCommand(new RecoverDirCommand());
    }
}
