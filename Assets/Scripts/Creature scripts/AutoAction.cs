using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Pathfinder;

public class AutoAction : MonoBehaviour
{
    public TAction attack;
    public int actionsleft;
    public Pathfinder pathfinder;
    public List<GameObject> PartyMembers;
    public List<TAction> ActionList;
    public Dictionary<Vector2Int, EvaluatedCell> CellValues;
    public Dictionary<Vector2Int, GameObject> EnemiesInRange;

    void Start()
    {
        CellValues = new Dictionary<Vector2Int, EvaluatedCell> ();
        ActionList = new List<TAction>();
        attack = new LightMeleeAttack(1.5F, gameObject, "bite");
        ActionList.Add(attack);
        pathfinder = gameObject.GetComponent<Pathfinder>();
    }

    public void InitTargetingMap()
    {
        foreach (Vector2Int x in MapController.Instance.map.Keys)
        {
            CellValues[x] = new EvaluatedCell(x,0);
        }
    }

    public void TakeTurn(){
        actionsleft = 1;
        pathfinder.moveLeft = pathfinder.speed;
        pathfinder.CreatePathfindingMap();
        pathfinder.DefinePaths(pathfinder.coords,pathfinder.speed);
        Act();
    }

    public void Act(){
        FindCellValues();
        //If there are reachable enemies, choose one to attack
        if (EnemiesInRange.Any()){
            float minDist = 300;
            GameObject chosenTarget = null;
            Vector2Int chosenCoords = pathfinder.coords;
            foreach(Vector2Int attackingCoords in EnemiesInRange.Keys){
                if (pathfinder.pathfindingMap[attackingCoords].distance < minDist){
                    minDist = pathfinder.pathfindingMap[attackingCoords].distance;
                    chosenTarget = EnemiesInRange[attackingCoords];
                    chosenCoords = attackingCoords;
                }
            }
            pathfinder.MoveTo(chosenCoords);
            Attack (chosenTarget);
        }
        //If there are none, move to the best value cell and wait
        else
            ValueBasedMove();
    }

    public void Attack (GameObject target){
        attack.actionTarget = target;
        attack.targetStats = target.GetComponent<StatBlock>();
        attack.Execute();
        actionsleft -= 1;
    }
    public void ValueBasedMove()
    {
        Vector2Int maxValueCell = pathfinder.coords;
        float maxvalue = CellValues[maxValueCell].totalValue;
        foreach (Vector2Int x in MapController.Instance.map.Keys)
        {
            if (pathfinder.IsTileReachable(x))
            {
                Debug.Log("Evaluating tile " + x + " Value = " + CellValues[x].totalValue);
                if (CellValues[x].totalValue >= maxvalue)
                {
                    if (CellValues[x].totalValue > maxvalue || MapController.Instance.calcLOSDistance(x,pathfinder.coords) < MapController.Instance.calcLOSDistance(maxValueCell, pathfinder.coords))
                    {
                        maxvalue = CellValues[x].totalValue;
                        maxValueCell = x;
                    }
                }
            }
                
        }
        pathfinder.MoveTo(maxValueCell);
    }
    public void FindCellValues()
    {
        EnemiesInRange = new Dictionary<Vector2Int, GameObject>();
        foreach (Vector2Int x in MapController.Instance.map.Keys)
        {
            if (pathfinder.IsTileReachable(x))
                CellValues[x] = new EvaluatedCell(x, 0);
        }
        int actionNumber = 0;
        int enemyActionNumber = 0;
        //Calculate offensive value for each action
        foreach (TAction checkedAction in ActionList)
        {
            // Adding for each target
            foreach (Transform child in InitiativeController.Instance.gameObject.transform.GetChild(1))
            {
                Pathfinder enemyDistance = child.GetComponent<Pathfinder>();
                foreach (Vector2Int x in CellValues.Keys)
                {
                    if (pathfinder.IsTileReachable(x))
                    {
                        if (MapController.Instance.calcLOSDistance(x, enemyDistance.coords) <= checkedAction.range)
                        {
                            CellValues[x].offensiveValues.Add(1.2F);
                            if (!EnemiesInRange.ContainsKey(x)){
                                EnemiesInRange.Add(x, child.gameObject);
                            }
                        }
                        else
                        {
                            float distance = MapController.Instance.calcLOSDistance(x, enemyDistance.coords);
                            CellValues[x].offensiveValues.Add((distance - (distance - checkedAction.range)) / distance);
                        }
                    }
                }
                actionNumber++;
            }
        }
        //Calculate defensive value
        foreach (Transform child in InitiativeController.Instance.gameObject.transform.GetChild(1))
        {
            Pathfinder enemyDistance = child.GetComponent<Pathfinder>();
            StatBlock enemyActions = child.GetComponent<StatBlock>();
            foreach (TAction checkedAction in enemyActions.actions)
            {
                float enemyRange = checkedAction.range;
                float maxCalcedRange = checkedAction.range + enemyDistance.speed;
                foreach (Vector2Int x in CellValues.Keys)
                {
                    if (pathfinder.IsTileReachable(x))
                    {
                        float distance = MapController.Instance.calcLOSDistance(x, enemyDistance.coords);
                        if (distance > enemyRange && distance <= maxCalcedRange)
                            CellValues[x].defensiveValues.Add(((maxCalcedRange - enemyRange) - (maxCalcedRange - distance)) / (maxCalcedRange - enemyRange));
                        else if (distance > maxCalcedRange)
                            CellValues[x].defensiveValues.Add(1);
                    }
                }
                enemyActionNumber++;
            }
        }
        //(And multiply offensive and defensive value by their weigths)
        foreach (Vector2Int x in MapController.Instance.map.Keys)
        {
            if (pathfinder.IsTileReachable(x))
            {
                float offensiveSum = 0;
                float defensiveSum = 0;
                foreach (float v in CellValues[x].offensiveValues)
                {
                    offensiveSum += v;
                }
                foreach (float v in CellValues[x].defensiveValues)
                {
                    defensiveSum += v;
                }
                offensiveSum /= actionNumber;
                defensiveSum /= enemyActionNumber;

                //For dumb wolf AI
                defensiveSum = 0;
                CellValues[x] = new EvaluatedCell(x, offensiveSum + defensiveSum);
            }
        }
    }

    public struct EvaluatedCell
    {
        public Vector2Int coords;
        public float totalValue;
        public List<float> offensiveValues;
        public List<float> defensiveValues;
        public EvaluatedCell(Vector2Int c, float v)
        {
            coords = c;
            totalValue = v;
            offensiveValues = new List<float>();
            defensiveValues = new List<float>();
        }
    }
}
