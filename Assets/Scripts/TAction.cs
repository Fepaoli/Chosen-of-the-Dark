using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAction
{
    public GameObject actionTarget;
    public StatBlock targetStats;
    public GameObject boundCreature;
    public StatBlock boundStats;
    public float range;
    public TAction(){
        
    }
    public TAction(float attRange){
        range = attRange;
    }

    public TAction(float attRange, GameObject attActor){
        range = attRange;
        boundCreature = attActor;
    }
    public void StartTargeting(){
        CursorController.Instance.targeting = true;
        CursorController.Instance.currentAction = this;
    }
    public void StopTargeting(){
        CursorController.Instance.targeting = false;
        CursorController.Instance.currentAction = null;
    }
    public void GetTarget(GameObject target){
        if (boundCreature.GetComponent<Pathfinder>().pathfindingMap[target.GetComponent<Pathfinder>().coords].distance < range){
            targetStats = target.GetComponent<StatBlock>();
            actionTarget = target;
            Execute();
        }
    }
    public void Execute(){
    }

    public void InitAction (GameObject actor, float newRange){
        range = newRange;
        boundCreature = actor;
        boundStats = boundCreature.GetComponent<StatBlock>();
    }
}
