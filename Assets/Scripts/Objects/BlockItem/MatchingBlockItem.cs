using EnumDefine;

/// <summary>
/// ��Ī ����� �Ǵ� ������
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
