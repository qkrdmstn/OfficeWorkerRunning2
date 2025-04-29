using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerModel
{
    public float timeLimit { get; private set; }
    public float elapsedTime { get; private set; }
    private bool flag = false;

    public TimerModel(float _timeLimit)
    {
        timeLimit = _timeLimit;
        elapsedTime = 0f;
        flag = false;
    }

    public void UpdateTime(float deltaTime)
    {
        elapsedTime += deltaTime;
        if (elapsedTime >= timeLimit - 8.856f && !flag)
        {
            SoundManager.instance.Play("Timer8secSound");
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
