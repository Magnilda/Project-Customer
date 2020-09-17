using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PurchasingButton : MonoBehaviour
{
    [SerializeField] private Upgrader upgrader;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button cameraButton;
    [SerializeField] private int price;
    private Currency wallet;

    //=================================================================
    //                         Start()
    //=================================================================
    void Start()
    {
        wallet = FindObjectOfType<Currency>();
    }

    //=================================================================
    //                     DisableButtons()
    //=================================================================
    public void DisableButtons()
    {
        upgradeButton.gameObject.SetActive(false);
        cameraButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);

        upgrader.BuyCameraman = false;
        upgrader.UpgradeHouse = false;
    }

    //=================================================================
    //                    EnableCameraman()
    //=================================================================
    public void EnableCameraman()
    {
        if (wallet.CurrentCurrency >= price)
        {
            upgrader.BuyCameraman = true;
            cameraButton.gameObject.SetActive(false);
        }
    }

    //=================================================================
    //                   EnableUpgradeHouse()
    //=================================================================
    public void EnableUpgradeHouse()
    {
        if (wallet.CurrentCurrency >= price)
        {
            Debug.Log("Order went through: " + wallet.CurrentCurrency + "," + price);
            upgrader.UpgradeHouse = true;
            upgradeButton.gameObject.SetActive(false);
            if (!cameraButton.gameObject.activeSelf) { cancelButton.gameObject.SetActive(false); }
            Purchase.PurchaseItem(price);
        }
    }
}
