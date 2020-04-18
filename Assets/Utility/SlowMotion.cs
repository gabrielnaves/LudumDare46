using UnityEngine;

public class SlowMotion : MonoBehaviour {

    public static SlowMotion instance { get; private set; }

    public float timeScale = 0.1f;

    public bool isActive { get; private set; }

    public void Activate() {
        isActive = true;
        Time.timeScale = timeScale;
        Time.fixedDeltaTime *= timeScale;
    }

    public void Deactivate() {
        isActive = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime /= timeScale;
    }

    void Awake() {
        if (instance)
            Debug.LogError("Multiple instances of slow motion");
        instance = this;
    }
}
