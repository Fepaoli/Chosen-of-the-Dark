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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int RollToDC (int dice, int bonus, int DC){
        int successes = 0;
        for (int i = 0; i<dice ; i++){
            int roll = UnityEngine.Random.Range(1,21) + (bonus);
            if (roll>= DC){
                successes += 1;
            }
        }
        return successes;
    }

    public int RollContested (int dice, int bonus, int enemydice, int enemybonus, int enemythreshold){
        int successes = 0;
        for (int i = 0; i<dice ; i++){
            int roll = UnityEngine.Random.Range(1,21) + (bonus);
            if (enemydice > i){
                if (roll>= UnityEngine.Random.Range(1,21) + (enemybonus)){
                    successes += 1;
                }
            }
            else{
                if (roll > enemythreshold){
                    successes += 1;
                }
            }
        }
        return successes;
    }
}
