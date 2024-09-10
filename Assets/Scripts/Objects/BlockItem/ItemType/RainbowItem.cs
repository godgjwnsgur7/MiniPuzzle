using EnumDefine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ������ �־��� ������ ������ '����' �� ����� ����� �ϴ� ������
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
            Managers.Game.DeductClearConditionCount(); // ������ �̸� ����

            ENUM_UIEFFECTOBJECT_NAME uiEffectObjectName = ENUM_UIEFFECTOBJECT_NAME.UIEffect_Rainbow_Item;
            EffectGenerator.SummonUIEffectObject(uiEffectObjectName, this.transform);

            DestroyMine();
        }
    }
}
