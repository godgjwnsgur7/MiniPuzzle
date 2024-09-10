using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 특정 위치에 소환될 이펙트 오브젝트
/// "Resources/Prefabs/EffectObjects/" 경로에 존재
/// </summary>
public class BaseEffectObject : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 10;
    }

    /// <summary>
    /// EffectObject Animation Clip Event 호출
    /// </summary>
    public void DestroyMine()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}
