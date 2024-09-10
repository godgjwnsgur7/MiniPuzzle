using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// GameScene������ ���� �Ŵ���
/// </summary>
public class GameManager
{
    GameCanvas gameCanvas;
    ItemManagement itemManagement = null;
    BlockNodeGroup blockNodeGroup = null;

    HashSet<int> MatchingBlockIndexSet = new HashSet<int>();
    
    Coroutine matchingEventCoroutine = null;
    Coroutine itemSpeedUpCoroutine = null;
    Coroutine gameClearCheckCoroutine = null;

    public bool IsMatchingEventState {get; private set;}
    private int blockItemSummomCount = 0;

    public float CurrItemMoveSpeed { get; private set; }
    public bool IsGamePaused { get; private set; }
    public int activeEffectUICount { get; private set; }

    private int moveCount = 18;
    private int clearConditionCount = 10;

    // ������
    public readonly float minItemMoveSpeed = 0;
    public readonly float maxItemMoveSpeed = 15;

    public void Init()
    {
        if (itemManagement == null) itemManagement = GameObject.FindObjectOfType<ItemManagement>();
        if (blockNodeGroup == null) blockNodeGroup = GameObject.FindObjectOfType<BlockNodeGroup>();
        gameCanvas = Managers.UI.currCanvas.GetComponent<GameCanvas>();
        gameCanvas.UpdateClearConditionCount(clearConditionCount);
        gameCanvas.UpdateMoveCount(moveCount);
    }

    public void EndGame()
    {
        IsGamePaused = true;

        gameClearCheckCoroutine = CoroutineHelper.StartCoroutine(IGameClearCheck());
    }

    public void AddMatchingBlockIndex(int index)
    {
        MatchingBlockIndexSet.Add(index);
    }

    public void AddBlockItemSummomCount() => blockItemSummomCount++;
    public void AddActiveEffectUICount() => activeEffectUICount++;
    public void DeductActiveEffectUICount() => activeEffectUICount--;

    public void DeductClearConditionCount()
    {
        clearConditionCount--;

        if (clearConditionCount == 0)
            EndGame();
    }

    public void UpdateClearConditionCount()
    {
        gameCanvas.UpdateClearConditionCount(clearConditionCount);
    }

    public bool IsItemEventCheck()
    {
        bool isEventState = itemManagement.IsItemEventCheck() 
            || activeEffectUICount > 0;
        return isEventState;
    }

    public void SetGamePausState(bool isGamePaused)
    {
        IsGamePaused = isGamePaused;
    }

    public void PlayMatchingEvent()
    {
        if (matchingEventCoroutine != null)
        {
            Debug.LogError("��Ī�̺�Ʈ�ڷ�ƾ �ߺ�");
            CoroutineHelper.StopCoroutine(matchingEventCoroutine);
        }

        if (gameClearCheckCoroutine != null)
            return;

        matchingEventCoroutine = CoroutineHelper.StartCoroutine(IMatchingEventExecuter());
    }

    private IEnumerator IMatchingEventExecuter()
    {
        IsMatchingEventState = true;

        if (itemSpeedUpCoroutine != null)
            CoroutineHelper.StopCoroutine(itemSpeedUpCoroutine);

        // ������ �̵��ӵ� 2�� ���� ������ ����
        itemSpeedUpCoroutine = CoroutineHelper.StartCoroutine(IItemSpeedUp(2f));

        yield return new WaitUntil(() => IsItemEventCheck() == false);
        
        // ��Ī �̺�Ʈ
        blockNodeGroup.MatchingEvent(MatchingBlockIndexSet.ToArray());
        MatchingBlockIndexSet.Clear();

        // ������ ��ȯ ����
        itemManagement.ItemSummon(blockItemSummomCount);
        blockItemSummomCount = 0;

        yield return new WaitUntil(() => IsItemEventCheck() == false
        && (!IsGamePaused || gameClearCheckCoroutine != null));

        // �ڷ�ƾ ����
        IsMatchingEventState = false;
        matchingEventCoroutine = null;

        // Ŭ���� ���� ����ó��
        if (gameClearCheckCoroutine != null)
            yield break;

        // ���Ӹ�Ī üũ
        bool isMatching = blockNodeGroup.MatchingCheckAll();

        // Ŭ���� üũ
        if(!isMatching)
        {
            moveCount--;
            gameCanvas.UpdateMoveCount(moveCount);

            if (moveCount == 0)
                EndGame();
        }
    }
    
    private IEnumerator IItemSpeedUp(float durationTime)
    {
        CurrItemMoveSpeed = minItemMoveSpeed;

        float runTime = 0.0f;

        while (runTime < durationTime)
        {
            runTime += Time.deltaTime;
            CurrItemMoveSpeed = Mathf.Lerp(minItemMoveSpeed, maxItemMoveSpeed, runTime / durationTime);

            yield return null;
        }

        CurrItemMoveSpeed = maxItemMoveSpeed;
        itemSpeedUpCoroutine = null;
    }

    private IEnumerator IGameClearCheck()
    {
        Debug.Log("���� ��");

        yield return new WaitUntil(() => matchingEventCoroutine == null);

        gameCanvas.Open_ClearNotifyWindow(clearConditionCount == 0);
    }

    public void Clear()
    {

    }
}
