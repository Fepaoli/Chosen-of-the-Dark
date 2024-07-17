using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Regions currentRegion;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void genStreamOverwrite()
    {
        //Determine where the stream flows to and from
        Vector2Int streamStart;
        Vector2Int streamEnd;
        directions streamdirection;

        //Determine the direction
        int direction = Random.Range(1,20);
        if (direction >= 1 && direction <= 5)
            streamdirection = directions.Horizontal;
        else if (direction >= 6 && direction <= 10)
            streamdirection = directions.Vertical;
        else if (direction >= 11 && direction <= 13)
            streamdirection = directions.DiagonalNorthEast;
        else if (direction >= 14 && direction <= 16)
            streamdirection = directions.DiagonalNorthWest;
        else if (direction == 17)
            streamdirection = directions.TopLeftCorner;
        else if (direction == 18)
            streamdirection = directions.TopRightCorner;
        else if (direction == 19)
            streamdirection = directions.BottomLeftCorner;
        else 
            streamdirection = directions.BottomRightCorner;

        //Determine the start and end point based on it
        int startBottom = Random.Range(0, 1);
        int endTop = Random.Range(0, 1);
        switch (streamdirection)
        {
            case directions.Horizontal:
                streamStart = new Vector2Int(1,Random.Range(7,24));
                streamEnd = new Vector2Int(30, Random.Range(7,24));
                break;
            case directions.Vertical:
                streamStart = new Vector2Int(Random.Range(7, 24),1);
                streamEnd = new Vector2Int(Random.Range(7, 24),30);
                break;
            case directions.DiagonalNorthEast:
                if (startBottom == 0)
                    streamStart = new Vector2Int(1, Random.Range(1, 5));
                else
                    streamStart = new Vector2Int(Random.Range(1, 5), 1);
                if (endTop == 0)
                    streamEnd = new Vector2Int(30, Random.Range(26, 30));
                else
                    streamEnd = new Vector2Int(Random.Range(26, 30),30);
                break;
            case directions.DiagonalNorthWest:
                if (startBottom == 0)
                    streamStart = new Vector2Int(30, Random.Range(1, 5));
                else
                    streamStart = new Vector2Int(Random.Range(26, 30), 1);
                if (endTop == 0)
                    streamEnd = new Vector2Int(1, Random.Range(26, 30));
                else
                    streamEnd = new Vector2Int(Random.Range(1, 5), 30);
                break;
            case directions.TopLeftCorner:
                streamStart = new Vector2Int(1, Random.Range(7, 24));
                streamEnd = new Vector2Int(Random.Range(7, 24), 30);
                break;
            case directions.TopRightCorner:
                streamStart = new Vector2Int(30, Random.Range(7, 24));
                streamEnd = new Vector2Int(Random.Range(7, 24), 30);
                break;
            case directions.BottomLeftCorner:
                streamStart = new Vector2Int(1, Random.Range(7, 24));
                streamEnd = new Vector2Int(Random.Range(7, 24), 1);
                break;
            case directions.BottomRightCorner:
                streamStart = new Vector2Int(30, Random.Range(7, 24));
                streamEnd = new Vector2Int(Random.Range(7, 24), 1);
                break;
            default:
                streamStart = new Vector2Int(1, Random.Range(7, 24));
                streamEnd = new Vector2Int(30, Random.Range(7, 24));
                break;
        }

        //Centre creation loop: create list of tiles that make up the main river line
        List <Vector2Int> flow = new List<Vector2Int> { streamStart, streamEnd };
        bool connectionComplete = false;
        while (!connectionComplete)
        {
            int flowIndex = 0;
            float markedDistance = 0;
            bool found = false;
            Vector2Int markedStart = new Vector2Int(0,0);
            Vector2Int markedEnd = new Vector2Int(0, 0);
            //Check if the connection between all points has been created
            while (flowIndex < flow.Count-1 && !found)
            {
                markedStart = flow[flowIndex];
                markedEnd = flow[flowIndex+1];
                markedDistance = MapController.Instance.calcDistance(markedStart, markedEnd);
                //Mark two points that have not been connected yet
                if (markedDistance > 1.5)
                    found = true;
                flowIndex++;
            }
            if (found)
            {
                //Determine the marked points' midpoint
                Vector2Int midPoint = new Vector2Int(markedStart[0] + (markedEnd[0] - markedStart[0]) / 2, markedStart[1] + (markedEnd[1] - markedStart[1]) / 2);
                //Determine the maximum variation the midpoint can have based on the orientation generated
                float maxVariationX = 0;
                float maxVariationY = 0;
                //Straight:
                if (streamdirection == directions.Vertical || streamdirection == directions.Horizontal)
                {
                    if (markedDistance > 10)
                    {
                        maxVariationX = markedDistance / 4;
                        maxVariationY = markedDistance / 4;
                    }
                    else
                    {
                        maxVariationX = markedDistance / 2;
                        maxVariationY = markedDistance / 2;
                    }
                        
                }
                //Diagonal:
                else if (streamdirection == directions.DiagonalNorthEast || streamdirection == directions.DiagonalNorthWest)
                {
                    if (markedDistance > 10)
                    {
                        maxVariationX = markedDistance / 3;
                        maxVariationY = markedDistance / 3;
                    }
                    else
                    {
                        maxVariationY = markedDistance / 2;
                        maxVariationY = markedDistance / 2;
                    }
                }
                //corners:
                else{
                    maxVariationX = System.Math.Abs(markedStart[0] - markedEnd[0]) / 2;
                    maxVariationX = System.Math.Abs(markedStart[1] - markedEnd[1]) / 2;
                }
                //Apply random variation in the ranges determined to the "midpoint"
                int negative = Random.Range(0, 1);
                midPoint[0] += Random.Range(0, (int)maxVariationX)*(1 + negative*-2);
                negative = Random.Range(0, 1);
                midPoint[1] += Random.Range(0, (int)maxVariationY) * (1 + negative * -2);

                flow.Insert(flowIndex, midPoint);
            }
            else
            {
                connectionComplete = true;
            }
        }
        //Randomly determine the stream's width
        float width = Random.Range(3,6)/2;
        // now that the main flow is present, determine stream width and mark all other cells
        List <Vector2Int> secondaryFlow = new List<Vector2Int>();
        foreach (Vector2Int tile in MapController.Instance.map.Keys)
        {
            foreach (Vector2Int cell in flow)
            {
                if (MapController.Instance.calcDistance(cell,tile) <= width)
                {
                    if (!secondaryFlow.Contains(tile))
                        secondaryFlow.Add(tile);
                }
            }
        }
        // final step: set appearance of all cells between flow and overflow to "ShallowWater"
        foreach (Vector2Int cell in flow)
        {
            if (cell[0] > 0 && cell[1] > 0 && cell[0] <31 && cell[1] < 31)
            {
                MapController.Instance.map[cell].terrain = TileController.TerrainType.ShallowWater;
                MapController.Instance.map[cell].TileGen();
            }
        }

        foreach (Vector2Int cell in secondaryFlow)
        {
            if (cell[0] > 0 && cell[1] > 0 && cell[0] < 31 && cell[1] < 31)
            {
                MapController.Instance.map[cell].terrain = TileController.TerrainType.ShallowWater;
                MapController.Instance.map[cell].TileGen();
            }
        }

        //Extra step: determine if there are unreachable map parts
        //Mark one part as "Reachable", connect another unreachable part to it, repeat
    }

    private void genWatersideOverwrite()
    {

    }

    public enum Regions {
        Ports,
        Farms,
        TradeRoads,
        Beaches,
        Estuary,
        Plains,
        CloseWoods,
        RiverLands,
        BorderCity,
        Cliffs,
        CliffsideOutposts,
        DeepSwamp,
        SwampEdge,
        Hillside,
        Highlands,
        Isles,
        HillsideClimb,
        MountainPath,
        TheWall,
        WildWoods,
        Savannah,
        LonePeaks,
        MountainOutpost,
        Mines,
        OutlawsHideout,
        Desert,
        OldRoad,
        LowCityRuins,
        UndergroundPassages,
        AbandonedRoads,
        IvorySpire
    }
    private enum directions
    {
        Vertical,
        Horizontal,
        DiagonalNorthEast,
        DiagonalNorthWest,
        TopLeftCorner,
        TopRightCorner,
        BottomLeftCorner,
        BottomRightCorner
    }
}