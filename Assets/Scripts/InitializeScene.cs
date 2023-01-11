using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class InitializeScene : MonoBehaviour
{
    public bool MapInitComplete;
    public int width;
    public int height;

    public MapGrid mapGridObj;

    public GameObject tileObj;
    public GameObject wallObj;
    public PlayerController playerObj;
    public CameraFollow cameraFollow;

    public GameObject itemObj;
    public GameObject boxObj;
    public EnemyController enemyObj;

    [Range(0f,0.3f)]
    public float zoom = 0.2f;
    public Vector2 shift = new Vector2(0, 0);

    public float noiseLevel = 0.55f;

    // Start is called before the first frame update
    void Start()
    {
        MapInitComplete = false;
        mapGridObj = new MapGrid(width, height);

        //MapGenerator.GenerateMapGrid(mapGridObj);
        var randomWidth = Random.Range(0, 50);
        var randomHeight = Random.Range(0, 50);

        //mapGridObj.mapGrid[randomWidth][randomHeight] = 1;

        GenerateMap();
        GenerateWalls();
        GenerateItems();
        PlacePlayer();
        
        MapInitComplete = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateMap()
    {
        int random = Random.Range(0, 10000);
        shift = new Vector2(Random.Range(0, 20000), Random.Range(0, 20000));
        for (var x = 0; x < mapGridObj.mapGrid.Length; x++)
        {
            var column = mapGridObj.mapGrid[x];
            for (var y = 0; y < column.Length; y++)
            {
                Vector2 pos = zoom * (new Vector2(x, y)) + shift;
                var noise = Mathf.PerlinNoise(pos.x, pos.y);

                if (noise < noiseLevel)
                {
                    mapGridObj.tileGrid[x][y].tileObj = Instantiate(tileObj, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    }

    public void GenerateWalls()
    {
        for (var x = 0; x < mapGridObj.mapGrid.Length; x++)
        {
            var column = mapGridObj.mapGrid[x];
            for (var y = 0; y < column.Length; y++)
            {
                var tile = mapGridObj.tileGrid[x][y].tileObj;
                if (tile == null)
                {
                    mapGridObj.tileGrid[x][y].tileObj = Instantiate(wallObj, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    }

    public void GenerateItems()
    {
        for (var x = 0; x < mapGridObj.mapGrid.Length; x++)
        {
            var column = mapGridObj.mapGrid[x];
            for (var y = 0; y < column.Length; y++)
            {
                var tile = mapGridObj.tileGrid[x][y].tileObj;
                if (tile.GetComponent<Collider2D>() == null)
                {
                    if (Random.Range(0, 4) != 3)
                    {
                        if (Random.Range(0, 20) == 0)
                        {
                            Instantiate(itemObj, new Vector3(x, y, 0), Quaternion.identity);
                        }
                        else if (Random.Range(0, 20) == 0)
                        {
                            var enemy = Instantiate(enemyObj, new Vector3(x, y, 0), Quaternion.identity);
                            if (Random.Range(0, 10) == 0)
                            {
                                enemy.gameObject.transform.localScale = new Vector3(1, 1, 1);
                                enemy.health = 50;
                            }
                        }
                        else if (Random.Range(0, 20) == 0)
                        {
                            Instantiate(boxObj, new Vector3(x, y, 0), Quaternion.identity);
                        }
                    }

                }
            }
        }
    }

    public void PlacePlayer()
    {
        bool placed = false;
        while (!placed)
        {
            var x = Random.Range(0, width);
            var y = Random.Range(0, height);
            var tile = mapGridObj.tileGrid[x][y].tileObj.GetComponent<Collider2D>();
            if (tile == null)
            {
                PlayerController player = Instantiate(playerObj, new Vector3(x, y, 0), Quaternion.identity);
                CameraFollow camera = Instantiate(cameraFollow, new Vector3(x, y, 0), Quaternion.identity);
                camera.player = player;
                placed = true;
                break;
            }
        }
        
    }

    public void ClearMap()
    {
        for (var x = 0; x < mapGridObj.mapGrid.Length; x++)
        {
            var column = mapGridObj.mapGrid[x];
            for (var y = 0; y < column.Length; y++)
            {
                Destroy(mapGridObj.tileGrid[x][y].tileObj);
                mapGridObj.tileGrid[x][y].tileObj = null;
            }
        }
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