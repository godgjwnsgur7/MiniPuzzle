using UnityEngine;
using EnumDefine;

/// <summary>
/// 특정 위치로 이펙트 오브젝트, UI 이펙트 오브젝트 소환할 때 사용
/// </summary>
public class EffectGenerator
{
    public static void SummonEffectObject(ENUM_EFFECTOBJECT_NAME effectObjectName, Transform summonPosTarget)
    {
        Vector2 summonPosVec = summonPosTarget.position;

        Managers.Resource.Instantiate($"EffectObjects/{effectObjectName}", summonPosVec);
    }

    /// <param name="summonPosVec"> 기준 : 월드좌표</param>
    public static void SummonEffectObject(Vector2 summonPosVec)
    {
            
    }

    public static void SummonUIEffectObject(ENUM_UIEFFECTOBJECT_NAME uiEffectObjectName, Transform summonPosTarget)
    {
        Managers.Resource.Instantiate($"UIEffectObjects/{uiEffectObjectName}", summonPosTarget.position, Quaternion.identity, Managers.UI.currCanvas.transform);
            
    }
}