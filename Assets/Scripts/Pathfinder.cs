using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class Pathfinder : MonoBehaviour
{
    public MapController mapFunctions;
    public Dictionary<Vector2Int, TileController> map;
    public Dictionary<Vector2Int, PathfindingGrid> pathfindingMap;
    public Vector2Int coords;

    public float speed;

    public void Start()
    {
        coords = new Vector2Int(0,0);
        speed = 10;
        map = mapFunctions.map;
        CreatePathfindingMap();
        DefinePaths(coords, speed);
    }

    public void CreatePathfindingMap()
    {
        foreach (TileController x in map.Values)
        {
            pathfindingMap.Add(x.coords, new PathfindingGrid(x.overlay, 300));
        }
    }
    public void DefinePaths(Vector2Int position, float movespeed)
    {
        //Initialize graph nodes
        List<PathfindingGrid> Q = new List<PathfindingGrid>();
        foreach (Vector2Int x in pathfindingMap.Keys)
        {
            OverlayController overlay = pathfindingMap[x].overlay;
            pathfindingMap[x] = new PathfindingGrid(overlay, 300);
            Q.Add(pathfindingMap[x]);
        }

        //Initialize starting cell
        PathfindingGrid startingcell = new PathfindingGrid (pathfindingMap[position].overlay, 0);
        pathfindingMap[position] = startingcell;


        while (Q.Any())
        {
            Q.Sort((s1,s2) => s2.distance.CompareTo(s1.distance));
            PathfindingGrid u = Q.First();
            Q.Remove(Q.First());
            List<PathfindingGrid> neighbours = FindNeighbours(u.overlay.coords);
            for (int i = 0; i < neighbours.Count; i++)
            {
                if (Q.Contains(neighbours[i]))
                {
                    float altDist = u.distance + neighbours[i].overlay.tile.moveMult;
                    if (altDist < neighbours[i].distance)
                    {
                        neighbours[i] = new PathfindingGrid(u.overlay, altDist);
                    }
                }
            }
        }

    }

    public List<PathfindingGrid> FindNeighbours (Vector2Int position)
    {
        List<PathfindingGrid> neighbours = new List<PathfindingGrid>();
        if (pathfindingMap.ContainsKey(position + new Vector2Int(1, 0))) { neighbours.Add(pathfindingMap[new Vector2Int(1,0)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(-1, 0))) { neighbours.Add(pathfindingMap[new Vector2Int(-1, 0)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(0, 1))) { neighbours.Add(pathfindingMap[new Vector2Int(0, 1)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(0, -1))) { neighbours.Add(pathfindingMap[new Vector2Int(0, -1)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(1, 1))) { neighbours.Add(pathfindingMap[new Vector2Int(1, 1)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(-1, 1))) { neighbours.Add(pathfindingMap[new Vector2Int(-1, 1)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(1, -1))) { neighbours.Add(pathfindingMap[new Vector2Int(1, -1)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(-1, -1))) { neighbours.Add(pathfindingMap[new Vector2Int(-1, -1)]); }
        return neighbours;
    }
    public struct PathfindingGrid
    {
        public OverlayController overlay;
        public float distance;
        public OverlayController previous;

        public PathfindingGrid(OverlayController ov, float dist)
        {
            overlay = ov;
            distance = dist;
            previous = ov;
        }
    }
}
