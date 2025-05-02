using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerModel
{
    public float timeLimit { get; private set; }
    public float elapsedTime { get; set; }
    public bool flag = false;

    public TimerModel(float _timeLimit)
    {
        timeLimit = _timeLimit;
        elapsedTime = 0f;
        flag = false;
    }

    public void UpdateTime(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (elapsedTime > timeLimit - 8.856f && !flag)
        {
            float startTime = 8.856f - (timeLimit - elapsedTime);
            if (startTime < 0f)
                startTime = 0f;
            SoundManager.instance.PlayFromTime("Timer8secSound", startTime);
            flag = true;
        }
    }

    public float GetRemainingRatio()
    {
        return (timeLimit - elapsedTime) / timeLimit;
    }

    public bool IsTimeOver()
    {
        return elapsedTime >= timeLimit;
    }

}
