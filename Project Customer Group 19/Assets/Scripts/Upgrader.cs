using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrader : MonoBehaviour
{
    [SerializeField] private GameObject upgradedHousePrefab;
    [SerializeField] private TownHall townHall;     //TODO add to townhall
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cameraButton;
    [SerializeField] private float yOffsetHouse;
    private bool upgradeMode = false;
    private bool upgradeHouse = false;
    private bool buyCameraman = false;
    private GameObject selectedGameObject;

    void Start()
    {
        DisableButtons();
    }

    void Update()
    {
        checkMouseClick();
        HandleUpgrading();
    }

    private void HandleUpgrading()
    {
        if (upgradeMode == true)
        {
            if (upgradeHouse)
            {
                Vector3 tempPos = selectedGameObject.transform.position;
                GameObject tempTile = selectedGameObject.GetComponentInParent<Building>().Tile;
                Quaternion rotToSetTo = selectedGameObject.transform.rotation;

                GameObject newBuilding = Instantiate(upgradedHousePrefab, new Vector3(tempPos.x, yOffsetHouse, tempPos.z), Quaternion.identity);
                newBuilding.transform.rotation = rotToSetTo;

                newBuilding.GetComponentInParent<Building>().Tile = tempTile;
                tempTile.GetComponentInParent<LandTile>().Building = newBuilding;

                Destroy(selectedGameObject);
                upgradeHouse = false;
            }
        }

        if (buyCameraman == true)
        {
            //TODO display cameraman
        }

    }

    private void checkMouseClick()
    {
        GameObject collidingObject = GetHitObject();

        if (collidingObject == null) { return; }

        if (collidingObject.GetComponentInParent<Building>())
        {
            //Debug.Log("Found building component");
            if (collidingObject.GetComponentInParent<Building>().Type == Building.BuildingType.HOUSE && Input.GetMouseButtonDown(0))
            {
                //Debug.Log("Building type is a townhall");
                upgradeMode = true;
                selectedGameObject = collidingObject;
                cancelButton.gameObject.SetActive(true);
                upgradeButton.gameObject.SetActive(true);
                cameraButton.gameObject.SetActive(true);
            }

            if (collidingObject.GetComponentInParent<Building>().Type == Building.BuildingType.UPGRADEDHOUSE && Input.GetMouseButtonDown(0))
            {
                upgradeMode = true;
                selectedGameObject = collidingObject;
                cancelButton.gameObject.SetActive(true);
                cameraButton.gameObject.SetActive(true); ;
            }
        }
    }

    private void DisableButtons()
    {
        cancelButton.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(false);
        cameraButton.gameObject.SetActive(false);
    }

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


    public bool UpgradeMode { get => upgradeMode; set => upgradeMode = value; }
    public bool UpgradeHouse { get => upgradeHouse; set => upgradeHouse = value; }
    public bool BuyCameraman { get => buyCameraman; set => buyCameraman = value; }
}
