using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFinishView : MonoBehaviour
{
    public GameObject [] finishUI = new GameObject[2]; //0: clearUI, 1: overUI

    [Header("GameOver Btn")]
    public GameObject reviveBtn; 
    public GameObject restartBtn;
    
    public void SetActiveGameFinishUI(GameFinishType type)
    {
        finishUI[(int)type].SetActive(true);
        finishUI[1-(int)type].SetActive(false);
        
        if(type == GameFinishType.Over)
        {
            bool flag = GameManager.instance.gameStateFlag[(int)GameState.REVIVE];
            reviveBtn.SetActive(!flag);
            restartBtn.SetActive(flag);
        }
    }

}
