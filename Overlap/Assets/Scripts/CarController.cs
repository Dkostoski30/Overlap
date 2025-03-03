using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    [Tooltip("How much the car drifts sideways. Lower values yield less drift.")]
    public float driftFactor = 0.25f; 
    [Tooltip("How fast the car accelerates.")]
    public float accelerationFactor = 40.0f; 
    [Tooltip("How quickly the car turns.")]
    public float turnFactor = 5.0f; 
    [Tooltip("Maximum speed of the car.")]
    public float maxSpeed = 12f; 

    [Header("Smoothing Settings")]
    [Tooltip("Responsiveness of acceleration input. Lower values yield snappier acceleration.")]
    public float accelerationSmoothing = 3f; 
    [Tooltip("Responsiveness of steering input. Lower values yield more immediate turning.")]
    public float steeringSmoothing = 3f; 
    [Tooltip("How quickly lateral (sideways) velocity is reduced.")]
    public float driftSmoothing = 8f; 
    
    [Header("Handbrake Settings")]
    [Tooltip("Multiplier applied to forward velocity when handbrake is active (0 = instant stop, 1 = no effect)")]
    public float handbrakeDeceleration = 0.5f; 

    // Raw input values (set externally via SetInputVector)
    private float accelerationInput = 0f;
    private float steeringInput = 0f;
    
    // Smoothed inputs used internally
    private float currentAccelerationInput = 0f;
    private float currentSteeringInput = 0f;

    private Rigidbody2D carRigidbody2D;

    // Flags for collision and handbrake state.
    private bool isColliding = false;
    private bool handbrakeActive = false;

    void Awake()
    {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        // Dampen unwanted rotation after collisions.
        carRigidbody2D.angularDamping = 3f;
    }

    void FixedUpdate()
    {
        // Smooth the raw input values for a more responsive feel.
        currentAccelerationInput = Mathf.Lerp(currentAccelerationInput, accelerationInput, accelerationSmoothing * Time.fixedDeltaTime);
        currentSteeringInput = Mathf.Lerp(currentSteeringInput, steeringInput, steeringSmoothing * Time.fixedDeltaTime);

        ApplyEngineForce();

        if (!isColliding)
        {
            ApplySteering();
        }
        else
        {
            // Reduce unwanted spin during collisions.
            carRigidbody2D.angularVelocity = Mathf.Lerp(carRigidbody2D.angularVelocity, 0, 2f * Time.fixedDeltaTime);
        }

        // Use handbrake adjustments when active; otherwise, perform normal drift correction.
        if (handbrakeActive)
        {
            ApplyHandbrake();
        }
        else
        {
            ApplyDrift();
        }
    }

    private void ApplyEngineForce()
    {
        // Add forward force based on current acceleration input.
        Vector2 engineForce = transform.up * currentAccelerationInput * accelerationFactor;
        carRigidbody2D.AddForce(engineForce, ForceMode2D.Force);

        // Clamp the velocity to the max speed.
        if (carRigidbody2D.linearVelocity.magnitude > maxSpeed)
        {
            carRigidbody2D.linearVelocity = carRigidbody2D.linearVelocity.normalized * maxSpeed;
        }

        // Apply extra drag when not accelerating.
        if (Mathf.Abs(currentAccelerationInput) < 0.01f)
        {
            carRigidbody2D.linearVelocity *= 0.98f;
        }
    }

    private void ApplySteering()
    {
        float speed = carRigidbody2D.linearVelocity.magnitude;
        if (speed > 0.1f)
        {
            // Determine if the car is moving forward or backward.
            float direction = Mathf.Sign(Vector2.Dot(carRigidbody2D.linearVelocity, transform.up));
            // Adjust turning based on speed (faster turning at lower speeds).
            float speedFactor = Mathf.Clamp01(speed / maxSpeed);
            float adjustedTurnFactor = Mathf.Lerp(turnFactor * 1.5f, turnFactor * 0.5f, speedFactor);
            float turnAngle = currentSteeringInput * adjustedTurnFactor * direction;
            carRigidbody2D.rotation -= turnAngle;
        }
    }

    private void ApplyDrift()
    {
        // Decompose the velocity into forward and lateral components.
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.linearVelocity, transform.up);
        Vector2 lateralVelocity = transform.right * Vector2.Dot(carRigidbody2D.linearVelocity, transform.right);
        
        // Reduce the lateral velocity to create a smoother, arcade-style drift.
        Vector2 desiredVelocity = forwardVelocity + lateralVelocity * driftFactor;
        carRigidbody2D.linearVelocity = Vector2.Lerp(carRigidbody2D.linearVelocity, desiredVelocity, driftSmoothing * Time.fixedDeltaTime);
    }
    
    private void ApplyHandbrake()
    {
        // Decompose the velocity.
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.linearVelocity, transform.up);
        Vector2 lateralVelocity = transform.right * Vector2.Dot(carRigidbody2D.linearVelocity, transform.right);
        
        // Apply strong deceleration to forward movement.
        forwardVelocity *= handbrakeDeceleration;
        carRigidbody2D.linearVelocity = forwardVelocity + lateralVelocity;
    }

    // Update the inputs from an external input manager.
    public void SetInputVector(Vector2 inputVector)
    {
        steeringInput = inputVector.x;
        accelerationInput = inputVector.y;
    }
    
    // Activate or deactivate the handbrake.
    public void SetHandbrake(bool active)
    {
        handbrakeActive = active;
    }

    float GetLateralVelocity()
    {
        return Vector2.Dot(transform.right, carRigidbody2D.linearVelocity);
    }

    public bool isTyreScreeching(out float lateralVelocity, out bool isBraking)
    {
        lateralVelocity = GetLateralVelocity();
        float speed = carRigidbody2D.linearVelocity.magnitude;
        bool tyreScreech = false;
        
        // Condition 1: Braking while moving fast triggers screeching.
        isBraking = accelerationInput < 0 && speed > 2f;
        if (isBraking)
        {
            tyreScreech = true;
        }
        
        // Condition 2: Large slip angle causes screeching.
        float slipAngle = Vector2.Angle(carRigidbody2D.linearVelocity, transform.up);
        if (speed > 2f && slipAngle > 20f)
        {
            tyreScreech = true;
        }
        
        // Condition 3: Excessive lateral movement triggers screeching.
        float driftRatio = Mathf.Abs(lateralVelocity) / (speed + 0.1f);
        if (driftRatio > 0.4f)
        {
            tyreScreech = true;
        }
        
        return tyreScreech;
    }

    // Disable steering when colliding with barriers.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Barrier"))
        {
            isColliding = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Barrier"))
        {
            isColliding = false;
        }
    }
}
