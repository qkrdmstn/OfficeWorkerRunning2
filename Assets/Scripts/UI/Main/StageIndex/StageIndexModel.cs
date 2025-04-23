using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIndexModel
{
    private int stageIndex;
    private int numOfStage;

    public StageIndexModel(int defaultStageIndex = 1, int maxStageCount = 30)
    {
        stageIndex = defaultStageIndex;
        numOfStage = maxStageCount;
    }
    public int StageIndex => stageIndex;
    public int NumOfStage => numOfStage;

    public void StageIdxChange(bool up)
    {
        if (up)
        {
            if (stageIndex < numOfStage)
                stageIndex++;
        }
        else
        {
            if (stageIndex > 1)
                stageIndex--;
        }
    }

}
