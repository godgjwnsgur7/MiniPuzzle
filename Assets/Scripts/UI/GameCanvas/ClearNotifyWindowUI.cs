using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ClearNotifyWindowUI : MonoBehaviour
{
    [SerializeField] Text titleText;

    public void Open(bool isGameClear)
    {
        if(isGameClear)
        {
            titleText.text = "Win!!";
        }
        else
        {
            titleText.text = "Lose..";
        }

        this.gameObject.SetActive(true);
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
