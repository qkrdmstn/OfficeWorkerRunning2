using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameFinishType
{
    Clear,
    Over
}

public class GameFinishController : MonoBehaviour
{
    [Header("GameFinish Settings")]
    public GameFinishView view;
    private GameFinishModel model;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnGameClear += SetGameClearUI;
        GameManager.instance.OnGameOver += SetGameOverUI;
    }

    public void SetGameClearUI()
    {
        view.gameObject.SetActive(true);
        view.SetActiveGameFinishUI(GameFinishType.Clear);
    }

    public void SetGameOverUI()
    {
        view.gameObject.SetActive(true);
        view.SetActiveGameFinishUI(GameFinishType.Over);
    }

    public void OnExitBtnClicked()
    {
        GameManager.instance.Exit();
        SoundManager.instance.Play("ClickSound");
    }

    public void OnNextBtnClicked()
    {
        GameManager.instance.NextStage();
        SoundManager.instance.Play("ClickSound");
    }

    public void OnReviveClicked()
    {
        //Todo. 살아나기 구현
        SoundManager.instance.Play("ClickSound");
    }

    public void OnRestartClicked()
    {
        GameManager.instance.StageRestart();
        SoundManager.instance.Play("ClickSound");
    }

    void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameClear -= SetGameClearUI;
            GameManager.instance.OnGameOver -= SetGameOverUI;
        }
    }
}
