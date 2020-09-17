using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Builder : MonoBehaviour
{
    public enum BuildingType
    {
        WATERFACILITY,
        HOUSE
    }
    [Header("Builder Settings")]
    [SerializeField] private GameObject housePrefab;
    [SerializeField] private GameObject townHallPrefab;
    [SerializeField] private GameObject waterPumpPrefab;

    [Header("Debug Settings")]
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button buildButton;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TownHall townHall;
    [SerializeField] private float yOffsetTownHall;
    [SerializeField] private float yOffsetWaterpump;
    [SerializeField] private float yOffsetHouse;
    private GameObject buildingPrefab;
    private bool canBuild = false;
    private bool townHallMode = false;
    private BuildingType buildingType;
    private int price;

    //=================================================================
    //                           Update()
    //=================================================================
    public void Update()
    {
        //HandlePrefabSelection();
        HandleBuilding();
        HandleTownHallBuilding();
    }

    //=================================================================
    //                       HandlePrefabSelection()
    //=================================================================
    private void HandlePrefabSelection()
    {
        //if (buildingType == BuildingType.TOWNHALL) { buildingPrefab = townHallPrefab; }
        if (buildingType == BuildingType.HOUSE) { buildingPrefab = housePrefab; }
        if (buildingType == BuildingType.WATERFACILITY) { buildingPrefab = waterPumpPrefab; }

    }

    //=================================================================
    //                      HandleTownHallBuilding()
    //=================================================================
    private void HandleTownHallBuilding()
    {
        if (townHallMode == true)
        {
            GameObject hitObject = GetHitObject();

            if (hitObject == null) { return; }

            if (hitObject.GetComponentInParent<LandTile>() && Input.GetMouseButtonDown(0))
            {
                if (hitObject.GetComponentInParent<LandTile>().TileOccupied == false)
                {
                    float x = hitObject.transform.position.x;
                    float z = hitObject.transform.position.z;
                    GameObject tempBuilding = Instantiate(townHallPrefab, new Vector3(x, yOffsetTownHall, z), Quaternion.identity);
                    hitObject.GetComponentInParent<LandTile>().Building = tempBuilding;
                    tempBuilding.GetComponentInParent<Building>().Tile = hitObject;
                    townHall = tempBuilding.GetComponent<TownHall>();
                    SpawnHouses(5, hitObject);

                    townHallMode = false;
                    gameManager.CurrentStage = GameManager.GameStage.GAME;
                }

            }
        }
    }

    //=================================================================
    //             SpawnHouses(int amount, GameObject tile)
    //=================================================================
    private void SpawnHouses(int amount, GameObject tile)
    {
        List<GameObject> neighbours = tile.GetComponentInParent<Tile>().GetNeighbours();

        for (int num = 0; num < amount; num++)
        {
            GameObject _tile = neighbours[num];

            if (_tile.GetComponent<LandTile>())
            {
                if (!_tile.GetComponent<LandTile>().TileOccupied)
                {
                    float xPos = _tile.transform.position.x;
                    float zPos = _tile.transform.position.z;

                    int temp = UnityEngine.Random.Range(0, 6);
                    GameObject tempBuilding = Instantiate(housePrefab, new Vector3(xPos, yOffsetHouse, zPos), Quaternion.Euler(0, temp * 60, 0));
                    _tile.GetComponentInParent<LandTile>().Building = tempBuilding;
                    tempBuilding.GetComponentInParent<Building>().Tile = _tile;
                    townHall.HouseAmount++;
                }
                //else
                //{
                //    foreach (var neighbour in _tile.GetComponent<Tile>().GetNeighbours())
                //    {
                //        if (neighbour.GetComponent<LandTile>() && !neighbour.GetComponent<LandTile>().TileOccupied)
                //        {
                //            float xPos = neighbour.transform.position.x;
                //            float zPos = neighbour.transform.position.z;

                //            int temp = UnityEngine.Random.Range(0, 6);
                //            GameObject tempBuilding = Instantiate(housePrefab, new Vector3(xPos, yOffsetHouse, zPos), Quaternion.Euler(0, temp * 60, 0));
                //            neighbour.GetComponentInParent<LandTile>().Building = tempBuilding;
                //            tempBuilding.GetComponentInParent<Building>().Tile = neighbour;
                //            townHall.HouseAmount++;

                //            return;
                //        }
                //    }
                //}
            }
            else
            {
                //foreach (var neighbour in _tile.GetComponent<Tile>().GetNeighbours())
                //{
                //    if (neighbour.GetComponent<LandTile>() && !neighbour.GetComponent<LandTile>().TileOccupied)
                //    {
                //        float xPos = neighbour.transform.position.x;
                //        float zPos = neighbour.transform.position.z;

                //        int temp = UnityEngine.Random.Range(0, 6);
                //        GameObject tempBuilding = Instantiate(housePrefab, new Vector3(xPos, yOffsetHouse, zPos), Quaternion.Euler(0, temp * 60, 0));
                //        neighbour.GetComponentInParent<LandTile>().Building = tempBuilding;
                //        tempBuilding.GetComponentInParent<Building>().Tile = neighbour;
                //        townHall.HouseAmount++;

                //        return;
                //    }
                //}
            }
        }
    }

    //=================================================================
    //                     HandleBuildingMode()
    //=================================================================
    private void HandleBuilding()
    {
        if (canBuild == true)
        {
            GameObject hitObject = GetHitObject();

            if (hitObject == null) { return; }

            if (hitObject.GetComponentInParent<WaterTile>() && Input.GetMouseButtonDown(0) && !isMouseOverUI())
            {
                if (hitObject.GetComponentInParent<WaterTile>().TileOccupied == false)
                {
                    Purchase.PurchaseItem(price);
                    float x = hitObject.transform.position.x;
                    float z = hitObject.transform.position.z;
                    GameObject tempBuilding = Instantiate(waterPumpPrefab, new Vector3(x, yOffsetWaterpump, z), Quaternion.identity);

                    hitObject.GetComponentInParent<WaterTile>().Building = tempBuilding;
                    tempBuilding.GetComponentInParent<Building>().Tile = hitObject;
                    townHall.WaterPumpAmount++;
                    canBuild = false;
                    cancelButton.gameObject.SetActive(false);
                    buildButton.gameObject.SetActive(true);
                }
            }
        }
    }

    //=================================================================
    //                   ChangePrice(int _price)
    //=================================================================
    public void ChangePrice(int _price)
    {
        price = _price;
    }

    //=================================================================
    //                        GetHitObject()
    //=================================================================
    private GameObject GetHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit entryPoint = new RaycastHit();

        if (Physics.Raycast(ray, out entryPoint))
        {
            GameObject hitObject = entryPoint.collider.transform.gameObject;
            return hitObject;
        }
        else
        {
            return null;
        }
    }

    //=================================================================
    //                       isMouseOverUI()
    //=================================================================
    private bool isMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }


    //-----------------------------------------------------------------
    public bool CanBuild { get => canBuild; set => canBuild = value; }
    public BuildingType BuildingType1 { get => buildingType; set => buildingType = value; }
    public bool TownHallMode { get => townHallMode; set => townHallMode = value; }
}
