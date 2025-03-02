using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float driftFactor = 0.3f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 10f; // Speed limit for stability

    [Header("Smoothing Settings")]
    [Tooltip("How quickly the car reaches the desired acceleration input")]
    public float accelerationSmoothing = 5f;
    [Tooltip("How quickly the car reaches the desired steering input")]
    public float steeringSmoothing = 5f;
    [Tooltip("How smoothly the lateral velocity is reduced (drift)")]
    public float driftSmoothing = 5f;

    // Raw input values (set externally via SetInputVector)
    private float accelerationInput = 0f;
    private float steeringInput = 0f;
    
    // Smoothed inputs used internally
    private float currentAccelerationInput = 0f;
    private float currentSteeringInput = 0f;

    private Rigidbody2D carRigidbody2D;

    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Smooth the raw input values for gradual acceleration and turning
        currentAccelerationInput = Mathf.Lerp(currentAccelerationInput, accelerationInput, accelerationSmoothing * Time.fixedDeltaTime);
        currentSteeringInput = Mathf.Lerp(currentSteeringInput, steeringInput, steeringSmoothing * Time.fixedDeltaTime);

        ApplyEngineForce();
        ApplySteering();
        ApplyDrift();
    }

    private void ApplyEngineForce()
    {
        // Apply force in the forward direction based on the smoothed acceleration input.
        Vector2 engineForce = transform.up * currentAccelerationInput * accelerationFactor;
        carRigidbody2D.AddForce(engineForce, ForceMode2D.Force);

        // Limit the car's speed.
        if (carRigidbody2D.linearVelocity.magnitude > maxSpeed)
        {
            carRigidbody2D.linearVelocity = carRigidbody2D.linearVelocity.normalized * maxSpeed;
        }

        // If there is little to no acceleration input, gradually reduce the velocity.
        if (Mathf.Abs(currentAccelerationInput) < 0.01f)
        {
            carRigidbody2D.linearVelocity *= 0.98f;
        }
    }

    private void ApplySteering()
    {
        float speed = carRigidbody2D.linearVelocity.magnitude;
        if (speed > 0.1f) // Only steer when the car is moving.
        {
            // Determine the relative direction (forward or reverse)
            float direction = Mathf.Sign(Vector2.Dot(carRigidbody2D.linearVelocity, transform.up));

            // Adjust turning responsiveness based on speed:
            // - At low speeds, the car turns more sharply.
            // - At high speeds, the turn is more subdued.
            float speedFactor = Mathf.Clamp01(speed / maxSpeed);
            float adjustedTurnFactor = Mathf.Lerp(turnFactor * 1.5f, turnFactor * 0.5f, speedFactor);

            // Calculate and apply the turn using the smoothed steering input.
            float turnAngle = currentSteeringInput * adjustedTurnFactor * direction;
            carRigidbody2D.rotation -= turnAngle;
        }
    }

    private void ApplyDrift()
    {
        // Decompose the current velocity into the car's forward and lateral components.
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.linearVelocity, transform.up);
        Vector2 lateralVelocity = transform.right * Vector2.Dot(carRigidbody2D.linearVelocity, transform.right);

        // Reduce lateral (sideways) velocity gradually using a Lerp for smoother drift correction.
        Vector2 desiredVelocity = forwardVelocity + lateralVelocity * driftFactor;
        carRigidbody2D.linearVelocity = Vector2.Lerp(carRigidbody2D.linearVelocity, desiredVelocity, driftSmoothing * Time.fixedDeltaTime);
    }

    // This method is called externally (for example, by your input manager) to update the inputs.
    public void SetInputVector(Vector2 inputVector)
    {
        // X controls steering, Y controls acceleration/braking.
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }
    float GetLateralVelocity()
    {
        //Returns how how fast the car is moving sideways.
        return Vector2.Dot(transform.right, carRigidbody2D.linearVelocity);
    }
    public bool isTyreScreetching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
    float speed = carRigidbody2D.linearVelocity.magnitude;
    bool tyreScreech = false;
    
    // Condition 1: Braking while moving above a minimal speed triggers screeching.
    // (We consider a minimal speed to avoid screeching when the car is nearly stopped.)
    isBraking = accelerationInput < 0 && speed > 2f;
    if (isBraking)
    {
        tyreScreech = true;
    }
    
    // Condition 2: A large slip angle (difference between velocity and forward direction) suggests loss of traction.
    float slipAngle = Vector2.Angle(carRigidbody2D.linearVelocity, transform.up);
    if (speed > 2f && slipAngle > 20f)
    {
        tyreScreech = true;
    }
    
    // Condition 3: High drift ratio indicates excessive sideways movement.
    // Adding a small value to avoid division by zero.
    float driftRatio = Mathf.Abs(lateralVelocity) / (speed + 0.1f);
    if (driftRatio > 0.4f)
    {
        tyreScreech = true;
    }
    
    return tyreScreech;
    }
}
