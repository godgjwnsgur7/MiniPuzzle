using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseUIEffect : MonoBehaviour
{
    protected RectTransform rectTr;

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        rectTr = this.GetComponent<RectTransform>();
    }

    protected virtual void DestroyMine()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}