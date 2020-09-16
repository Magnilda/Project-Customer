using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBuilder : MonoBehaviour
{
    [SerializeField] private Builder builder;
    [SerializeField] private Button cancelButton;

    //=================================================================
    //                           Start()
    //=================================================================
    public void Start()
    {
        cancelButton.gameObject.SetActive(false);
    }

    //=================================================================
    //                        SetBuilderMode()
    //=================================================================
    public void EnableBuilderMode()
    {

        builder.CanBuild = true;
        EnableCancelButton();
        this.gameObject.SetActive(false);
        //Debug.Log("CHANGED TO TRUE");
    }

    //=================================================================
    //                      EnableCancelButton()
    //=================================================================
    public void EnableCancelButton()
    {
        cancelButton.gameObject.SetActive(true);
    }

    //=================================================================
    //                      DisableCancelButton()
    //=================================================================
    public void DisableCancelButton()
    {
        cancelButton.gameObject.SetActive(false);
        builder.CanBuild = false;
        this.gameObject.SetActive(true);
    }
}

