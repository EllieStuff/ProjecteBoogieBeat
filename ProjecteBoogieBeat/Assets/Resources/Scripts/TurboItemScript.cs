using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurboItemScript : MonoBehaviour
{
    const float DEFAULT_DISABLE_TIME = 3.0f;

    [SerializeField] [Range(0.01f, 1)] float turboAddAmmount = 0.1f;
    [SerializeField] GameObject noteObject;

    bool isActive = true;
    
    public bool IsActive { get { return isActive; } }
    public float TurboAddAmmount { get { return turboAddAmmount; } }


    public void DeactivateOnTime(float _disableTime = DEFAULT_DISABLE_TIME)
    {
        StartCoroutine(DeactivateOnTimeCoroutine(_disableTime));
    }

    IEnumerator DeactivateOnTimeCoroutine(float _disableTime)
    {
        isActive = false;
        noteObject.SetActive(false);

        yield return new WaitForSeconds(_disableTime);

        isActive = true;
        noteObject.SetActive(true);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("AICar"))
        {
            DeactivateOnTime();
        }

    }

}
