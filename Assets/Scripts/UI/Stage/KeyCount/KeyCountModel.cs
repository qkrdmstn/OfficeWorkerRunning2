using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCountModel
{
    private int remainingKey;

    public void UpdateRemainingKey()
    {
        remainingKey = StageManager.instance.totalMoney - StageManager.instance.moneyCount;
    }

    public int GetRemainingKey()
    {
        UpdateRemainingKey();
        return remainingKey;
    }
}
