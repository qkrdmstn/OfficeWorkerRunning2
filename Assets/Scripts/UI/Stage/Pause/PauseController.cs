using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnPause += ActivePauseUI;
        GameManager.instance.OnResume += InActivePauseUI;
    }

    private void ActivePauseUI()
    {
        pausePanel.SetActive(true);
    }

    private void InActivePauseUI()
    {
        pausePanel.SetActive(false);
    }

    public void OnPauseButtonClicked()
    {
        GameManager.instance.Pause(); // �Ǵ� �̱���/Find�� ����
    }

    public void OnResumeButtonClicked()
    {
        GameManager.instance.Resume();
    }

    public void OnExitButtonClicked()
    {
        GameManager.instance.Exit();
    }

    void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnPause -= ActivePauseUI;
            GameManager.instance.OnResume -= InActivePauseUI;
        }
    }
}
