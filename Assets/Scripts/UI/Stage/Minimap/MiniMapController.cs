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

    void Awake()
    {
        StageManager.instance.StageLoadCompleted += InitializeMiniMapUI;
        StageManager.instance.OnMoneyCollected += HandleMoneyCollected;
    }

    void Update()
    {
        if (model == null || view == null)
            return;
        
        // �÷��̾� ������ ��ġ ������Ʈ
        Vector3 playerPos = model.GetPlayerPosition();
        view.UpdatePlayerIcon(playerPos);
    }

    private void InitializeMiniMapUI()
    {
        model = new MiniMapModel(playerTransform);
        view.SetWorldToMapScale(miniMapRect.rect.width / (StageManager.gridDist * (StageManager.MAP_SIZE + 1)));

        //view���� �� �����ۿ� ���� �̴ϸ� ������ ����
        for (int i = (int)MapData.MONEY; i <= (int)MapData.COFFEE; i++)
            view.GenerateIcons((MapData)i, model.GetItemPositionList((MapData)i));
    }

    private void HandleMoneyCollected(Vector3 pos)
    {
        view.GenerateDeadMoneyIcon(pos);
    }

}

