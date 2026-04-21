using System.Collections;
using System.Timers;
using UnityEngine;

public class UIFade : MonoBehaviour
{
    public CanvasGroup canvas;

    public Coroutine UIRoutine;
    private bool isFading;

    [SerializeField] private float fadeDuration = 1f;

    public void StartRoutine()
    {
        Debug.Log("Starting Routine");
        if (UIRoutine != null)
            StopCoroutine(UIRoutine);

        isFading = true;
        UIRoutine = StartCoroutine(FadeInOutRoutine());
    }

    public void StopRoutine()
    {
        Debug.Log("Stopping Routine");
        if (UIRoutine != null)
        {
            StopCoroutine(UIRoutine);
            UIRoutine = null;
        } 

        isFading = true;
        canvas.alpha = 0f;
    }

    public IEnumerator FadeInOutRoutine()
    {
        while (true) 
        {
            yield return Fade(0f, 1f);
            yield return Fade(1f, 0f);
        }
    }

    private IEnumerator Fade(float from, float to)
    {
        float elasped = 0f;

        while (elasped < fadeDuration)
        {
            elasped += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(from, to, elasped / fadeDuration);
            yield return null;
        }

        canvas.alpha = to;
    }
}
