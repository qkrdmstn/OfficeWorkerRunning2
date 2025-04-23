using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCountController : MonoBehaviour
{
    [Header("KeyCount Settings")]
    public MoneyCountView view;
    private MoneyCountModel model;

    // Start is called before the first frame update
    void Awake()
    {
        model = new MoneyCountModel();
        StageManager.instance.StageLoadCompleted += UpdateMoneyCountUI;
        StageManager.instance.OnMoneyCollected += _ => UpdateMoneyCountUI(); //���ٽ����� �Ķ���� ���� ���ֱ�
    }

    private void UpdateMoneyCountUI()
    {
        view.UpdateMoneyCountText(model.GetRemainingMoney());
    }
}
