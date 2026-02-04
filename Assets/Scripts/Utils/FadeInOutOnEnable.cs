using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class FadeInOutOnEnable : MonoBehaviour
{
    [SerializeField] private float totalDuration = 1.2f;   // 전체 시간
    [SerializeField] private float fadeInRatio = 0.5f;   // 인 비율 (나머지는 아웃)

    private CanvasGroup canvasGroup;
    private Coroutine fadeRoutine;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        float fadeInTime = totalDuration * fadeInRatio;
        float fadeOutTime = totalDuration - fadeInTime;

        canvasGroup.alpha = 0f;

        // Fade In
        float t = 0f;
        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(t / fadeInTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        // Fade Out
        t = 0f;
        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = 1f - Mathf.Clamp01(t / fadeOutTime);
            yield return null;
        }

        canvasGroup.alpha = 0f;

        // 자동 비활성화
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
            fadeRoutine = null;
        }
    }
}