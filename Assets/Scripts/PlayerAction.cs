using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public StatBlock stats;
    public Pathfinder pathfinding;
    public float movementleft;
    public int actionsleft;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnReset(){
        actionsleft = 1;
        movementleft = stats.baseSpeed + stats.agi;
    }
}
