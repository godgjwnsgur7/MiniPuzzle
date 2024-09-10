using System;
using System.Collections;
using EnumDefine;
using UnityEngine;

/// <summary>
/// 아이템을 생성, 생성된 아이템 관리
/// </summary>
public class ItemManagement : MonoBehaviour
{
    [SerializeField] private BlockNode rootBlockNode;

    [SerializeField] private BaseBlockItem blockItem;
    private Coroutine blockItemCreateCoroutine;

    private BaseBlockItem[] blockItemArray;

    private void Start()
    {
        blockItemArray = GameObject.FindObjectsOfType<BaseBlockItem>();
    }

    private void OnDisable()
    {
        if (blockItemCreateCoroutine != null)
            blockItemCreateCoroutine = null;
    }

    /// <summary>
    /// 아이템의 이벤트가 진행 중인지 체크
    /// </summary>
    public bool IsItemEventCheck()
    {
        if (blockItemCreateCoroutine != null)
            return true;

        foreach (BaseBlockItem item in blockItemArray)
            if (item == null || item.IsMoveState)
                return true;

        return false;
    }

    public void ItemSummon(int blockItemSummomCount)
    {
        if (blockItemCreateCoroutine == null)
            blockItemCreateCoroutine = StartCoroutine(IBlockItemCreate(blockItemSummomCount));
        else
            Debug.LogError("중복 실행 : 아이템 소환 코루틴");
    }

    public void StopItemSummon()
    {
        if(blockItemCreateCoroutine != null)
            StopCoroutine(blockItemCreateCoroutine);
    }

    protected IEnumerator IBlockItemCreate(int blockItemSummomCount)
    {
        Transform targetTr = null;
        while (blockItemSummomCount != 0)
        {
            if(targetTr != null)
                yield return new WaitUntil(() => 
                targetTr.transform.position.y + 1f <= this.transform.position.y);

            yield return new WaitUntil(() => rootBlockNode.blockItem == null);

            ENUM_BLOCKITEM_NAME itemName = (ENUM_BLOCKITEM_NAME)UnityEngine.Random.Range(0, 6);
            blockItem = Managers.Resource.Instantiate($"BlockItem/{itemName}", this.transform).GetComponent<BaseBlockItem>();
            blockItemSummomCount--;
            for (int i = 0; i < blockItemArray.Length; i++)
                if (blockItemArray[i] == null)
                {
                    blockItemArray[i] = blockItem;
                    break;
                }

            if (blockItem == null)
            {
                Debug.LogError("blockItem Is Null");
                yield break;
            }

            targetTr = blockItem.transform;

            blockItem.transform.parent = rootBlockNode.transform;
            blockItem.AddMoveTarget(rootBlockNode);
        }

        blockItemCreateCoroutine = null;
    }
}
