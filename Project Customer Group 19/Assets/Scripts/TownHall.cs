using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownHall : MonoBehaviour
{
    [Header("Townhall Settings")]
    [SerializeField] private int baseIncome;
    [SerializeField] private int baseWaterUsage;
    [SerializeField] private int farmWaterUsage;
    [SerializeField] private int houseWaterUsage;

    [Header("Debug Settings")]
    [SerializeField] private int townHallLevel = 1;
    [SerializeField] private int houseAmount = 0;
    [SerializeField] private int waterPumpAmount = 0;
    [SerializeField] private int farmAmount = 0;
    [SerializeField] private int cameraManAmount = 0;
    [SerializeField] private int dailyIncome = 0;
    [SerializeField] private int dailyWaterIncome = 0;
    [SerializeField] private Currency UI_currency_script;

    //=================================================================
    //                           Start()
    //=================================================================
    public void Start()
    {
        System.Action temp = TimeManager.OnNextDayAction;        //Hot fix!: the OnNextDayAcion is not an 'event' Action (event in this case is like private) 
        TimeManager.OnNextDayAction = HandleVillageIncomes;      //First i made a temporary action with all the methods in the list then i added 
        TimeManager.OnNextDayAction += temp;                     //this method in front of the list to execute it first.

        UI_currency_script = FindObjectOfType<Currency>();
    }

    //=================================================================
    //                          OnDestroy()
    //=================================================================
    public void OnDestroy()
    {
        TimeManager.OnNextDayAction -= HandleVillageIncomes;
    }

    //=================================================================
    //                           Update()
    //=================================================================
    public void Update()
    {
        //empty
    }

    //=================================================================
    //                      HandleVillageIncomes()
    //=================================================================
    private void HandleVillageIncomes()
    {
        dailyIncome = CalculateIncome();
        dailyWaterIncome = CalculateWaterIncome();

        UI_currency_script.AddAmount = dailyIncome;
        UI_currency_script.AddWaterAmount = dailyWaterIncome;

        //dailyIncome = 0;
        //dailyWaterIncome = 0;
    }

    //=================================================================
    //                       CalculateIncome()
    //=================================================================
    private int CalculateIncome() //Income of money per day
    {
        int value = cameraManAmount /* * cameramanSalary */ + townHallLevel * baseIncome;
        return value;
    }

    //=================================================================
    //                     CalculateWaterIncome()
    //=================================================================
    private int CalculateWaterIncome()  //Income of water per day
    {
        int value = (waterPumpAmount /* * waterPumpIncome */ ) - (farmWaterUsage + houseWaterUsage + baseWaterUsage); // * amount of them
        return value;
    }

    public int WaterPumpAmount { get => waterPumpAmount; set => waterPumpAmount = value; }
    public int FarmAmount { get => farmAmount; set => farmAmount = value; }
    public int CameraManAmount { get => cameraManAmount; set => cameraManAmount = value; }
    public int HouseAmount { get => houseAmount; set => houseAmount = value; }
}
