using System;

namespace EnumDefine
{
    /// <summary>
    /// ��ϳ�尡 �����ϴ� ���� �ε���
    /// </summary>
    [Serializable]
    public enum ENUM_BLOCKNODE_DIRECTION
    {
        LeftUp = 0,
        Up = 1,
        RightUp = 2,
        RightDown = 3,
        Down = 4,
        LeftDown = 5,
        Max = 6,
    }

    /// <summary>
    /// ��ϳ�� �ȿ� �� ������ ������ �̸� ����Ʈ
    /// "Resources/Prefabs/BlockItem/" ��ο� ����
    /// </summary>
    [Serializable]
    public enum ENUM_BLOCKITEM_NAME
    {
        Green_Item = 0,
        Blue_Item = 1,
        Orange_Item = 2,
        Purple_Item = 3,
        Red_Item = 4,
        Yellow_Item = 5,
        Rainbow_Item = 6,
    }

    /// <summary>
    /// ����Ʈ ������Ʈ ������ �̸� ����Ʈ
    /// "Resources/Prefabs/EffectObjects/" ��ο� ����
    /// </summary>
    [Serializable]
    public enum ENUM_EFFECTOBJECT_NAME
    {
        Blue_Item_DestroyEffect = 0,
        Green_Item_DestroyEffect = 1,
        Orange_Item_DestroyEffect = 2,
        Purple_Item_DestroyEffect = 3,
        Red_Item_DestroyEffect = 4,
        Yellow_Item_DestroyEffect = 5,
        Rainbow_Item_DestroyEffect = 6,
    }

    /// <summary>
    /// ����Ʈ ������Ʈ ������ �̸� ����Ʈ
    /// "Resources/Prefabs/UIEffectObjects/" ��ο� ����
    /// </summary>
    [Serializable]
    public enum ENUM_UIEFFECTOBJECT_NAME
    {
        UIEffect_Rainbow_Item = 0,

    }

    /// <summary>
    /// ��ϳ�� �ȿ� ���� �������� ���� - �ּ�Ȯ��
    /// </summary>
    [Serializable]
    public enum ENUM_BLOCKITEM_TYPE
    {
        Matching = 0, // �ڱ� �ڽ��� ��Ī ����� �� (�⺻)
        Nearby = 1, // ��ó ��尡 ��Ī���� �� �̺�Ʈ�� ����
    }
}


