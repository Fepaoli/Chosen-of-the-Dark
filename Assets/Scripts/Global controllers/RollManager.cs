using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollManager : MonoBehaviour
{
    private static RollManager RMInstance;
    public static RollManager Instance
    {
        get
        {
            if (RMInstance == null)
            {
                Debug.Log("No roll manager?");
            }
            return RMInstance;
        }
    }
    void Awake(){
        RMInstance = this;
    }

    public int RollToDC (int dice, int bonus, int DC){
        int successes = 0;
        for (int i = 0 ; i<dice ; i++){
            int roll = UnityEngine.Random.Range(1,21) + (bonus);
            if (roll>= DC){
                successes += 1;
            }
        }
        return successes;
    }

    public int RollContested (int dice, int bonus, int enemydice, int enemybonus, int enemythreshold){
        int successes = 0;
        List<int> defenseResults = new List<int>();
        for (int i = 0; i<enemydice ; i++){
            defenseResults.Add(UnityEngine.Random.Range(1,21) + enemybonus);
        }
        defenseResults.Sort();
        defenseResults.Reverse();
        for (int i = 0; i<dice ; i++){
            int roll = UnityEngine.Random.Range(1,21) + bonus;
            if (i < defenseResults.Count){
                if (roll>=defenseResults[i+1]){
                    successes += 1;
                }
            }
            else{
                if (roll > 10-bonus){
                    successes += 1;
                }
            }
        }
        return successes;
    }
}
