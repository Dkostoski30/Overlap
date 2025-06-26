using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool isFinishLine = false;
    public int checkPointNumber = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (isFinishLine && (other.CompareTag("Player") || other.CompareTag("AI")))
        {
            Debug.Log($"{other.tag} crossed the finish line!");
            GameManager.instance.OnRaceCompleted();
        }
    }
}
