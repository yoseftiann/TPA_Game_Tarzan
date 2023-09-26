using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeDuration = 0.5f;
    public bool isFading = false;

    private void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update() {
        if (GameManager.instance.isLockAWSD && !isFading && canvasGroup.alpha != 1)
        {
            FadeIn();
        }
        else if (!GameManager.instance.isLockAWSD && !isFading && canvasGroup.alpha != 0)
        {
            FadeOut();
        }
    }

    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, fadeDuration));
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0, fadeDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime)
    {
        isFading = true;

        float timeStartedLerping = Time.realtimeSinceStartup;
        float timeSinceStarted = Time.realtimeSinceStartup - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        while (percentageComplete < 1)
        {
            timeSinceStarted = Time.realtimeSinceStartup - timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);
            cg.alpha = currentValue;

            yield return new WaitForEndOfFrame();
        }

        cg.alpha = end;
        isFading = false;
    }
}
