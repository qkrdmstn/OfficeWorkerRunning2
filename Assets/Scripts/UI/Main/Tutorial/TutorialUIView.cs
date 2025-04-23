using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIView : MonoBehaviour
{
    public GameObject[] tutorialPage = new GameObject[2];
    public GameObject[] pageBtn = new GameObject[2]; //0: nextBtn, 1: prevBtn

    public void UpdateTutorialView(int curPage)
    {
        tutorialPage[curPage].SetActive(true);
        pageBtn[curPage].SetActive(true);

        tutorialPage[1-curPage].SetActive(false);
        pageBtn[1-curPage].SetActive(false);
    }
}
