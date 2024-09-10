using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ư�� ��ġ�� ��ȯ�� ����Ʈ ������Ʈ
/// "Resources/Prefabs/EffectObjects/" ��ο� ����
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
    /// EffectObject Animation Clip Event ȣ��
    /// </summary>
    public void DestroyMine()
    {
        Managers.Resource.Destroy(this.gameObject);
    }
}
