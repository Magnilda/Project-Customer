using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TilemapGenerator : MonoBehaviour
{
    [Header("Tilemap settings")]
    //public static TilemapGenerator tileMapGenerator;
    [SerializeField] private LandTileList landPrefabList;
    [SerializeField] private GameObject waterPrefab;

    //Size of map in terms of the number of hexagons.
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float noiseScale = 0.8f;
    [SerializeField] private int seed;

    [Header("Debug settings")]
    [SerializeField] private float xNoiseOffset;
    [SerializeField] private float zNoiseOffset;

    //The offset determines the rightful location of the hexagon tile.
    [SerializeField] private float xOffset = 0.886f;
    [SerializeField] private float zOffset = 0.766f;
    private GameObject[,] hexTilemapArray;

    //======================================================
    //                    Start()
    //======================================================
    // Start is called before the first frame update
    void Start()
    {
        //tileMapGenerator = this;
        UnityEngine.Random.InitState(seed);
        RandomizeNoiseOffset();
        CreateHexTileMap();
        setTileNeighbours();
    }

    //======================================================
    //                 CreateHexTileMap()
    //======================================================
    private void CreateHexTileMap()
    {
        hexTilemapArray = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                //Generate noise and determine tiletype.
                float noiseValue = calculateNoiseValue(x, z);
                //Debug.Log(noiseValue);
                GameObject tempTileType;
                if (noiseValue < 0.3f)
                {
                    tempTileType = waterPrefab;
                }
                else
                {
                    tempTileType = landPrefabList.pickRandomLandTile();
                }

                //Offset to postion the hexagons on the map.
                float xPos = x * xOffset;
                if (z % 2 == 1)
                {
                    xPos += xOffset * 0.5f;
                }
                int temp = UnityEngine.Random.Range(0, 6);
                GameObject tempHexTile = Instantiate(tempTileType, new Vector3(xPos, 0, z * zOffset), Quaternion.Euler(0, temp * 60, 0)); //identity means no rotations

                //Setting parent and name of tile.
                tempHexTile.transform.parent = transform;
                tempHexTile.name = "Hex: " + x + " , " + z;

                //Setting location and type of tile in the tile component.
                tempHexTile.GetComponent<Tile>().X = x;
                tempHexTile.GetComponent<Tile>().Z = z;

                if (tempHexTile.GetComponent<LandTile>() != null)
                {
                    tempHexTile.GetComponent<Tile>().Type = Tile.TileType.EARTH;
                }
                if (tempHexTile.GetComponent<WaterTile>() != null)
                {
                    tempHexTile.GetComponent<Tile>().Type = Tile.TileType.WATER;
                }

                //Insert into array
                hexTilemapArray[x, z] = tempHexTile;
            }
        }
    }

    //======================================================
    //               calculateTileType()
    //======================================================
    private float calculateNoiseValue(float x, float z)
    {
        float value = Mathf.PerlinNoise((x * noiseScale + xNoiseOffset), (z * noiseScale + zNoiseOffset));
        return value;
    }

    //======================================================
    //               RandomizeNoiseOffset()
    //======================================================
    private void RandomizeNoiseOffset()
    {
        xNoiseOffset = UnityEngine.Random.Range(0f, 100f);
        zNoiseOffset = UnityEngine.Random.Range(0f, 100f);
    }

    //======================================================
    //                 setTileNeigbours()
    //======================================================
    private void setTileNeighbours()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject currentHexTile = hexTilemapArray[x, z];

                //The current lines of code below is a shorter way to write an if statement
                //First u create a variable
                //if statement ? yes do this else do this.

                //2 in current row
                bool index = CheckIndexExist((x - 1), z);
                currentHexTile.GetComponent<Tile>().LeftTile = index ? hexTilemapArray[(x - 1), z] : null;
                index = CheckIndexExist((x + 1), z);
                currentHexTile.GetComponent<Tile>().RightTile = index ? hexTilemapArray[(x + 1), z] : null;


                int idx_offs = z % 2 != 0 ? 0 : 1;

                // 2 in row above
                index = CheckIndexExist(x - idx_offs, (z + 1));
                currentHexTile.GetComponent<Tile>().TopLeftTile = index ? hexTilemapArray[x - idx_offs, (z + 1)] : null;
                index = CheckIndexExist((x + 1) - idx_offs, (z + 1));
                currentHexTile.GetComponent<Tile>().TopRightTile = index ? hexTilemapArray[(x + 1) - idx_offs, (z + 1)] : null;

                // 2 in row below
                index = CheckIndexExist(x - idx_offs, (z - 1));
                currentHexTile.GetComponent<Tile>().BottomLeftTile = index ? hexTilemapArray[x - idx_offs, (z - 1)] : null;
                index = CheckIndexExist((x + 1) - idx_offs, (z - 1));
                currentHexTile.GetComponent<Tile>().BottomRightTile = index ? hexTilemapArray[(x + 1) - idx_offs, (z - 1)] : null;

            }
        }

    }

    //======================================================
    //         GetSingleDigitIndex(int x, int z)
    //======================================================
    private bool CheckIndexExist(int x, int z)
    {
        if ((x >= 0 && x < width) && (z >= 0 && z < height))
            return true;
        return false;
    }

    //======================================================
    //                 TilemapArrayGetter()
    //======================================================
    public GameObject[,] GetTilemapArray()
    {
        return hexTilemapArray;
    }
}
