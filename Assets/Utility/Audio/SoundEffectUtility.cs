using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ObjectPooler))]
public class SoundEffectUtility : MonoBehaviour {

    static public SoundEffectUtility instance { get; private set; }

    ObjectPooler pooler;

    void Awake() {
        instance = (SoundEffectUtility)Singleton.Setup(this, instance);
        pooler = GetComponent<ObjectPooler>();
    }

    public void PlaySoundDelayed(AudioClip sound, float delay, float volume = 1) {
        StartCoroutine(DelayedSoundRoutine(sound, delay, volume));
    }

    IEnumerator DelayedSoundRoutine(AudioClip sound, float delay, float volume) {
        yield return new WaitForSeconds(delay);
        PlaySound(sound, volume);
    }

    public void PlaySound(AudioClip sound, float volume = 1, bool spatial3d=false) {
        GameObject soundObj = pooler.GetObject();
        soundObj.SetActive(true);
        AudioSource source = soundObj.GetComponent<AudioSource>();
        source.Stop();
        source.volume = volume;
        source.loop = false;
        source.clip = sound;
        source.spatialBlend = spatial3d ? 1 : 0;
        source.Play();
        StartCoroutine(WaitForSoundEndRoutine(source));
    }

    IEnumerator WaitForSoundEndRoutine(AudioSource source) {
        yield return new WaitUntil(() => !source.isPlaying);
        pooler.ReturnObject(source.gameObject);
    }
}
