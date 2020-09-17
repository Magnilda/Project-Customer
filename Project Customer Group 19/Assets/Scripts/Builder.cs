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
    [SerializeField] private GameObject farmPrefab;
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
    private uint level;

    [SerializeField] private ChangeText error;

    List<LandTile> openSet = new List<LandTile>();
    List<LandTile> closedSet = new List<LandTile>();

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
    //                      OnLevelStart()
    //=================================================================
    private void OnLevelStart()
    { 
        level++;
        switch(level)
        {
            case 0:
                break;
            case 1:
                ExpandVillage(2, 1);
                break;
            default:
                ExpandVillage(3, 1);
                break;
        }
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

            LandTile tile = hitObject.GetComponentInParent<LandTile>();
            if (tile && Input.GetMouseButtonDown(0))
            {
                if (tile.TileOccupied == false)
                {
                    float x = hitObject.transform.position.x;
                    float z = hitObject.transform.position.z;
                    GameObject tempBuilding = Instantiate(townHallPrefab, new Vector3(x, yOffsetTownHall, z), Quaternion.identity);
                    tile.Building = tempBuilding;
                    tempBuilding.GetComponentInParent<Building>().Tile = hitObject;
                    townHall = tempBuilding.GetComponent<TownHall>();

                    closedSet.Add(tile);
                    foreach (GameObject neighbour in tile.GetNeighbours())
                    {
                        LandTile neighbourTile = neighbour.GetComponent<LandTile>();
                        if (neighbourTile && !closedSet.Contains(neighbourTile) && !openSet.Contains(neighbourTile))
                            openSet.Add(neighbourTile);
                    }

                    ExpandVillage(5, 0);

                    TimeManager.OnNextDayAction += OnLevelStart; ///TODO: MEI REMAP THIS TO NEXT LEVEL INSTEAD OF NEXT DAY. Good luck with your deadline ;)

                    townHallMode = false;
                    gameManager.CurrentStage = GameManager.GameStage.GAME;
                }

            }
        }
    }

    //=================================================================
    //             ExpandVillage(uint houses, uint farms)
    //=================================================================
    void ExpandVillage(uint houses, uint farms)
    {
        if (openSet.Count == 0)
        {
            Debug.Log("Boats still need to be invented...");
            return;
        }
        List<LandTile> placementSet = new List<LandTile>(openSet);

        for (uint i = 0; i < houses; i++)
        {
            if(placementSet.Count == 0)
                placementSet = new List<LandTile>(openSet);

            LandTile tile = placementSet[Random.Range(0, placementSet.Count)];
            SpawnHouse(tile);
            openSet.Remove(tile);
            placementSet.Remove(tile);
            closedSet.Add(tile);
            foreach (GameObject neighbour in tile.GetNeighbours())
            {
                LandTile neighbourTile = neighbour.GetComponent<LandTile>();
                if (neighbourTile && !closedSet.Contains(neighbourTile) && !openSet.Contains(neighbourTile))
                    openSet.Add(neighbourTile);
            }
        }

        for (uint i = 0; i < farms; i++)
        {
            if (placementSet.Count == 0)
                placementSet = new List<LandTile>(openSet);

            LandTile tile = placementSet[Random.Range(0, placementSet.Count)];
            SpawnFarm(tile);
            openSet.Remove(tile);
            placementSet.Remove(tile);
            closedSet.Add(tile);
            foreach (GameObject neighbour in tile.GetNeighbours())
            {
                LandTile neighbourTile = neighbour.GetComponent<LandTile>();
                if (neighbourTile && !closedSet.Contains(neighbourTile) && !openSet.Contains(neighbourTile))
                    openSet.Add(neighbourTile);
            }
        }
    }

    //=================================================================
    //             SpawnHouse(GameObject tile)
    //=================================================================
    private void SpawnHouse(LandTile tile)
    {
        float xPos = tile.transform.position.x;
        float zPos = tile.transform.position.z;

        int temp = UnityEngine.Random.Range(0, 6);
        GameObject tempBuilding = Instantiate(housePrefab, new Vector3(xPos, yOffsetHouse, zPos), Quaternion.Euler(0, temp * 60, 0));
        tile.Building = tempBuilding;
        tempBuilding.GetComponentInParent<Building>().Tile = tile.gameObject;
        townHall.HouseAmount++;
        tile.OnBuildingPlaced();
    }

    //=================================================================
    //             SpawnFarm(GameObject tile)
    //=================================================================
    private void SpawnFarm(LandTile tile)
    {
        float xPos = tile.transform.position.x;
        float zPos = tile.transform.position.z;

        int temp = UnityEngine.Random.Range(0, 6);
        GameObject tempBuilding = Instantiate(farmPrefab, new Vector3(xPos, yOffsetHouse, zPos), Quaternion.Euler(0, temp * 60, 0));
        tile.Building = tempBuilding;
        tempBuilding.GetComponentInParent<Building>().Tile = tile.gameObject;
        townHall.FarmAmount++;
        tile.OnBuildingPlaced();
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
