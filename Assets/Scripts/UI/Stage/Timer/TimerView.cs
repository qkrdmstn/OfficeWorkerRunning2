using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
    [Header("UI")]
    public Slider timeSlider;

    public void Initialize(float timeLimit)
    {
        if (timeSlider != null)
        {
            timeSlider.maxValue = 1f;
            timeSlider.value = 1f;
            timeSlider.fillRect.gameObject.SetActive(true);
        }
    }

    public void UpdateSlider(float ratio)
    {
        if (timeSlider != null)
            timeSlider.value = ratio;
        
    }

    public void OnTimeOver()
    {
        timeSlider.fillRect.gameObject.SetActive(false);
    }
}
