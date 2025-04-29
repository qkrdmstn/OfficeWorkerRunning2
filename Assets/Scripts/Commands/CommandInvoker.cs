using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CommandInvoker : MonoBehaviour
{
    public Controller[] controller;

    [Header("Recording info")]
    public bool isRecording;
    [SerializeField] private int recordingFrame;

    [Header("Replaying info")]
    public bool isReplaying;
    [SerializeField] private int replayFrame;
    [SerializeField] private int replayIdx;

    [Header("Boss info")]
    [SerializeField] private bool isBoss;
    [SerializeField] private int bossFrame;
    [SerializeField] private int delayFrame;
    [SerializeField] private int commandIdx;

    [SerializeField] private SortedList<int, ICommand> recordedCommands = new SortedList<int, ICommand>();
    [SerializeField] private Queue<ControllerSnapshot> snapshots = new Queue<ControllerSnapshot>();
    [SerializeField] private int snapshotInterval;

    private void Update()
    {
        InputHadle();
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.IsPlaying())
            return;

        if (isRecording)
        {
            recordingFrame++;
            SaveSnapshot();
        }

        if (isReplaying)
        {
            replayFrame++;

            if (recordedCommands.Count > 0 && recordedCommands.Count > replayIdx)
            {
                if (replayFrame == recordedCommands.Keys[replayIdx])
                {
                    Debug.Log($"replay Time : {replayFrame}");
                    Debug.Log($"reply command : {recordedCommands.Values[replayIdx]}");

                    recordedCommands.Values[replayIdx++].Execute(controller[(int)ControllerType.PLAYER]);
                    //recordedCommands.RemoveAt(0);
                }
            }
            else
            {
                isReplaying = false;
            }
        }

        if(isBoss)
        {
            bossFrame++;

            if (recordedCommands.Count > 0 && recordedCommands.Count > commandIdx)
            {
                if (bossFrame == recordedCommands.Keys[commandIdx])
                {
                    recordedCommands.Values[commandIdx++].Execute(controller[(int)ControllerType.BOSS_AI]);
                }
            }
        }
    }

    public void InputHadle()
    {
        if (GameManager.instance.IsPlaying() && !isReplaying)
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
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute(controller[(int)ControllerType.PLAYER]);

        if (isRecording)
        {
            recordedCommands.Add(recordingFrame, command);
            //string str = "";
            //for (int i = 0; i < recordedCommands.Count; i++)
            //{
            //    str += recordedCommands.ElementAt(i).ToString() + " ";
            //}
            //Debug.Log(str);
        }
    }

    public void OnJumpButtonPressed()
    {
        if (GameManager.instance.IsPlaying() && !isReplaying)
            ExecuteCommand(new JumpCommand());
    }

    public void OnLeftRotateButtonPressed()
    {
        if (GameManager.instance.IsPlaying() && !isReplaying)
            ExecuteCommand(new RotateCommand(Command.ROTATE_LEFT));
    }

    public void OnRightRotateButtonPressed()
    {
        if (GameManager.instance.IsPlaying() && !isReplaying)
            ExecuteCommand(new RotateCommand(Command.ROTATE_RIGHT));
    }

    public void OnMoveButtonPressed()
    {
        if (GameManager.instance.IsPlaying() && !isReplaying)
            ExecuteCommand(new RecoverDirCommand());
    }

    private void SaveSnapshot()
    {
        if (recordingFrame % snapshotInterval == 0)
        {
            var player = controller[(int)ControllerType.PLAYER];
            snapshots.Enqueue(new ControllerSnapshot(recordingFrame, player));

            if (snapshots.Count > delayFrame)
                snapshots.Dequeue();
        }
    }

    public void StartBossPlayback()
    {
        isBoss = true;
        bossFrame = recordingFrame - delayFrame;

        //sanpshot에서 복원
        ControllerSnapshot snapshot = snapshots.Dequeue();
        snapshot.PasteToController(controller[(int)ControllerType.BOSS_AI]);
        
        //delay 프레임에 가장 가까운 명령어 찾기
        commandIdx = recordedCommands.Count;
        for (int i = 0; i < recordedCommands.Count; i++)
        {
            if (recordedCommands.Keys[i] >= bossFrame)
            {
                commandIdx = i;
                break;
            }
        }
    }

    public void StopBoss()
    {
        isBoss = false;
    }
}
