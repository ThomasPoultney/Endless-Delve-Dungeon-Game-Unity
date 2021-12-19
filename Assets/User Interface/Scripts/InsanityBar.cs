using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class responsible for controlling the insanity bar HUD element.
/// </summary>
public class InsanityBar : MonoBehaviour
{
    ///The slider of the insanity bar to be updated.
    public Slider slider;
    ///A gradient of colors that is used to determine the color of the insanity bar
    public Gradient gradient;
    ///The image of the insanity bar to be updated.
    public Image fill;


    /// <summary>
    /// Sets the value of the attached slider to the insanity amount passed to function.
    /// </summary>
    /// <param name="insanity"></param>
    /// The insanity value to set the insanity UI element to.
    public void SetInsanity(float insanity)
    {
        slider.value = insanity;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    /// <summary>
    /// Sets the max value of the attached slider to the insanity amount passed to function.
    /// </summary>
    /// <param name="insanity"></param>
    /// The max insanity value to set the insanity slider ui element to.
    public void SetMaxInsanity(float insanity)
    {
        slider.maxValue = insanity;
        slider.value = insanity;
        fill.color = gradient.Evaluate(1f);

    }
}
