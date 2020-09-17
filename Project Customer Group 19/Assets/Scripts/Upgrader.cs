using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    [SerializeField] private GameObject upgradedHousePrefab;
    [SerializeField] private GameObject cameraPrefab;
    [SerializeField] private TownHall townHall;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cameraButton;
    [SerializeField] private float yOffsetHouse;
    [SerializeField] private float yOffsetCamera;

    private bool upgradeMode = false;
    private bool upgradeHouse = false;
    private bool buyCameraman = false;
    private GameObject selectedGameObject;

    //=================================================================
    //                          Start()
    //=================================================================
    void Start()
    {
        DisableButtons();
    }

    //=================================================================
    //                          Update()
    //=================================================================
    void Update()
    {
        checkMouseClick();
        HandleUpgrading();
        if (townHall == null) { townHall = FindObjectOfType<TownHall>(); }
    }

    //=================================================================
    //                      HandleUpgrading()
    //=================================================================
    private void HandleUpgrading()
    {
        if (upgradeMode == true)
        {
            if (upgradeHouse)
            {
                Vector3 tempPos = selectedGameObject.transform.position;
                GameObject tempTile = selectedGameObject.GetComponentInParent<Building>().Tile;
                Quaternion rotToSetTo = selectedGameObject.transform.rotation;
                bool _hasCameraman = selectedGameObject.GetComponent<Building>().HasCamerman;

                GameObject newBuilding = Instantiate(upgradedHousePrefab, new Vector3(tempPos.x, yOffsetHouse, tempPos.z), Quaternion.identity);
                newBuilding.transform.rotation = rotToSetTo;
                newBuilding.GetComponent<Building>().HasCamerman = _hasCameraman;
                newBuilding.GetComponentInParent<Building>().Tile = tempTile;
                tempTile.GetComponentInParent<LandTile>().Building = newBuilding;
                townHall.UpgradedHouseAmount++;
                townHall.HouseAmount--;

                Destroy(selectedGameObject);
                upgradeHouse = false;
            }

            if (buyCameraman == true)
            {
                Vector3 tempPos = selectedGameObject.transform.position;

                GameObject newBuilding = Instantiate(cameraPrefab, new Vector3(tempPos.x, yOffsetCamera, tempPos.z), Quaternion.identity);
                selectedGameObject.GetComponent<Building>().HasCamerman = true;
                townHall.CameraManAmount++;

                buyCameraman = false;
            }
        }

    }

    //=================================================================
    //                      checkMouseClick()
    //=================================================================
    private void checkMouseClick()
    {
        GameObject collidingObject = GetHitObject();

        if (collidingObject == null) { return; }

        if (collidingObject.GetComponentInParent<Building>())
        {
            //Debug.Log("Found building component");
            if (collidingObject.GetComponentInParent<Building>().Type == Building.BuildingType.HOUSE && Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Building type is a house");
                upgradeMode = true;
                selectedGameObject = collidingObject;
                cancelButton.gameObject.SetActive(true);
                upgradeButton.gameObject.SetActive(true);
                if (!collidingObject.GetComponentInParent<Building>().HasCamerman) { cameraButton.gameObject.SetActive(true); }
            }

            if (collidingObject.GetComponentInParent<Building>().Type == Building.BuildingType.UPGRADEDHOUSE && Input.GetMouseButtonDown(0))
            {
                upgradeMode = true;
                selectedGameObject = collidingObject;
                cancelButton.gameObject.SetActive(true);
                cameraButton.gameObject.SetActive(true); ;
            }

            if (collidingObject.GetComponentInParent<Building>().Type == Building.BuildingType.UPGRADEDHOUSE && 
                collidingObject.GetComponentInParent<Building>().HasCamerman && Input.GetMouseButtonDown(0))
            {
                upgradeMode = false;
                DisableButtons();
            }
        }
    }

    //=================================================================
    //                      DisableButtons()
    //=================================================================
    private void DisableButtons()
    {
        cancelButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        cameraButton.gameObject.SetActive(false);
    }

    //=================================================================
    //                     GetHitObject()
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


    public bool UpgradeMode { set => upgradeMode = value; }
    public bool UpgradeHouse { set => upgradeHouse = value; }
    public bool BuyCameraman { set => buyCameraman = value; }
}
