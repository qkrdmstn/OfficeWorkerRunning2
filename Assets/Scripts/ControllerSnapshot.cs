using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType { Move, Rotate, Jump }

public class ControllerSnapshot
{
    public int frame;

    [Header("Move info")]
    public int facingDir;
    public float moveSpeed;
    public float rotateSpeed;
    public Vector3 position;
    public Quaternion rotation;

    [Header("State boolean")]
    public bool isJump;
    public bool isRotate;
    public CommandType curCommand;
    public StateType currentStateType;

    public ControllerSnapshot(int frame, Controller controller)
    {
        this.frame = frame;
        facingDir = controller.facingDir;
        moveSpeed = controller.moveSpeed;
        rotateSpeed = controller.rotateSpeed;
        position = controller.gameObject.transform.position;
        rotation = controller.gameObject.transform.rotation;

        isJump = controller.isJump;
        isRotate = controller.isRotate;
        curCommand = controller.curCommand;

        if(controller.stateMachine.currentState == controller.moveState)
            currentStateType = StateType.Move;
        else if(controller.stateMachine.currentState == controller.rotateState)
            currentStateType = StateType.Rotate;
        else if (controller.stateMachine.currentState == controller.jumpState)
            currentStateType = StateType.Jump;
    }

    public void PasteToController(Controller controller)
    {
        controller.facingDir = facingDir;
        controller.moveSpeed = moveSpeed;
        controller.rotateSpeed = rotateSpeed;
        controller.gameObject.transform.position = position;
        controller.gameObject.transform.rotation = rotation;

        controller.isJump = isJump;
        controller.isRotate = isRotate;
        controller.curCommand = curCommand;

        // PasteToController
        switch (currentStateType)
        {
            case StateType.Move:
                controller.stateMachine.Initialize(controller.moveState);
                break;
            case StateType.Rotate:
                controller.stateMachine.Initialize(controller.rotateState);
                break;
            case StateType.Jump:
                controller.stateMachine.Initialize(controller.jumpState);
                break;
        }
    }
}

