using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIController : MonoBehaviour
{
    [Header("TutorialUI Settings")]
    public TutorialUIView view;
    private TutorialUIModel model;

    // Start is called before the first frame update
    void Start()
    {
        model = new TutorialUIModel(0);
    }

    public void OnNextBtnClicked()
    {
        model.NextPage();
        view.UpdateTutorialView(model.GetCurPage());
    }

    public void OnPrevBtnClicked()
    {
        model.PrevPage();
        view.UpdateTutorialView(model.GetCurPage());
    }

    public void OnTutorialBtnClicked()
    {
        model.InitPage();
        view.UpdateTutorialView(model.GetCurPage());
        view.gameObject.SetActive(true);
    }

    public void OnCloseBtnClicked()
    {
        view.gameObject.SetActive(false);
    }
}
