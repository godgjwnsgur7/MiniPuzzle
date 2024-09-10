using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClearConditionUIEffect : BaseUIEffect
{
    Coroutine motionEffectCoroutine = null;

    bool isInit = false;

    private void OnDisable()
    {
        if (motionEffectCoroutine != null)
            StopCoroutine(motionEffectCoroutine);
    }

    protected override void Init()
    {
        if (isInit)
            return;

        isInit = true;
        base.Init();

        Managers.Game.AddActiveEffectUICount();

        StartCoroutine(IMotionEffect(new Vector3(-1.74f, 4f, 0f)));
    }

    protected override void DestroyMine()
    {
        // 터지는 이펙트 소환

        Managers.Game.DeductActiveEffectUICount();

        base.DestroyMine();
    }

    /// <summary>
    /// 직선으로 이동
    /// </summary>
    protected IEnumerator IMotionEffect(Vector3 targetPosVec)
    {
        float runTime = 0f;
        float durationTime = 0.3f;
        float currScale;

        // 1. 그 자리에서 서서히 커짐
        while (runTime < durationTime)
        {
            runTime += Time.deltaTime;

            currScale = Mathf.Lerp(1f, 1.3f, runTime / durationTime);

            this.rectTr.localScale = new Vector3(currScale, currScale, 0);

            yield return null;
        }

        runTime = 0f;
        durationTime = 0.5f;
        float startPosX = rectTr.transform.position.x;
        float startPosY = rectTr.transform.position.y;
        float currPosX, currPosY;

        while (runTime < durationTime)
        {
            runTime += Time.deltaTime;

            currPosX = Mathf.Lerp(startPosX, targetPosVec.x, runTime / durationTime);
            currPosY = Mathf.Lerp(startPosY, targetPosVec.y, runTime / durationTime);
            currScale = Mathf.Lerp(1.3f, 0.7f, runTime / durationTime);

            this.rectTr.transform.position = new Vector3(currPosX, currPosY, 0);
            this.rectTr.localScale = new Vector3(currScale, currScale, 0);

            yield return null;
        }

        Managers.Game.UpdateClearConditionCount();
        DestroyMine();
    }
}
