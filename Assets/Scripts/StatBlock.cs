using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MPE;
using UnityEngine;

public class StatBlock : MonoBehaviour
{
    public bool controlled = true;
    public int str;
    public int agi;
    public int wit;
    public int emp;
    public List<Traits> traits;
    public List<IAction> actions;

    public int LWSkill;
    public int MWSkill;
    public int HWSkill;
    public int LRWSkill;
    public int HRWSkill;
    public int DMSkill;
    public int OMSkill;
    public int baseSpeed;
    public int HP;
    public int currentHP;
    public int WP;
    public int currentWP;
    public int baseStam;
    public int stamina;
    public int staminaLeft;
    public int RolledInitiative;
    // Start is called before the first frame update
    void Start()
    {
        SecondaryCalcs();
        currentHP = HP;
        currentWP = WP;
        staminaLeft = stamina;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RollInitiative(){
        Debug.Log("rolling initiative for " + gameObject);
        RolledInitiative = RollManager.Instance.RollToDC((int)agi/2+2,(int)agi/2,10);
    }

    public void SecondaryCalcs(){
        HP = 10 + 2*str;
        WP = 5 + 2*wit + 2*emp;
        stamina = baseStam + (int)Math.Truncate(str/4D) + (int)Math.Truncate(agi/4D);
    }

    public void AddAction(IAction newAction)
    {
        actions.Add(newAction);
    }

    public void RemoveAction(IAction newAction)
    {
        actions.Remove(newAction);
    }
}
