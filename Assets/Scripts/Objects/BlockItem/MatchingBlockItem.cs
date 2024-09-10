using EnumDefine;

/// <summary>
/// 매칭 대상이 되는 아이템
/// </summary>
public class MatchingBlockItem : BaseBlockItem
{
    public override void Init()
    {
        base.Init();

        ItemType = ENUM_BLOCKITEM_TYPE.Matching;
    }

    public override void MatchingEvent()
    {
        base.MatchingEvent();

        DestroyMine();
    }
}
