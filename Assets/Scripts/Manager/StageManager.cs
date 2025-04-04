using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.Rendering.VolumeComponent;

public enum MapData
{
    MONEY = 1,
    FILE_STACK,
    MAIL,
    POSTIT,
    COFFEE,
    START_POS,
    START_DIR,
    DEAD_MONEY,
}

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

    [Header("Map info")]
    public const int MAP_SIZE = 29;
    public const float gridDist = 2.0f;
    public MapData[,] mapData = new MapData[MAP_SIZE, MAP_SIZE];
    public Dictionary<Vector2, GameObject> moneyDictionary = new Dictionary<Vector2, GameObject>();

    [Header("Stage info")]
    [SerializeField]
    private int stageIndex;
    private int timeLimit;
    public int totalMoney;
    public int moneyCount;
    public Transform player;

    [Header("Item Info")]
    public GameObject[] itemPrefabs;
    public GameObject[] itemParents;
    public float itemYPos;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); // 중복 방지
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        stageIndex = GameManager.instance.stageIndex;
        LoadStageData();
    }

    private void LoadStageData()
    {
        //Stage Load
        if (stageIndex <= 0)
            return;
        TextAsset stageData = Resources.Load<TextAsset>("Stage/Stage" + stageIndex); //Load Stage.text

        string[] lines = stageData.text.Split('\n');

        for (int i = 0; i < MAP_SIZE; i++) //Split stage data
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                int data = int.Parse(lines[i][j].ToString());
                mapData[i, j] = (MapData)data;
            }
        }
        timeLimit = int.Parse(lines[29]); //Last line is TimeLimit

        GenrateStage();
    }

    private void GenrateStage()
    {
        for (int i = 0; i < MAP_SIZE; i++) //Create Stage
        {
            for (int j = 0; j < MAP_SIZE; j++)
            {
                //Todo Item마다 높이 다르게
                if (mapData[i, j] >= MapData.MONEY && mapData[i, j] <= MapData.COFFEE)
                {
                    GameObject item = Instantiate(itemPrefabs[(int)mapData[i, j] - 1], GetGridPos(i, itemYPos, j), Quaternion.identity);
                    item.name = "(" + i + "," + j + ")";
                    item.transform.parent = itemParents[(int)mapData[i, j] - 1].transform;

                    if (mapData[i,j] == MapData.MONEY)
                        moneyDictionary.Add(new Vector2Int(i, j), item);
                }
                else if (mapData[i, j] == MapData.START_POS)
                {
                    //플레이어 시작 위치 설정
                    Vector3 playerPos = GetGridPos(i, 0.0f, j);
                    player.position = playerPos;

                    //플레이어 시작 방향 설정
                    SetStartDir(i, j);
                }
            }
        }
    }

    private void SetStartDir(int i, int j)
    {
        //플레이어 시작 방향을 알려주는 MAP_DATA는 항상 플레이어의 상하좌우에 존재
        int[] dx = new int[4] { 0, 1, 0, -1 };
        int[] dy = new int[4] { 1, 0, -1, 0 };
        for (int dir = 0; dir < 4; dir++)
        {
            int nx = i + dx[dir];
            int ny = j + dy[dir];
            if (mapData[nx, ny] == MapData.START_DIR)
            {
                Vector3 playerDir = GetGridPos(nx, 0.0f, ny);
                player.LookAt(playerDir);
                player.GetComponent<PlayerController>().facingDir = dir;
                return;
            }
        }
        Debug.LogWarning("Player Direction Not Found!");
        return;
    }


    public float GetGridDist()
    {
        return gridDist;
    }

    public Vector3 GetGridPos(int x, float y, int z)
    {
        return new Vector3(x * gridDist + gridDist / 2, y, z * gridDist + gridDist / 2);
    }

    public Vector2Int GetGridIndex(Vector3 pos)
    {
        int xIndex = Mathf.FloorToInt((pos.x - gridDist / 2f) / gridDist);
        int zIndex = Mathf.FloorToInt((pos.z - gridDist / 2f) / gridDist);
        return new Vector2Int(xIndex, zIndex);
    }

    public void GetMoney(Vector2Int pos)
    {
        mapData[pos.x, pos.y] = MapData.DEAD_MONEY;
        Destroy(moneyDictionary[pos]);
        moneyCount++;
    }

    public void GetSurroundedMoney(List<Vector2Int> posList)
    {
        foreach (Vector2Int pos in posList)
        {
            mapData[pos.x, pos.y] = MapData.DEAD_MONEY;
            Destroy(moneyDictionary[pos]);
            moneyCount++;
        }
    }
}
