using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StageIndexView : MonoBehaviour
{
    public TextMeshProUGUI stageIndexText;

    public void UpdateKeyCountText(int curStageIdx, int numOfStage)
    {
        stageIndexText.text = curStageIdx.ToString() + "/" + numOfStage.ToString();
    }
}
