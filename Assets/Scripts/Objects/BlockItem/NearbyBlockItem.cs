using EnumDefine;

/// <summary>
/// ��ó ��尡 ��Ī���� �� �̺�Ʈ�� �޾Ƽ� ó���Ǵ� ������
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
