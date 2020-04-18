using UnityEngine;

/// <summary> Implements screen fading effect (fade in/out). </summary>
[RequireComponent (typeof(CanvasGroupFader))]
public class ScreenFader : MonoBehaviour {

    static public ScreenFader instance { get; private set; }

    public float fadeTime = 1f;
    public bool startWithOverlay = true;
    public bool fadeInOnStart = true;
    public bool useUnscaledTime { get; set; }

    CanvasGroupFader fader;

    public void RequestFadeOut() {
        if (fader.CanFadeToVisible())
            fader.RequestFadeIn(fadeTime, useUnscaledTime);
        else
            Debug.LogWarning("[ScreenFader]RequestFadeOut: Cannot execute fade out at the moment");
    }

    public void RequestFadeIn() {
        if (fader.CanFadeToTransparent())
            fader.RequestFadeOut(fadeTime, useUnscaledTime);
        else
            Debug.LogWarning("[ScreenFader]RequestFadeIn: Cannot execute fade in at the moment");
    }

    public bool IsIdle() {
        return fader.IsIdle();
    }

    public void EnableOverlay() {
        fader.SetAlphaValue(1);
    }

    public void ClearOverlay() {
        fader.SetAlphaValue(0);
    }

    void Awake() {
        instance = (ScreenFader)Singleton.Setup(this, instance);
        fader = GetComponent<CanvasGroupFader>();
        useUnscaledTime = true;
    }

    void Start() {
        fader.SetAlphaValue(startWithOverlay ? 1 : 0);
        if (fadeInOnStart)
            RequestFadeIn();
    }
}
