using EnumDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 과제로 주어진 가산점 영역의 '팽이' 와 비슷한 기능을 하는 아이템
/// </summary>
public class RainbowItem : NearbyBlockItem
{
    protected Animator anim = null;

    public override void MatchingEvent()
    {
        base.MatchingEvent();

        if(matchingEventCount <= 1)
        {
            anim = GetComponent<Animator>();
            if(anim == null )
                anim = gameObject.AddComponent<Animator>();

            anim.runtimeAnimatorController = Managers.Resource.GetAnimator("Rainbow_Item_Animater");
        }
        else
        {
            Managers.Game.DeductClearConditionCount(); // 정보는 미리 전달

            ENUM_UIEFFECTOBJECT_NAME uiEffectObjectName = ENUM_UIEFFECTOBJECT_NAME.UIEffect_Rainbow_Item;
            EffectGenerator.SummonUIEffectObject(uiEffectObjectName, this.transform);

            DestroyMine();
        }
    }
}
