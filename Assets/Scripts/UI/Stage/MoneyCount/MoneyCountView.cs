using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyCountView : MonoBehaviour
{
    public TextMeshProUGUI moneyCountText;

    public void UpdateMoneyCountText(int remainingMoneyCnt)
    {
        moneyCountText.text = remainingMoneyCnt.ToString();
    }
}
