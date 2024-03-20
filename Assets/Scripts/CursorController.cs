using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public GameObject examinedTile;
    public GameObject examinedCreature;
    private Collider2D tilePresent;
    private Collider2D creaturePresent;
    public SpriteRenderer sprite;
    public Vector2Int examinedCoords;
    private bool isCreatureSelected;
    public GameObject selectedCreature;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tilePresent = FindTile().collider;
        if (tilePresent!=null)
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
            if (Input.GetMouseButtonDown(0))
            {
                if (isCreatureSelected)
                {
                    if (creaturePresent == null && tilePresent != null)
                    {
                        if (examinedCreature.GetComponent<Pathfinder>().IsTileReachable(examinedCoords))
                        {
                            examinedCreature.GetComponent<Pathfinder>().MoveTo(examinedCoords);
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
                        if (examinedCreature.GetComponent<StatBlock>().controlled)
                        {
                            selectedCreature = examinedCreature;
                            isCreatureSelected = true;
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
                        isCreatureSelected = false;
                    }
                }
            }
        }
    }

    public RaycastHit2D FindTile()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursor2d = new Vector2(cursorPosition.x, cursorPosition.y);
        RaycastHit2D hit = Physics2D.Raycast(cursor2d, Vector2.zero, Mathf.Infinity, 3);
        return hit;
    }

    public RaycastHit2D FindCreature()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursor2d = new Vector2(cursorPosition.x, cursorPosition.y);
        RaycastHit2D hit = Physics2D.Raycast(cursor2d, Vector2.zero, Mathf.Infinity, 6);
        return hit;
    }
}
