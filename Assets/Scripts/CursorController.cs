using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public GameObject examinedTile;
    public Collider2D tilePresent;
    public SpriteRenderer sprite;
    private bool lookingAtTile;
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
        }
        else
        {
            sprite.color = Color.clear;
        }

    }

    public RaycastHit2D FindTile()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 cursor2d = new Vector2(cursorPosition.x, cursorPosition.y);
        lookingAtTile = Physics2D.Raycast(cursor2d, Vector2.zero);
        RaycastHit2D hit = Physics2D.Raycast(cursor2d, Vector2.zero);
        return hit;
    }
}
