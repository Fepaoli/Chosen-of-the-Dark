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

    public float speed;

    public void Start()
    {
        StateManager.Instance.OnRoundStart.AddListener(CreatePathfindingMap);
        StateManager.Instance.OnBattleStart.AddListener(InitMap);
        gameObject.SetActive(false);
    }

    public void InitMap(){
        gameObject.SetActive(true);
        coords = mapFunctions.WorldToGrid(gameObject.transform.position);
        speed = 10;
        map = mapFunctions.map;
        pathfindingMap = new Dictionary<Vector2Int, PathfindingGrid>();
    }

    public void MoveTo(Vector2Int targetCoords)
    {
        coords = targetCoords;
        //Crea lista di coordinate a cui muoverti.
        List<Vector2Int> moveNodes = new List<Vector2Int>();
        float distanceMoved =  pathfindingMap[targetCoords].distance;
        Vector2Int currentNode = targetCoords;
        while (currentNode != pathfindingMap[currentNode].previous){
            moveNodes.Add(currentNode);
            currentNode = pathfindingMap[currentNode].previous;
        }
        //Scorri lista e sposta il personaggio.
        moveNodes.Reverse();
        while (moveNodes.Any()){
            Vector2Int destination = moveNodes.First();
            gameObject.transform.position = mapFunctions.GridToWorld(destination);
            Debug.Log(moveNodes.First());
            moveNodes.Remove(moveNodes.First());
        }
        DefinePaths(coords, speed);
    }

    public bool IsTileReachable(Vector2Int coords)
    {
        Debug.Log(pathfindingMap[coords].distance);
        return (pathfindingMap[coords].distance <= speed) && (map[coords].walkable);
    }

    public void CreatePathfindingMap()
    {
        speed = 10;
        map = mapFunctions.map;
        foreach (TileController x in map.Values)
        {
            pathfindingMap.Add(x.coords, new PathfindingGrid(x.coords, 300, x.coords));
        }
        DefinePaths(coords, speed);
    }
    public void DefinePaths(Vector2Int position, float movespeed)
    {
        //Initialize starting cell
        PathfindingGrid startingcell = new PathfindingGrid (position, 0, position);
        pathfindingMap[position] = startingcell;

        //Initialize graph nodes
        List<PathfindingGrid> Q = new List<PathfindingGrid>();
        foreach (Vector2Int x in pathfindingMap.Keys)
        {
            Q.Add(pathfindingMap[x]);
        }

        while (Q.Any())
        {
            Q.Sort((s1,s2) => s1.distance.CompareTo(s2.distance));
            PathfindingGrid u = Q.First();
            Q.Remove(Q.First());
            List<PathfindingGrid> neighbours = FindNeighbours(u.coords);
            for (int i = 0; i < neighbours.Count; i++)
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
