using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private static CursorController CCInstance;
    public static CursorController Instance
    {
        get
        {
            if (CCInstance == null)
            {
                Debug.Log("No cursor?");
            }
            return CCInstance;
        }
    }
    public TAction currentAction;
    public GameObject examinedTile;
    public GameObject examinedCreature;
    public GameObject map;
    private Collider2D tilePresent;
    private Collider2D creaturePresent;
    public SpriteRenderer sprite;
    public Vector2Int examinedCoords;
    public bool isCreatureSelected;
    public GameObject selectedCreature;
    public bool targeting = false;
    public GameObject targeter;
    public float targetingRange;
    public bool acting = false;
    public bool needToUpdateTurn = false;
    public GameObject actor = null;
    public bool isOnUI = false;
    void Awake()
    {
        CCInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //Update turn shower if necessary
        if (needToUpdateTurn && !acting){
            Debug.Log("Switching actor after action has ended");
            needToUpdateTurn = false;
            BattleUIManager.Instance.turnVisualizer.GetComponent<TMP_Text>().text = "Currently acting = " + actor.name;
        }

        //Find if looking at tile or creature
        tilePresent = FindTile().collider;
        if (tilePresent != null)
        {
            examinedTile = tilePresent.gameObject;
            transform.position = examinedTile.transform.position + new Vector3(0, 0.1F, 0);
            sprite.color = Color.red;
            examinedCoords = tilePresent.GetComponent<TileController>().coords;
            BattleUIManager.Instance.InspectTerrain(examinedTile);
            BattleUIManager.Instance.UpdateTerrainInspector(true);
        }
        else
        {
            sprite.color = Color.clear;
            BattleUIManager.Instance.UpdateTerrainInspector(false);
        }

        creaturePresent = FindCreature().collider;
        if (creaturePresent != null)
        {
            examinedCreature = creaturePresent.gameObject;
            examinedCoords = creaturePresent.GetComponent<Pathfinder>().coords;
            examinedTile = CreatureToTile(examinedCreature);
            transform.position = examinedTile.transform.position + new Vector3(0, 0.1F, 0);
            sprite.color = Color.blue;
        }

        if (StateManager.stateType == StateList.goodTurn)
        {
            if (tilePresent != null)
            {
                //show tile characteristics in UI
            }

            //Mouse input controller
            if (!isOnUI){
                //Behavior when targeting with an action
                if (targeting){
                    if (Input.GetMouseButtonDown(0)){
                        if (creaturePresent != null)
                        {
                            currentAction.GetTarget(examinedCreature);
                        }
                    }
                    else
                        if (Input.GetMouseButtonDown(1))
                            currentAction.StopTargeting();
                }


                //Behavior when not targeting
                else{
                    if (Input.GetMouseButtonDown(0))
                    {
                        //Behavior when a character is selected on LMB
                        if (isCreatureSelected)
                        {
                            UpdateOverlays();
                            if (creaturePresent == null && tilePresent != null && !acting)
                            {
                                Pathfinder creatureMove = selectedCreature.GetComponent<Pathfinder>();
                                if (creatureMove.IsTileReachable(examinedCoords))
                                {
                                    creatureMove.MoveTo(examinedCoords);
                                }
                            }
                        }



                        //Behavior with no character selected on LMB
                        else
                        {
                            if (creaturePresent != null && !acting)
                            {
                                if (examinedCreature.GetComponent<StatBlock>().controlled && InitiativeController.Instance.IsActing(examinedCreature))
                                {
                                    selectedCreature = examinedCreature;
                                    isCreatureSelected = true;
                                    UpdateOverlays();
                                }
                            }
                        }
                    }
                }
            }
        }
        //Inspect behavior
        if (Input.GetMouseButtonDown(1))
        {
            //Behavior when a character is selected on RMB
            if (isCreatureSelected)
            {
                if (creaturePresent == null)
                {
                    BattleUIManager.Instance.DeselectCreature();
                    Deselect();
                }
                else
                {
                    BattleUIManager.Instance.InspectCreature(examinedCreature);
                }
            }


            //Behavior with no character selected on RMB
            else
            {
                if (creaturePresent != null)
                    BattleUIManager.Instance.InspectCreature(examinedCreature);
                else
                {
                    BattleUIManager.Instance.DeselectCreature();
                    Deselect();
                }
            }
        }
    }
    public void ShowActionRange (TAction action, Pathfinder actor){
        foreach (Vector2Int x in MapController.Instance.map.Keys){
            if (MapController.Instance.calcDistance(x,actor.coords)<=action.range){
                MapController.Instance.map[x].overlay.threatState = OverlayController.TileState.Threatened;
                MapController.Instance.map[x].overlay.ShowThreatState();
            }
            else{
                MapController.Instance.map[x].overlay.threatState = OverlayController.TileState.NotReachable;
                MapController.Instance.map[x].overlay.ShowThreatState();
            }
        }
    }

    public void HideActionRange (){
        foreach (Vector2Int x in MapController.Instance.map.Keys){
            MapController.Instance.map[x].overlay.Hide();
        }
    }

    public void SwitchActingCharacter(GameObject character){
        if (!acting){
            Debug.Log("Directly changed actor");
            BattleUIManager.Instance.turnVisualizer.GetComponent<TMP_Text>().text = "Currently acting = " + character.name;
        }
        else{
            Debug.Log("Waiting to change actor");
            needToUpdateTurn = true;
            actor = character;
        }
    }

    public void Deselect(){
        isCreatureSelected = false;
        foreach (KeyValuePair<Vector2Int, TileController> x in MapController.Instance.map){
            x.Value.overlay.Hide();
        }
        if (selectedCreature != null){
            selectedCreature.GetComponent<Pathfinder>().Deselect();
            selectedCreature = null;
        }
    }

    public RaycastHit2D FindTile()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursor2d = new Vector2(cursorPosition.x, cursorPosition.y);
        RaycastHit2D hit = Physics2D.Raycast(cursor2d, Vector2.zero, Mathf.Infinity, 1<<3, -Mathf.Infinity, Mathf.Infinity);
        return hit;
    }

    public RaycastHit2D FindCreature()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursor2d = new Vector2(cursorPosition.x, cursorPosition.y);
        RaycastHit2D hit = Physics2D.Raycast(cursor2d, Vector2.zero, Mathf.Infinity, 1<<6, -Mathf.Infinity, Mathf.Infinity);
        return hit;
    }

    public GameObject CreatureToTile(GameObject creature){
        GameObject tile = creature.GetComponent<Pathfinder>().map[examinedCoords].gameObject;
        return tile;
    }
    
    public void UpdateOverlays(){
        selectedCreature.GetComponent<Pathfinder>().UpdateMoveMap();
    }
}
