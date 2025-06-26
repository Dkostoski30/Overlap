using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    void Start()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        CarData[] carDatas = Resources.LoadAll<CarData>("CarData/");

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            Transform spawnPoint = spawnPoints[i].transform;
            int playerNumber = i + 1;
            int playerSelectedCarID = PlayerPrefs.GetInt($"P{playerNumber}SelectedCarID");

            foreach (CarData cardata in carDatas)
            {
                if (cardata.CarUniqueID == playerSelectedCarID)
                {
                    GameObject car = Instantiate(cardata.CarPrefab, spawnPoint.position, spawnPoint.rotation);
                    bool isAI = PlayerPrefs.GetInt($"P{playerNumber}_IsAI") == 1;

                    car.name = isAI ? $"AI {playerNumber}" : $"Player {playerNumber}";
                    car.GetComponent<CarInputHandler>().playerNumber = playerNumber;

                    if (isAI)
                    {
                        car.GetComponent<CarInputHandler>().enabled = false;
                        car.tag = "AI";
                    }
                    else
                    {
                        car.GetComponent<CarAIHandler>().enabled = false;
                        car.GetComponent<AStarLite>().enabled = false;
                        car.tag = "Player";
                    }

                    break;
                }
            }
        }
    }
}
