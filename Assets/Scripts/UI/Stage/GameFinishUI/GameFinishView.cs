using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinishView : MonoBehaviour
{
    public GameObject [] finishUI = new GameObject[2]; //0: clearUI, 1: overUI

    public void SetActiveGameFinishUI(GameFinishType type)
    {
        finishUI[(int)type].SetActive(true);
        finishUI[1-(int)type].SetActive(false);
    }
}
