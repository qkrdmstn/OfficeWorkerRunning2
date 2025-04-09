using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int stageIndex;
    public bool isGameClear;
    public bool isGameOver;

    public static GameManager instance;
    public event Action OnGameClear;
    public event Action OnGameOver;
    public event Action OnPause;
    public event Action OnResume;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else if(instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameClear()
    {
        isGameClear = true;
        Debug.Log("GameClear!!!!");
        OnGameClear.Invoke();
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("GameOver!!!!");
        OnGameOver.Invoke();
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        OnPause.Invoke();

    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        OnResume.Invoke();
    }

    public void Exit()
    {

    }
}
