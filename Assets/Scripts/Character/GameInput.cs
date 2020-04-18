using UnityEngine;

public class GameInput : MonoBehaviour {

    static public GameInput instance { get; private set; }

    [ViewOnly] public Vector2 movement;

    void Awake() {
        instance = (GameInput)Singleton.Setup(this, instance);
    }

    void Update() {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
    }
}
