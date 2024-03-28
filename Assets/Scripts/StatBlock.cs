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
    public int baseStam;
    public int stamina;
    public int RolledInitiative;
    // Start is called before the first frame update
    void Start()
    {
        SecondaryCalcs();
        currentHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RollInitiative(){
        Debug.Log("rolling initiative for " + gameObject);
        RolledInitiative = 0;
        for (int i = 0; i<(agi/2+2) ; i++){
            int roll = UnityEngine.Random.Range(1,21) + (agi/2);
            Debug.Log("Roll number " + i + " = " + roll);
            if (roll>= 10){
                Debug.Log("Roll succesful!");
                RolledInitiative++;
            }
        }
    }

    public void SecondaryCalcs(){
        HP = 10 + 2*str;
        WP = 5 + 2*wit + 2*emp;
        stamina = baseStam + (int)Math.Truncate(str/4D) + (int)Math.Truncate(agi/4D);
    }
}
