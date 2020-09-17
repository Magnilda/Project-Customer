using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    //The different tiletypes.
    public enum TileType
    {
        EARTH,
        WATER
    }

    //X and Y coord on the map and not in the world.
    private float x;
    private float z;
    private GameObject leftTile;
    private GameObject rightTile;
    private GameObject topLeftTile;
    private GameObject topRightTile;
    private GameObject bottomLeftTile;
    private GameObject bottomRightTile;
    private TileType type;

    //====================================================
    //               Getter and Setters
    //====================================================
    public float X { get => x; set => x = value; }
    public float Z { get => z; set => z = value; }
    public GameObject LeftTile { get => leftTile; set => leftTile = value; }
    public GameObject RightTile { get => rightTile; set => rightTile = value; }
    public GameObject TopLeftTile { get => topLeftTile; set => topLeftTile = value; }
    public GameObject TopRightTile { get => topRightTile; set => topRightTile = value; }
    public GameObject BottomLeftTile { get => bottomLeftTile; set => bottomLeftTile = value; }
    public GameObject BottomRightTile { get => bottomRightTile; set => bottomRightTile = value; }
    public TileType Type { get => type; set => type = value; }

    //======================================================
    //                   GetNeighbours()
    //======================================================
    public List<GameObject> GetNeighbours()
    {
        List<GameObject> existingNeighbours = new List<GameObject>();

        if (leftTile != null)
            existingNeighbours.Add(leftTile);
        if (rightTile != null)
            existingNeighbours.Add(rightTile);
        if (topLeftTile != null)
            existingNeighbours.Add(topLeftTile);
        if (topRightTile != null)
            existingNeighbours.Add(topRightTile);
        if (bottomLeftTile != null)
            existingNeighbours.Add(bottomLeftTile);
        if (bottomRightTile != null)
            existingNeighbours.Add(bottomRightTile);

        return existingNeighbours;
    }



    //----------------------DEBUG-------------------------------
    //======================================================
    //              ChangeColor(Color color)
    //======================================================
    public void ChangeColor(Color color)
    {
        MeshRenderer mRender = this.GetComponentInChildren<MeshRenderer>();
        mRender.material.color = color;
    }

    //======================================================
    //          ChangeNeighboursColor(Color color)
    //======================================================
    public void ChangeNeighboursColor(Color color)
    {
        List<GameObject> existingNeighbours = new List<GameObject>();

        if (leftTile != null)
            existingNeighbours.Add(leftTile);
        if (rightTile != null)
            existingNeighbours.Add(rightTile);
        if (topLeftTile != null)
            existingNeighbours.Add(topLeftTile);
        if (topRightTile != null)
            existingNeighbours.Add(topRightTile);
        if (bottomLeftTile != null)
            existingNeighbours.Add(bottomLeftTile);
        if (bottomRightTile != null)
            existingNeighbours.Add(bottomRightTile);

        foreach (GameObject tile in existingNeighbours)
        {
            MeshRenderer mRender = tile.GetComponentInChildren<MeshRenderer>();
            mRender.material.color = color;
        }
    }
}
