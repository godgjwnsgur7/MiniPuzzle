using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDefine;

public class BaseBlockItem : MonoBehaviour
{
    public ENUM_BLOCKITEM_NAME ItemName { get; private set; }
    public ENUM_BLOCKITEM_TYPE ItemType { get; protected set; }
    public bool IsMoveState { get; protected set; }

    Coroutine moveCoroutine = null;

    private void Start()
    {
        Init();
    }

    private void OnDisable()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
    }

    public virtual void Init()
    {
        ItemName = (ENUM_BLOCKITEM_NAME)Enum.Parse(typeof(ENUM_BLOCKITEM_NAME), gameObject.name.ToString());
    }
    /// <summary>
    /// 사용 전에 IsMoveState 체크
    /// </summary>
    public void AddMoveTarget(BlockNode blockNode, bool isItemChange = false)
    {
        if (moveCoroutine != null)
        {
            Debug.LogError("무브 코루틴 중복 실행");
            StopCoroutine(moveCoroutine);
        }

        moveCoroutine = StartCoroutine(IMove(blockNode, isItemChange)) ;
    }

    public virtual void MatchingEvent() { }
    
    public void DestroyMine()
    {
        // 파괴 이벤트 구분
        if (ItemType == ENUM_BLOCKITEM_TYPE.Matching)
        {
            ENUM_EFFECTOBJECT_NAME effectObjectName = (ENUM_EFFECTOBJECT_NAME)Enum.Parse(typeof(ENUM_EFFECTOBJECT_NAME)
                , $"{ItemName}_DestroyEffect");

            EffectGenerator.SummonEffectObject(effectObjectName, this.transform);
        }
        else if (ItemType == ENUM_BLOCKITEM_TYPE.Nearby)
        {
            // (임시)
            ENUM_EFFECTOBJECT_NAME effectObjectName = (ENUM_EFFECTOBJECT_NAME)Enum.Parse(typeof(ENUM_EFFECTOBJECT_NAME)
                , $"{ItemName}_DestroyEffect");

            EffectGenerator.SummonEffectObject(effectObjectName, this.transform);
        }

        Managers.Game.AddBlockItemSummomCount();
        Managers.Resource.Destroy(this.gameObject);
    }

    protected IEnumerator IMove(BlockNode blockNode, bool isItemChange = false)
    {
        IsMoveState = true;
        float itemMoveSpeed = 7f;

        Vector3 targetPos = blockNode.transform.position;
        blockNode.SetBlockItem(this); 

        if (isItemChange)
            itemMoveSpeed = 7f;

        while (transform.position != targetPos)
        {
            if (isItemChange == false)
                itemMoveSpeed = Managers.Game.CurrItemMoveSpeed;
            
            transform.position = Vector3.MoveTowards(transform.position, targetPos, itemMoveSpeed * Time.deltaTime);

            yield return null;
        }

        moveCoroutine = null;
        IsMoveState = false;
    }
}