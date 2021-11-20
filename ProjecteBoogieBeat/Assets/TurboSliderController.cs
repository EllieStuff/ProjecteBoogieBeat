using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurboSliderController : MonoBehaviour
{
    [SerializeField] Image fill;
    [SerializeField] Color initColor;
    [SerializeField] Color emptyColor = Color.white;

    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        fill.color = initColor;
    }


    public float GetValue()
    {
        return slider.value;
    }
    public void SetValue(float _value)
    {
        if (_value > 0)
        {
            slider.value = _value;
            fill.color = initColor;
        }
        else
        {
            slider.value = 0.0f;
            fill.color = emptyColor;
        }
    }


}
