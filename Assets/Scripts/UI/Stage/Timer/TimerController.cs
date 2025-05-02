using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class TimerController : MonoBehaviour
{
    public TimerView view;

    private TimerModel model;
    private bool isTimeRunning = false;

    void Awake()
    {
        StageManager.instance.StageLoadCompleted += InitializeTimerUI;
        GameManager.instance.OnResume += TimerResume;
        GameManager.instance.OnDelayRevive += InitializeTimerUI;
    }

    public void InitializeTimerUI()
    {
        model = new TimerModel(StageManager.instance.timeLimit);
        view.Initialize(model.timeLimit);
        isTimeRunning = true;
    }

    void Update()
    {
        if (!isTimeRunning || model == null)
            return;

        if (!GameManager.instance.IsPlaying())
            return;

        model.UpdateTime(Time.deltaTime);
        view.UpdateSlider(model.GetRemainingRatio());

        if (model.IsTimeOver())
        {
            isTimeRunning = false;
            view.OnTimeOver();
            GameManager.instance.GameOver();
            SoundManager.instance.Play("TimeOverSound");
        }
    }

    public void TimerResume()
    {
        model.flag = false;
    }

    void OnDestroy()
    {
        GameManager.instance.OnResume -= TimerResume;
        GameManager.instance.OnDelayRevive -= InitializeTimerUI;
    }
}
