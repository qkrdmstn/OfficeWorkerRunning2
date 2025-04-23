using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIModel 
{
    private int curPage;

    public TutorialUIModel(int curPage)
    {
        this.curPage = curPage;
    }
    public void InitPage()
    {
        curPage = 0;
    }

    public int GetCurPage()
    {
        return curPage;
    }


    public void NextPage()
    {
        curPage++;
    }

    public void PrevPage()
    {
        curPage--;
    }
}
