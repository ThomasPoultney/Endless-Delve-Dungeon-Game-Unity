using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Reduces the light produced by the player based upon their current insanity.
/// </summary>
public class InsanityDemonstration : MonoBehaviour
{
    // Start is called before the first frame update

    ///The rate at which instanity is reduced
    public float insanityLoss = 0.01f;
    ///The minimum radius the light can be reduced too.
    public float minRadius = 0.10f;
    ///The starting outer radius of the light
    public float startOuterRadius = 9;
    ///The starting inner radius of the light
    public float startInnerRadius = 4;
    ///how often instanity is lost
    public float insanityLossIncrement = 0.2f;
    ///The light to apply the effect too
    UnityEngine.Rendering.Universal.Light2D light = null;
    ///The instanity bar hud element that is updated when insanity is reduced
    public InsanityBar insanityBar;
    void Start()
    {

        light = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        resetInsanityLight();
    }

    /// <summary>
    /// Resets the lights radius to its starting values.
    /// </summary>
    public void resetInsanityLight()
    {
        light.pointLightOuterRadius = startOuterRadius;
        light.pointLightInnerRadius = startInnerRadius;
        insanityBar.SetMaxInsanity(light.pointLightOuterRadius);
    }

    float elapsed = 0f;
    /// <summary>
    /// Monobeaviour that is called every frame, 
    /// If the time passed since the last insanity loss event is > the cooldown we trigger the insanity loss function.
    /// </summary>
    void Update()
    {
        insanityBar.SetInsanity(light.pointLightOuterRadius);
        elapsed += Time.deltaTime;
        if (elapsed >= insanityLossIncrement)
        {
            elapsed = elapsed % insanityLossIncrement;
            InsanityLoss();
        }
    }


    /// <summary>
    /// Reduces the lights inner and outer raidus until it is at the minimum value. Updates the insanity bar.
    /// </summary>
    private void InsanityLoss()
    {
       

       if (light.pointLightInnerRadius > minRadius)
        {
            light.pointLightInnerRadius = light.pointLightInnerRadius - insanityLoss;
        }
       if (light.pointLightOuterRadius > minRadius)
       {
            light.pointLightOuterRadius = light.pointLightOuterRadius - insanityLoss;
       }

        insanityBar.SetInsanity(light.pointLightOuterRadius);
    }
}

 