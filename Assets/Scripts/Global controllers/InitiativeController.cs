using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using TMPro;

public class InitiativeController : MonoBehaviour
{
    private static InitiativeController ICinstance;
    public static InitiativeController Instance
    {
        get
        {
            if (ICinstance == null)
            {
                Debug.Log("No initiative?");
            }
            return ICinstance;
        }
    }

    public List<GameObject> InitiativeOrder;
    public GameObject currentActor;
    public int actorIndex;
    public Transform parent;
    //Action test

    private void Awake()
    {
        ICinstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform;
        actorIndex = 0;
        StateManager.Instance.OnRoundStart.AddListener(NextInInitiative);
    }
    public void SpawnPlayerCharacters(){
        Vector2Int coords = new Vector2Int(10,2);
        foreach (Transform child in parent.GetChild(1)){
            //Determine player character spawn coordinates
            coords[0] +=1;
            //Actually spawn player characterw
            child.gameObject.SetActive(true);
            child.gameObject.GetComponent<StatBlock>().SpawnOnBattlefield(coords);
            string name = child.gameObject.name;
            //Placeholder: manually assigns attacks to all party members
            if (name == "Priest")
                child.gameObject.GetComponent<StatBlock>().AddAction(new LightMeleeAttack(1.5F, child.gameObject,"Mace"));
            else if (name == "General")
                child.gameObject.GetComponent<StatBlock>().AddAction(new HeavyMeleeAttack(1.5F, child.gameObject,"Greatsword"));
            else if (name == "Hunter"){
                child.gameObject.GetComponent<StatBlock>().AddAction(new LightMeleeAttack(1.5F, child.gameObject,"Shortsword"));
                child.gameObject.GetComponent<StatBlock>().AddAction(new LightRangedAttack(7.5F, child.gameObject,"Hunting bow"));
            }
            else if (name == "Bureaucrat"){
                child.gameObject.GetComponent<StatBlock>().AddAction(new MediumMeleeAttack(2.5F, child.gameObject,"Spear"));
            }
            else if (name == "Hero")
            {
                child.gameObject.GetComponent<StatBlock>().AddAction(new MediumMeleeAttack(1.5F, child.gameObject,"Warhammer"));
                child.gameObject.GetComponent<StatBlock>().AddAction(new HeavyRangedAttack(10F, child.gameObject,"Longbow"));
            }

        }
    }

    public void SpawnEnemies(){
        //Determine base spawn position for enemies
        Vector2Int coords = new Vector2Int(15,25);

        //Randomly generate single coords for each enemy


        //Instantiate enemies
        foreach (Transform child in parent.GetChild(0)){
            child.gameObject.SetActive(true);
            string name = child.gameObject.name;
            //Placeholder to give enemies their actions and their position
            if (name == "Thief")
            {
                child.gameObject.GetComponent<AutoAction>().attack = new LightMeleeAttack(1.5F, child.gameObject, "Dagger");
                child.gameObject.GetComponent<StatBlock>().SpawnOnBattlefield(coords + new Vector2Int(-3, -5));
            }
            else if (name == "Thief (1)")
            {
                child.gameObject.GetComponent<AutoAction>().attack = new LightMeleeAttack(1.5F, child.gameObject, "Dagger");
                child.gameObject.GetComponent<StatBlock>().SpawnOnBattlefield(coords + new Vector2Int(-7, -5));
            }
            else if (name == "Thief (2)")
            {
                child.gameObject.GetComponent<AutoAction>().attack = new LightMeleeAttack(1.5F, child.gameObject, "Dagger");
                child.gameObject.GetComponent<StatBlock>().SpawnOnBattlefield(coords + new Vector2Int(-5, -7));
            }
            else if (name == "Beastmaster")
            {
                child.gameObject.GetComponent<AutoAction>().attack = new HeavyRangedAttack(7.5F, child.gameObject, "Crossbow");
                child.gameObject.GetComponent<StatBlock>().SpawnOnBattlefield(coords + new Vector2Int(+10, 0));
            }
            else if (name == "Brigand")
            {
                child.gameObject.GetComponent<AutoAction>().attack = new HeavyMeleeAttack(1.5F, child.gameObject, "Maul");
                child.gameObject.GetComponent<StatBlock>().SpawnOnBattlefield(coords + new Vector2Int(-5, -5));
            }
            else
            {
                child.gameObject.GetComponent<StatBlock>().SpawnOnBattlefield(coords + new Vector2Int(+11, -2));
            }
        }
    }

    public bool IsOccupied(Vector2Int coords)
    {
        foreach (GameObject creature in InitiativeOrder)
        {
            if (creature.gameObject.GetComponent<Pathfinder>().coords == coords) {
                return true;
            }
        }
        return false;
    }
    public void RollInitiative(){
        // Get enemies
        foreach (Transform child in parent.GetChild(0)){
            InitiativeOrder.Add(child.gameObject);
            child.gameObject.GetComponent<StatBlock>().RollInitiative();
        }
        // Get party
        foreach (Transform child in parent.GetChild(1)){
            child.gameObject.SetActive(true);
            InitiativeOrder.Add(child.gameObject);
            child.gameObject.GetComponent<StatBlock>().RollInitiative();
        }

        // Finalize initiative order
        InitiativeOrder.Sort((s1,s2) => s2.GetComponent<StatBlock>().RolledInitiative.CompareTo(s1.GetComponent<StatBlock>().RolledInitiative));
        actorIndex = 0;
    }

    public void KillCreature(GameObject creature){
        InitiativeOrder.Remove(creature);
        if (creature.GetComponent<StatBlock>().controlled){
            creature.GetComponent<PlayerAction>().HasDied();
        }
        else{
            Destroy(creature,0F);
        }
        int livingAllies = 0;
        int livingEnemies = 0;
        foreach(GameObject living in InitiativeOrder){
            if (living.GetComponent<StatBlock>().controlled){
                livingAllies ++;
            }
            else{
                livingEnemies ++;
            }
        }
        if (livingAllies == 0){
            ClearInitiative();
            BattleEndLoss();
        }
        else if (livingEnemies == 0){
            ClearInitiative();
            BattleEndWin();
        }
    }

    public void ClearInitiative(){
        InitiativeOrder = new List<GameObject>();
    }
    public bool IsActing(GameObject creature)
    {
        if (creature == currentActor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void BattleEndWin(){
        StateManager.Instance.UpdateState(StateList.battleEnd);
        //Go to "Win" scene
        Debug.Log("You win!");
    }

    public void BattleEndLoss(){
        StateManager.Instance.UpdateState(StateList.battleEnd);
        //Go to "Lose" scene
        Debug.Log("You lose");
    }

    public void NextInInitiative()
    {
        CursorController.Instance.Deselect();
        if (InitiativeOrder.Any()){
            if (actorIndex >= InitiativeOrder.Count)
            {
                actorIndex = 0;
                StateManager.Instance.UpdateState(StateList.newround);
            }
            currentActor = InitiativeOrder[actorIndex];
            CursorController.Instance.SwitchActingCharacter(currentActor);
            if (currentActor.GetComponent<StatBlock>().controlled)
            {
                StateManager.Instance.UpdateState(StateList.goodTurn);
                currentActor.GetComponent<PlayerAction>().TurnReset();
            }
            else
            {
                StateManager.Instance.UpdateState(StateList.evilTurn);
                currentActor.GetComponent<AutoAction>().TakeTurn();
            }
            actorIndex++;
        }
    }
    public void SetupPathfinding()
    {
        // Get enemies
        foreach (Transform child in parent.GetChild(0))
        {
            child.gameObject.GetComponent<Pathfinder>().InitMap();
            child.gameObject.GetComponent<AutoAction>().InitTargetingMap();
        }
        // Get party
        foreach (Transform child in parent.GetChild(1))
        {
            child.gameObject.GetComponent<Pathfinder>().InitMap();
        }
    }
}

