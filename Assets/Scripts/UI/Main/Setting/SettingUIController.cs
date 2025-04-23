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
        //Todo. SoundManager�� ���� �������� �� �ʱ�ȭ

        //�� ��ȯ ��, SoundManager -> model -> view���� ������ ����
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
