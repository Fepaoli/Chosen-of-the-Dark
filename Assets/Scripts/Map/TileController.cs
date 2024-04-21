using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    public TerrainType terrain;
    public float moveMult;
    public int cover;
    public bool walkable;
    public bool sightBlock;
    public Vector2Int coords;
    public OverlayController overlay;
    public MapController map;
    // Start is called before the first frame update
    void Awake()
    {
        map = GetComponentInParent<MapController>();
        terrain = TerrainType.Grass;
        TileGen();
    }

    public void Setcoords(Vector2Int givenCoords)
    {
        coords = givenCoords;
    }

    public void Highlight()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void Hide()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
    }

    public void TileGen()
    {
        switch (terrain)
        {
            case TerrainType.Grass:
                moveMult = 1;
                cover = 0;
                walkable = true;
                sightBlock = false;
                break;
            case TerrainType.ShallowWater:
                moveMult = 2;
                cover = 0;
                walkable = true;
                sightBlock = false;
                break;
            case TerrainType.DeepWater:
                moveMult = 2;
                gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                cover = 0;
                walkable = false;
                sightBlock = false;
                break;
            case TerrainType.Mud:
                moveMult = 3;
                cover = 0;
                walkable = true;
                sightBlock = false;
                break;
            case TerrainType.Bush:
                moveMult = 2;
                cover = 1;
                walkable = true;
                sightBlock = true;
                break;
            case TerrainType.Blocker:
                moveMult = 10;
                cover = 10;
                walkable = false;
                sightBlock = true;
                break;
            case TerrainType.Path:
                moveMult = 0.7F;
                cover = 0;
                walkable = true;
                sightBlock = false;
                break;
        }
        gameObject.name = terrain.ToString();
    }
    public enum TerrainType
    {
        Grass,
        ShallowWater,
        DeepWater,
        Mud,
        Bush,
        Blocker,
        Path
    }
}
