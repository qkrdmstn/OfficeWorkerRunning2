using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
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
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);

    }

    private void Start()
    {
        string clipName = "MainSceneBGM";
        if (SceneManager.GetActiveScene().name == "Stage")
        {
            clipName = "StageScene_";
            if (stageIndex < 10)
                clipName += "Normal";
            else if (stageIndex < 20)
                clipName += "Hard";
            else
                clipName += "Chaos";
        }
        SoundManager.instance.Play(clipName, SoundType.BGM, true);
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

        string clipName = "MainSceneBGM";
        if (name == SceneName.Stage)
        {
            clipName = "StageScene_";
            if (stageIndex < 10)
                clipName += "Normal";
            else if (stageIndex < 20)
                clipName += "Hard";
            else 
                clipName += "Chaos";
        }
        SoundManager.instance.Play(clipName, SoundType.BGM, true);
    }

    public void GameClear()
    {
        if (isGameClear)
            return;
        isGameClear = true;
        PlayerController player = FindObjectOfType<PlayerController>();
        player.GameEnd(true);

        StartCoroutine(GameClearDelay(player.animDelay));
    }

    private IEnumerator GameClearDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // �ִϸ��̼� �ð���ŭ ���
        Time.timeScale = 0f;
        OnGameClear.Invoke(); // �̺�Ʈ ȣ��
    }

    public void GameOver()
    {
        if (isGameOver)
            return;
        isGameOver = true;
        PlayerController player = FindObjectOfType<PlayerController>();
        player.GameEnd(false);

        StartCoroutine(GameOverDelay(player.animDelay));
    }

    private IEnumerator GameOverDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // �ִϸ��̼� �ð���ŭ ���
        Time.timeScale = 0f;
        OnGameOver.Invoke(); // �̺�Ʈ ȣ��
    }


    public void Pause()
    {
        Time.timeScale = 0.0f;
        OnPause.Invoke();
    }

    public void Resume()
    {
        OnResume.Invoke();
    }

    public void Exit()
    {
        LoadScene(SceneName.Main);
    }

    public void NextStage()
    {
        if (stageIndex < numOfStage)
            stageIndex++;
        LoadScene(SceneName.Stage);
    }

    public void StageRestart()
    {
        LoadScene(SceneName.Stage);
    }
}
