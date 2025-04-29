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
        //Todo. SoundManager�� ���� �������� �� �ʱ�ȭ
        model = new SettingUIModel(SoundManager.instance.bgmVolume, SoundManager.instance.sfxVolume);

        //�� ��ȯ ��, SoundManager -> model -> view���� ������ ����
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

    //Slider�� ���� �ٲ��, view->model���� ������ ���� ����
    public void OnSFXSliderValueChanged()
    {
        model.SetSoundVal(SoundType.SFX, view.GetSliderValue(SoundType.SFX));
    }

    public void OnBGMSliderValueChanged()
    {
        model.SetSoundVal(SoundType.BGM, view.GetSliderValue(SoundType.BGM));
    }

}
