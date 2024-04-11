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
    public List<TAction> actions;

    public int LWSkill;
    public int MWSkill;
    public int HWSkill;
    public int LRWSkill;
    public int HRWSkill;
    public int DMSkill;
    public int OMSkill;
    public int lightDef;
    public int medDef;
    public int heavyDef;
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
        actions = new List<TAction>();
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

    public void AddAction(TAction newAction)
    {
        actions.Add(newAction);
        Debug.Log(actions);
    }

    public void RemoveAction(TAction newAction)
    {
        actions.Remove(newAction);
    }

    public void TakeDamage(int amount){
        currentHP -= amount;
        if (currentHP<=0)
            currentHP = 0;
    }

    public void HealDamage(int amount){
        currentHP += amount;
        if (currentHP>=HP)
            currentHP = HP;
    }
}
