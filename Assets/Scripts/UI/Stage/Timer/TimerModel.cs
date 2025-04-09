using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerModel
{
    public float timeLimit { get; private set; }
    public float elapsedTime { get; private set; }

    public TimerModel(float _timeLimit)
    {
        timeLimit = _timeLimit;
        elapsedTime = 0f;
    }

    public void UpdateTime(float deltaTime)
    {
        elapsedTime += deltaTime;
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
