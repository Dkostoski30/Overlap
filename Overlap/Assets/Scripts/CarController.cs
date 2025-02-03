using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car settings")]
    public float driftFactor = 0.3f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 10f; // Limit speed to prevent instability

    private float accelerationInput = 0;
    private float steeringInput = 0;
    private float rotationAngle = 0;

    private Rigidbody2D carrigidbody2D;

    void Awake()
    {
        carrigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ApplyEngineForce();
        ApplySteering();
        KillOrthogonalVelocity();
    }

    private void ApplyEngineForce()
    {
        // Apply forward force based on acceleration input
        Vector2 engineForce = transform.up * accelerationInput * accelerationFactor;
        carrigidbody2D.AddForce(engineForce, ForceMode2D.Force);
    
        // Limit speed
        if (carrigidbody2D.linearVelocity.magnitude > maxSpeed)
        {
            carrigidbody2D.linearVelocity = carrigidbody2D.linearVelocity.normalized * maxSpeed;
        }
    
        // **Apply gradual slowdown when no acceleration input is given**
        if (Mathf.Abs(accelerationInput) < 0.01f) // When player releases the button
        {
            carrigidbody2D.linearVelocity *= 0.98f; // Reduce speed gradually (98% of previous speed)
        }
    }
    
    private void ApplySteering()
    {
        if (carrigidbody2D.linearVelocity.magnitude > 0.1f) // Only turn when moving
        {
            float velocityMagnitude = carrigidbody2D.linearVelocity.magnitude;
            float direction = Mathf.Sign(Vector2.Dot(carrigidbody2D.linearVelocity, transform.up)); // 1 for forward, -1 for reverse
            
            // Adjust turning responsiveness based on speed (slower turning at higher speeds)
            float speedFactor = Mathf.Clamp01(velocityMagnitude / maxSpeed); // 0 (slow) to 1 (max speed)
            float adjustedTurnFactor = Mathf.Lerp(turnFactor * 1.5f, turnFactor * 0.5f, speedFactor); // Sharper at low speeds
    
            float turnAngle = steeringInput * adjustedTurnFactor * direction;
            carrigidbody2D.rotation -= turnAngle;
        }
    }

    private void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carrigidbody2D.linearVelocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carrigidbody2D.linearVelocity, transform.right);

        carrigidbody2D.linearVelocity = forwardVelocity + rightVelocity * driftFactor;
    }

    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }
}
