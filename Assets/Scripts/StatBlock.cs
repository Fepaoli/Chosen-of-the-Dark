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
    public int WP;
    public int baseStam;
    public int RolledInitiative;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RollInitiative(){
        RolledInitiative = 0;
        for (int i = 0; i<agi+2 ; i++){
            int roll = Random.Range(1,21);
            if (roll + agi/2 >= 10){
                RolledInitiative++;
            }
        }
    }
}
