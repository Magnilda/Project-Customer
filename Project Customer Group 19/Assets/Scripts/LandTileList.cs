using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandTileList : MonoBehaviour
{
    [SerializeField] private List<GameObject> landTiles = new List<GameObject>();

    public GameObject pickRandomLandTile()
    {
        if (landTiles.Count <= 0)
        {
            return null;
        }

        GameObject tempObject;
        int randomIndex = Random.Range(0, landTiles.Count);
        tempObject = landTiles[randomIndex];

        return tempObject;
    }
}
