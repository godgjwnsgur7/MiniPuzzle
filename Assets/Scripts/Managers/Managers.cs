using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;
    static Managers Instance { get { Init(); return s_Instance; } }

    private GameManager game = new GameManager();
    private UIManager ui = new UIManager();
    private ResourceManager resource = new ResourceManager();

    public static GameManager Game { get { return Instance.game; } }
    public static UIManager UI { get { return Instance.ui; } }
    public static ResourceManager Resource { get { return Instance.resource; } }

    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (s_Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();

            s_Instance.ui.Init();
            s_Instance.game.Init(); // GameScene에서만 호출해야 함 (씬 추가 시 확인)
        }
    }

    /// <summary>
    /// 씬이 넘어갈때마다 호출
    /// </summary>
    public static void Clear()
    {
        if (s_Instance == null)
            return;

        // Game.Clear();
        UI.Clear();
    }
}
