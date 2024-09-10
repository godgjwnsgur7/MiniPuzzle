using System;

namespace EnumDefine
{
    /// <summary>
    /// 블록노드가 참조하는 방향 인덱스
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
    /// 블록노드 안에 들어갈 아이템 프리팹 이름 리스트
    /// "Resources/Prefabs/BlockItem/" 경로에 존재
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
    /// 이펙트 오브젝트 프리팹 이름 리스트
    /// "Resources/Prefabs/EffectObjects/" 경로에 존재
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
    /// 이펙트 오브젝트 프리팹 이름 리스트
    /// "Resources/Prefabs/UIEffectObjects/" 경로에 존재
    /// </summary>
    [Serializable]
    public enum ENUM_UIEFFECTOBJECT_NAME
    {
        UIEffect_Rainbow_Item = 0,

    }

    /// <summary>
    /// 블록노드 안에 들어가는 아이템의 종류 - 주석확인
    /// </summary>
    [Serializable]
    public enum ENUM_BLOCKITEM_TYPE
    {
        Matching = 0, // 자기 자신이 매칭 대상이 됨 (기본)
        Nearby = 1, // 근처 노드가 매칭됐을 때 이벤트를 받음
    }
}


