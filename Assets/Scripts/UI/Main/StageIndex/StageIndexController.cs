using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIndexController : MonoBehaviour
{
    [Header("StageIndex Settings")]
    public StageIndexView view;
    private StageIndexModel model;

    // Start is called before the first frame update
    void Start()
    {
        model = new StageIndexModel(GameManager.instance.stageIndex, GameManager.instance.numOfStage);
        UpdateStageUI();
    }

    public void ChangeStageIdx(bool up)
    {
        model.StageIdxChange(up);
        GameManager.instance.stageIndex = model.StageIndex;
        UpdateStageUI();
    }

    private void UpdateStageUI()
    {
        view.UpdateKeyCountText(model.StageIndex, model.NumOfStage);
    }

    public void OnStartButtonClicked()
    {
        GameManager.instance.StartStage();
    }

}
