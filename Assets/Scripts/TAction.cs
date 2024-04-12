using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TAction
{
    public GameObject actionTarget;
    public StatBlock targetStats;
    public GameObject boundCreature;
    public StatBlock boundStats;
    public float range;
    public string actionName;
    public TAction(){
        
    }
    public TAction(float attRange){
        range = attRange;
    }

    public TAction(float attRange, GameObject attActor){
        range = attRange;
        boundCreature = attActor;
        boundStats = attActor.GetComponent<StatBlock>();
    }

    public TAction(float attRange, GameObject attActor, string name)
    {
        range = attRange;
        boundCreature = attActor;
        boundStats = attActor.GetComponent<StatBlock>();
        actionName = name;
    }
    public virtual void StartTargeting(){
        Debug.Log("Start targeting");
        CursorController.Instance.targeting = true;
        CursorController.Instance.currentAction = this;
    }
    public virtual void StopTargeting(){
        Debug.Log("Stop targeting");
        CursorController.Instance.targeting = false;
        CursorController.Instance.currentAction = null;
    }
    public virtual void GetTarget(GameObject target){
        Debug.Log ("Actor = " + boundCreature.name);
        Debug.Log ("Target = " + target.name);
        int boundx = boundCreature.GetComponent<Pathfinder>().coords[0];
        int boundy = boundCreature.GetComponent<Pathfinder>().coords[1];
        int targetx = target.GetComponent<Pathfinder>().coords[0]; 
        int targety = target.GetComponent<Pathfinder>().coords[1]; 
        Debug.Log("creature coordinates: " + boundx + " " + boundy);
        Debug.Log("target coordinates: " + targetx + " " + targety);
        Debug.Log("Distance = " + Math.Sqrt(Math.Pow(Math.Abs(boundx - targetx),2)+ Math.Pow(Math.Abs(boundy - targety), 2)));
        Debug.Log("Range = " + range);
        if (Math.Sqrt(Math.Pow(Math.Abs(boundx - targetx),2)+ Math.Pow(Math.Abs(boundy - targety), 2)) <= range){
            Debug.Log("Eligible target found");
            targetStats = target.GetComponent<StatBlock>();
            actionTarget = target;
            Execute();
        }
    }
    public virtual void Execute(){
    }

    public void InitAction (GameObject actor, float newRange){
        range = newRange;
        boundCreature = actor;
        boundStats = boundCreature.GetComponent<StatBlock>();
    }
}
