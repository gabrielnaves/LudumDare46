using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFader : MonoBehaviour {

    public float defaultFadeTime = 1f;
    public bool startTransparent;
    public bool fadeInOnStart;

    public CanvasGroup canvasGroup { get; private set; }
    bool fading;
    bool transparent;
    float fadeTime;

    void Awake() {
        canvasGroup = GetComponent<CanvasGroup>();
        fading = false;
        transparent = startTransparent;
    }

    void Start() {
        SetAlphaValue(transparent ? 0 : 1);
        if (fadeInOnStart) RequestFadeIn();
    }

    public void SetAlphaValue(float alpha) {
        canvasGroup.alpha = alpha;
        transparent = alpha == 0;
        canvasGroup.blocksRaycasts = alpha > 0;
    }

    public bool IsIdle() {
        return !fading;
    }

    public bool CanFadeToTransparent() {
        return IsIdle() && !transparent;
    }

    public bool CanFadeToVisible() {
        return IsIdle() && transparent;
    }

    public void RequestFadeIn(bool useUnscaledTime = true) {
        RequestFadeIn(defaultFadeTime, useUnscaledTime);
    }

    public void RequestFadeIn(float fadeTime, bool useUnscaledTime=false) {
        if (fading)
            StopAllCoroutines();
        this.fadeTime = fadeTime;
        StartCoroutine(FadeRoutine(0, 1, useUnscaledTime));
    }

    public void RequestFadeOut(bool useUnscaledTime = true) {
        RequestFadeOut(defaultFadeTime, useUnscaledTime);
    }

    public void RequestFadeOut(float fadeTime, bool useUnscaledTime = true) {
        if (fading)
            StopAllCoroutines();
        this.fadeTime = fadeTime;
        StartCoroutine(FadeRoutine(1, 0, useUnscaledTime));
    }

    IEnumerator FadeRoutine(float startAlpha, float finalAlpha, bool useUnscaledTime) {
        fading = true;
        SetAlphaValue(startAlpha);
        float elapsedTime = 0;
        while (elapsedTime < fadeTime) {
            yield return null;
            elapsedTime += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            SetAlphaValue(Mathf.Lerp(startAlpha, finalAlpha, elapsedTime / fadeTime));
        }
        SetAlphaValue(finalAlpha);
        fading = false;
    }
}
