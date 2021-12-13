using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsanityBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Sets the value of the attached slider to the health amount passed to function.
    /// </summary>
    /// <param name="health"></param>
    public void SetInsanity(int insanity)
    {
        slider.value = insanity;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    /// <summary>
    /// Sets the max value of the attached slider to the health amount passed to function.
    /// </summary>
    /// <param name="health"></param>
    public void SetMaxInsanity(int insanity)
    {
        slider.maxValue = insanity;
        slider.value = insanity;
        fill.color = gradient.Evaluate(1f);

    }
}
