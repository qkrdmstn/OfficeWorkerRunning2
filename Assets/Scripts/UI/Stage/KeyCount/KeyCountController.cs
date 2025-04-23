using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCountController : MonoBehaviour
{
    [Header("KeyCount Settings")]
    public KeyCountView view;
    private KeyCountModel model;

    // Start is called before the first frame update
    void Awake()
    {
        model = new KeyCountModel();
        StageManager.instance.StageLoadCompleted += UpdateKeyCountUI;
        StageManager.instance.OnMoneyCollected += _ => UpdateKeyCountUI(); //���ٽ����� �Ķ���� ���� ���ֱ�
    }

    private void UpdateKeyCountUI()
    {
        view.UpdateKeyCountText(model.GetRemainingKey());
    }
}
