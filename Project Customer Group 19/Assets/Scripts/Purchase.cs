using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Purchase : MonoBehaviour
{
    public static event Action <int> OnPurchase;

    public static void PurchaseItem(int price)
    {
        Debug.Log("item purchased for: " + price);
        OnPurchase?.Invoke(price);
    }

}
