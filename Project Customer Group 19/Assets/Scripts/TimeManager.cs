using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static Action OnNextDayAction;
    public static event Action OnMaximumDaysAction;

    [SerializeField] private int numberOfDays;
    [SerializeField] private int gameTime;
    [SerializeField] private Text countDownText;
    [SerializeField] private Text dayText;

    private float timePerDay;
    private float currentTime;
    private int currentDay;

    //=================================================================
    //                         Start()
    //=================================================================
    public void Start()
    {
        timePerDay = calculateTimePerDay();
        currentTime = timePerDay;
        currentDay = 1;
        dayText.text = "Day: " + currentDay;
        //Debug.Log(timePerDay);
    }

    //=================================================================
    //                         Update()
    //=================================================================
    public void Update()
    {
        handleCountDown();
    }

    //=================================================================
    //                      handleCountDown()
    //=================================================================
    private void handleCountDown()
    {
        currentTime -= Time.deltaTime;
        displayTime();
        //countDownText.text = currentTime.ToString("N0");    //N0 displays no decimals

        if(currentTime < 0)
        {
            currentTime = timePerDay;
            currentDay++;
            dayText.text = "Day: " + currentDay;
            OnNextDayAction?.Invoke();
        }
        if (currentDay > numberOfDays)
        {
            OnMaximumDaysAction?.Invoke();
        }
        
    }

    //=================================================================
    //                        displayTime()
    //=================================================================
    private void displayTime()
    {
        string minutes = ((int)currentTime / 60).ToString();
        string seconds = (currentTime % 60).ToString("00");
        if (seconds == "60") seconds = "59";
        countDownText.text = minutes + " : " + seconds;
    }

    //=================================================================
    //                    calculateTimePerDay()
    //=================================================================
    private float calculateTimePerDay()
    {
        return gameTime * 100 / numberOfDays; //miliseconds =   *1000
    }

    public int CurrentDay { get => currentDay; set => currentDay = value; }
}
 