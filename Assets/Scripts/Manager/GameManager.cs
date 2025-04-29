using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    READY,
    PAUSE,
    CLEAR,
    OVER
}

public enum SceneName
{
    Main,
    Stage
}

public class GameManager : MonoBehaviour
{
    public int stageIndex;
    public int numOfStage = 30;
    public bool[] gameStateFlag = new bool[4];

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
        InitGameState();
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
        if (gameStateFlag[(int)GameState.CLEAR])
            return;
        gameStateFlag[(int)GameState.CLEAR] = true;
        PlayerController player = FindObjectOfType<PlayerController>();
        player.GameEnd(true);

        SoundManager.instance.Stop(SoundType.TIMER);
        SoundManager.instance.Play("ClearSound");

        StartCoroutine(GameClearDelay(player.animDelay));
    }

    private IEnumerator GameClearDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 애니메이션 시간만큼 대기
        Time.timeScale = 0f;
        OnGameClear.Invoke(); // 이벤트 호출
    }

    public void GameOver()
    {
        if (gameStateFlag[(int)GameState.OVER])
            return;
        gameStateFlag[(int)GameState.OVER] = true;
        PlayerController player = FindObjectOfType<PlayerController>();
        player.GameEnd(false);

        SoundManager.instance.Stop(SoundType.TIMER);
        SoundManager.instance.Play("DeadSound");

        StartCoroutine(GameOverDelay(player.animDelay));
    }

    private IEnumerator GameOverDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // 애니메이션 시간만큼 대기
        Time.timeScale = 0f;
        OnGameOver.Invoke(); // 이벤트 호출
    }

    public void Pause()
    {
        if (gameStateFlag[(int)GameState.OVER] || gameStateFlag[(int)GameState.CLEAR])
            return;

        Time.timeScale = 0.0f;
        SoundManager.instance.Stop(SoundType.SFX);
        SoundManager.instance.Stop(SoundType.TIMER);
        gameStateFlag[(int)GameState.PAUSE] = true;
        OnPause.Invoke();
    }

    public void Resume()
    {
        gameStateFlag[(int)GameState.PAUSE] = false;
        OnResume.Invoke();
    }

    public void Exit()
    {
        SoundManager.instance.Stop(SoundType.SFX);
        SoundManager.instance.Stop(SoundType.TIMER);
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
        SoundManager.instance.Stop(SoundType.SFX);
        SoundManager.instance.Stop(SoundType.TIMER);
        LoadScene(SceneName.Stage);
    }

    public bool IsPlaying()
    {
        foreach (GameState type in Enum.GetValues(typeof(GameState)))
        {
            if (gameStateFlag[(int)type])
                return false;
        }
        return true;
    }

    public void InitGameState()
    {
        foreach (GameState type in Enum.GetValues(typeof(GameState)))
            gameStateFlag[(int)type] = false;
    }
}
