using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSourceEx))]
public class MusicPlaylistPlayer : MonoBehaviour {

    public AudioClip[] playlist;
    public int[] repetitions;
    public float transitionDelay = 2;

    AudioSourceEx source;
    int currentMusic = 0;
    int currentRepetition = 0;
    bool waitingForPlay;

    List<Pair<AudioClip, int>> musicList = new List<Pair<AudioClip, int>>();

    void Awake() {
        source = GetComponent<AudioSourceEx>();
        for (int i = 0; i < playlist.Length; ++i)
            musicList.Add(new Pair<AudioClip, int>(playlist[i], repetitions[i]));
    }

    void Start() {
        if (musicList.Count > 0)
            Play();
    }

    void LateUpdate() {
        if (musicList.Count > 0) {
            if (!source.playing && !waitingForPlay)
                StartCoroutine(UpdateMusicRoutine());
        }
    }

    void Play() {
        source.intro = musicList[currentMusic].first;
        source.Play();
    }

    IEnumerator UpdateMusicRoutine() {
        waitingForPlay = true;
        if (++currentRepetition >= musicList[currentMusic].second) {
            currentRepetition = 0;
            currentMusic++;
            if (currentMusic >= musicList.Count)
                currentMusic = 0;
            yield return new WaitForSecondsRealtime(transitionDelay);
        }
        Play();
        waitingForPlay = false;
    }

    public void FadeOut() {
        source.FadeOut(ScreenFader.instance.fadeTime);
    }
}
