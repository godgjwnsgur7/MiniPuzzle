using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : BaseCanvas
{
    [SerializeField] GameInfoUI gameInfoUI;
    [SerializeField] ClearNotifyWindowUI clearNotifyWindowUI;

    public override void Init()
    {
        base.Init();
    }

    public void Open_ClearNotifyWindow(bool isGameClear)
    {
        clearNotifyWindowUI.Open(isGameClear);
    }

    public void UpdateClearConditionCount(int count)
    {
        gameInfoUI.UpdateClearConditionCount(count);
    }

    public void UpdateMoveCount(int count)
    {
        gameInfoUI.UpdateMoveCount(count);
    }
}
