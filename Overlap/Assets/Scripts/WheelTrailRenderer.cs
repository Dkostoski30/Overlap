using UnityEngine;

public class WheelTrailRenderer : MonoBehaviour
{
    private CarController controller;
    private TrailRenderer trailRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        controller = GetComponentInParent<CarController>();
        trailRenderer = GetComponent<TrailRenderer>();
        trailRenderer.emitting = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.isTyreScreeching(out float lateralVelocity, out bool isBraking)){
            trailRenderer.emitting = true;
        }else{
            trailRenderer.emitting = false;
        }
    }
}
