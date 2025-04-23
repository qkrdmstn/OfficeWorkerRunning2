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
        StageManager.instance.OnMoneyCollected += _ => UpdateKeyCountUI(); //람다식으로 파라미터 전달 없애기
    }

    private void UpdateKeyCountUI()
    {
        view.UpdateKeyCountText(model.GetRemainingKey());
    }
}
