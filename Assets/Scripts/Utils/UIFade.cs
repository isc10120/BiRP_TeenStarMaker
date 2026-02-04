using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    [SerializeField] private Image img;

    public void FadeIn(float duration) => StartCoroutine(Fade(0f, 1f, duration));
    public void FadeOut(float duration) => StartCoroutine(Fade(1f, 0f, duration));

    private IEnumerator Fade(float start, float end, float duration)
    {
        float elapsed = 0f;
        Color c = img.color;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(start, end, elapsed / duration);
            img.color = c;
            yield return null;
        }
        c.a = end;
        img.color = c;
    }
}