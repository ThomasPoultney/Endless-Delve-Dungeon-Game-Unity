using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script responsible for updating the health bar HUD element.
/// </summary>
public class HealthBarScript : MonoBehaviour
{
    ///The slider of the health bar to be updated.
    public Slider slider;
    ///A gradient of colors that is used to determine the color of the health bar
    public Gradient gradient;
    ///The of the health bar to be updated.
    public Image fill;
 

    /// <summary>
    /// Sets the value of the attached slider to the health amount passed to function.
    /// </summary>
    /// <param name="health"></param>
    /// The players current health
    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    /// <summary>
    /// Sets the max value of the attached slider to the health amount passed to function.
    /// </summary>
    /// <param name="health"></param>
    /// The players max health
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);

    }
}
