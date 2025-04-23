using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUIView : MonoBehaviour
{
    public GameObject creditPage;
    public Slider [] soundSlider = new Slider[2]; //0: slider, 1: bgm

    public void SetActiveCreditPage()
    {
        creditPage.SetActive(true);
    }

    public void SetInActiveCreditPage()
    {
        creditPage.SetActive(false);
    }

    public void UpdateSliderValue(float sfxVal, float bgmVal)
    {
        soundSlider[(int)SoundType.SFX].value = sfxVal;
        soundSlider[(int)SoundType.BGM].value = bgmVal;
    }

    public float GetSliderValue(SoundType ui)
    {
        if (ui == SoundType.SFX)
            return soundSlider[(int)SoundType.SFX].value;
        else
            return soundSlider[(int)SoundType.BGM].value;
    }
}
