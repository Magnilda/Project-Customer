using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : Tile
{
    private bool tileOccupied = false;
    private GameObject building;   //building on the tile

    public void Update()
    {
        //If a building is on the tile set the bool to true
        if (building != null)
        {
            tileOccupied = true;
        }
    }

    public GameObject Building { get => building; set => building = value; }
    public bool TileOccupied { get => tileOccupied; set => tileOccupied = value; }
}
