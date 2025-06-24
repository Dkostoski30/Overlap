using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectCarUIHandler : MonoBehaviour
{
    [Header("Car Prefab")]
    public GameObject carPrefab;

    [Header("Spawn On")]
    public Transform spawnOnTransform;

    public bool isCarChanging = false;
    private CarUIHandler carUIHandler = null;
    private int selectedCarIndex = 0;

    private CarData[] carDatas;

    public void OnPreviousCar()
    {
        if (isCarChanging || carDatas.Length == 0)
            return;

        selectedCarIndex--;
        if (selectedCarIndex < 0)
            selectedCarIndex = carDatas.Length - 1;

        StartCoroutine(SpawnCarCO(true));
    }

    public void OnNextCar()
    {
        if (isCarChanging || carDatas.Length == 0)
            return;

        selectedCarIndex++;
        if (selectedCarIndex >= carDatas.Length)
            selectedCarIndex = 0;

        StartCoroutine(SpawnCarCO(false));
    }
    public void OnSelectCar()
    {
        PlayerPrefs.SetInt("P1SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt("P2SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt("P3SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt("P4SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);

        PlayerPrefs.Save();
        SceneManager.LoadScene("SpawnCar");

    }
    void Start()
    {
        carDatas = Resources.LoadAll<CarData>("CarData/");

        if (carDatas.Length > 0)
        {
            StartCoroutine(SpawnCarCO(true));
        }
    }

    void Update()
    {
        if (carDatas.Length == 0)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnPreviousCar();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnNextCar();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSelectCar();
        }
    }

    IEnumerator SpawnCarCO(bool isCarAppearingOnRightSide)
    {
        if (carDatas.Length == 0)
            yield break;

        isCarChanging = true;

        if (carUIHandler != null)
        {
            carUIHandler.StartCarExitAnimation(!isCarAppearingOnRightSide);
        }

        GameObject instantiatedCar = Instantiate(carPrefab, spawnOnTransform);

        carUIHandler = instantiatedCar.GetComponent<CarUIHandler>();
        if (carUIHandler != null)
        {
            carUIHandler.SetupCar(carDatas[selectedCarIndex]);
            carUIHandler.StartCarEntranceAnimation(isCarAppearingOnRightSide);
        }

        yield return new WaitForSeconds(0.48f);
        isCarChanging = false;
    }
}
