using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject tile;
    public GameObject overlay;
    public GameObject levelGrid;
    public MapGenerator generator;
    public Dictionary<Vector2Int, TileController> map;

    private static MapController MCinstance;
    public static MapController Instance
    {
        get
        {
            if (MCinstance == null)
            {
                Debug.Log("Map?");
            }
            return MCinstance;
        }
    }
    private void Awake()
    {
        MCinstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StateManager.Instance.OnBattleStart.AddListener(MapSetup);
        gameObject.SetActive(false);
    }

    public void MapSetup(){
        map = new Dictionary<Vector2Int, TileController>();
        Generate();
        InitiativeController.Instance.ClearInitiative();
        InitiativeController.Instance.SpawnEnemies();
        InitiativeController.Instance.SpawnPlayerCharacters();
        InitiativeController.Instance.RollInitiative();
        InitiativeController.Instance.SetupPathfinding();
        StateManager.Instance.UpdateState(StateList.newround);
    }

    public void Generate()
    {
        for (int x = 1; x <= 30; x++)
        {
            for (int y = 1; y <= 30; y++)
            {
                //Create and place tile
                Vector2Int coords = new Vector2Int(x, y);
                Vector3 tileLocation = GridToWorld(coords);
                GameObject createdTile = Instantiate(tile,levelGrid.transform);
                createdTile.transform.position = tileLocation;

                //Create and place overlay
                GameObject createdOverlay = Instantiate(overlay, createdTile.transform);
                createdOverlay.transform.position = tileLocation + new Vector3(0,0.05F,1);
                createdOverlay.GetComponent<OverlayController>().coords = coords;
                createdOverlay.GetComponent<OverlayController>().tile = createdTile.GetComponent<TileController>();
                TileController createdController = createdTile.GetComponent<TileController>();
                createdController.overlay = createdOverlay.GetComponent<OverlayController>();

                createdController.Setcoords(coords);
                map.Add(coords, createdController);
            }
        }
        generator.genStreamOverwrite();
    }

    public float calcDistance(Vector2Int startingCell, Vector2Int targetCell)
    {
        float distance = (float)Math.Sqrt(Math.Pow(Math.Abs(startingCell[0] - targetCell[0]), 2) + Math.Pow(Math.Abs(startingCell[1] - targetCell[1]), 2));
        return distance;
    }

    public Vector2Int WorldToGrid(Vector3 coordinates)
    {
        return new Vector2Int((int)Mathf.Round(coordinates[0] - coordinates[1] * 2), (int)Mathf.Round(coordinates[0] + coordinates[1]*2));
    }

    public Vector3 GridToWorld(Vector2Int coordinates)
    {
        return new Vector3(coordinates[0] * 0.5F + coordinates[1] * 0.5F, coordinates[0] * -0.25F + coordinates[1] * 0.25F, 0);
    }
}
