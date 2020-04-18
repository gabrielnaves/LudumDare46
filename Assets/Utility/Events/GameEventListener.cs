using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour {

    public GameEvent gameEvent;
    public GameEvent[] multiEvents;
    public UnityEvent response;

    void OnEnable() {
        gameEvent.RegisterListener(this);
        foreach (var gameEvent in multiEvents)
            gameEvent.RegisterListener(this);
    }

    void OnDisable() {
        gameEvent.UnregisterListener(this);
        foreach (var gameEvent in multiEvents)
            gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised() {
        response?.Invoke();
    }
}
