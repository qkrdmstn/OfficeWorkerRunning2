using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : Controller
{
    #region State


    #endregion
    [Header("Boss info")]
    [SerializeField] private bool isBoss;
    [SerializeField] private int bossFrame;
    [SerializeField] private int commandIdx;
    [SerializeField] float despawnDelay = 5.0f;
    [SerializeField] float deadDist = 0.2f;
    private Transform playerTransform;
    private PlayerController playerController;
    private Coroutine despawnCoroutine;

    public float soundPeriod;
    public float soundTimer;

    protected override void OnAwake()
    {
        base.OnAwake();
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

        if(Vector3.Distance(this.transform.position, playerTransform.position) < deadDist && GameManager.instance.IsPlaying())
            GameManager.instance.GameOver();

        soundTimer -= Time.deltaTime;
        if(soundTimer < 0)
        {
            SoundManager.instance.Play("BossSound");
            soundTimer = soundPeriod;
        }

        if (isBoss)
        {
            bossFrame++;

            if (DataManager.instance.recordedCommands.Count > 0 && DataManager.instance.recordedCommands.Count > commandIdx)
            {
                if (bossFrame == DataManager.instance.recordedCommands.Keys[commandIdx])
                {
                    DataManager.instance.recordedCommands.Values[commandIdx++].Execute(this);
                }
            }
        }
    }

    public override State GetStateByCurrentCommand()
    {
        switch (curCommand)
        {
            case CommandType.ROTATE_LEFT:
            case CommandType.ROTATE_RIGHT:
                return rotateState;
            case CommandType.JUMP:
                return jumpState;
            default:
                return moveState;
        }
    }

    public void StartChase()
    {
        //�̹� Ȱ��ȭ �Ǿ� �ִٸ� ���� �ʱ�ȭ
        if(gameObject.activeSelf)
        {
            if (despawnCoroutine != null)
                StopCoroutine(despawnCoroutine);
            isBoss = false;
        }

        //���� Ȱ��ȭ �� �ʱ�ȭ
        gameObject.SetActive(true);
        StartBossPlayback();
        despawnCoroutine = StartCoroutine(DespawnAfterDelay());
    }

    public void StartBossPlayback()
    {
        isBoss = true;
        bossFrame = DataManager.instance.recordingFrame - DataManager.instance.delayFrame;

        //sanpshot���� ����
        ControllerSnapshot snapshot = DataManager.instance.snapshots.Dequeue();
        snapshot.PasteToController(this);

        //delay �����ӿ� ���� ����� ��ɾ� ã��
        commandIdx = DataManager.instance.recordedCommands.Count;
        for (int i = 0; i < DataManager.instance.recordedCommands.Count; i++)
        {
            if (DataManager.instance.recordedCommands.Keys[i] >= bossFrame)
            {
                commandIdx = i;
                break;
            }
        }
    }

    IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(despawnDelay);
        gameObject.SetActive(false);
        isBoss = false;
    }
}
