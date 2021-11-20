using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCarTurbo : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float turboFuel = 0.3f;
    [SerializeField] float fuelDecreaseSpeed = 1.0f;

    //Slider turboSlider;
    TurboSliderController turboSlider;

    public bool HasFuel { get { return turboFuel > 0.0f; } }

    // Start is called before the first frame update
    void Start()
    {
        turboSlider = GameObject.FindGameObjectWithTag("TurboSlider").GetComponent<TurboSliderController>();
        turboSlider.SetValue(turboFuel);
    }


    public void DecreaseTurboFuel()
    {
        turboFuel -= fuelDecreaseSpeed * Time.deltaTime;
        if (turboFuel < 0) turboFuel = 0;

        turboSlider.SetValue(turboFuel);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("TurboItem"))
        {
            TurboItemScript turboItem = other.gameObject.GetComponent<TurboItemScript>();
            if (turboItem.IsActive)
            {
                turboFuel += turboItem.TurboAddAmmount;
                if (turboFuel > 1.0f) turboFuel = 1.0f;

                turboSlider.SetValue(turboFuel);

                turboItem.DeactivateOnTime();
            }

        }
    }

}
