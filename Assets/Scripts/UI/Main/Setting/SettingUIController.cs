using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    SFX,
    BGM
}

public class SettingUIController : MonoBehaviour
{
    [Header("TutorialUI Settings")]
    public SettingUIView view;
    private SettingUIModel model;

    // Start is called before the first frame update
    void Start()
    {
        //Todo. SoundManager의 현재 볼륨으로 모델 초기화

        //씬 전환 시, SoundManager -> model -> view로의 데이터 변경
        model = new SettingUIModel(1f, 1f);
        view.UpdateSliderValue(model.GetSoundVal(SoundType.SFX), model.GetSoundVal(SoundType.BGM));
    }

    public void OnSettingBtnClicked()
    {
        view.gameObject.SetActive(true);

    }

    public void OnCloseBtnClicked()
    {
        view.SetInActiveCreditPage();
        view.gameObject.SetActive(false);
    }

    public void OnCreditBtn()
    {
        view.SetActiveCreditPage();
    }

    public void OnCreditCloseBtn()
    {
        view.SetInActiveCreditPage();
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
