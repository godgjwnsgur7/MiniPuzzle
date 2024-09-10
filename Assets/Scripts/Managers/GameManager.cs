using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// GameScene에서만 사용될 매니저
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

    // 데이터
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
            Debug.LogError("매칭이벤트코루틴 중복");
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

        // 아이템 이동속도 2초 동안 서서히 증가
        itemSpeedUpCoroutine = CoroutineHelper.StartCoroutine(IItemSpeedUp(2f));

        yield return new WaitUntil(() => IsItemEventCheck() == false);
        
        // 매칭 이벤트
        blockNodeGroup.MatchingEvent(MatchingBlockIndexSet.ToArray());
        MatchingBlockIndexSet.Clear();

        // 아이템 소환 감지
        itemManagement.ItemSummon(blockItemSummomCount);
        blockItemSummomCount = 0;

        yield return new WaitUntil(() => IsItemEventCheck() == false
        && (!IsGamePaused || gameClearCheckCoroutine != null));

        // 코루틴 종료
        IsMatchingEventState = false;
        matchingEventCoroutine = null;

        // 클리어 이후 예외처리
        if (gameClearCheckCoroutine != null)
            yield break;

        // 연속매칭 체크
        bool isMatching = blockNodeGroup.MatchingCheckAll();

        // 클리어 체크
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
        Debug.Log("게임 끝");

        yield return new WaitUntil(() => matchingEventCoroutine == null);

        gameCanvas.Open_ClearNotifyWindow(clearConditionCount == 0);
    }

    public void Clear()
    {

    }
}
