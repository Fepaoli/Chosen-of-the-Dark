using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
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
    private bool isCreatureSelected;
    public GameObject selectedCreature;
    public bool targeting = false;
    public GameObject targeter;
    public float targetingRange;
    public bool acting;
    public bool isOnUI = false;
    void Awake()
    {
        CCInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
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
                if (targeting){
                    if (Input.GetMouseButtonDown(0)){
                        if (creaturePresent != null)
                        {
                            Debug.Log("Getting target");
                            currentAction.GetTarget(examinedCreature);
                        }
                    }
                    else
                        if (Input.GetMouseButtonDown(1)){
                            Debug.Log("Interrupting targeting");
                            currentAction.StopTargeting();
                            foreach(Transform btn in InitiativeController.Instance.gameObject.transform.GetChild(1)){
                                btn.gameObject.GetComponent<ActionBtn>().ResetButton();
                            }
                        }
                }
                else{
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (isCreatureSelected)
                        {
                            if (creaturePresent == null && tilePresent != null && !acting)
                            {
                                Pathfinder creatureMove = selectedCreature.GetComponent<Pathfinder>();
                                if (creatureMove.IsTileReachable(examinedCoords))
                                {
                                    acting = true;
                                    creatureMove.MoveTo(examinedCoords);
                                }
                                //Check if tile is reachable
                                //If tile is not reachable, do nothing
                                //If tile is reachable, call "move" function
                            }
                        }
                        else
                        {
                            if (creaturePresent != null)
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
                    if (Input.GetMouseButtonDown(1))
                    {
                        //show stats in depth
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
                        else
                        {
                            if (creaturePresent != null)
                                BattleUIManager.Instance.InspectCreature(examinedCreature);
                            else{
                                BattleUIManager.Instance.DeselectCreature();
                                Deselect();
                            }
                        }
                    }
                }
            }
        }
    }

    public void Deselect(){
        isCreatureSelected = false;
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
        Debug.Log("UpdateOverlays Called");
        selectedCreature.GetComponent<Pathfinder>().UpdateMoveMap();
    }
}
