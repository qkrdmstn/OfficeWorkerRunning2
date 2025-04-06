using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapModel
{
    private Transform playerTransform;
    private Dictionary<MapData, List<Vector3>> itemWorldPos = new Dictionary<MapData, List<Vector3>>();
    private MapData[,] mapData;

    public MiniMapModel(Transform player)
    {
        this.playerTransform = player;

        for(int i=(int)MapData.MONEY; i<=(int)MapData.COFFEE; i++)
        {
            itemWorldPos.Add((MapData)i, new List<Vector3>());
        }
        
        //Setting ItemPos Data
        mapData = StageManager.instance.mapData;
        for (int i = 0; i < StageManager.MAP_SIZE; i++) //Create Stage
        {
            for (int j = 0; j < StageManager.MAP_SIZE; j++)
            {
                Debug.Log(mapData[i, j] + ", pos:  " + StageManager.instance.GetGridPos(i, 0f, j));
                if (mapData[i,j] == MapData.None || mapData[i, j] == MapData.START_POS || mapData[i, j] == MapData.START_DIR)
                    continue;

                Debug.Log(mapData[i, j] + ", pos:  " + StageManager.instance.GetGridPos(i, 0f, j));
                itemWorldPos[mapData[i, j]].Add(StageManager.instance.GetGridPos(i, 0f, j));
            }
        }
    }

    public Vector3 GetPlayerPosition()
    {
        return playerTransform.position;
    }

    public List<Vector3> GetItemPositionList(MapData item)
    {
        return itemWorldPos[item];
    }
}

