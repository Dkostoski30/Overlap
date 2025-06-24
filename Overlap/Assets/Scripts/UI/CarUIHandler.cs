using UnityEngine;
using UnityEngine.UI;

public class CarUIHandler : MonoBehaviour
{
    Animator animator = null;
    [Header("Car Details")]
    public Image carImage;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetupCar(CarData carData)
    {
        carImage.sprite = carData.CarUISprite;
    }

    public void StartCarEntranceAnimation(bool isAppearingOnRightSide)
    {
        if (isAppearingOnRightSide)
        {
            animator.Play("Car UI Appear From Right");
        }
        else
        {
            animator.Play("Car UI Appear From Left");
        }
    }
    public void StartCarExitAnimation(bool isExitingOnRightSide)
    {
        if (isExitingOnRightSide)
        {
            animator.Play("Car UI Disappear To Right");
        }
        else
        {
            animator.Play("Car UI Disappear To Left");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    //Event
    public void OnCarExitAnimationCompleted()
    {
        Destroy(gameObject);
    }
}
