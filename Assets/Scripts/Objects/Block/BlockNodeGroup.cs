using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumDefine;

public class BlockNodeGroup : MonoBehaviour
{
    [SerializeField] List<BlockNode> blockNodeList = new List<BlockNode>();

    private Coroutine mouseDragCheckCoroutine;
    private bool IsOnMouseEventLock = false;
    public int selectedIndex = -1;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        SetBlockAreas();
    }

    public void SetBlockAreas()
    {
        blockNodeList.Clear();
        int numOfChild = this.transform.childCount;
        
        for (int i = 0; i < numOfChild; i++)
            if(transform.GetChild(i).GetComponent<BlockNode>() != null)
                blockNodeList.Add(transform.GetChild(i).GetComponent<BlockNode>());

        for(int i = 0; i < blockNodeList.Count; i++)
            blockNodeList[i].Init(OnMouseDownCallBack, OnMouseUpCallBack, i);
    }

    public void OnMouseDownCallBack(int index)
    {
        if (Managers.Game.IsGamePaused) return;

        if (IsOnMouseEventLock || mouseDragCheckCoroutine != null
            || Managers.Game.IsMatchingEventState)
            return;

        IsOnMouseEventLock = true;
        selectedIndex = index;

        mouseDragCheckCoroutine = StartCoroutine(IMouseDragCheck());
    }

    public void OnMouseUpCallBack(int index)
    {
        if (index == selectedIndex)
        {
            selectedIndex = -1;
            IsOnMouseEventLock = false;
        }
    }

    public bool MatchingCheckAll()
    {
        bool isMatching = false;
        for(int i = 0; i < blockNodeList.Count; i++)
        {
            bool _isMatching = blockNodeList[i].MatchingCheck(); 
            if (_isMatching) isMatching = true;
        }

        if (isMatching) // ��Ī�� �߻����� ���ӸŴ������� �˸�
            Managers.Game.PlayMatchingEvent();

        return isMatching;
    }

    public void MatchingEvent(int[] matchingIndexArray)
    {
        foreach (int index in matchingIndexArray)
        {
            blockNodeList[index].MatchingEvent();
        }

        return;
    }

    private IEnumerator IMouseDragCheck()
    {
        Vector3 startPos = Input.mousePosition;
        Vector3 endPos;
        float moveX, moveY;

        while (IsOnMouseEventLock)
        {
            endPos = Input.mousePosition;
            float distance = Vector2.Distance(startPos, endPos);

            // ���� �Ÿ� �̻� �巡�� �ߴٸ�
            if (distance > 15f)
            {
                moveX = endPos.x - startPos.x;
                moveY = endPos.y - startPos.y;

                float angle = Mathf.Atan2(moveY, moveX) * Mathf.Rad2Deg;
                int selectedIndex = this.selectedIndex;
                
                ENUM_BLOCKNODE_DIRECTION checkDir;
                if (angle < -120) checkDir = ENUM_BLOCKNODE_DIRECTION.LeftDown;
                else if (angle < -60) checkDir = ENUM_BLOCKNODE_DIRECTION.Down;
                else if (angle < 0) checkDir = ENUM_BLOCKNODE_DIRECTION.RightDown;
                else if (angle < 60) checkDir = ENUM_BLOCKNODE_DIRECTION.RightUp;
                else if (angle < 120) checkDir = ENUM_BLOCKNODE_DIRECTION.Up;
                else checkDir = ENUM_BLOCKNODE_DIRECTION.LeftUp;

                bool isChange = blockNodeList[selectedIndex].ItemChange(checkDir);
                
                if(isChange)
                {
                    // ��Ī�� �߻��ϸ� �Լ� ���ο��� ���ӸŴ������� �˸�
                    bool isMatching = MatchingCheckAll();

                    yield return new WaitUntil(() => Managers.Game.IsMatchingEventState == false);

                    if (!isMatching)
                    {
                        yield return new WaitUntil(() => blockNodeList[selectedIndex].blockItem.IsMoveState == false
                        && blockNodeList[selectedIndex].blockNodeArray[(int)checkDir].blockItem.IsMoveState == false);

                        blockNodeList[selectedIndex].ItemChange(checkDir);

                        yield return new WaitUntil(() => Managers.Game.IsMatchingEventState == false
                        && Managers.Game.IsItemEventCheck() == false);
                    }
                }

                IsOnMouseEventLock = false;
                mouseDragCheckCoroutine = null;
                yield break;
            }

            yield return null;
        }

        IsOnMouseEventLock = false;
        mouseDragCheckCoroutine = null;
    }
}
