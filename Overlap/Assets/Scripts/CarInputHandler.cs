using UnityEngine;

public class CarInput : MonoBehaviour
{
    CarController controller;

    void Awake()
    {
        controller = GetComponent<CarController>();
    }

    void Update()
    {
        // Gather steering and acceleration inputs.
        Vector2 inputVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        controller.SetInputVector(inputVector);

        // Check if the handbrake key (space bar) is pressed.
        bool handbrake = Input.GetKey(KeyCode.Space);
        controller.SetHandbrake(handbrake);
    }
}