using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathfindingTest : MonoBehaviour {

    public Tilemap obstacleMap;
    public Transform start;
    public Transform goal;

    [ViewOnly] public List<Vector2> path = new List<Vector2>();

    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    void Update() {
        stopwatch.Start();
        path = Pathfinding.CalculatePath(obstacleMap, start.position, goal.position);
        stopwatch.Stop();
        Debug.Log(stopwatch.Elapsed.TotalMilliseconds);
        stopwatch.Reset();
    }

    void OnDrawGizmos() {
        if (!Application.isPlaying) {
            Gizmos.color = Color.blue;
            DrawSphere(start.position);
            Gizmos.color = Color.green;
            DrawSphere(goal.position);
        }
        else {
            if (path.Count > 0) {
                Gizmos.color = Color.blue;
                DrawSphere(start.position);
                Gizmos.color = Color.green;
                DrawSphere(goal.position);
                Gizmos.color = Color.white;
                for (int i = 1; i < path.Count - 1; ++i) {
                    Gizmos.DrawLine(path[i-1], path[i]);
                    DrawSphere(path[i]);
                }
                Gizmos.DrawLine(path[path.Count - 2], path[path.Count - 1]);
            }
            else {
                Gizmos.color = Color.red;
                DrawSphere(start.position);
                DrawSphere(goal.position);
            }
        }
    }

    void DrawSphere(Vector3 position) {
        Gizmos.DrawSphere(position, 0.2f);
    }
}
