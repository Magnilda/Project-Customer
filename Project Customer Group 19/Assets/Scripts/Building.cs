using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public enum BuildingType
    {
        WATERFACILITY,
        FARM,
        HOUSE,
        UPGRADEDHOUSE,
        TOWNHALL
    }
    [Header("Building Settings")]
    [SerializeField] private int buildingCost;
    [SerializeField] private BuildingType type;

    [Header("Debug Settings")]
    [SerializeField] private GameObject tile;
    [SerializeField] private bool hasCamerman = false;

    public BuildingType Type { get => type; set => type = value; }
    public GameObject Tile { get => tile; set => tile = value; }
    public bool HasCamerman { get => hasCamerman; set => hasCamerman = value; }
}
