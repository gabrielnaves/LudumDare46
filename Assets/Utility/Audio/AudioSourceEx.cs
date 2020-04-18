using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceEx : MonoBehaviour {

    public AudioClip intro;
    public AudioClip loop;
    public bool playOnStart = true;
    public bool dontDestroyOnLoad = false;
    [ViewOnly] public bool playing = false;
    [ViewOnly] public bool playingIntro = false;

    public AudioSource source { get; private set; }

    void Awake() {
        source = GetComponent<AudioSource>();
        source.playOnAwake = false;
        source.loop = false;
    }

    void Start() {
        if (playOnStart)
            Play();
        if (dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }

    void Update () {
        if (playing && playingIntro && !source.isPlaying) {
            if (loop)
                PlayMusic(fromIntro:false);
            else
                playing = playingIntro = false;
        }
    }

    void PlayMusic(bool fromIntro) {
        playing = true;
        playingIntro = fromIntro;
        source.clip = fromIntro ? intro : loop;
        source.loop = !fromIntro;
        source.Play();
    }

    public void Play() {
        if (!playing) {
            if (intro)
                PlayMusic(fromIntro:true);
            else
                PlayMusic(fromIntro:false);
        }
    }

    public void Stop() {
        if (playing) {
            playing = playingIntro = false;
            source.Stop();
        }
    }

    public void FadeOut(float fadeTime) {
        StartCoroutine(FadeRoutine(fadeTime, source.volume, 0));
    }

    public void FadeOutAndStop(float fadeTime) {
        StartCoroutine(FadeRoutine(fadeTime, source.volume, 0, () => Stop()));
    }

    IEnumerator FadeRoutine(float fadeTime, float startVol, float endVol, Action postFadeAction=null) {
        float elapsedTime = 0;
        while (elapsedTime < fadeTime) {
            yield return null;
            elapsedTime += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(startVol, endVol, elapsedTime / fadeTime);
        }
        postFadeAction?.Invoke();
    }
}
