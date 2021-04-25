using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrusterBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxThruster(float thruster)
    {
        slider.maxValue = thruster;
        slider.value = thruster;
    }

    public void SetThruster(float thruster)
    {
        slider.value = thruster;
    }
}
