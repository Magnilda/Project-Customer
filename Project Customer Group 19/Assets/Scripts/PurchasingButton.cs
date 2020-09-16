using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchasingButton : MonoBehaviour
{
    [SerializeField] private Upgrader upgrader;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cameraButton;

    public void DisableButtons()
    {
        upgradeButton.gameObject.SetActive(false);
        cameraButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        upgrader.BuyCameraman = false;
        upgrader.UpgradeHouse = false;
    }

    public void EnableCameraman()
    {
        upgrader.BuyCameraman = true;
        cameraButton.gameObject.SetActive(false);
    }

    public void EnableUpgradeHouse()
    {
        upgrader.UpgradeHouse = true;
        upgradeButton.gameObject.SetActive(false);
    }
}
