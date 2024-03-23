using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static Game Instance;

    [HideInInspector] public List<Route> readyRoutes = new();

    private int totalRoutes;
    private int successfulParks;

    public UnityAction<Route> OnCarEntersPark;
    public UnityAction OnCarCollision;
    private void Awake() 
    {
        Instance = this;
    }

    private void Start()
    {
        totalRoutes = transform.GetComponentsInChildren<Route>().Length;

        successfulParks = 0;

        OnCarEntersPark += OnCarEntersParkHandler;
        OnCarCollision += OnCarCollisionHandler;
    }

    private void OnCarCollisionHandler()
    {
        // TODO (taha): Make Lose Popup
        Debug.Log("GAME OVER");

        //int currentLevel = SceneManager.GetActiveScene().buildIndex;

        DOVirtual.DelayedCall(2f, () =>
        {
           
            int currentLevel = SceneManager.GetActiveScene().buildIndex;    
            SceneManager.LoadScene(currentLevel);
            
        });

    }

    private void OnCarEntersParkHandler(Route route)
    {
        route.car.StopDancingAnim();
        successfulParks++;

        if(successfulParks == totalRoutes)
        {
            // TODO (taha): Make WIN/LOSE Popup
            Debug.Log("You Win");

            // TODO: scriptble object level system
            int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;

            DOVirtual.DelayedCall(1.3f, () =>
            {
                if (nextLevel < SceneManager.sceneCountInBuildSettings)
                    SceneManager.LoadScene(nextLevel);
                else
                    Debug.LogWarning("No next level to load");
            });
        }
    }

    public void RegisterRoute(Route route)
    {
        readyRoutes.Add(route);

        if(readyRoutes.Count == totalRoutes)
        {
            MoveAllCars();
        }
    }
    
    private void MoveAllCars()
    {
        foreach(var route in readyRoutes)
        {
            route.car.Move(route.linePoints);
        }
    }
}
