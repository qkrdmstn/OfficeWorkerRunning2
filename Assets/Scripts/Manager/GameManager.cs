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
    OVER,
    REPLAY,
    REVIVE,
}

public enum SceneName
{
    Main,
    Stage
}

public class GameManager : MonoBehaviour
{
    [Header("Stage info")]
    public int stageIndex;
    public int numOfStage = 30;

    [Header("State info")]
    public bool[] gameStateFlag = new bool[5];

    [Header("Revive info")]
    public float reviveDelay = 0.5f;

    public static GameManager instance;
    public event Action OnGameClear;
    public event Action OnGameOver;
    public event Action OnPause;
    public event Action OnResume;
    public event Action OnRevive;
    public event Action OnDelayRevive;

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
        DataManager.instance.ClearData();
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

        SoundManager.instance.Stop(SoundType.TIMER);
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

        if (!gameStateFlag[(int)GameState.REPLAY] && !gameStateFlag[(int)GameState.REVIVE])
            DataManager.instance.SaveReplayData();
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

        DataManager.instance.ClearData();
    }

    public void Replay()
    {
        if (DataManager.instance.LoadReplayData())
        {
            LoadScene(SceneName.Stage);
            gameStateFlag[(int)GameState.REPLAY] = true;
        }
    }

    public void Revive()
    {
        Time.timeScale = 1.0f;
        gameStateFlag[(int)GameState.REVIVE] = true;
        gameStateFlag[(int)GameState.OVER] = false;
        OnRevive.Invoke();

        StartCoroutine(DelayRevive());
    }

    private IEnumerator DelayRevive()
    {
        yield return new WaitForSecondsRealtime(reviveDelay); // 애니메이션 시간만큼 대기
        Time.timeScale = 0f;
        OnDelayRevive.Invoke(); // 이벤트 호출
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
        return !(gameStateFlag[(int)GameState.READY] || (gameStateFlag[(int)GameState.PAUSE]) || (gameStateFlag[(int)GameState.OVER]) || (gameStateFlag[(int)GameState.CLEAR]));
    }

    public void InitGameState()
    {
        for(int i=0; i<=(int)GameState.REVIVE; i++)
            gameStateFlag[i] = false;
    }
}
