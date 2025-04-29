using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.Mathematics;
using static UnityEditor.PlayerSettings;
using UnityEditor.Search;

public class PlayerInteraction : MonoBehaviour
{
    private MapData[,] mapData;
    private int[] dx = { -1, -1, 0, 1, 1, 1, 0, -1 };
    private int[] dy = { 0, 1, 1, 1, 0, -1, -1, -1 };

    [SerializeField] private PlayerController player;
    [SerializeField] private BossController boss;
    [SerializeField] private CoffeeEffectManager coffeeEffectManager;

    [Header("Particles")]
    [SerializeField] private GameObject getMoneybyRuleEffect;
    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        mapData = StageManager.instance.mapData;
        ps = getMoneybyRuleEffect.GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Money"))
        {
            int x, y; //Keyname split
            string keyName = collision.name;
            keyName = keyName.Trim('(', ')');
            string[] parts = keyName.Split(',');
            x = int.Parse(parts[0]);
            y = int.Parse(parts[1]);

            if (mapData[x, y] == MapData.MONEY)
            {
                StageManager.instance.GetMoney(new Vector2Int(x, y));
                SoundManager.instance.Play("MoneySound");

                AroundSearch(x, y);
            }
        }
        else if (collision.CompareTag("PostIt"))
        {
            player.isBump = true;
            player.ToggleMoveDir();
            player.stateMachine.ChangeState(player.moveState);
            SoundManager.instance.Play("PostITSound");
        }
        else if (collision.CompareTag("FileStack"))
        {
            GameManager.instance.GameOver();
            SoundManager.instance.Play("FileStackSound");
        }
        else if (collision.CompareTag("Mail"))
        {
            boss.StartChase();
            SoundManager.instance.Play("MailSound");
        }
        else if (collision.CompareTag("Coffee"))
        {
            coffeeEffectManager.TriggerEffect();
            SoundManager.instance.Play("CoffeeSound");
        }
    }

    private bool OutOfMap(int x, int y, int size)
    {
        return (x < 0 || x >= size || y < 0 || y >= size);
    }

    private void AroundSearch(int x, int y)
    {
        const int MAP_SIZE = StageManager.MAP_SIZE;
        for (int dir = 0; dir < 8; dir++)
        {
            int nx = x + dx[dir];
            int ny = y + dy[dir];

            if (OutOfMap(nx, ny, MAP_SIZE))
                continue;
            if (mapData[nx, ny] == MapData.MONEY)
            {
                if(BFS(nx, ny))
                    break;
            }
        }
    }

    private bool BFS(int x, int y)
    {
        const int MAP_SIZE = StageManager.MAP_SIZE;
        bool[,] visited = new bool[MAP_SIZE, MAP_SIZE];
        
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(new Vector2Int(x, y));
        visited[x, y] = true;

        List<Vector2Int> posList = new List<Vector2Int>();
        while (queue.Count > 0)
        {
            Vector2Int pos = queue.Dequeue();
            posList.Add(pos);
            for (int dir = 0; dir < 8; dir++)
            {
                int nx = pos.x + dx[dir];
                int ny = pos.y + dy[dir];

                //맵 밖으로 나가도 실패
                if (OutOfMap(nx, ny, MAP_SIZE))
                    return false;
                //돈이 아닌 곳을 만나면 실패
                if (mapData[nx, ny] != MapData.MONEY && mapData[nx, ny] != MapData.DEAD_MONEY)
                    return false;
                if (visited[nx, ny])
                    continue;

                if (mapData[nx, ny] == MapData.MONEY)
                {
                    queue.Enqueue(new Vector2Int(nx, ny));
                    visited[nx, ny] = true;
                }
            }
        }

        if(posList.Count != 0)
        {
            PlayGetMoneybyRuleParticles(posList);
            SoundManager.instance.Play("MoneyByRuleSound");
        }
        foreach (Vector2Int pos in posList)
            StageManager.instance.GetMoney(pos);
        return true;
    }

    void PlayGetMoneybyRuleParticles(List<Vector2Int> posList)
    {
        var bursts = ps.emission.GetBurst(0);
        bursts.count = posList.Count * 5;
        ps.emission.SetBurst(0, bursts);
        getMoneybyRuleEffect.SetActive(true);
    }
}