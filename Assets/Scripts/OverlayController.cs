using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayController : MonoBehaviour
{
    public Vector2Int coords;
    public TileController tile;
    public TileState state;
    public SpriteRenderer colorRenderer;
    // Start is called before the first frame update
    void Start()
    {
        state = TileState.NotReachable;
        colorRenderer = gameObject.GetComponent<SpriteRenderer>();
        colorRenderer.color = Color.clear;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowState(){
        switch(state){
            case TileState.Reachable:
                colorRenderer.color = new Color(0,1.0F,0.25F,0.25F);
                break;
            case TileState.Threatened:
                colorRenderer.color = new Color(1.0F,0,0,0.25F);
                break;
            case TileState.NotReachable:
                colorRenderer.color = Color.clear;
                break;
            case TileState.Blocked:
                colorRenderer.color = new Color(0,0,0,0.25F);
                break;
            case TileState.Occupied:
                colorRenderer.color = new Color(0,0,0.5F,0.25F);
                break;
            default:
                Debug.Log("No state found for this tile");
                break;
        }
    }

    public void Hide(){
        colorRenderer.color = Color.clear;
    }

    public enum TileState {
        Reachable,
        Threatened,
        NotReachable,
        Blocked,
        Occupied
    }
}
