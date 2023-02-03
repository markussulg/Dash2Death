using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public static AnimationController Instance;
    public AnimationCurve moveAnimationCurve;
    public AnimationCurve scaleAnimationCurve;
    private void Awake()
    {
        Instance = this;
    }

    IEnumerator MoveUsingCurve(GameObject animationFrom, Vector3 target, float duration)
    {
        Vector3 origin = animationFrom.transform.position;
        float timePassed = 0f;
        while (timePassed <= duration)
        {
            timePassed += Time.deltaTime;
            float percent = Mathf.Clamp01(timePassed / duration);
            float curvePercent = moveAnimationCurve.Evaluate(percent);
            animationFrom.transform.position = Vector3.LerpUnclamped(origin, target, curvePercent);
            yield return null;
        }
    }

    IEnumerator ScaleObjectUsingCurve(GameObject targetObject, Vector3 startScale, Vector3 EndScale, float duration, bool closeOnEnd = false)
    {
        targetObject.transform.localScale = startScale;
        float timePassed = 0f;
        while (timePassed <= duration)
        {
            timePassed += Time.deltaTime;
            float percent = Mathf.Clamp01(timePassed / duration);
            float curvePercent = scaleAnimationCurve.Evaluate(percent);
            targetObject.transform.position = Vector3.LerpUnclamped(startScale, EndScale, curvePercent);
            yield return null;
        }
        if (closeOnEnd)
        {
            targetObject.SetActive(false);
        }
    }
}
