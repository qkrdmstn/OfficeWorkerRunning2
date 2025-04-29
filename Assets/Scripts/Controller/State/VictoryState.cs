using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryState : ControllerState
{
    public VictoryState(Controller _controller, Rigidbody _rb, Animator _anim, StateMachine _stateMachine) : base(_controller, _rb, _anim, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        controller.SetVelocity(0.0f);

        int rand = Random.Range(1, 3);
        PlayerController player = controller as PlayerController;
        if (player != null)
        {
            if (rand == 1)
                player.animDelay = 4.5f;
            else
                player.animDelay = 1.8f;
        }
        if (controller.controllerType == ControllerType.PLAYER) animator.SetInteger("IsVictory", rand);
        SoundManager.instance.Play("ClearSound");
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
