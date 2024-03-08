using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public GameObject tile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Generate()
    {
        for (int x = 1; x <= 20; x++)
        {
            for (int y = 1; y <= 20; y++)
            {

            }
        }
    }

    public Vector2Int WorldtoGrid(Vector3 coordinates)
    {
        return new Vector2Int((int)Mathf.Round(coordinates[0] - coordinates[1] * 2), (int)Mathf.Round(coordinates[0] + coordinates[1]*2));
    }
}
