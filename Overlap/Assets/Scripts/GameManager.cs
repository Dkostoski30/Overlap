using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GameStates { running, raceOver };

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    GameStates gameState = GameStates.running;
    float raceStartedTime = 0;
    float raceCompletedTime = 0;

    public event Action<GameManager> OnGameStateChanged;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void LevelStart()
    {
        gameState = GameStates.running;
        raceStartedTime = Time.time;
    }

    public GameStates GetGameState() => gameState;

    void ChangeGameState(GameStates newGameState)
    {
        if (gameState != newGameState)
        {
            gameState = newGameState;
            OnGameStateChanged?.Invoke(this);
        }
    }

    public float GetRaceTime()
    {
        if (gameState == GameStates.raceOver)
            return raceCompletedTime - raceStartedTime;
        else
            return Time.time - raceStartedTime;
    }

    public void OnRaceCompleted()
    {
        Debug.Log("OnRaceCompleted called");
        raceCompletedTime = Time.time;
        ChangeGameState(GameStates.raceOver);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
    }
}

