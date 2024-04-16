using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public StatBlock stats;
    public Pathfinder pathfinding;
    public Lifestate characterState;
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

    public void RemoveFromBattle(){
        stats.Despawn();
    }

    public void HasDied(){
        RemoveFromBattle();
        if (characterState == Lifestate.Living){
            characterState = Lifestate.Fragmented;
            stats.currentHP = stats.HP;
        }
        else if (characterState == Lifestate.Fragmented){
            characterState = Lifestate.Hollow;
            stats.currentHP = stats.HP;
        }
        else {
            RemoveFromGame();
        }
    }

    public void RemoveFromGame(){

    }

    public enum Lifestate{
        Living,
        Fragmented,
        Hollow
    }
}
