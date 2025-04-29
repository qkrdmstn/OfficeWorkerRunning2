using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : Controller
{
    #region State


    #endregion
    [SerializeField] float despawnDelay = 5.0f;
    [SerializeField] float deadDist = 0.2f;
    private Transform playerTransform;
    private PlayerController playerController;
    private CommandInvoker commandInvoker;
    private Coroutine despawnCoroutine;

    public float soundPeriod;
    public float soundTimer;

    protected override void OnAwake()
    {
        base.OnAwake();
        commandInvoker = FindObjectOfType<CommandInvoker>();
        playerController = FindObjectOfType<PlayerController>();
        playerTransform = playerController.gameObject.transform;
    }

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if(Vector3.Distance(this.transform.position, playerTransform.position) < deadDist && !GameManager.instance.isGameOver)
            GameManager.instance.GameOver();

        soundTimer -= Time.deltaTime;
        if(soundTimer < 0)
        {
            SoundManager.instance.Play("BossSound");
            soundTimer = soundPeriod;
        }
    }

    public override State GetStateByCurrentCommand()
    {
        switch (curCommand)
        {
            case Command.ROTATE_LEFT:
            case Command.ROTATE_RIGHT:
                return rotateState;
            case Command.JUMP:
                return jumpState;
            default:
                return moveState;
        }
    }

    public void StartChase()
    {
        //이미 활성화 되어 있다면 보스 초기화
        if(gameObject.activeSelf)
        {
            if (despawnCoroutine != null)
                StopCoroutine(despawnCoroutine);
            commandInvoker.StopBoss();
        }

        //보스 활성화 및 초기화
        gameObject.SetActive(true);
        commandInvoker.StartBossPlayback();
        despawnCoroutine = StartCoroutine(DespawnAfterDelay());
    }

    IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(despawnDelay);
        gameObject.SetActive(false);
        commandInvoker.StopBoss();
    }
}
