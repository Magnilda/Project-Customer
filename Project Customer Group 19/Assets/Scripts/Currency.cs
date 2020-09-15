using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Currency : MonoBehaviour
{
    //TODO Add a listener for an action to fire add method!

    [Header("Currency Settings")]
    //[SerializeField] private int startingCurrency;
    [SerializeField] private Text currencyText;

    [Header("Debug Settings")]
    private int currentCurrency;
    private int subtractAmount;
    private int addAmount;

    //=================================================================
    //                           Start()
    //=================================================================
    public void Start()
    {
        //currentCurrency = startingCurrency;
        Purchase.OnPurchase += HandleSubtraction;
        TimeManager.OnNextDayAction += HandleAdding;
    }

    //=================================================================
    //                          OnDestroy()
    //=================================================================
    public void OnDestroy()
    {
        Purchase.OnPurchase -= HandleSubtraction;
        TimeManager.OnNextDayAction -= HandleAdding;
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
    //                 SubtractCurrency(int amount)
    //=================================================================
    private void SubtractCurrency()
    {
        currentCurrency -= subtractAmount;
    }

    //=================================================================
    //                  AddCurrency(int amount)
    //=================================================================
    private void AddCurrency()
    {
        currentCurrency += addAmount;
    }

    //=================================================================
    //                      updateCurrency()
    //=================================================================
    private void UpdateCurrency()
    {
        currencyText.text = "Currency: " + currentCurrency;
    }

    //Getter & Setters
    public int CurrentCurrency { get => currentCurrency;}
    public int SubtractAmount { set => subtractAmount = value; }
    public int AddAmount {set => addAmount = value; }
}
