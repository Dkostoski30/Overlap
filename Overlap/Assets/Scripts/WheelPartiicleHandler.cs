using UnityEngine;

public class WheelPartiicleHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float particleEmissionRate = 0;
    private CarController controller;

    private ParticleSystem particleSystem;
    private ParticleSystem.EmissionModule particleSystemEmissionModule;
    void Start()
    {
        controller = GetComponentInParent<CarController>();
        particleSystem = GetComponent<ParticleSystem>();


        particleSystemEmissionModule = particleSystem.emission;
        particleEmissionRate = 0;
    }

    // Update is called once per frame
    void Update()
    {
        particleEmissionRate = Mathf.Lerp(particleEmissionRate, 0, Time.deltaTime * 5);
        particleSystemEmissionModule.rateOverTime = particleEmissionRate;

        if(controller.isTyreScreetching(out float lateralVelocity, out bool isBraking)){
            if (isBraking){
                particleEmissionRate = 30;
            }else{
                particleEmissionRate = Mathf.Abs(lateralVelocity) * 2;
            }
        }
    }
}
