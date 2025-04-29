using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingUIController : MonoBehaviour
{
    [Header("TutorialUI Settings")]
    public SettingUIView view;
    private SettingUIModel model;

    // Start is called before the first frame update
    void Start()
    {
        //Todo. SoundManager의 현재 볼륨으로 모델 초기화
        model = new SettingUIModel(SoundManager.instance.bgmVolume, SoundManager.instance.sfxVolume);

        //씬 전환 시, SoundManager -> model -> view로의 데이터 변경
        view.UpdateSliderValue(model.GetSoundVal(SoundType.SFX), model.GetSoundVal(SoundType.BGM));
    }

    public void OnSettingBtnClicked()
    {
        view.gameObject.SetActive(true);
        SoundManager.instance.Play("ClickSound");
    }

    public void OnCloseBtnClicked()
    {
        view.SetInActiveCreditPage();
        view.gameObject.SetActive(false);
        SoundManager.instance.Play("ClickSound");
    }

    public void OnCreditBtn()
    {
        view.SetActiveCreditPage();
        SoundManager.instance.Play("ClickSound");
    }

    public void OnCreditCloseBtn()
    {
        view.SetInActiveCreditPage();
        SoundManager.instance.Play("ClickSound");
    }

    //Slider의 값이 바뀌면, view->model로의 데이터 변경 수행
    public void OnSFXSliderValueChanged()
    {
        model.SetSoundVal(SoundType.SFX, view.GetSliderValue(SoundType.SFX));
    }

    public void OnBGMSliderValueChanged()
    {
        model.SetSoundVal(SoundType.BGM, view.GetSliderValue(SoundType.BGM));
    }

}
