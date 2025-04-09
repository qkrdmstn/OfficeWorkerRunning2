using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class TimerController : MonoBehaviour
{
    public TimerView view;

    private TimerModel model;
    private bool isTimeRunning = false;

    void Start()
    {
        StageManager.instance.StageLoadCompleted += InitializeTimerUI;
    }

    void InitializeTimerUI()
    {
        model = new TimerModel(StageManager.instance.timeLimit);
        view.Initialize(model.timeLimit);
        isTimeRunning = true;
    }

    void Update()
    {
        if (!isTimeRunning || model == null)
            return;

        model.UpdateTime(Time.deltaTime);
        view.UpdateSlider(model.GetRemainingRatio());

        if (model.IsTimeOver())
        {
            isTimeRunning = false;
            view.OnTimeOver();
            GameManager.instance.GameOver();
        }
    }
}
