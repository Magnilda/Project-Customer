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
        TOWNHALL
    }

    [SerializeField] private int buildingCost;
    [SerializeField] private BuildingType type;

    public BuildingType Type { get => type; set => type = value; }
}
