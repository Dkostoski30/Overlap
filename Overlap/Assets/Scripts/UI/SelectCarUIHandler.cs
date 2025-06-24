using System.Collections;
using UnityEngine;

public class SelectCarUIHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Car Prefab")]
    public GameObject carPrefab;

    [Header("Spawn On")]
    public Transform spawnOnTransform;

    public bool isCarChanging = false;
    CarUIHandler carUIHandler = null;
    void Start()
    {
        StartCoroutine(SpawnCarCO(true));
    }

    // Update is called once per frame
        void Update()
    {
if (Input.GetKey(KeyCode.LeftArrow) && !isCarChanging)
        {
            StartCoroutine(SpawnCarCO(true));
        }

    }

    IEnumerator SpawnCarCO(bool isCarAppearingOnRightSide)
    {
        isCarChanging = true;
        if (carUIHandler != null)
        {
             carUIHandler.StartCarExitAnimation(!isCarAppearingOnRightSide);
        }
        GameObject instantiatedCar = Instantiate(carPrefab, spawnOnTransform);
        carUIHandler = instantiatedCar.GetComponentInChildren<CarUIHandler>();
        carUIHandler.StartCarEntranceAnimation(isCarAppearingOnRightSide);
        yield return new WaitForSeconds(0.4f);

        isCarChanging = false;
    }
}
