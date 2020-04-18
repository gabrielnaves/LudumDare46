using UnityEngine;

static public class Singleton {
    public static MonoBehaviour Setup(MonoBehaviour source, MonoBehaviour destination) {
        if (destination == null)
            destination = source;
        else
            Debug.LogError("[Singleton]Setup: Multiple instances of " + source);
        return destination;
    }
}
