using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleTilemap : MonoBehaviour {

    static public ObstacleTilemap instance { get; private set; }

    public Tilemap map;

    void Awake() {
        instance = (ObstacleTilemap)Singleton.Setup(this, instance);
    }
}
