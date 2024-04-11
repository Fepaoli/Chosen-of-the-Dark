using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAction : MonoBehaviour
{
    public TAction attack;

    void Start(){
        attack = new Attack(1.5F,gameObject);
    }
    public void TakeTurn(){
        Debug.Log("Enemy should have acted but AI doesn't exist yet");
    }
}
