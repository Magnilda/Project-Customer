using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownHall : Building
{
    [SerializeField] private GameObject townHallPrefab;
    [SerializeField] private int baseIncome;
    [SerializeField] private int farmWaterUsage;
    [SerializeField] private int houseWaterUsage;

    [SerializeField] private int townHallLevel = 1;
    [SerializeField] private int waterPumpAmount = 0;
    [SerializeField] private int farmAmount = 0;
    [SerializeField] private int cameraManAmount = 0;


    private int calculateIncome() //Income of money per day
    {
        int value = cameraManAmount * /* cameramanSalary * */ townHallLevel + baseIncome;
        return value;
    }

    private int calculateWaterIncome()  //Income of water per day
    {
        int value = (waterPumpAmount /* * waterPumpIncome */ ) - (farmWaterUsage + houseWaterUsage); // * amount of them
        return value;
    }

}
