using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public StatBlock stats;
    public Pathfinder pathfinding;
    public float movementleft;
    public int actionsleft;
    public bool lookingForTarget = false;
    public GameObject currentTarget;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnReset(){
        actionsleft = 1;
        pathfinding.moveLeft = pathfinding.speed;
        pathfinding.CreatePathfindingMap();
        pathfinding.DefinePaths(pathfinding.coords, pathfinding.moveLeft);
    }
}
