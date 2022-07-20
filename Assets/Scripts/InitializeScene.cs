using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class InitializeScene : MonoBehaviour
{
    public bool MapInitComplete;
    public int width;
    public int height;

    public MapGrid mapGridObj;

    public GameObject tileObj;
    public GameObject playerObj;
    public GameObject cameraFollow;

    public GameObject itemObj;
    public GameObject enemyObj;
    // Start is called before the first frame update
    void Start()
    {
        MapInitComplete = false;
        mapGridObj = new MapGrid(width, height);

        //MapGenerator.GenerateMapGrid(mapGridObj);
        var randomWidth = Random.Range(0, 50);
        var randomHeight = Random.Range(0, 50);

        mapGridObj.mapGrid[randomWidth][randomHeight] = 1;

        for (var x = 0; x < mapGridObj.mapGrid.Length; x++)
        {
            var column = mapGridObj.mapGrid[x];
            for (var y = 0; y < column.Length; y++)
            {
                var cell = column[y];
                if (cell == 1)
                {
                    Debug.Log($"{x}, {y} contains {cell}");
                }

                if (Random.Range(0, 4) != 3)
                {
                    mapGridObj.tileGrid[x][y].tileObj = Instantiate(tileObj, new Vector3(x, y, 0), Quaternion.identity);
                    if (Random.Range(0, 20) == 0)
                    {
                        Instantiate(itemObj, new Vector3(x, y, 0), Quaternion.identity);
                    }
                    else if (Random.Range(0, 20) == 1)
                    {
                        Instantiate(enemyObj, new Vector3(x, y, 0), Quaternion.identity);
                    }
                }
            }
        }

        var player = Instantiate(playerObj, new Vector3(0, 0, 0), Quaternion.identity);
        cameraFollow.GetComponent<CameraFollow>().player = player.GetComponent<PlayerController>();
        MapInitComplete = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class MapTile
{
    public GameObject tileObj;
}

public class MapGrid
{
    public int[][] mapGrid;
    public MapTile[][] tileGrid;

    public MapGrid(int width, int height)
    {
        mapGrid = new int[width][];
        tileGrid = new MapTile[width][];
        for (var x = 0; x < mapGrid.Length; x++)
        {
            mapGrid[x] = new int[height];
            tileGrid[x] = new MapTile[height];
            for (int y = 0; y < height; y++)
            {
                tileGrid[x][y] = new MapTile();
            }
            Array.Clear(mapGrid[x], 0 , mapGrid.Length);
        }
    }
}

public class MapGenerator
{
    public static void GenerateMapGrid(MapGrid map)
    {
        for (var x = 0; x < map.mapGrid.Length; x++)
        {
            var column = map.mapGrid[x];
            for (var y = 0; y < column.Length; y++)
            {
                //map.mapGrid[x][y] = 0;
            }
        }
        
    }
}