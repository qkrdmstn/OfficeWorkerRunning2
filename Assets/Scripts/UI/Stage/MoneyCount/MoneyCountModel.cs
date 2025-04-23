using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCountModel
{
    private int remainingMoney;

    public void UpdateRemainingMoney()
    {
        remainingMoney = StageManager.instance.totalMoney - StageManager.instance.moneyCount;
    }

    public int GetRemainingMoney()
    {
        UpdateRemainingMoney();
        return remainingMoney;
    }
}
