using UnityEngine;

/// <summary>
/// Implements pause/resume funcionality by setting time scale to zero.
/// </summary>
public class SimplePause : MonoBehaviour {

    static public SimplePause instance { get; private set; }

    public bool resumeOnAwake = true;
    public GameEvent onPause;
    public GameEvent onResume;

    [ViewOnly] public bool paused;

    public void Pause() {
        Time.timeScale = 0;
        paused = true;
    }

    public void Resume() {
        Time.timeScale = 1;
        paused = false;
    }

    public void ToggleAndSendEvents() {
        if (paused) {
            onResume.Raise();
            Resume();
        }
        else {
            onPause.Raise();
            Pause();
        }
    }

    void Awake() {
        instance = (SimplePause)Singleton.Setup(this, instance);
        if (resumeOnAwake)
            Resume();
    }
}
