using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<GameObject> objectsToActivate;
    [SerializeField] private Builder builder;
    [SerializeField] private TimeManager timeManager;

    public enum GameStage
    {
        MENU,
        TOWNHALL,
        GAME,
        GAMEOVER,
        WIN
    }
    private GameStage currentStage;

    //=================================================================
    //                         Start()
    //=================================================================
    void Start()
    {
        currentStage = GameStage.TOWNHALL;
    }

    //=================================================================
    //                        Update()
    //=================================================================
    void Update()
    {
        HandleGameStages();
    }

    //=================================================================
    //                      HandleGameStages()
    //=================================================================
    private void HandleGameStages()
    {
        if (currentStage == GameStage.TOWNHALL)
        {
            //TODO set the camera active?
            //canvas.gameObject.SetActive(false);
            builder.TownHallMode = true;
        }
        if (currentStage == GameStage.GAME)
        {
            foreach(GameObject gm in objectsToActivate)
            {
                gm.SetActive(true);
            }
            //canvas.gameObject.SetActive(true);
        }
    }

    public GameStage CurrentStage { get => currentStage; set => currentStage = value; }
}
