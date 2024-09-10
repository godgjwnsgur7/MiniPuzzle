using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseCanvas : MonoBehaviour
{
    Canvas canvas = null;
    CanvasScaler scaler = null;
    bool isInit = false;

    private void Start()
    {
        Managers.UI.Init();
    }

    private void OnPreCull()
    {
        GL.Clear(true, true, Color.black);
    }

    public virtual void Init()
    {
        if (isInit)
            return;

        isInit = true;

        scaler = GetComponent<CanvasScaler>();
        canvas = GetComponent<Canvas>();

        SetCanvas();
        SetCanvasScaler();
        SetResolution();
    }

    private void SetCanvas()
    {
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = Camera.main;
        canvas.sortingOrder = 1000;
    }

    private void SetCanvasScaler()
    {
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(533,888);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 1.0f;
    }

    public void SetResolution()
    {
        int setWidth = 533; // ����� ���� �ʺ�
        int setHeight = 888; // ����� ���� ����

        int deviceWidth = Screen.width; // ��� �ʺ� ����
        int deviceHeight = Screen.height; // ��� ���� ����

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

        if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
        }
        else // ������ �ػ� �� �� ū ���
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
        }

        OnPreCull();
    }

}
