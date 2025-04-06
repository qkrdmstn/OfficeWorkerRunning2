using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MiniMapController : MonoBehaviour
{
    [Header("MiniMap Settings")]
    public MiniMapView view;
    public Transform playerTransform;
    public RectTransform miniMapRect;
    private MiniMapModel model;

    void Start()
    {
        StartCoroutine(OnStart());
    }

    IEnumerator OnStart()
    {
        //StageMager의 맵 로드를 기다림
        yield return new WaitUntil(()=>StageManager.instance != null &&  StageManager.instance.isStageReady); 

        model = new MiniMapModel(playerTransform);
        view.SetWorldToMapScale(miniMapRect.rect.width / (StageManager.gridDist * (StageManager.MAP_SIZE + 1)));

        //view에서 각 아이템에 대한 미니맵 아이콘 생성
        for (int i = (int)MapData.MONEY; i <= (int)MapData.COFFEE; i++)
            view.GenerateIcons((MapData)i, model.GetItemPositionList((MapData)i));
    }

    void Update()
    {
        if (model == null || view == null)
            return;
        
        // 플레이어 아이콘 위치 업데이트
        Vector3 playerPos = model.GetPlayerPosition();
        view.UpdatePlayerIcon(playerPos);
    }


}

