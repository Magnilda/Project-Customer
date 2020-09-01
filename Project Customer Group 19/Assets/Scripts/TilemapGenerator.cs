using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{
    public GameObject hexPrefab;
    public GameObject[,] hexTilemapArray; 

    //Size of map in terms of the number of hexagons.
    [SerializeField] int width = 20;
    [SerializeField] int height = 20;

    //The offset determines the rightful location of the hexagon tile.
    [SerializeField] float xOffset = 0.886f;
    [SerializeField] float zOffset = 0.766f;

    //======================================================
    //                    Start()
    //======================================================
    // Start is called before the first frame update
    void Start()
    {
        CreateHexTileMap();
    }

    //======================================================
    //                 CreateHexTileMap()
    //======================================================
    private void CreateHexTileMap()
    {
        hexTilemapArray = new GameObject[width,height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                float xPos = x * xOffset;

                if (z % 2 == 1)
                {
                    xPos += xOffset * 0.5f;
                }

                GameObject tempHexTile =  Instantiate(hexPrefab, new Vector3(xPos, 0, z * zOffset), Quaternion.identity); //identity means no rotations
                hexTilemapArray[x, z] = tempHexTile;
            }
        }
    }

    //======================================================
    //                 TilemapArrayGetter()
    //======================================================
    public GameObject[,] TilemapArrayGetter()
    {
        return hexTilemapArray;
    }
}
