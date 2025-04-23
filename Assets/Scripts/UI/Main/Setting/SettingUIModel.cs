using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIModel
{
    private float [] soundVal = new float[2];

    public SettingUIModel(float _bgmSoundVal, float _sfxSoundVal)
    {
        soundVal[(int)SoundType.SFX] = _sfxSoundVal;
        soundVal[(int)SoundType.BGM] = _bgmSoundVal;
    }

    public void SetSoundVal(SoundType ui, float _val)
    {
        soundVal[(int)ui] = _val;
        //Todo. SoundManager의 음량 조절

    }

    public float GetSoundVal(SoundType ui)
    {
        return soundVal[(int)ui];
    }
}
