using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIManager
{
    BaseCanvas s_CurrCanvas;

    public BaseCanvas currCanvas
    {
        get
        {
            if (s_CurrCanvas == null)
                s_CurrCanvas = GameObject.FindObjectOfType<BaseCanvas>();

            return s_CurrCanvas;
        }
        set { s_CurrCanvas = value; }
    }



    public void Init()
    {
        currCanvas = GameObject.FindObjectOfType<BaseCanvas>();
        if (currCanvas == null)
        {
            Debug.LogError($"currCanvas is Null!");
            return;
        }

        currCanvas.Init();
    }

    public void Clear()
    {
        currCanvas = null;
    }
}
