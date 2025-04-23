using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName
{
    Main,
    Stage
}

public class GameManager : MonoBehaviour
{
    public int stageIndex;
    public int numOfStage = 30;
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

    public void StartStage()
    {
        LoadScene(SceneName.Stage);
    }

    public void LoadScene(SceneName name)
    {
        string sceneName = name.ToString();

        Time.timeScale = 1.0f;
        isGameClear = false;
        isGameOver = false;
        SceneManager.LoadScene(sceneName);
    }

    public void GameClear()
    {
        isGameClear = true;
        Time.timeScale = 0.0f;
        OnGameClear.Invoke();
    }

    public void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0.0f;
        OnGameOver.Invoke();
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        OnPause.Invoke();
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
        OnResume.Invoke();
    }

    public void Exit()
    {
        LoadScene(SceneName.Main);
    }

    public void NextStage()
    {
        if(stageIndex < numOfStage)
            stageIndex++;
        LoadScene(SceneName.Stage);
    }

    public void StageRestart()
    {
        LoadScene(SceneName.Stage);
    }
}
