using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    [Header("Currency Settings")]
    [SerializeField] private int startingCurrency;
    [SerializeField] private int startingWaterAmount;
    [SerializeField] private Text currencyText;
    [SerializeField] private Text waterText;

    [Header("Debug Settings")]
    private int currentCurrency;
    private int currentWaterAmount;
    private int subtractAmount;
    private int addAmount;
    private int addWaterAmount;

    //=================================================================
    //                           Start()
    //=================================================================
    public void Start()
    {
        currentCurrency = startingCurrency;
        currentWaterAmount = startingWaterAmount;
        UpdateCurrency();
        UpdateWater();

        Purchase.OnPurchase += HandleSubtraction;
        TimeManager.OnNextDayAction += HandleAdding;
        TimeManager.OnNextDayAction += HandleWaterChanges;
    }

    //=================================================================
    //                          OnDestroy()
    //=================================================================
    public void OnDestroy()
    {
        Purchase.OnPurchase -= HandleSubtraction;
        TimeManager.OnNextDayAction -= HandleAdding;
        TimeManager.OnNextDayAction -= HandleWaterChanges;
    }

    //=================================================================
    //                        HandleAdding()
    //=================================================================
    private void HandleAdding()
    {
        AddCurrency();
        UpdateCurrency();
    }

    //=================================================================
    //                       HandleSubtraction()
    //=================================================================
    private void HandleSubtraction()
    {
        SubtractCurrency();
        UpdateCurrency();
    }

    //=================================================================
    //                      HandleWaterChanges()
    //=================================================================
    private void HandleWaterChanges()
    {
        AddToWaterMeter();
        UpdateWater();
    }

    //=================================================================
    //                       SubtractCurrency()
    //=================================================================
    private void SubtractCurrency()
    {
        currentCurrency -= subtractAmount;
    }

    //=================================================================
    //                        AddCurrency()
    //=================================================================
    private void AddCurrency()
    {
        currentCurrency += addAmount;
    }

    //=================================================================
    //                        AddWaterAmount()
    //=================================================================
    private void AddToWaterMeter()
    {
        currentWaterAmount += addWaterAmount;
    }

    //=================================================================
    //                      updateCurrency()
    //=================================================================
    private void UpdateCurrency()
    {
        currencyText.text = "Currency: " + currentCurrency;
    }

    //=================================================================
    //                       UpdateWater()
    //=================================================================
    private void UpdateWater()
    {
        if (currentWaterAmount < 0) { waterText.text = "Water: " + 0; }
        else { waterText.text = "Water: " + currentWaterAmount; }
    }

    //Getter & Setters
    public int CurrentCurrency { get => currentCurrency;}
    public int SubtractAmount { set => subtractAmount = value; }
    public int AddAmount {set => addAmount = value; }
    public int AddWaterAmount { set => addWaterAmount = value; }
}
