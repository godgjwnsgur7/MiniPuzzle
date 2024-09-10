using EnumDefine;

/// <summary>
/// 근처 노드가 매칭됐을 때 이벤트를 받아서 처리되는 아이템
/// </summary>
public class NearbyBlockItem : BaseBlockItem
{
    protected int matchingEventCount = 0;

    public override void Init()
    {
        base.Init();

        ItemType = ENUM_BLOCKITEM_TYPE.Nearby;
    }

    public override void MatchingEvent()
    {
        base.MatchingEvent();

        matchingEventCount++;
    }
}
