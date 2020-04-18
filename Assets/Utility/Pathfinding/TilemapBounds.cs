using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapBounds : MonoBehaviour {

    static public TilemapBounds instance { get; private set; }

    public bool drawGizmos;
    [ViewOnly] public Bounds bounds;

    void Awake() {
        instance = (TilemapBounds)Singleton.Setup(this, instance);
        CalculateBounds();
    }

    void CalculateBounds() {
        var tilemaps = GetComponentsInChildren<Tilemap>();
        Vector3 min = new Vector3();
        Vector3 max = new Vector3();
        foreach (var map in tilemaps) {
            map.CompressBounds();
            min.x = Mathf.Min(min.x, map.localBounds.min.x);
            min.y = Mathf.Min(min.y, map.localBounds.min.y);
            max.x = Mathf.Max(max.x, map.localBounds.max.x);
            max.y = Mathf.Max(max.y, map.localBounds.max.y);
        }
        bounds = new Bounds((max + min) / 2f, new Vector3(max.x - min.x, max.y - min.y, 1));
    }

    void OnDrawGizmos() {
        if (drawGizmos && Application.isPlaying) {
            Gizmos.color = new Color(1, 1, 1, .5f);
            Gizmos.DrawCube(bounds.center, bounds.size);
        }
    }
}
