using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyUIController : MonoBehaviour
{
    [Header("MoneyCount Settings")]
    public ReadyUIView view;
    private ReadyUIModel model;

    void Awake()
    {
        GameManager.instance.OnResume += StartReadyUI;
        StageManager.instance.StageLoadCompleted += StartReadyUI;
    }

    public void StartReadyUI()
    {
        StartCoroutine(readyCoroutine());
    }

    private IEnumerator readyCoroutine()
    {
        Time.timeScale = 0.0f;
        view.gameObject.SetActive(true);
        view.readyUIImage.SetActive(true);
        for (int i=3; i>=1; i--)
        {
            view.UpdateReadyText(i.ToString());
            yield return new WaitForSecondsRealtime(1f);
        }

        Time.timeScale = 1.0f;
        view.readyUIImage.SetActive(false);
        view.UpdateReadyText("Start!");
        yield return new WaitForSecondsRealtime(0.5f);
        view.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        GameManager.instance.OnResume -= StartReadyUI;
    }
}
