using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCarTurbo : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float turboAmmount = 0.5f;
    
    Slider turboSlider;

    // Start is called before the first frame update
    void Start()
    {
        turboSlider = GameObject.FindGameObjectWithTag("TurboSlider").GetComponent<Slider>();
        turboSlider.value = turboAmmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("TurboItem"))
        {
            TurboItemScript turboItem = other.gameObject.GetComponent<TurboItemScript>();
            if (turboItem.IsActive)
            {
                turboAmmount += turboItem.TurboAddAmmount;
                if (turboAmmount > 1.0f) turboAmmount = 1.0f;
                turboSlider.value = turboAmmount;

                turboItem.DeactivateOnTime();
            }

        }
    }

}
