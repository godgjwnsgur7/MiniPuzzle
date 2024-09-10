using System;
using System.Collections;
using UnityEngine;
using EnumDefine;

public class BlockNode : MonoBehaviour
{
    public BaseBlockItem blockItem;
    public BlockNode[] blockNodeArray = new BlockNode[(int)ENUM_BLOCKNODE_DIRECTION.Max];

    Action<int> OnMouseDownCallBack;
    Action<int> OnMouseUpCallBack;
    public int index { get; private set; }

    Coroutine findNearNodeItemCoroutine;

    public void Init(Action<int> OnMouseDownCallBack, Action<int> OnMouseUpCallBack, int index)
    {
        this.OnMouseDownCallBack = OnMouseDownCallBack;
        this.OnMouseUpCallBack = OnMouseUpCallBack;
        this.index = index;
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BlockNode>() == null)
            return;

        if(this.transform.position.y < collision.transform.position.y)
        {
            if(this.transform.position.x == collision.transform.position.x)
                blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.Up] = collision.GetComponent<BlockNode>();
            else if(this.transform.position.x > collision.transform.position.x)
                blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.LeftUp] = collision.GetComponent<BlockNode>();
            else if(this.transform.position.x < collision.transform.position.x)
                blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.RightUp] = collision.GetComponent<BlockNode>();
        }
        else
        {
            if (this.transform.position.x == collision.transform.position.x)
                blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.Down] = collision.GetComponent<BlockNode>();
            else if (this.transform.position.x > collision.transform.position.x)
                blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.LeftDown] = collision.GetComponent<BlockNode>();
            else if (this.transform.position.x < collision.transform.position.x)
                blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.RightDown] = collision.GetComponent<BlockNode>();
        }
    }

    private void OnMouseDown()
    {
        OnMouseDownCallBack?.Invoke(index);
    }

    private void OnMouseUp()
    {
        OnMouseUpCallBack?.Invoke(index);
    }

    public void SetBlockItem(BaseBlockItem blockItem) => this.blockItem = blockItem;
    public void GetBlockItem(BlockNode blockNode)
    {
        blockNode.SetBlockItem(blockItem);
        FindNearNodeItem();
        blockItem = null;
    }

    public bool ItemChange(ENUM_BLOCKNODE_DIRECTION targetDir)
    {
        if (blockNodeArray[(int)targetDir] == null ||
            blockNodeArray[(int)targetDir].blockItem == null)
            return false;

        // ������ ���� �ְ�ޱ�
        var blockItem = this.blockItem;
        this.blockItem = blockNodeArray[(int)targetDir].blockItem;
        blockNodeArray[(int)targetDir].blockItem = blockItem;
        
        this.blockItem.transform.parent = this.transform;
        blockNodeArray[(int)targetDir].blockItem.transform.parent
            = blockNodeArray[(int)targetDir].transform;

        // ������ ������Ʈ ��ġ����
        this.blockItem.AddMoveTarget(this, true);
        blockItem.AddMoveTarget(blockNodeArray[(int)targetDir], true);

        return true;
    }

    /// <summary>
    /// ��Ī üũ - ��Ī�� ����� ���ӸŴ������� ����
    /// </summary>
    public bool MatchingCheck()
    {
        if (blockItem == null || blockItem.ItemType != ENUM_BLOCKITEM_TYPE.Matching)
            return false;

        ENUM_BLOCKITEM_NAME itemName = blockItem.ItemName;

        bool isMatching = false;

        // 3�� �̻� ���ӵ� ������ ã��
        for(int i = 0; i < (int)ENUM_BLOCKNODE_DIRECTION.Max; i++)
            if(blockNodeArray[i] != null)
            {
                bool _isMatching = blockNodeArray[i].MatchingCheck((ENUM_BLOCKNODE_DIRECTION)i, itemName);
                if (_isMatching) isMatching = true;
            }

        // 4�� �̻� ���ִ� ������ ã��
        for (int i = 0; i < (int)ENUM_BLOCKNODE_DIRECTION.Max; i++)
        {
            int num = i;
            if (blockNodeArray[num] == null) continue;
            else if (blockNodeArray[num].blockItem.ItemName != itemName) continue;

            num++;
            if (num == (int)ENUM_BLOCKNODE_DIRECTION.Max) num = 0;

            if (blockNodeArray[num] == null) continue;
            else if (blockNodeArray[num].blockItem.ItemName != itemName) continue;

            num++;
            if (num == (int)ENUM_BLOCKNODE_DIRECTION.Max) num = 0;
            
            if (blockNodeArray[num] == null) continue;
            else if (blockNodeArray[num].blockItem.ItemName != itemName) continue;

            isMatching = true;
            Managers.Game.AddMatchingBlockIndex(index);

            foreach (BlockNode node in blockNodeArray)
                if (node != null && node.blockItem.ItemName == itemName)
                {
                    NearNodeMatchingCheck(node);
                    Managers.Game.AddMatchingBlockIndex(node.index);
                }
        }

        // 3�� �̻� ���ӵ� ������ ��Ī
        if (isMatching)
        {
            // ��Ī Ÿ��
            Managers.Game.AddMatchingBlockIndex(index);

            // Ư�� Ÿ�� (���� : �ֺ� ��Ī)
            foreach(BlockNode node in blockNodeArray)
                if (node != null && node.blockItem != null && node.blockItem.ItemType == ENUM_BLOCKITEM_TYPE.Nearby)
                    Managers.Game.AddMatchingBlockIndex(node.index);
        }
        
        return isMatching;
    }

    /// <summary>
    /// ��ó�� ���� ��带 �� ��Ī������� ��
    /// </summary>
    public void NearNodeMatchingCheck(BlockNode blockNode)
    {
        foreach (BlockNode node in blockNode.blockNodeArray)
            if (node != null && blockItem.ItemName == node.blockItem.ItemName)
                Managers.Game.AddMatchingBlockIndex(node.index);
    }

    /// <summary>
    /// ������ �ִ� ������ ��Ī üũ
    /// </summary>
    /// <returns></returns>
    public bool MatchingCheck(ENUM_BLOCKNODE_DIRECTION MatchingDir, ENUM_BLOCKITEM_NAME itemName)
    {
        bool isMatching = false;
        
        if (blockNodeArray[(int)MatchingDir] == null)
            return isMatching;

        if (blockNodeArray[(int)MatchingDir].blockItem == null)
        {
            Debug.LogError("�������� ��ġ�Ǳ� ���� ��Īüũ�� ��");
            return isMatching;
        }

        bool isEqual = itemName == blockItem.ItemName;

        // ���� ���⸸ üũ
        if (isEqual)
            isMatching = itemName == blockNodeArray[(int)MatchingDir].blockItem.ItemName;

        if (isMatching)
        {
            // ��Ī Ÿ��
            Managers.Game.AddMatchingBlockIndex(blockNodeArray[(int)MatchingDir].index);
            Managers.Game.AddMatchingBlockIndex(index);
            
            // Ư�� Ÿ�� (���� : �ֺ� ��Ī
            foreach (BlockNode node in blockNodeArray)
                if (node != null && node.blockItem != null && node.blockItem.ItemType == ENUM_BLOCKITEM_TYPE.Nearby)
                    Managers.Game.AddMatchingBlockIndex(node.index);

            BlockNode _node = blockNodeArray[(int)MatchingDir];
            foreach (BlockNode node in _node.blockNodeArray)
                if (node != null && node.blockItem != null && node.blockItem.ItemType == ENUM_BLOCKITEM_TYPE.Nearby)
                    Managers.Game.AddMatchingBlockIndex(node.index);
        }
        
        return isMatching;
    }

    public void MatchingEvent()
    {
        blockItem.MatchingEvent();
        FindNearNodeItem();
    }

    /// <summary>
    /// ��ó ���(���� �ִ� ���)�� Ž���� Item�� ������
    /// </summary>
    public void FindNearNodeItem()
    {
        if(findNearNodeItemCoroutine != null)
            StopCoroutine(findNearNodeItemCoroutine);
        
        findNearNodeItemCoroutine = StartCoroutine(IFindNearNodeItem());
    }

    protected IEnumerator IFindNearNodeItem()
    {
        yield return null;

        yield return new WaitUntil(() => blockItem == null);

        while(blockItem == null)
        {
            if (blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.Up] != null
            && blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.Up].blockItem != null
            && blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.Up].blockItem.IsMoveState == false)
            {
                blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.Up].GetBlockItem(this);
            }
            else if (blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.LeftUp] != null
                && blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.LeftUp].blockItem != null
                && blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.LeftUp].blockItem.IsMoveState == false)
            {
                blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.LeftUp].GetBlockItem(this);
            }
            else if (blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.RightUp] != null
                && blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.RightUp].blockItem != null
                && blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.RightUp].blockItem.IsMoveState == false)
            {
                blockNodeArray[(int)ENUM_BLOCKNODE_DIRECTION.RightUp].GetBlockItem(this);
            }

            yield return null;
        }

        if (blockItem == null)
        {
            findNearNodeItemCoroutine = null;
            yield break;
        }

        yield return new WaitUntil(() => blockItem.IsMoveState == false);
        blockItem.transform.parent = this.transform;
        blockItem.AddMoveTarget(this);
        findNearNodeItemCoroutine = null;
    }

    public void RemovePhysicsComponent()
    {
        Destroy(this.GetComponent<CircleCollider2D>());
    }
}
