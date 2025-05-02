using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayController : MonoBehaviour
{
    //[Header("Replay Settings")]
    //public ReplayView view;
    //private ReplayModel model;

    public void OnReplayButtonClicked()
    {
        GameManager.instance.Replay();
        SoundManager.instance.Play("ClickSound");
    }
}
