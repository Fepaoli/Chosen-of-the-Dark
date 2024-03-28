using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAction : MonoBehaviour
{
    public void TakeTurn(){
        InitiativeController.Instance.NextInInitiative();
    }
}
