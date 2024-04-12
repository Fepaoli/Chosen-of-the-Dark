using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pathfinder;

public class AutoAction : MonoBehaviour
{
    public TAction attack;
    public int actionsleft;
    public Pathfinder pathfinder;
    public List<GameObject> PartyMembers;
    public Dictionary<Vector2Int, EvaluatedCell> CellValues;

    void Start()
    {
        attack = new LightMeleeAttack(1.5F, gameObject, "bite");
        pathfinder = gameObject.GetComponent<Pathfinder>();
        foreach (TileController in MapController.Instance.map)
        {

        }
    }

    public void TakeTurn(){
        Debug.Log("Enemy should have acted but AI doesn't exist yet");
    }

    public void FindCellValues()
    {
        //Calculate offensive value, repeating for each action
        foreach (Transform child in InitiativeController.Instance.gameObject.transform.GetChild(1))
        {
            Pathfinder enemydistance = child.GetComponent<Pathfinder>();
            foreach (PathfindingGrid tile in enemydistance.pathfindingMap.Values)
            {

            }
        }
        //Calculate defensive value

        //Multiply offensive and defensive value by their weigths
    }

    public struct EvaluatedCell
    {
        public Vector2Int coords;
        public float value;
        public EvaluatedCell(Vector2Int c)
        {
            coords = c;
            value = 0;
        }
    }
}
