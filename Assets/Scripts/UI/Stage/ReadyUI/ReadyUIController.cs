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
        StageManager.instance.StageLoadCompleted += StartReadyUI;
        GameManager.instance.OnResume += StartReadyUI;
        GameManager.instance.OnDelayRevive += StartReadyUI;
        GameManager.instance.OnPause += StopReadyUI;
    }

    public void StartReadyUI()
    {
        StartCoroutine(readyCoroutine());
        GameManager.instance.gameStateFlag[(int)GameState.READY] = true;
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
        GameManager.instance.gameStateFlag[(int)GameState.READY] = false;

        view.UpdateReadyText("Start!");
        yield return new WaitForSecondsRealtime(0.5f);
        view.gameObject.SetActive(false);
    }

    public void StopReadyUI()
    {
        StopAllCoroutines();
        view.readyUIImage.SetActive(false);
        view.gameObject.SetActive(false);
        GameManager.instance.gameStateFlag[(int)GameState.READY] = false;
    }

    private void OnDestroy()
    {
        GameManager.instance.OnResume -= StartReadyUI;
        GameManager.instance.OnDelayRevive -= StartReadyUI;
        GameManager.instance.OnPause -= StopReadyUI;
    }
}
