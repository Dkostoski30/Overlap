﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositionHandler : MonoBehaviour
{
    LeaderboardUIHandler leaderboardUIHandler;

    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();
    private void Awake()
    {

    }

    void Start()
    {
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        carLapCounters = carLapCounterArray.ToList<CarLapCounter>();

        foreach (CarLapCounter lapCounters in carLapCounters)
            lapCounters.OnPassCheckpoint += OnPassCheckpoint;

        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();

        if (leaderboardUIHandler != null)
            leaderboardUIHandler.UpdateList(carLapCounters);
    }

    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        carLapCounters = carLapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckPoint()).ToList();

        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;

        carLapCounter.SetCarPosition(carPosition);

        if (leaderboardUIHandler != null)
            leaderboardUIHandler.UpdateList(carLapCounters);
    }
}
