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
    public GameObject examinedTile;
    public GameObject examinedCreature;
    public GameObject map;
    private Collider2D tilePresent;
    private Collider2D creaturePresent;
    public SpriteRenderer sprite;
    public Vector2Int examinedCoords;
    private bool isCreatureSelected;
    public GameObject selectedCreature;
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
        }
        else
        {
            sprite.color = Color.clear;
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
            if (creaturePresent != null)
            {
                if (examinedCreature.GetComponent<StatBlock>().controlled)
                {
                    //show friendly characteristics in UI

                }
                else
                {
                    //show enemy characteristics in UI
                }
            }

            //Mouse input controller
            if (!isOnUI){
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Click");
                    if (isCreatureSelected)
                    {
                        Debug.Log("Creature was selected");
                        if (creaturePresent == null && tilePresent != null && !acting)
                        {
                            Debug.Log("Tried to move");
                            Pathfinder creatureMove = selectedCreature.GetComponent<Pathfinder>();
                            if (creatureMove.IsTileReachable(examinedCoords))
                            {
                                Debug.Log("Tile is reachable");
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
                        Debug.Log("No creature selected");
                        if (creaturePresent != null)
                        {
                            Debug.Log("Creature found");
                            if (examinedCreature.GetComponent<StatBlock>().controlled && InitiativeController.Instance.IsActing(examinedCreature))
                            {
                                Debug.Log(InitiativeController.Instance.IsActing(examinedCreature));
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
                            Deselect();
                        }
                    }
                }
            }
        }
    }

    public void Deselect(){
        HideOverlays();
        Debug.Log("Creature not selected anymore");
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
        foreach (Transform child in map.transform){
            child.GetChild(0).GetComponent<OverlayController>().ShowState();
        }
    }

    public void HideOverlays(){
        foreach (Transform child in map.transform){
            child.GetChild(0).GetComponent<OverlayController>().Hide();
        }
    }
}
