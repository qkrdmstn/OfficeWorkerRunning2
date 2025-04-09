using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeyCountView : MonoBehaviour
{
    public TextMeshProUGUI keyCountText;

    public void UpdateKeyCountText(int remainingKeyCnt)
    {
        keyCountText.text = remainingKeyCnt.ToString();
    }
}
