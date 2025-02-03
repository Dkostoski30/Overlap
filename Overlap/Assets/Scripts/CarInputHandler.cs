using UnityEngine;

public class CarInput : MonoBehaviour
{
    CarController controller;

    void Awake()
    {
        controller = GetComponent<CarController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        controller.SetInputVector(inputVector);
    }
}
