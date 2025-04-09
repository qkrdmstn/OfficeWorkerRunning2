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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActivePauseUI()
    {
        pausePanel.SetActive(true);
    }

    private void InActivePauseUI()
    {
        pausePanel.SetActive(false);
    }
}
