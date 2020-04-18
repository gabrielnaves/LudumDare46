using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

static public class Pathfinding {

    class Node {
        public float f, g, h;
        public Node parent;
        public Vector3Int pos;

        public Node(Vector3Int pos) {
            this.pos = pos;
        }
    }

    static public List<Vector2> CalculatePath(Tilemap obstacleMap, Vector3 startPos, Vector3 goalPos) {
        Vector3Int startTile = obstacleMap.WorldToCell(startPos);
        Vector3Int goalTile = obstacleMap.WorldToCell(goalPos);
        BoundsInt mapBounds = obstacleMap.cellBounds;
        List<Vector2> path = new List<Vector2>();

        if (startTile == goalTile) {
            path.Add(startPos);
            path.Add(goalPos);
            return path;
        }
        if (!StartAndGoalAreValid(obstacleMap, mapBounds, startTile, goalTile))
            return path;


        List<Node> openTiles = new List<Node>();
        List<Node> closedTiles = new List<Node>();

        Node[,] nodes = NodeMatrixFromTiles(obstacleMap.GetTilesBlock(mapBounds), mapBounds);
        startTile -= mapBounds.min;
        goalTile -= mapBounds.min;

        openTiles.Add(nodes[startTile.y, startTile.x]);
        Vector3Int currentTile = startTile;
        Vector3Int[] adjacentTiles = GetAdjacentTileArray();
        nodes[currentTile.y, currentTile.x].parent = null;

        while (currentTile != goalTile && openTiles.Count > 0) {
            openTiles.Remove(nodes[currentTile.y, currentTile.x]);
            closedTiles.Add(nodes[currentTile.y, currentTile.x]);

            for (int i = 0; i < 8; ++i) {
                Vector3Int tile = currentTile + adjacentTiles[i];
                if (IsTileOutOfBounds(tile, mapBounds))
                    continue;
                if (i >= 4 && IsDiagonalInvalid(nodes, currentTile, adjacentTiles[i]))
                    continue;
                if (HasTile(nodes, tile) && !closedTiles.Contains(nodes[tile.y, tile.x])) {
                    float a = goalTile.x - tile.x;
                    float b = goalTile.y - tile.y;
                    float g = nodes[currentTile.y, currentTile.x].g + (i < 4 ? 1 : 1.4f);
                    float h = Mathf.Sqrt(a * a + b * b);
                    float f = g + h;
                    if (!openTiles.Contains(nodes[tile.y, tile.x])) {
                        nodes[tile.y, tile.x].parent = nodes[currentTile.y, currentTile.x];
                        openTiles.Add(nodes[tile.y, tile.x]);
                        nodes[tile.y, tile.x].g = g;
                        nodes[tile.y, tile.x].h = h;
                        nodes[tile.y, tile.x].f = f;
                    }
                    else if (g < nodes[tile.y, tile.x].g) {
                        nodes[tile.y, tile.x].parent = nodes[currentTile.y, currentTile.x];
                        nodes[tile.y, tile.x].g = g;
                        nodes[tile.y, tile.x].f = f;
                    }
                }
            }
            openTiles.Sort( (a, b) => a.f.CompareTo(b.f) );
            if (openTiles.Count > 0)
                currentTile = openTiles[0].pos;
        }
        if (currentTile == goalTile) {
            path.Add(goalPos);
            while (nodes[currentTile.y, currentTile.x].parent != null) {
                currentTile = nodes[currentTile.y, currentTile.x].parent.pos;
                path.Add(obstacleMap.GetCellCenterWorld(nodes[currentTile.y, currentTile.x].pos + mapBounds.min));
            }
            path[path.Count - 1] = startPos;
            path.Reverse();
            ShrinkPath(path);
        }
        return path;
    }

    static bool StartAndGoalAreValid(Tilemap obstacleMap, BoundsInt mapBounds, Vector3Int start, Vector3Int goal) {
        bool result = obstacleMap.GetTile(start) == null && obstacleMap.GetTile(goal) == null;
        result = result && start.x >= mapBounds.min.x && start.x < mapBounds.max.x;
        result = result && start.y >= mapBounds.min.y && start.y < mapBounds.max.y;
        result = result && goal.x >= mapBounds.min.x && goal.x < mapBounds.max.x;
        result = result && goal.y >= mapBounds.min.y && goal.y < mapBounds.max.y;
        return result;
    }

    static Node[,] NodeMatrixFromTiles(TileBase[] tiles, BoundsInt mapBounds) {
        Node[,] result = new Node[mapBounds.size.y, mapBounds.size.x];
        int index = 0;
        for (int i = 0; i < mapBounds.size.y; ++i) {
            for (int j = 0; j < mapBounds.size.x; ++j) {
                if (tiles[index] == null)
                    result[i, j] = new Node(new Vector3Int(j, i, 0));
                index++;
            }
        }
        return result;
    }

    static Vector3Int[] GetAdjacentTileArray() {
        Vector3Int[] adjacentTiles = new Vector3Int[8];
        adjacentTiles[0] = new Vector3Int(1, 0, 0);
        adjacentTiles[1] = new Vector3Int(0, 1, 0);
        adjacentTiles[2] = new Vector3Int(-1, 0, 0);
        adjacentTiles[3] = new Vector3Int(0, -1, 0);
        adjacentTiles[4] = new Vector3Int(1, 1, 0);
        adjacentTiles[5] = new Vector3Int(-1, 1, 0);
        adjacentTiles[6] = new Vector3Int(-1, -1, 0);
        adjacentTiles[7] = new Vector3Int(1, -1, 0);
        return adjacentTiles;
    }

    static bool IsTileOutOfBounds(Vector3Int tile, BoundsInt mapBounds) {
        return tile.x < 0 || tile.x >= mapBounds.size.x || tile.y < 0 || tile.y >= mapBounds.size.y;
    }

    static bool IsDiagonalInvalid(Node[,] nodes, Vector3Int currentTile, Vector3Int targetDiagonal) {
        Vector3Int adjacent1 = targetDiagonal;
        Vector3Int adjacent2 = targetDiagonal;
        adjacent1.x = 0;
        adjacent2.y = 0;
        return !HasTile(nodes, currentTile + adjacent1) || !HasTile(nodes, currentTile + adjacent2);
    }

    static bool HasTile(Node[,] nodes, Vector3Int position) {
        return nodes[position.y, position.x] != null;
    }

    static void ShrinkPath(List<Vector2> path) {
        if (path.Count > 2) {
            for (int i = 0; i < path.Count - 2; ++i) {
                int amount = -1;
                for (int k = i + 1; k < path.Count; ++k) {
                    Vector3 segment = path[k] - path[i];
                    var hit = Physics2D.CircleCast(path[i], .4f, segment, segment.magnitude);
                    if (hit)
                        break;
                    amount++;
                }
                if (amount > 0)
                    path.RemoveRange(i+1, amount);
            }
        }
    }

    static public float CalculatePathLength(List<Vector2> path) {
        float result = 0;
        for (int i = 1; i < path.Count; ++i)
            result += (path[i] - path[i - 1]).magnitude;
        return result;
    }
}
