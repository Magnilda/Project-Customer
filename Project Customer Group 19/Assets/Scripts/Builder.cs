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
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TownHall townHall;
    private GameObject buildingPrefab;
    private bool canBuild = false;
    private bool townHallMode = false;
    private BuildingType buildingType;

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
                    GameObject tempBuilding = Instantiate(townHallPrefab, new Vector3(x, 0, z), Quaternion.identity);

                    hitObject.GetComponentInParent<LandTile>().Building = tempBuilding;
                    townHall = tempBuilding.GetComponent<TownHall>();
                    townHallMode = false;
                    gameManager.CurrentStage = GameManager.GameStage.GAME;
                }

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
                    float x = hitObject.transform.position.x;
                    float z = hitObject.transform.position.z;
                    GameObject tempBuilding = Instantiate(waterPumpPrefab, new Vector3(x, 0, z), Quaternion.identity);

                    hitObject.GetComponentInParent<WaterTile>().Building = tempBuilding;
                    townHall.WaterPumpAmount++;
                    canBuild = false;
                    cancelButton.gameObject.SetActive(false);
                }
            }
        }
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
