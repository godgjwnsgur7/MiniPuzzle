using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{
    public void OnClickPause()
    {
        if (Managers.Game.IsGamePaused)
            return;

        this.gameObject.SetActive(true);

        Managers.Game.SetGamePausState(true);
    }

    public void OnClickPauseRelease()
    {
        this.gameObject.SetActive(false);

        if (!Managers.Game.IsGamePaused)
            return;

        Managers.Game.SetGamePausState(false);
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
