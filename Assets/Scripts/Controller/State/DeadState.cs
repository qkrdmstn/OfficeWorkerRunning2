using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DeadState : ControllerState
{
    public DeadState(Controller _controller, Rigidbody _rb, Animator _anim, StateMachine _stateMachine) : base(_controller, _rb, _anim, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.SetVelocity(0.0f);

        PlayerController player = controller as PlayerController;
        if (player != null) player.animDelay = 3.1f;
        if(controller.controllerType == ControllerType.PLAYER)
            animator.SetBool("IsGameOver", true);

        SoundManager.instance.Play("DeadSound");
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
