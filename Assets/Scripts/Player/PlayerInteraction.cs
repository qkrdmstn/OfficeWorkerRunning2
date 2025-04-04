using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Unity.Mathematics;
using static UnityEditor.PlayerSettings;

public class PlayerInteraction : MonoBehaviour
{
    private MapData[,] mapData;
    private int[] dx = { -1, -1, 0, 1, 1, 1, 0, -1 };
    private int[] dy = { 0, 1, 1, 1, 0, -1, -1, -1 };
    bool findEmpty = false; //Empty is Goal

    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        mapData = StageManager.instance.mapData;
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
                BFS(x, y);
            }
        }
        else if (collision.CompareTag("PostIt"))
        {
            player.isBump = true;
            player.ToggleMoveDir();
            player.stateMachine.ChangeState(player.playerMoveState);
        }
        else if (collision.CompareTag("FileStack"))
        {
            player.stateMachine.ChangeState(player.playerDeadState);
        }
        else if (collision.CompareTag("Mail"))
        {

        }
        else if (collision.CompareTag("Coffee"))
        {

        }
    }

    private bool OutOfMap(int x, int y, int size)
    {
        return (x < 0 || x >= size || y < 0 || y >= size);
    }

    private void BFS(int x, int y)
    {
        const int MAP_SIZE = StageManager.MAP_SIZE;
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        bool[,] visited = new bool[MAP_SIZE, MAP_SIZE];

        //탐색 시작 노드 queue에 삽입
        for (int dir = 0; dir < 8; dir++)
        {
            int nx = x + dx[dir];
            int ny = y + dy[dir];

            if (OutOfMap(nx, ny, MAP_SIZE))
                continue;
            if (mapData[nx,ny] == MapData.MONEY)
            {
                queue.Enqueue(new Vector2Int(nx,ny));
                visited[nx, ny] = true;
            }
        }
        if (queue.Count == 0)
            return;

        bool surrounded = true;
        List<Vector2Int> posList = new List<Vector2Int>();
        while (queue.Count > 0 && surrounded)
        {
            Vector2Int pos = queue.Dequeue();
            posList.Add(pos);

            for (int dir = 0; dir < 8; dir++)
            {
                int nx = pos.x + dx[dir];
                int ny = pos.y + dy[dir];

                //맵 밖으로 나가도 실패
                if (OutOfMap(nx, ny, MAP_SIZE))
                {
                    surrounded = false;
                    continue;
                }
                if (visited[nx, ny])
                    continue;

                if (mapData[nx, ny] == MapData.MONEY)
                {
                    queue.Enqueue(new Vector2Int(nx, ny));
                    visited[nx, ny] = true;
                }
                //돈이 아닌 곳을 만나면 실패
                else if (mapData[nx, ny] != MapData.DEAD_MONEY)
                    surrounded = false;
            }
        }

        if (surrounded)
            StageManager.instance.GetSurroundedMoney(posList);
    }
}