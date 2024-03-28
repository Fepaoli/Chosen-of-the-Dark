using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    private bool moving = false;
    private List<Vector3> pathCoords;
    public List<PathfindingGrid> Q;

    public float moveLeft;
    public float speed;

    void Start()
    {
        StateManager.Instance.OnBattleStart.AddListener(InitMap);
        StateManager.Instance.OnRoundStart.AddListener(CreatePathfindingMap);
        pathCoords = new List<Vector3>();
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (moving)
        {
            if (pathCoords.Any())
            {
                if (pathCoords.First() == transform.position)
                {
                    coords = mapFunctions.WorldToGrid(transform.position);
                    pathCoords.RemoveAt(0);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, pathCoords.First(), Time.deltaTime);
                }
            }
            else
            {
                moveLeft -= pathfindingMap[coords].distance;
                moving = false;
                CursorController.Instance.acting = false;
                CreatePathfindingMap();
                DefinePaths(coords, moveLeft);
                UpdateMoveMap();
            }
        }
    }

    public void InitMap(){
        gameObject.SetActive(true);
        coords = mapFunctions.WorldToGrid(gameObject.transform.position);
        map = mapFunctions.map;

        //placeholder
        speed = 10;
        Debug.Log("Initmap finished");
    }

    public void MoveTo(Vector2Int target)
    {
        pathCoords.Clear();
        moving = true;
        while (pathfindingMap[target].previous != pathfindingMap[target].coords)
        {
            Debug.Log(target);
            pathCoords.Add(mapFunctions.GridToWorld(target));
            target = pathfindingMap[target].previous;
        }
        pathCoords.Reverse();
        Debug.Log("Moveto finished");
    }

    public bool IsTileReachable(Vector2Int coords)
    {
        return (pathfindingMap[coords].distance <= moveLeft) && (map[coords].walkable);
    }

    public void CreatePathfindingMap()
    {
        pathfindingMap = new Dictionary<Vector2Int, PathfindingGrid>();
        map = mapFunctions.map;
        foreach (TileController x in map.Values)
        {
            pathfindingMap.Add(x.coords, new PathfindingGrid(x.coords, 300, x.coords));
        }
        Debug.Log("create map finished");
    }
    public void DefinePaths(Vector2Int position, float movespeed)
    {
        //Initialize starting cell
        PathfindingGrid startingcell = new PathfindingGrid (position, 0, position);
        pathfindingMap[position] = startingcell;

        //Initialize graph nodes
        Q = new List<PathfindingGrid>();
        foreach (Vector2Int x in pathfindingMap.Keys)
        {
            Q.Add(pathfindingMap[x]);
        }

        while (Q.Any())
        {
            Q.Sort((s1,s2) => s1.distance.CompareTo(s2.distance));
            PathfindingGrid u = Q.First();
            Q.Remove(Q.First());
            if (u.distance <= moveLeft){
                map[u.coords].overlay.state = OverlayController.TileState.Reachable;
                List<PathfindingGrid> neighbours = FindNeighbours(u.coords);
                for (int i = 0; i < neighbours.Count(); i++)
                {
                    if (Q.Contains(neighbours[i]))
                    {
                        float altDist;
                        bool diagonal = false;
                        if ((neighbours[i].coords[0] + neighbours[i].coords[1] - u.coords[0] - u.coords[1]) % 2 == 0){
                            diagonal = true;                        
                        }
                        if (diagonal){
                            altDist = u.distance + (map[neighbours[i].coords].moveMult * 1.4F);
                        }
                        else{
                            altDist = u.distance + map[neighbours[i].coords].moveMult;
                        }
                        if (altDist < neighbours[i].distance)
                        {
                            pathfindingMap[neighbours[i].coords] = new PathfindingGrid(neighbours[i].coords, altDist, u.coords);
                            //Update distance inside list
                            Q.Remove(Q.Find(toupdate => toupdate.coords == neighbours[i].coords));
                            Q.Add(new PathfindingGrid(neighbours[i].coords, altDist, u.coords));
                        }
                    }
                }
            }
            else{
                map[u.coords].overlay.state = OverlayController.TileState.NotReachable;
            }
        }
        Debug.Log("finish pathfinding finished");
    }

    public List<PathfindingGrid> FindNeighbours (Vector2Int position)
    {
        List<PathfindingGrid> neighbours = new List<PathfindingGrid>();
        if (pathfindingMap.ContainsKey(position + new Vector2Int(1, 0))) { neighbours.Add(pathfindingMap[position + new Vector2Int(1,0)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(-1, 0))) { neighbours.Add(pathfindingMap[position + new Vector2Int(-1, 0)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(0, 1))) { neighbours.Add(pathfindingMap[position + new Vector2Int(0, 1)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(0, -1))) { neighbours.Add(pathfindingMap[position + new Vector2Int(0, -1)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(1, 1))) { neighbours.Add(pathfindingMap[position + new Vector2Int(1, 1)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(-1, 1))) { neighbours.Add(pathfindingMap[position + new Vector2Int(-1, 1)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(1, -1))) { neighbours.Add(pathfindingMap[position + new Vector2Int(1, -1)]); }
        if (pathfindingMap.ContainsKey(position + new Vector2Int(-1, -1))) { neighbours.Add(pathfindingMap[position + new Vector2Int(-1, -1)]); }
        return neighbours;
    }

    public void Deselect (){
        foreach (KeyValuePair<Vector2Int, TileController> x in map){
            x.Value.overlay.Hide();
        }
    }

    public void UpdateMoveMap (){
        foreach (KeyValuePair<Vector2Int, TileController> x in map){
            x.Value.overlay.ShowState();
        }
    }
    public struct PathfindingGrid
    {
        public Vector2Int coords;
        public float distance;
        public Vector2Int previous;

        public PathfindingGrid(Vector2Int c, float dist, Vector2Int prevCoords)
        {
            coords = c;
            distance = dist;
            previous = prevCoords;
        }
    }
}
