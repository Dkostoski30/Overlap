using UnityEngine;

public class SpawnCars : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject[] SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        CarData[] carDatas = Resources.LoadAll<CarData>("CarData/");

        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            Transform spawnPoint = SpawnPoints[i].transform;

            int playerID = PlayerPrefs.GetInt($"P{i + 1}SelectedCarID");
            foreach (CarData carData in carDatas)
            {
                if (carData.CarUniqueID == playerID)
                {
                    GameObject car = Instantiate(carData.CarPrefab, spawnPoint.position, spawnPoint.rotation);
                    car.GetComponent<CarInputHandler>().playerNumber = i + 1;
                    break;
                }
            }
                
        }
        
    }

}
